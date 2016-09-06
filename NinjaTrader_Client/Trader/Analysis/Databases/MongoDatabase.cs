﻿using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NinjaTrader_Client.Trader
{
    public class MongoDatabase
    {
        public int errors = 0;
        MongoFacade mongodb;
        MongoCollection<BsonDocument> pricesCollection, dataCollection;

        public MongoDatabase(MongoFacade mongoDbFacade)
        {
            mongodb = mongoDbFacade;

            pricesCollection = mongodb.getDB().GetCollection("prices");
            dataCollection = mongodb.getDB().GetCollection("data");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public TickData getPrice(long timestamp, string instrument, bool caching = true)
        {
            return getPriceInternal(timestamp, instrument);
        }

        public List<TickData> getPrices(long startTimestamp, long endTimestamp, string instrument)
        {
            var docs = pricesCollection.FindAs<BsonDocument>(Query.And(Query.EQ("instrument", instrument), Query.LT("timestamp", endTimestamp + 1), Query.GT("timestamp", startTimestamp - 1))).SetSortOrder(SortBy.Ascending("timestamp"));

            List<TickData> output = new List<TickData>();
            foreach (BsonDocument doc in docs)
            {
                output.Add(new TickData(doc["timestamp"].AsInt64, doc["last"].AsDouble, doc["bid"].AsDouble, doc["ask"].AsDouble, doc["instrument"].AsString));
            }

            return output;
        }

        public void setPrice(TickData td, string instrument)
        {
            try
            {
                BsonDocument input = new BsonDocument();
                input.Set("timestamp", td.timestamp);
                input.Set("last", td.last);
                input.Set("bid", td.bid);
                input.Set("ask", td.ask);
                input.Set("instrument", instrument);

                pricesCollection.Insert(input);
            }
            catch { errors++; }
        }

        public void setData(TimeValueData data, string dataName, string instrument) 
        {
            try
            {
                BsonDocument input = new BsonDocument();
                input.SetElement(new BsonElement("timestamp", data.timestamp));
                input.SetElement(new BsonElement("value", data.value));
                input.SetElement(new BsonElement("dataName", dataName));


                dataCollection.Insert(input);
            }
            catch { errors++; }
        }

        public TimeValueData getData(long timestamp, string dataName, string instrument)
        {
            var docsDarunter = dataCollection.FindAs<BsonDocument>(Query.And(Query.EQ("dataName", dataName), Query.LT("timestamp", timestamp + 1), Query.GT("timestamp", timestamp - (3 * 60 * 1000)))).SetSortOrder(SortBy.Descending("timestamp")).SetLimit(1);
            BsonDocument darunter = docsDarunter.ToList<BsonDocument>()[0]; //Will throw interrupt when the timestamp is to early in history. Should be handled ???

            return new TimeValueData(darunter["timestamp"].AsInt64, darunter["value"].AsDouble);
        }

        public List<TimeValueData> getDataInRange(long startTimestamp, long endTimestamp, string dataName, string instrument)
        {
            var docs = dataCollection.FindAs<BsonDocument>(Query.And(Query.EQ("dataName", dataName), Query.LT("timestamp", endTimestamp + 1L), Query.GT("timestamp", startTimestamp - 1L))).SetSortOrder(SortBy.Ascending("timestamp"));

            List<TimeValueData> output = new List<TimeValueData>();
            foreach (BsonDocument doc in docs)
            {
                output.Add(new TimeValueData(doc["timestamp"].AsInt64, doc["value"].AsDouble));
            }

            return output;
        }

        public long getSetsCount()
        {
            long all = 0;
            foreach (MongoCollection<BsonDocument> collection in mongodb.getCollections())
                all += collection.Count(); //Could be done more effictient by counting inserts

            return all;
        }

        public long getLastTimestamp()
        {
            var cursor = pricesCollection.FindAs<BsonDocument>(Query.Exists("timestamp")).SetSortOrder(SortBy.Descending("timestamp")).SetLimit(1); //Does not limit! ???
            BsonDocument doc = cursor.First<BsonDocument>();

            return doc["timestamp"].AsInt64;
        }

        public long getFirstTimestamp()
        {
                var docs = pricesCollection.FindAs<BsonDocument>(Query.Exists("timestamp")).SetSortOrder(SortBy.Ascending("timestamp")).SetLimit(1);
                return docs.ToList<BsonDocument>()[0]["timestamp"].AsInt64;
        }

        public void shutdown()
        {
            mongodb.shutdown();
        }

        //##Not in interface
        
        private TickData getPriceInternal(long timestamp, string instrument)
        {
            var docsDarunter = pricesCollection.FindAs<BsonDocument>(Query.And(Query.EQ("instrument", instrument), Query.LT("timestamp", timestamp + 1), Query.GT("timestamp", timestamp - (3 * 60 * 1000)))).SetSortOrder(SortBy.Descending("timestamp")).SetLimit(1);
            BsonDocument darunter = docsDarunter.ToList<BsonDocument>()[0];

            return new TickData(darunter["timestamp"].AsInt64, darunter["last"].AsDouble, darunter["bid"].AsDouble, darunter["ask"].AsDouble, darunter["instrument"].AsString);
        }

        public void exportData(long startTimestamp, string basePath)
        {
            if (Directory.Exists(basePath + "/export") == false)
                Directory.CreateDirectory(basePath + "/export");

            string now = Timestamp.getNow().ToString();
            string nowReadable = DateTime.Now.ToString("yyyy-MM-dd");

            int i = 0;
            foreach (MongoCollection<BsonDocument> collection in mongodb.getCollections())
            {
                if (collection.Name.Contains("system.indexes") == false)
                {
                    string export_str = getExportData(startTimestamp, collection).ToString();
                    export_str = StringCompressor.CompressString(export_str);

                    File.WriteAllText(basePath + "/export/database_" + nowReadable + "_" + startTimestamp + "_" + now + "_PART-" + i + ".json", export_str); //1443611923418
                    i++;
                }
            }
        }

        private BsonDocument getExportData(long startTime, MongoCollection<BsonDocument> collection)
        {
            BsonDocument exportData = new BsonDocument();

            var docs = collection.Find(Query.GT("timestamp", startTime - 1L)).SetSortOrder(SortBy.Ascending("timestamp"));

            var array = new BsonArray();
            foreach (var item in docs)
            {
                array.Add(item);
            }

            exportData[collection.Name] = array;

            return exportData;
        }

        public void importData(string data)
        {
            var options = new MongoInsertOptions() { Flags = InsertFlags.ContinueOnError };

            BsonDocument doc = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(data);
            foreach (string name in doc.Names)
            {
                try
                {
                    var result = mongodb.getDB().GetCollection(name).InsertBatch(doc[name].AsBsonArray, options);
                }
                catch { errors++; }
                //.Insert(doc[name].AsBsonArray); //Untested
            }

            /*int errors = 0;
            foreach(string name in doc.Names)
            {
                foreach (BsonDocument entry in doc[name].AsBsonArray)
                    try
                    {
                        mongodb.getCollection(name).Insert(entry);
                    }
                    catch { errors++;  }
            }*/
        }

        public void migrate(SQLiteDatabase sql)
        {
            /*List<MongoCollection> list = mongodb.getCollections();

            foreach (MongoCollection<BsonDocument> collection in list)
            {
                if (collection.Name.Contains("system.indexes") == false)
                    collection.ReIndex();
            }*/

            long firstTimestamp = 1442606605172;
            long step = 60 * 1000 * 10;

            int error = 0;

            foreach (var collection in mongodb.getCollections())
            {
                string collectionName = collection.Name;
                if (collectionName.Length != 6 && collectionName.Contains("system.indexes") == false)
                {
                    long now = firstTimestamp;
                    while(now < Timestamp.getNow()
                        && collectionName != "AUDUSD" && collectionName != "AUDCAD" && collectionName != "AUDJPY" && collectionName != "CHFJPY" && collectionName != "EURCHF" && collectionName != "EURGBP" && collectionName != "EURJPY" && collectionName != "EURUSD")
                    {
                        try
                        {
                            var cursor = mongodb.getDB().GetCollection<BsonDocument>(collectionName).FindAs<BsonDocument>(Query.And(Query.GT("timestamp", now - step), Query.LT("timestamp", now)));
                            List<BsonDocument> docs = cursor.ToList<BsonDocument>();
                            foreach (BsonDocument doc in docs)
                            {
                                if (collectionName.Contains("_") == false)
                                {
                                    sql.setPrice(new TickData(doc["timestamp"].AsInt64, doc["last"].AsDouble, doc["bid"].AsDouble, doc["ask"].AsDouble, doc["instrument"].AsString));
                                }
                                else
                                {
                                    string instrument = collectionName.Substring(0, collectionName.IndexOf("_"));
                                    string dataName = collectionName.Substring(instrument.Length + 1);
                                    sql.setData(new TimeValueData(doc["timestamp"].AsInt64, doc["value"].AsDouble), dataName, instrument);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            error++;
                            break;
                        }

                        now += step;
                    }
                    //Send buffer?
                }
            }

            error = error + 0;

            //Create Index
            /*foreach (MongoCollection<BsonDocument> collection in list)
            {
                if (collection.Name.Contains("system.indexes") == false)
                    collection.CreateIndex("timestamp");
            }*/

            //SSI vor 1442832915118 muss - 0.5 und dann * 2!
            //Hat nicht funktioniert!
            /*foreach (MongoCollection<BsonDocument> collection in list)
            {
                if (collection.Name.EndsWith("_ssi-mt4"))
                {
                    var docs = collection.Find(Query.LT("timestamp", 1442883030534));

                    foreach (BsonDocument doc in docs)
                    {
                        double value = doc["value"].AsDouble;
                        value -= 0.5d;
                        value = value * 2;

                        collection.Update(Query.EQ("timestamp", doc["timestamp"].AsInt64), Update.Set("value", value));
                    }
                }
            }*/

            //Lösche Daten von SSI-win vor 1442883027534
            /*foreach(MongoCollection<BsonDocument> collection in list)
            {
                if(collection.Name.EndsWith("_ssi-win-mt4"))
                    collection.Remove(Query.LT("timestamp", 1442883030534));
            }*/

            /*foreach (MongoCollection<BsonDocument> collection in list)
            {
                if (collection.Name.EndsWith("_ssi-mt4"))
                    collection.Remove(Query.LT("timestamp", 1442883104095));
            }*/

            //remove bid and ask == 0
        }
    }
}