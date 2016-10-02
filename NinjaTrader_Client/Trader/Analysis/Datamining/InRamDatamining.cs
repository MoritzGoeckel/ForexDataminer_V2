using NinjaTrader_Client.Trader.Datamining;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using NinjaTrader_Client.Trader.Indicators;
using System.Threading;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver;
using NinjaTrader_Client.Trader.Utils;
using System.Data;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System.Runtime.CompilerServices;
using NinjaTrader_Client.Trader.Datamining.AI;
using System.Collections.Concurrent;
using NinjaTrader_Client.Trader.Streaming.Strategies;
using NinjaTrader_Client.Trader.TradingAPIs;
using NinjaTrader_Client.Trader.Backtest;
using NinjaTrader_Client.Trader.Streaming;
using NinjaTrader_Client.Trader.Analysis.IndicatorCollections;
using NinjaTrader_Client.Trader.Analysis.Datamining;

namespace NinjaTrader_Client.Trader
{
    //Todo: way too big!
    public class InRamDatamining
    {
        MongoFacade mongodb;

        int threadsCount = 4;

        ProgressDict progress = new ProgressDict();
        Random z = new Random();

        Dictionary<string, List<AdvancedTickData>> dataInRam = new Dictionary<string, List<AdvancedTickData>>();
        Dictionary<string, PairDataInformation> infoDict = new Dictionary<string, PairDataInformation>();

        int doneOperations = 0;
        DateTime lastChecked = DateTime.Now;

        public InRamDatamining(MongoFacade mongoDbFacade)
        {
            mongodb = mongoDbFacade;
        }

        private static void waitForThreads(List<Thread> threads)
        {
            foreach (Thread thread in threads)
                thread.Join();
        }

        public double getOperationsPerSecond()
        {
            double output = doneOperations / (DateTime.Now.Subtract(lastChecked).TotalSeconds);

            lastChecked = DateTime.Now;
            doneOperations = 0;

            return output;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void doneWriteOperation()
        {
            doneOperations++;
        }

        public void loadPair(string pair)
        {
            if (dataInRam.ContainsKey(pair))
                dataInRam.Remove(pair);

            dataInRam.Add(pair, new List<AdvancedTickData>());

            List<AdvancedTickData> list = dataInRam[pair];

            //To avoid a to small buffer on the db
            mongodb.getDB().GetCollection("pair_" + pair).CreateIndex("timestamp");

            var cursors = mongodb.getDB().GetCollection("pair_" + pair).FindAllAs<AdvancedTickData>().SetSortOrder(SortBy.Ascending("timestamp"));
            double max = cursors.Count();
            double current = 0;

            foreach (AdvancedTickData tickdata in cursors)
            {
                list.Add(tickdata);

                current++;
                doneWriteOperation();

                progress.setProgress("Loading " + pair, Convert.ToInt32((current / max) * 100d));
            }

            progress.remove("Loading " + pair);
        }

        public long loadPair(string pair, long fromTimestamp, int count)
        {
            if (dataInRam.ContainsKey(pair))
                dataInRam.Remove(pair);

            dataInRam.Add(pair, new List<AdvancedTickData>());

            List<AdvancedTickData> list = dataInRam[pair];

            //To avoid a to small buffer on the db
            mongodb.getDB().GetCollection("pair_" + pair).CreateIndex("timestamp");

            var cursors = mongodb.getDB().GetCollection("pair_" + pair).FindAs<AdvancedTickData>(Query.GTE("timestamp", fromTimestamp)).SetSortOrder(SortBy.Ascending("timestamp")).SetLimit(count);
            long max = cursors.Count();
            double current = 0;

            foreach (AdvancedTickData tickdata in cursors)
            {
                list.Add(tickdata);

                current++;
                doneWriteOperation();

                progress.setProgress("Loading " + pair, Convert.ToInt32((current / Convert.ToDouble(max)) * 100d));
            }

            progress.remove("Loading " + pair);

            return max;
        }

        public void reduceData(string pair, int count)
        {
            dataInRam[pair] = getSomeSamplesFromData(pair, count, 0);
        }

        public List<AdvancedTickData> getSomeSamplesFromData(string pair, int count, int offset)
        {
            List<AdvancedTickData> list = dataInRam[pair];
            double stepSize = Convert.ToDouble(list.Count()) / Convert.ToDouble(count);

            List<AdvancedTickData> selectedSamples = new List<AdvancedTickData>();

            int i = 0;
            while (i < count)
            {
                int id = Convert.ToInt32(i * stepSize) + offset;

                if (id < list.Count && id >= 0)
                    selectedSamples.Add(list[id]);

                i++;
            }

            return selectedSamples;
        }

        //Todo: Something xD
        public void updateInfo(string pair, int maxDatasets = 150 * 1000)
        {
            PairDataInformation info = new PairDataInformation(pair);
            List<AdvancedTickData> list = dataInRam[pair];

            double stepSize = Convert.ToDouble(list.Count()) / Convert.ToDouble(maxDatasets);

            int i = 0;
            while (i < maxDatasets)
            {
                info.checkTickdata(list[Convert.ToInt32(i * stepSize)]);
                doneWriteOperation();
                i++;
            }

            info.AllDatasets = list.Count();

            if (infoDict.ContainsKey(pair))
                infoDict.Remove(pair);
            
            infoDict.Add(pair, info);
        }

        public void unloadPair(string pair)
        {
            if(dataInRam.ContainsKey(pair))
                dataInRam.Remove(pair);
        }

        public void savePair(string instrument)
        {
            List<AdvancedTickData> inRamList = dataInRam[instrument];

            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            while (threadId < threadsCount)
            {
                Thread thread = new Thread(delegate (object actualThreadId)
                {
                    int indexBeginning = start + (indexFrame * Convert.ToInt32(actualThreadId));
                    int indexEnd = indexBeginning + indexFrame;

                    string name = "saving " + instrument + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        AdvancedTickData currentTickdata = inRamList[currentId];

                        if(currentTickdata.changed == true)
                        {
                            mongodb.getDB().GetCollection("pair_" + instrument).Update(Query.EQ("_id", currentTickdata._id), Update.Replace(currentTickdata.ToBsonDocument()));
                            currentTickdata.changed = false;
                        }

                        doneWriteOperation();
                        currentId++;
                    }

                    progress.remove(name);

                });

                thread.Start(threadId);
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        public void deleteAll()
        {
            foreach(string collectionName in mongodb.getDB().GetCollectionNames())
            {
                mongodb.getDB().GetCollection(collectionName).RemoveAll();
            }

            foreach(KeyValuePair<string, List<AdvancedTickData>> pair in dataInRam)
            {
                pair.Value.Clear();
            }

            dataInRam.Clear();
        }

        public Dictionary<string, PairDataInformation> getInfo()
        {
            return infoDict;
        }

        public List<string> getLoadedPairs()
        {
            List<string> pairs = new List<string>();
            foreach (KeyValuePair<string, List<AdvancedTickData>> pair in dataInRam)
                pairs.Add(pair.Key);

            return pairs;
        }

        public void importPair(string pair, long start, long end, SQLiteDatabase otherDatabase)
        {
            List<Thread> threads = new List<Thread>();

            var collection = mongodb.getDB().GetCollection("pair_" + pair);
            collection.CreateIndex("timestamp");

            long timeframe = Convert.ToInt64(Convert.ToDouble(end - start) / Convert.ToDouble(threadsCount));
            int threadId = 0;

            while (threadId < threadsCount)
            {
                Thread thread = new Thread(delegate (object actualThreadId)
                {
                    long threadBeginning = start + timeframe * Convert.ToInt32(actualThreadId);
                    long threadEnd = threadBeginning + timeframe;

                    string name = "import " + " ID_" + threadBeginning + ":" + threadEnd;
                    progress.setProgress(name, 0);
                    int done = 0;

                    List<TickData> data = otherDatabase.getPrices(threadBeginning, threadEnd, pair);
                    long count = data.Count();

                    foreach (TickData d in data)
                    {
                        collection.Insert(d.toAdvancedTickData().ToBsonDocument());

                        done++;
                        doneWriteOperation();
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(done) / Convert.ToDouble(count) * 100d));
                    }

                    progress.remove(name);
                });

                thread.Start(threadId);
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        public void addOutcome(long timeframe, string instrument)
        {                       
            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            //Starte threads mit unterschiedlichen bereichen
            while (threadId < threadsCount)
            {
                Thread thread = new Thread(delegate (object actualThreadId)
                {
                    List<AdvancedTickData> dataInTimeframe = new List<AdvancedTickData>();
                    double min = double.MaxValue, max = double.MinValue;
                    
                    int indexBeginning = start + (indexFrame * Convert.ToInt32(actualThreadId));
                    int indexEnd = indexBeginning + indexFrame;

                    int outcomeIndex = indexBeginning;
                    
                    string name = "outcome " + timeframe + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    List<AdvancedTickData> inBetweenData = new List<AdvancedTickData>();

                    //Durchlaufe IDs in ThreadFrame
                    while (currentId <= indexEnd)
                    {
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        AdvancedTickData currentTickdata = dataInRam[instrument][currentId];

                        //Wenn Daten noch nicht da sind
                        if (currentTickdata.values.ContainsKey("outcomeMin_" + timeframe) == false)
                        {
                            bool hasToUpdateMinMax = false;
                            
                            //Zu alte entfernen
                            while (inBetweenData.Count > 0 && inBetweenData[0].timestamp < currentTickdata.timestamp)
                            {
                                if (inBetweenData[0].values["mid"] == max || inBetweenData[0].values["mid"] == min)
                                    hasToUpdateMinMax = true;

                                inBetweenData.RemoveAt(0);
                            }

                            //Wenn min oder max entfernt, neu durchgehen
                            if(hasToUpdateMinMax)
                            {
                                min = double.MaxValue;
                                max = double.MinValue;

                                foreach (AdvancedTickData data in inBetweenData)
                                {
                                    double mid = data.values["mid"];
                                    if (mid > max)
                                        max = mid;

                                    if (mid < min)
                                        min = mid;
                                }
                            }
                        
                            //Neue hinzufügen
                            while (outcomeIndex < dataInRam[instrument].Count() && (dataInRam[instrument][outcomeIndex].timestamp - currentTickdata.timestamp) < timeframe)
                            {
                                outcomeIndex++;
                                double mid = dataInRam[instrument][outcomeIndex].values["mid"];
                                if (mid > max)
                                    max = mid;

                                if (mid < min)
                                    min = mid;

                                inBetweenData.Add(dataInRam[instrument][outcomeIndex]);
                            }

                            //Das actual outcome
                            AdvancedTickData outcomeData = dataInRam[instrument][outcomeIndex];
                                
                            currentTickdata.values.Add("outcomeMin_" + timeframe, min);
                            currentTickdata.values.Add("outcomeMax_" + timeframe, max);
                            currentTickdata.values.Add("outcomeActual_" + timeframe, outcomeData.values["mid"]);
                            currentTickdata.changed = true;

                            doneWriteOperation();
                        }
                        currentId++;
                    }

                    progress.remove(name);

                });

                thread.Start(threadId);
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        public void removeIndicator(string indicatorID, string instrument)
        {
            List<AdvancedTickData> inRamList = dataInRam[instrument];

            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            while (threadId < threadsCount)
            {
                Thread thread = new Thread(delegate (object actualThreadId)
                {
                    int indexBeginning = start + (indexFrame * Convert.ToInt32(actualThreadId));
                    int indexEnd = indexBeginning + indexFrame;

                    string name = "Remove Indicator " + indicatorID + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        currentId++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        AdvancedTickData currentTickdata = inRamList[currentId];

                        if (currentTickdata.values.ContainsKey(indicatorID) == false)
                        {
                            currentTickdata.values.Remove(indicatorID);
                            currentTickdata.changed = true;

                            doneWriteOperation();
                        }
                    }

                    progress.remove(name);

                });

                thread.Start(threadId);
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        public void addData(string dataname, SQLiteDatabase database, string instrument)
        {
            List<AdvancedTickData> inRamList = dataInRam[instrument];
            
            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            while (threadId < threadsCount)
            {
                Thread thread = new Thread(delegate (object actualThreadId)
                {
                    int indexBeginning = start + (indexFrame * Convert.ToInt32(actualThreadId));
                    int indexEnd = indexBeginning + indexFrame;

                    string name = "data " + dataname + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        currentId++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        AdvancedTickData currentTickdata = inRamList[currentId];
                        
                        if (currentTickdata.values.ContainsKey(dataname) == false)
                        {
                            TimeValueData data = database.getData(currentTickdata.timestamp, dataname, instrument);

                            if (data != null)
                            {
                                currentTickdata.values.Add(dataname, data.value);
                                currentTickdata.changed = true;

                                doneWriteOperation();
                            }
                        }
                    }

                    progress.remove(name);

                });

                thread.Start(threadId);
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        public class OutcomeCountPair
        {
            public double MinSum;
            public double MaxSum;
            public double ActualSum;
            public double Count;
        };
        
        public double getOutcomeIndicatorSampling(SampleOutcomeExcelGenerator excel, string indicatorId, int outcomeTimeframe, int steps, DistributionRange samplingRange, string instrument)
        {
            ConcurrentDictionary<double, OutcomeCountPair> valueCounts = new ConcurrentDictionary<double, OutcomeCountPair>();

            List<AdvancedTickData> inRamList = dataInRam[instrument];

            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            double stepSize = samplingRange.max - samplingRange.min / Convert.ToDouble(steps);
            
            while (threadId < threadsCount)
            {
                Thread thread = new Thread(delegate (object actualThreadId)
                {
                    int indexBeginning = start + (indexFrame * Convert.ToInt32(actualThreadId));
                    int indexEnd = indexBeginning + indexFrame;

                    string name = "indicator sampling " + indicatorId + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        AdvancedTickData currentTickdata = inRamList[currentId];
                        if (currentTickdata.values.ContainsKey(indicatorId) 
                            && currentTickdata.values[indicatorId] >= samplingRange.min
                            && currentTickdata.values[indicatorId] <= samplingRange.max
                            && currentTickdata.values.ContainsKey("outcomeMin_" + outcomeTimeframe)
                            && currentTickdata.values.ContainsKey("outcomeMax_" + outcomeTimeframe)
                            && currentTickdata.values.ContainsKey("outcomeActual_" + outcomeTimeframe))
                        {
                            double indicatorKey = Math.Floor(currentTickdata.values[indicatorId] / stepSize) * stepSize;

                            if (valueCounts.ContainsKey(indicatorKey) == false)
                            {
                                OutcomeCountPair pair = new OutcomeCountPair();
                                pair.MaxSum = currentTickdata.values["outcomeMax_" + outcomeTimeframe];
                                pair.MinSum = currentTickdata.values["outcomeMin_" + outcomeTimeframe];
                                pair.ActualSum = currentTickdata.values["outcomeActual_" + outcomeTimeframe];
                                pair.Count = 1;

                                valueCounts.TryAdd(indicatorKey, pair);
                            }
                            else
                            {
                                valueCounts[indicatorKey].Count++;

                                valueCounts[indicatorKey].MinSum += currentTickdata.values["outcomeMin_" + outcomeTimeframe];
                                valueCounts[indicatorKey].MaxSum += currentTickdata.values["outcomeMax_" + outcomeTimeframe];
                                valueCounts[indicatorKey].ActualSum += currentTickdata.values["outcomeActual_" + outcomeTimeframe];

                                doneWriteOperation();
                            }
                        }

                        currentId++;
                    }

                    progress.remove(name);

                });

                thread.Start(threadId);
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);

            if (excel != null)
            {
                string sheetName = indicatorId + "_" + (outcomeTimeframe / 1000 / 60) + "_" + instrument;

                if (sheetName.Length >= 30)
                    sheetName = sheetName.Substring(0, 29);

                excel.CreateSheet(sheetName);

                foreach (KeyValuePair<double, OutcomeCountPair> pair in valueCounts)
                {
                    double maxAvg = pair.Value.MaxSum / pair.Value.Count;
                    double minAvg = pair.Value.MinSum / pair.Value.Count;
                    double actualAvg = pair.Value.ActualSum / pair.Value.Count;

                    double minVsMax = pair.Value.MinSum + pair.Value.MaxSum / pair.Value.Count;

                    excel.addRow(sheetName, pair.Key, pair.Key + stepSize, Convert.ToInt32(pair.Value.Count), maxAvg, minAvg, minVsMax, actualAvg, (maxAvg + minAvg));

                    doneWriteOperation();
                }

                excel.FinishSheet(sheetName);
            }

            return PredictivePowerAnalyzer.getPredictivePower(valueCounts);
        }

        public class OutcomeCodeCountPair
        {
            public double buySum = 0;
            public double sellSum = 0;
            public double Count = 0;
        };

        public double getOutcomeCodeIndicatorSampling(SampleOutcomeCodeExcelGenerator excel, string indicatorId, int steps, DistributionRange samplingRange, double normalizedDifference, int outcomeTimeframe, string instrument)
        {
            ConcurrentDictionary<double, OutcomeCodeCountPair> valueCounts = new ConcurrentDictionary<double, OutcomeCodeCountPair>();

            List<AdvancedTickData> inRamList = dataInRam[instrument];

            List<Thread> threads = new List<Thread>();

            string outcomeCodeFieldName = "outcomeCode-" + normalizedDifference + "_" + outcomeTimeframe;
            
            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            double stepSize = samplingRange.max - samplingRange.min / Convert.ToDouble(steps);
            
            while (threadId < threadsCount)
            {
                Thread thread = new Thread(delegate (object actualThreadId)
                {
                    int indexBeginning = start + (indexFrame * Convert.ToInt32(actualThreadId));
                    int indexEnd = indexBeginning + indexFrame;

                    string name = "indicator sampling " + indicatorId + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        AdvancedTickData currentTickdata = inRamList[currentId];
                        if (currentTickdata.values.ContainsKey(indicatorId) 
                        && currentTickdata.values[indicatorId] >= samplingRange.min 
                        && currentTickdata.values[indicatorId] <= samplingRange.max 
                        && currentTickdata.values.ContainsKey("sell-" + outcomeCodeFieldName) 
                        && currentTickdata.values.ContainsKey("buy-" + outcomeCodeFieldName))
                        {
                            double indicatorKey = Math.Floor(currentTickdata.values[indicatorId] / stepSize) * stepSize;

                            if (valueCounts.ContainsKey(indicatorKey) == false)
                            {
                                OutcomeCodeCountPair pair = new OutcomeCodeCountPair();
                                pair.buySum += currentTickdata.values["buy-" + outcomeCodeFieldName];
                                pair.sellSum += currentTickdata.values["sell-" + outcomeCodeFieldName];
                                pair.Count = 1;

                                valueCounts.TryAdd(indicatorKey, pair);
                            }
                            else
                            {
                                valueCounts[indicatorKey].Count++;

                                valueCounts[indicatorKey].buySum += currentTickdata.values["buy-" + outcomeCodeFieldName];
                                valueCounts[indicatorKey].sellSum += currentTickdata.values["sell-" + outcomeCodeFieldName];
                                valueCounts[indicatorKey].Count++;

                                doneWriteOperation();
                            }
                        }

                        currentId++;
                    }

                    progress.remove(name);

                });

                thread.Start(threadId);
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);

            if (excel != null)
            {
                string sheetName = "OutcomeCode_" + indicatorId + "_" + (outcomeTimeframe / 1000 / 60) + "_" + instrument;

                if (sheetName.Length >= 30)
                    sheetName = sheetName.Substring(0, 29);

                excel.CreateSheet(sheetName);

                foreach (KeyValuePair<double, OutcomeCodeCountPair> pair in valueCounts)
                {
                    double buyAvg = pair.Value.buySum / pair.Value.Count;
                    double sellAvg = pair.Value.sellSum / pair.Value.Count;

                    excel.addRow(sheetName, pair.Key, pair.Key + stepSize, Convert.ToInt32(pair.Value.Count), buyAvg, sellAvg);

                    doneWriteOperation();
                }

                excel.FinishSheet(sheetName);
            }

            return PredictivePowerAnalyzer.getPredictivePower(valueCounts);
        }

        public ProgressDict getProgress()
        {
            return progress;
        }

        public void addIndicator(WalkerIndicator indicator, string instrument, string fieldId)
        {
            string name = "Indicator " + indicator.getName() + " " + instrument + " " + fieldId;
            progress.setProgress(name, 0);
            int done = 0;

            string indicatorID = fieldId + "-" + indicator.getName();

            foreach (AdvancedTickData currentTickdata in dataInRam[instrument])
            {
                progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(done) / Convert.ToDouble(dataInRam[instrument].Count()) * 100d));
                done++;

                indicator.setNextData(currentTickdata.timestamp, currentTickdata.values[fieldId]);

                if (currentTickdata.values.ContainsKey(indicatorID) == false)
                {
                    if (indicator.isValid(currentTickdata.timestamp))
                    {
                        currentTickdata.values.Add(indicatorID, indicator.getIndicator().value);
                        currentTickdata.changed = true;
                    }

                    doneWriteOperation();
                }
            }

            progress.remove(name);
        }

        public void addMetaIndicatorSum(string[] ids, double[] weights, string fieldName, string instrument)
        {
            List<AdvancedTickData> inRamList = dataInRam[instrument];

            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            while (threadId < threadsCount)
            {
                Thread thread = new Thread(delegate (object actualThreadId)
                {
                    int indexBeginning = start + (indexFrame * Convert.ToInt32(actualThreadId));
                    int indexEnd = indexBeginning + indexFrame;

                    string name = "indicator sum " + fieldName + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        AdvancedTickData currentTickdata = inRamList[currentId];

                        if (currentTickdata.values.ContainsKey(fieldName) == false)
                        {
                            double value = 0;
                            for (int i = 0; i < ids.Length; i++)
                                value += currentTickdata.values[ids[i]] * weights[i];

                            currentTickdata.values.Add(fieldName, value);
                            currentTickdata.changed = true;

                            doneWriteOperation();
                        }

                        currentId++;
                    }

                    progress.remove(name);

                });

                thread.Start(threadId);
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        public void addOutcomeCode(double normalizedDifference, int outcomeTimeframe, string instrument)
        {
            string outcomeCodeFieldName = "outcomeCode-" + normalizedDifference + "_" + outcomeTimeframe;
            
            List<AdvancedTickData> inRamList = dataInRam[instrument];

            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            while (threadId < threadsCount)
            {
                Thread thread = new Thread(delegate (object actualThreadId)
                {
                    int indexBeginning = start + (indexFrame * Convert.ToInt32(actualThreadId));
                    int indexEnd = indexBeginning + indexFrame;

                    string name = "add outcome code " + outcomeCodeFieldName + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        AdvancedTickData currentTickdata = inRamList[currentId];

                        if (currentTickdata.values.ContainsKey("buy-" + outcomeCodeFieldName) == false 
                        && currentTickdata.values.ContainsKey("outcomeMax_" + outcomeTimeframe) 
                        && currentTickdata.values.ContainsKey("outcomeMin_" + outcomeTimeframe))
                        {
                            double maxDiff = currentTickdata.values["outcomeMax_" + outcomeTimeframe] / currentTickdata.values["mid"] - 1;
                            double minDiff = currentTickdata.values["outcomeMin_" + outcomeTimeframe] / currentTickdata.values["mid"] - 1;

                            currentTickdata.values.Add("buy-" + outcomeCodeFieldName, (maxDiff >= normalizedDifference ? 1 : 0));
                            currentTickdata.values.Add("sell-" + outcomeCodeFieldName, (minDiff <= -normalizedDifference ? 1 : 0));
                            currentTickdata.changed = true;

                            doneWriteOperation();
                        }

                        currentId++;
                    }

                    progress.remove(name);
                });

                thread.Start(threadId);
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        public string getOutcomeCodeDistribution(double normalizedDifference, int outcomeTimeframe, string instrument)
        {
            string outcomeCodeFieldName = "outcomeCode-" + normalizedDifference + "_" + outcomeTimeframe;

            List<AdvancedTickData> inRamList = dataInRam[instrument];

            double datasets = 0;
            double buys = 0;
            double sells = 0;

            foreach(AdvancedTickData data in inRamList)
            {
                if(data.values.ContainsKey("buy-" + outcomeCodeFieldName))
                {
                    datasets++;
                    buys += Convert.ToInt32(data.values["buy-" + outcomeCodeFieldName]);
                    sells += Convert.ToInt32(data.values["sell-" + outcomeCodeFieldName]);

                }
            }

            double buyRatio = buys / datasets;
            double sellRatio = sells / datasets;

            return "b:" + buyRatio + " s:" + sellRatio;
        }

        //Not tested ???
        public string getSuccessRate(int outcomeTimeframe, string indicator, double min, double max, string instrument, double tpPercent, double slPercent, bool buy)
        {
            List<AdvancedTickData> inRamList = dataInRam[instrument];

            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            int successes = 0;
            double result = 0;

            while (threadId < threadsCount)
            {
                Thread thread = new Thread(delegate (object actualThreadId)
                {
                    int indexBeginning = start + (indexFrame * Convert.ToInt32(actualThreadId));
                    int indexEnd = indexBeginning + indexFrame;

                    string name = "successrate ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));
                        doneWriteOperation();

                        AdvancedTickData currentTickdata = inRamList[currentId];

                        try
                        {
                            double onePercent = currentTickdata.values["mid"] / 100d;

                            double maxDiff = currentTickdata.values["outcomeMax_" + outcomeTimeframe] / onePercent - 100; //calculate percent difference
                            double minDiff = currentTickdata.values["outcomeMin_" + outcomeTimeframe] / onePercent - 100;
                            double actualDiff = currentTickdata.values["outcomeActual_" + outcomeTimeframe] / onePercent - 100;

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
                        catch { }

                        currentId++;
                    }

                    progress.remove(name);

                });

                thread.Start(threadId);
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);

            long count = inRamList.Count();
            
            string seperator = "\t";

            double successRate = (Convert.ToDouble(successes) / Convert.ToDouble(count));
            double slTpRatio = tpPercent / slPercent;

            return "outcomeTimeframeSeconds:" + outcomeTimeframe + " Indicator:" + indicator + " min:" + min + " max:" + max + " instrument:" + instrument + " tp:" + tpPercent + " sl:" + slPercent + " buy:" + buy + Environment.NewLine
                    + "Sucesses" + seperator + "Count" + seperator + "SucessRate" + seperator + "Result" + seperator + "Percent gained" + Environment.NewLine
                    + successes + seperator + count + seperator + successRate + seperator + (successRate * slTpRatio) + seperator + result;
        }

        public void getInputOutputArrays(string[] inputFields, string outcomeField, string instrument, ref double[][] inputs, ref double[][] outputs, int dataCountReduction = 0, int offsetReduction = 0)
        {
            progress.setProgress("Creating input/output array", 0);

            List<AdvancedTickData> dataCollection;

            if (dataCountReduction == 0)
                dataCollection = dataInRam[instrument];
            else
                dataCollection = getSomeSamplesFromData(instrument, dataCountReduction, offsetReduction);

            double dataCount = dataCollection.Count();
            double doneData = 0;

            List<double[]> inputsList = new List<double[]>();
            List<double[]> outputsList = new List<double[]>();

            foreach (AdvancedTickData data in dataCollection)
            {
                bool validTick = true;
                double[] input = new double[inputFields.Length];

                if (data.values.ContainsKey("sell-" + outcomeField) == false || data.values.ContainsKey("buy-" + outcomeField) == false)
                    continue;

                int i = 0;
                foreach(string inputField in inputFields)
                {
                    if (data.values.ContainsKey(inputField))
                        input[i] = data.values[inputField];
                    else
                    {
                        validTick = false;
                        break;
                    }

                    i++;
                }

                if (validTick)
                {
                    try {
                        inputsList.Add(input);
                        outputsList.Add(new double[] { data.values["buy-" + outcomeField], data.values["sell-" + outcomeField] });

                        doneWriteOperation();
                    }
                    catch { }
                }

                doneData++;

                progress.setProgress("Creating input/output array", Convert.ToInt32(doneData / dataCount * 100d));
            }

            inputs = inputsList.ToArray();
            inputsList.Clear();

            outputs = outputsList.ToArray();
            outputsList.Clear();

            progress.remove("Creating input/output array");
        }

        public long getLastTimestamp(string pair)
        {
            return dataInRam[pair][dataInRam[pair].Count - 1].timestamp;
        }

        public void backtestStreaming(string pair, Strategy strategy, ExecutionStrategy execStrat, IndicatorCollection indicators, GeneralExcelGenerator excel)
        {
            FakeTradingAPI api = new FakeTradingAPI();
            StreamingModul tradingStreamProcessor = new StreamingModul(indicators, strategy, execStrat, api, pair);

            foreach (AdvancedTickData data in dataInRam[pair])
            {
                api.setPair(data.ToTickdata());
                tradingStreamProcessor.pushDataAndTrade(data.ToTickdata());
            }

            api.closePositions(pair);
            BacktestData result = new BacktestData(0, pair, strategy.getName());
            result.setPositions(api.getHistory(pair));
            result.setParameter(strategy.getParameters());
            result.setResult(strategy.getResult());

            if (excel != null)
            {
                string sheetName = "Backtest-" + strategy.getName();
                if (excel.doesSheetExist(sheetName) == false)
                    excel.CreateSheet(sheetName, result.getExcelHeader());

                excel.addRow(sheetName, result.getExcelRow());
            }
        }
    }
}
