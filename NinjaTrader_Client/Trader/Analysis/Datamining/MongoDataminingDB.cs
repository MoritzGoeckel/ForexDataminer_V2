using NinjaTrader_Client.Trader.Datamining;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using NinjaTrader_Client.Trader.Indicators;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;
using NinjaTrader_Client.Trader.Utils;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using MongoDB.Driver.Builders;

namespace NinjaTrader_Client.Trader
{
    [Obsolete]
    public class MongoDataminingDB : IDataminingDatabase
    {
        TraderMongoDB database;
        MongoFacade mongodb;

        int threadsCount = 20;

        ProgressDict progress = new ProgressDict();

        Random z = new Random();
        
        public MongoDataminingDB(MongoFacade mongoDbFacade)
        {
            mongodb = mongoDbFacade;
            database = new TraderMongoDB(mongodb);
        }

        private static void waitForThreads(List<Thread> threads)
        {
            foreach (Thread thread in threads)
                thread.Join();
        }

        void IDataminingDatabase.importPair(string pair, long start, long end, SQLiteDatabase otherDatabase)
        {
            List<Thread> threads = new List<Thread>();

            long timeframe = Convert.ToInt64(Convert.ToDouble(end - start) / Convert.ToDouble(threadsCount));
            int threadId = 0;
            
            while (threadId < threadsCount)
            {
                long threadBeginning = start + timeframe * threadId;
                long threadEnd = threadBeginning + timeframe;

                Thread thread = new Thread(delegate ()
                {
                    string name = "import " + " ID_" + threadBeginning + ":" + threadEnd;
                    progress.setProgress(name, 0);
                    int done = 0;

                    List<TickData> data = otherDatabase.getPrices(threadBeginning, threadEnd, pair);
                    long count = data.Count();

                    foreach (TickData d in data)
                    {
                        database.setPrice(d, pair);
                        done++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(done) / Convert.ToDouble(count) * 100d));
                    }

                    progress.remove(name);
                });

                thread.Start();
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        void IDataminingDatabase.addOutcome(long timeframe, string instrument)
        {
            //Do in walker? Faster? Todo...
            //{ outcome_max_1800: { $exists: true } }

            List<Thread> threads = new List<Thread>();
            
            long start = database.getFirstTimestamp();
            long end = database.getLastTimestamp();

            long oneThreadTimeframe = (end - start) / threadsCount;
            int threadId = 0;

            var collection = mongodb.getDB().GetCollection("prices");

            while (threadId < threadsCount)
            {
                long threadBeginning = start + (oneThreadTimeframe * threadId);
                long threadEnd = threadBeginning + oneThreadTimeframe;

                Thread thread = new Thread(delegate ()
                {
                    string name = "outcome " + timeframe + " ID_" + threadBeginning + ":" + threadEnd;
                    progress.setProgress(name, 0);
                    int done = 0;
                    long count = 0;
                    
                    var docs = collection.FindAs<BsonDocument>(Query.And(Query.EQ("instrument", instrument), Query.NotExists("outcome_max_" + timeframe), Query.LT("timestamp", threadEnd), Query.GTE("timestamp", threadBeginning))).SetSortOrder(SortBy.Ascending("timestamp"));
                    docs.SetFlags(QueryFlags.NoCursorTimeout);

                    count = docs.Count();
                    foreach (BsonDocument doc in docs)
                    {
                        done++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(done) / Convert.ToDouble(count) * 100d));

                        try
                        {
                            TickData inTimeframe = database.getPrice(doc["timestamp"].AsInt64 + timeframe, doc["instrument"].AsString);

                            var min_doc = collection.FindAs<BsonDocument>(Query.And(Query.EQ("instrument", doc["instrument"].AsString), Query.LT("timestamp", doc["timestamp"].AsInt64 + timeframe), Query.GT("timestamp", doc["timestamp"].AsInt64)))
                             .SetSortOrder(SortBy.Ascending("bid"))
                             .SetLimit(1)
                             .SetFields("bid")
                             .Single();

                            var max_doc = collection.FindAs<BsonDocument>(Query.And(Query.EQ("instrument", doc["instrument"].AsString), Query.LT("timestamp", doc["timestamp"].AsInt64 + timeframe), Query.GT("timestamp", doc["timestamp"].AsInt64)))
                             .SetSortOrder(SortBy.Descending("ask"))
                             .SetLimit(1)
                             .SetFields("ask")
                             .Single();

                            string s = max_doc["ask"].AsDouble.ToString();
                            s = s + "";

                            collection.FindAndModify(new FindAndModifyArgs()
                            {
                                Query = Query.EQ("_id", doc["_id"]),
                                Update = Update.Combine(
                                        Update.Set("outcome_max_" + timeframe, max_doc["ask"]),
                                        Update.Set("outcome_min_" + timeframe, min_doc["bid"]),
                                        Update.Set("outcome_actual_" + timeframe, inTimeframe.getAvgPrice())
                                    )
                            });
                        }
                        catch { }
                    }

                    progress.remove(name);

                });

                thread.Start();
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        void IDataminingDatabase.addData(string dataname, SQLiteDatabase database, string instrument)
        {
            List<Thread> threads = new List<Thread>();
            
            long start = database.getFirstTimestamp();
            long end = database.getLastTimestamp();

            long timeframe = (end - start) / threadsCount;
            int threadId = 0;

            var collection = mongodb.getDB().GetCollection("prices");

            while (threadId < threadsCount)
            {
                long threadBeginning = start + (timeframe * threadId);
                long threadEnd = threadBeginning + timeframe;

                Thread thread = new Thread(delegate ()
                {

                    string name = "data " + dataname + " ID_" + threadBeginning + ":" + threadEnd;
                    progress.setProgress(name, 0);
                    int done = 0;
                    long count = 0;

                    var docs = collection.FindAs<BsonDocument>(Query.And(Query.EQ("instrument", instrument), Query.NotExists(dataname), Query.LT("timestamp", threadEnd), Query.GTE("timestamp", threadBeginning))).SetSortOrder(SortBy.Ascending("timestamp"));
                    docs.SetFlags(QueryFlags.NoCursorTimeout);

                    count = docs.Count();
                    foreach (BsonDocument doc in docs)
                    {
                        done++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(done) / Convert.ToDouble(count) * 100d));

                        if (doc.ContainsValue(dataname))
                            continue;

                        try
                        {
                            TimeValueData data = database.getData(doc["timestamp"].AsInt64, dataname, doc["instrument"].AsString);

                            collection.FindAndModify(new FindAndModifyArgs()
                            {
                                Query = Query.EQ("_id", doc["_id"]),
                                Update = Update.Set(dataname, data.value)
                            });
                        }
                        catch { }
                    }

                    progress.remove(name);

                });

                thread.Start();
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        void IDataminingDatabase.getOutcomeIndicatorSampling(SampleOutcomeExcelGenerator excel, double min, double max, int steps, string indicatorId, int outcomeTimeframe, string instument)
        {
            List<Thread> threads = new List<Thread>();

            string sheetName = indicatorId + "_" + outcomeTimeframe + "_" + instument;

            if (sheetName.Length >= 30)
                sheetName = sheetName.Substring(0, 29);
            
            excel.CreateSheet(sheetName);

            var collection = mongodb.getDB().GetCollection("prices");
            double stepsize = (Convert.ToDouble(max - min) / Convert.ToDouble(steps));

            int startedThreads = 0;

            double current = min;
            while(current <= max)
            {
                startedThreads++;
                double valueMin = current;
                double valueMax = current + stepsize;
                Thread thread = new Thread(delegate ()
                {
                    instument = null;

                    IMongoQuery query = Query.And(Query.Exists(indicatorId), Query.Exists("last"), Query.Exists("outcome_actual_" + outcomeTimeframe), Query.Exists("outcome_max_" + outcomeTimeframe), Query.Exists("outcome_min_" + outcomeTimeframe), Query.LT(indicatorId, valueMax), Query.GTE(indicatorId, valueMin));
                    if (instument != null)
                        query = Query.And(query, Query.EQ("instrument", instument));

                    var docs = collection.FindAs<BsonDocument>(query);
                    double sumMax = 0d, sumMin = 0d, sumActual = 0d;
                    double count = docs.Count();
                    foreach (BsonDocument doc in docs)
                    {
                        double onePercent = doc["last"].AsDouble / 100d;

                        double maxDiff = doc["outcome_max_" + outcomeTimeframe].AsDouble / onePercent - 100d; //calculate percent difference
                        double minDiff = doc["outcome_min_" + outcomeTimeframe].AsDouble / onePercent - 100d;
                        double actualDiff = doc["outcome_actual_" + outcomeTimeframe].AsDouble / onePercent - 100d;

                        sumMax += maxDiff;
                        sumMin += minDiff;
                        sumActual += actualDiff;
                    }

                    if (count == 0)
                        excel.addRow(sheetName, 0, 0, 0, 0, 0, 0, 0, 0);
                    else
                    {
                        double maxAvg = sumMax / count;
                        double minAvg = sumMin / count;
                        double actualAvg = sumActual / count;

                        double minVsMax = sumMin + sumMax / count;

                        excel.addRow(sheetName, valueMin, valueMax, Convert.ToInt32(count), maxAvg, minAvg, minVsMax, actualAvg, (maxAvg + minAvg));
                    }

                });

                thread.Start();
                threads.Add(thread);

                current += stepsize;
            }

            //Darstellungsthread
            waitForThreads(threads);

            excel.FinishSheet(sheetName);
        }

        ProgressDict IDataminingDatabase.getProgress()
        {
            return progress;
        }

        void IDataminingDatabase.addIndicator(WalkerIndicator indicator, string instrument, string fieldId)
        {
            var collection = mongodb.getDB().GetCollection("prices");

            long start = database.getFirstTimestamp();
            long end = database.getLastTimestamp();

            string name = "Indicator " + indicator.getName() + " " + instrument + " " + fieldId;
            progress.setProgress(name, 0);
            int done = 0;
            long count = 0;

            var docs = collection.FindAs<BsonDocument>(Query.And(Query.Exists(fieldId), Query.EQ("instrument", instrument), Query.LT("timestamp", end), Query.GTE("timestamp", start))).SetSortOrder(SortBy.Ascending("timestamp"));
            docs.SetFlags(QueryFlags.NoCursorTimeout);
            count = docs.Count();

            foreach (var doc in docs)
            {
                progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(done) / Convert.ToDouble(count) * 100d));
                done++;

                indicator.setNextData(doc["timestamp"].AsInt64, doc[fieldId].AsDouble);

                collection.FindAndModify(new FindAndModifyArgs()
                {
                    Query = Query.EQ("_id", doc["_id"]),
                    Update = Update.Set(indicator.getName() + "_" + fieldId, indicator.getIndicator().value)
                });
            }

            progress.remove(name);
        }

        void IDataminingDatabase.addMetaIndicatorSum(string[] ids, double[] weights, string fieldName, string instrument)
        {
            List<Thread> threads = new List<Thread>();

            long start = database.getFirstTimestamp();
            long end = database.getLastTimestamp();

            long timeframe = (end - start) / threadsCount;
            int threadId = 0;

            var collection = mongodb.getDB().GetCollection("prices");

            IMongoQuery fieldsExistQuery = Query.And(Query.NotExists(fieldName), Query.EQ("instrument", instrument));
            foreach (string id in ids)
                fieldsExistQuery = Query.And(fieldsExistQuery, Query.Exists(id));

            while (threadId < threadsCount)
            {
                long threadBeginning = start + (timeframe * threadId);
                long threadEnd = threadBeginning + timeframe;

                Thread thread = new Thread(delegate () {

                    string name = "MetaindicatorSum" + " ID_" + threadBeginning + ":" + threadEnd;
                    progress.setProgress(name, 0);
                    int done = 0;
                    long count = 0;

                    var docs = collection.FindAs<BsonDocument>(Query.And(fieldsExistQuery, Query.LT("timestamp", threadEnd), Query.GTE("timestamp", threadBeginning)));
                    docs.SetFlags(QueryFlags.NoCursorTimeout);

                    count = docs.Count();
                    foreach (BsonDocument doc in docs)
                    {
                        done++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(done) / Convert.ToDouble(count) * 100d));

                        double value = 0;
                        for (int i = 0; i < ids.Length; i++)
                            value += doc[ids[i]].AsDouble * weights[i];

                        collection.FindAndModify(new FindAndModifyArgs()
                        {
                            Query = Query.EQ("_id", doc["_id"]),
                            Update = Update.Set(fieldName, value)
                        });
                    }

                    progress.remove(name);

                });

                thread.Start();
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        void IDataminingDatabase.addMetaIndicatorDifference(string id, string id_subtract, string fieldName, string instrument)
        {
            List<Thread> threads = new List<Thread>();

            long start = database.getFirstTimestamp();
            long end = database.getLastTimestamp();

            long timeframe = (end - start) / threadsCount;
            int threadId = 0;

            var collection = mongodb.getDB().GetCollection("prices");

            IMongoQuery fieldsExistQuery = Query.And(Query.EQ("instrument", instrument), Query.NotExists(fieldName), Query.Exists(id), Query.Exists(id_subtract));

            while (threadId < threadsCount)
            {
                long threadBeginning = start + (timeframe * threadId);
                long threadEnd = threadBeginning + timeframe;

                Thread thread = new Thread(delegate ()
                {

                    string name = "MetaindicatorDifference" + id + "-" + id_subtract + " " + threadBeginning + ":" + threadEnd;
                    progress.setProgress(name, 0);
                    int done = 0;
                    long count = 0;

                    var docs = collection.FindAs<BsonDocument>(Query.And(fieldsExistQuery, Query.LT("timestamp", threadEnd), Query.GTE("timestamp", threadBeginning)));
                    docs.SetFlags(QueryFlags.NoCursorTimeout);

                    count = docs.Count();
                    foreach (BsonDocument doc in docs)
                    {
                        done++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(done) / Convert.ToDouble(count) * 100d));

                        double value = doc[id].AsDouble - doc[id_subtract].AsDouble;
                        collection.FindAndModify(new FindAndModifyArgs()
                        {
                            Query = Query.EQ("_id", doc["_id"]),
                            Update = Update.Set(fieldName, value)
                        });
                    }

                    progress.remove(name);

                });

                thread.Start();
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        //Machine Learning

        void IDataminingDatabase.doMachineLearning(string[] inputFields, string outcomeField, string instrument, string savePath)
        {
            string name = "ANN";

            double learningRate = 0.1;
            double sigmoidAlphaValue = 2;
            int iterations = 100;

            bool useRegularization = false;
            bool useNguyenWidrow = false;
            bool useSameWeights = false;

            progress.setProgress(name, "Creating ANN...");
            
            // create multi-layer neural network
            ActivationNetwork ann = new ActivationNetwork(
                new BipolarSigmoidFunction(sigmoidAlphaValue),
                inputFields.Length, 20, 2); //How many neuros ???? Standart is 1

            if (useNguyenWidrow)
            {
                progress.setProgress(name, "Creating NguyenWidrow...");
                
                if (useSameWeights)
                    Accord.Math.Random.Generator.Seed = 0;

                NguyenWidrow initializer = new NguyenWidrow(ann);
                initializer.Randomize();
            }

            progress.setProgress(name, "Creating LevenbergMarquardtLearning...");
            
            // create teacher
            LevenbergMarquardtLearning teacher = new LevenbergMarquardtLearning(ann, useRegularization); //, JacobianMethod.ByBackpropagation

            // set learning rate and momentum
            teacher.LearningRate = learningRate;

            IMongoQuery fieldsExistQuery = Query.And(Query.Exists(outcomeField + "_buy"), Query.Exists(outcomeField + "_sell"));
            foreach (string inputField in inputFields)
                fieldsExistQuery = Query.And(fieldsExistQuery, Query.Exists(inputField));

            progress.setProgress(name, "Importing...");
                
            // Load Data
            long start = database.getFirstTimestamp();
            long end = database.getLastTimestamp();

            var collection = mongodb.getDB().GetCollection("prices");
            var docs = collection.FindAs<BsonDocument>(Query.And(fieldsExistQuery, Query.EQ("instrument", instrument), Query.LT("timestamp", end), Query.GTE("timestamp", start))).SetSortOrder(SortBy.Ascending("timestamp"));
            docs.SetFlags(QueryFlags.NoCursorTimeout);
            long resultCount = docs.Count();

            //Press into Array from
            progress.setProgress(name, "Casting to array...");
                
            double[][] inputs = new double[resultCount][]; // [inputFields.Length]
            double[][] outputs = new double[resultCount][]; // [2]

            int row = 0;
            foreach (var doc in docs)
            {
                outputs[row] = new double[] { doc[outcomeField + "_buy"].AsInt32, doc[outcomeField + "_sell"].AsInt32 };

                double[] inputRow = new double[inputFields.Length];

                for (int i = 0; i < inputFields.Length; i++)
                {
                    double value = doc[inputFields[i]].AsDouble;
                    if (double.IsInfinity(value) || double.IsNegativeInfinity(value) || double.IsNaN(value))
                        throw new Exception("Invalid value!");
                    else
                        inputRow[i] = value;
                }

                inputs[row] = inputRow;

                //Check these! :) ???

                row++;
            }
                
            // Teach the ANN
            for (int iteration = 0; iteration < iterations; iteration++)
            {
                progress.setProgress(name, "Teaching... " + iteration + " of " + iterations);
                double error = teacher.RunEpoch(inputs, outputs);

                if (savePath != null)
                    ann.Save(savePath);
            }

            //Compute Error
            progress.setProgress(name, "Calculating error...");
                
            int successes = 0;
            int fails = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                var realOutput = outputs[i];

                //Buys
                double[] calculated = ann.Compute(inputs[i]);
                if (calculated[0] == 0 || calculated[0] == realOutput[0])
                    successes++;

                if (calculated[0] == 1 && realOutput[0] == 0)
                    fails++;

                //Sells
                if (calculated[1] == 0 || calculated[1] == realOutput[1])
                    successes++;

                if (calculated[1] == 1 && realOutput[1] == 0)
                    fails++;
            }

            double successRate = (double)successes / (inputs.Length * 2);
            double failRate = (double)fails / (inputs.Length * 2);

            progress.setProgress(name, "Finished with successRate of " + successRate + " failRate of " + failRate);
        }

        void IDataminingDatabase.addOutcomeCode(double percentDifference, int outcomeTimeframe, string instrument)
        {
            List<Thread> threads = new List<Thread>();

            long start = database.getFirstTimestamp();
            long end = database.getLastTimestamp();

            long oneThreadTimeframe = (end - start) / threadsCount;
            int threadId = 0;

            var collection = mongodb.getDB().GetCollection("prices");

            string outcomeCodeFieldName = "outcome_code_" + outcomeTimeframe + "_" + percentDifference;

            while (threadId < threadsCount)
            {
                long threadBeginning = start + (oneThreadTimeframe * threadId);
                long threadEnd = threadBeginning + oneThreadTimeframe;

                Thread thread = new Thread(delegate ()
                {

                    string name = "outcome code " + percentDifference + " ID_" + threadBeginning + ":" + threadEnd;
                    progress.setProgress(name, 0);
                    int done = 0;
                    long count = 0;

                    var docs = collection.FindAs<BsonDocument>(Query.And(Query.EQ("instrument", instrument), Query.Exists("outcome_max_" + outcomeTimeframe), Query.NotExists(outcomeCodeFieldName + "_buy"), Query.LT("timestamp", threadEnd), Query.GTE("timestamp", threadBeginning))).SetSortOrder(SortBy.Ascending("timestamp"));
                    docs.SetFlags(QueryFlags.NoCursorTimeout);

                    count = docs.Count();
                    foreach (BsonDocument doc in docs)
                    {
                        done++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(done) / Convert.ToDouble(count) * 100d));

                        try
                        {
                            double onePercent = doc["last"].AsDouble / 100d;

                            double maxDiff = doc["outcome_max_" + outcomeTimeframe].AsDouble / onePercent - 100; //calculate percent difference
                            double minDiff = doc["outcome_min_" + outcomeTimeframe].AsDouble / onePercent - 100;

                            collection.FindAndModify(new FindAndModifyArgs()
                            {
                                Query = Query.EQ("_id", doc["_id"]),
                                Update = Update.Combine(
                                    Update.Set(outcomeCodeFieldName + "_buy", (maxDiff >= percentDifference ? 1 : 0)),
                                    Update.Set(outcomeCodeFieldName + "_sell", (minDiff <= -percentDifference ? 1 : 0))
                                    )
                            });
                        }
                        catch { }
                    }

                    progress.remove(name);

                });

                thread.Start();
                threads.Add(thread);
                
                threadId++;
            }

            waitForThreads(threads);
        }

        void IDataminingDatabase.deleteAll()
        {
            var collection = mongodb.getDB().GetCollection("prices");
            collection.RemoveAll();
        }

        string IDataminingDatabase.getSuccessRate(int outcomeTimeframe, string indicator, double min, double max, string instrument, double tpPercent, double slPercent, bool buy)
        {
            var collection = mongodb.getDB().GetCollection("prices");
            var docs = collection.FindAs<BsonDocument>(
                    Query.And(
                        Query.EQ("instrument", instrument),
                        Query.Exists("outcome_max_" + outcomeTimeframe), Query.Exists("outcome_min_" + outcomeTimeframe), Query.Exists("outcome_actual_" + outcomeTimeframe),
                        Query.LTE(indicator, max), Query.GTE(indicator, min)
                    )
                ).SetSortOrder(SortBy.Ascending("timestamp"));

            int successes = 0;

            double result = 0;

            long count = docs.Count();
            foreach (BsonDocument doc in docs)
            {
                double onePercent = doc["last"].AsDouble / 100d;

                double maxDiff = doc["outcome_max_" + outcomeTimeframe].AsDouble / onePercent - 100; //calculate percent difference
                double minDiff = doc["outcome_min_" + outcomeTimeframe].AsDouble / onePercent - 100;
                double actualDiff = doc["outcome_actual_" + outcomeTimeframe].AsDouble / onePercent - 100;

                if (buy)
                {
                    if (maxDiff >= tpPercent && minDiff > -slPercent) //Kein SL ABER EIN TP -> TP
                    {
                        successes++;
                        result += tpPercent;
                    }
                    else if (minDiff > -slPercent) //Kein SL Kein TP -> Actual
                    {
                        result += actualDiff;
                    }
                    else
                        result -= slPercent; //-> SL

                }
                else
                {
                    if (minDiff <= -tpPercent && maxDiff < slPercent) //Kein SL ABER EIN TP -> TP
                    {
                        successes++;
                        result += tpPercent;
                    }
                    else if (maxDiff < slPercent) //Kein SL Kein TP -> Actual
                    {
                        result -= actualDiff;
                    }
                    else
                        result -= slPercent; //-> SL
                }
            }

            string seperator = "\t";

            double successRate = (Convert.ToDouble(successes) / Convert.ToDouble(count));
            double slTpRatio = tpPercent / slPercent;

            return "outcomeTimeframe:" + outcomeTimeframe + " Indicator:" + indicator + " min:" + min + " max:" + max + " instrument:" + instrument + " tp:" +tpPercent+ " sl:" +slPercent+ " buy:" + buy + Environment.NewLine
                    + "Sucesses" + seperator + "Count" + seperator + "SucessRate" + seperator + "Result" + seperator + "Percent gained" + Environment.NewLine
                    + successes + seperator + count + seperator + successRate + seperator + (successRate * slTpRatio) + seperator + result;
        }

        void IDataminingDatabase.getOutcomeIndicatorSampling(SampleOutcomeExcelGenerator excel, string indicatorId, int outcomeTimeframe, string instument)
        {
            throw new NotImplementedException();
        }

        List<DatasetInfo> IDataminingDatabase.getInfo()
        {
            throw new NotImplementedException();
        }
    }
}
