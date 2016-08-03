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

namespace NinjaTrader_Client.Trader
{
    public class InRamDatamining
    {
        MongoFacade mongodb;

        int threadsCount = 20;

        ProgressDict progress = new ProgressDict();
        Random z = new Random();

        Dictionary<string, List<DataminingTickdata>> dataInRam = new Dictionary<string, List<DataminingTickdata>>();
        Dictionary<string, DataminingPairInformation> infoDict = new Dictionary<string, DataminingPairInformation>();

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

            DataminingPairInformation info = new DataminingPairInformation(pair);

            dataInRam.Add(pair, new List<DataminingTickdata>());

            List<DataminingTickdata> list = dataInRam[pair];

            //To avoid a to small buffer on the db
            mongodb.getDB().GetCollection("pair_" + pair).CreateIndex("timestamp");

            var cursors = mongodb.getDB().GetCollection("pair_" + pair).FindAllAs<DataminingTickdata>().SetSortOrder(SortBy.Ascending("timestamp"));
            double max = cursors.Count();
            double current = 0;

            foreach (DataminingTickdata tickdata in cursors)
            {
                list.Add(tickdata);

                info.checkTickdata(tickdata);

                current++;
                doneWriteOperation();

                progress.setProgress("Loading " + pair, Convert.ToInt32((current / max) * 100d));
            }

            if (infoDict.ContainsKey(pair))
                infoDict.Remove(pair);

            info.AllDatasets = list.Count();
            infoDict.Add(pair, info);

            progress.remove("Loading " + pair);
        }

        public void updateInfo(string pair, int maxDatasets = 10 * 1000)
        {
            DataminingPairInformation info = new DataminingPairInformation(pair);
            List<DataminingTickdata> list = dataInRam[pair];

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
            dataInRam.Remove(pair);
        }

        public void savePair(string instrument)
        {
            List<DataminingTickdata> inRamList = dataInRam[instrument];

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

                        DataminingTickdata currentTickdata = inRamList[currentId];

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

            foreach(KeyValuePair<string, List<DataminingTickdata>> pair in dataInRam)
            {
                pair.Value.Clear();
            }

            dataInRam.Clear();
        }

        public Dictionary<string, DataminingPairInformation> getInfo()
        {
            return infoDict;
        }

        public List<string> getLoadedPairs()
        {
            List<string> pairs = new List<string>();
            foreach (KeyValuePair<string, List<DataminingTickdata>> pair in dataInRam)
                pairs.Add(pair.Key);

            return pairs;
        }

        public void importPair(string pair, long start, long end, Database otherDatabase)
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

                    List<Tickdata> data = otherDatabase.getPrices(threadBeginning, threadEnd, pair);
                    long count = data.Count();

                    foreach (Tickdata d in data)
                    {
                        collection.Insert(d.ToDataminingTickdata().ToBsonDocument());

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
                    List<DataminingTickdata> dataInTimeframe = new List<DataminingTickdata>();
                    double min = double.MaxValue, max = double.MinValue;
                    
                    int indexBeginning = start + (indexFrame * Convert.ToInt32(actualThreadId));
                    int indexEnd = indexBeginning + indexFrame;

                    int outcomeIndex = indexBeginning;
                    
                    string name = "outcome " + timeframe + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    List<DataminingTickdata> inBetweenData = new List<DataminingTickdata>();

                    //Durchlaufe IDs in ThreadFrame
                    while (currentId <= indexEnd)
                    {
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        DataminingTickdata currentTickdata = dataInRam[instrument][currentId];

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

                                foreach (DataminingTickdata data in inBetweenData)
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
                            DataminingTickdata outcomeData = dataInRam[instrument][outcomeIndex];
                                
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

        public void addData(string dataname, Database database, string instrument)
        {
            List<DataminingTickdata> inRamList = dataInRam[instrument];
            
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

                        DataminingTickdata currentTickdata = inRamList[currentId];
                        
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

        private class OutcomeCountPair
        {
            public double MinSum;
            public double MaxSum;
            public double ActualSum;
            public double Count;
        };

        //Todo: Excel reporting
        public void getOutcomeIndicatorSampling(DataminingExcelGenerator excel, string indicatorId, int outcomeTimeframeSeconds, double stepSize, string instrument)
        {
            ConcurrentDictionary<double, OutcomeCountPair> valueCounts = new ConcurrentDictionary<double, OutcomeCountPair>();

            List<DataminingTickdata> inRamList = dataInRam[instrument];

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

                    string name = "indicator sampling " + indicatorId + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        DataminingTickdata currentTickdata = inRamList[currentId];
                        if (currentTickdata.values.ContainsKey(indicatorId) 
                            && currentTickdata.values.ContainsKey("outcomeMin_" + outcomeTimeframeSeconds)
                            && currentTickdata.values.ContainsKey("outcomeMax_" + outcomeTimeframeSeconds)
                            && currentTickdata.values.ContainsKey("outcomeActual_" + outcomeTimeframeSeconds))
                        {
                            double indicatorKey = Math.Floor(currentTickdata.values[indicatorId] / stepSize) * stepSize;

                            if (valueCounts.ContainsKey(indicatorKey) == false)
                            {
                                OutcomeCountPair pair = new OutcomeCountPair();
                                pair.MaxSum = currentTickdata.values["outcomeMax_" + outcomeTimeframeSeconds];
                                pair.MinSum = currentTickdata.values["outcomeMin_" + outcomeTimeframeSeconds];
                                pair.ActualSum = currentTickdata.values["outcomeActual_" + outcomeTimeframeSeconds];
                                pair.Count = 1;

                                valueCounts.TryAdd(indicatorKey, pair);
                            }
                            else
                            {
                                valueCounts[indicatorKey].Count++;

                                valueCounts[indicatorKey].MinSum += currentTickdata.values["outcomeMin_" + outcomeTimeframeSeconds];
                                valueCounts[indicatorKey].MaxSum += currentTickdata.values["outcomeMax_" + outcomeTimeframeSeconds];
                                valueCounts[indicatorKey].ActualSum += currentTickdata.values["outcomeActual_" + outcomeTimeframeSeconds];

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

            //Excel sheet...

            string sheetName = indicatorId + "_" + (outcomeTimeframeSeconds / 1000 / 60) + "_" + instrument;

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

            foreach (DataminingTickdata currentTickdata in dataInRam[instrument])
            {
                progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(done) / Convert.ToDouble(dataInRam[instrument].Count()) * 100d));
                done++;

                indicator.setNextData(currentTickdata.timestamp, currentTickdata.values[fieldId]);

                if (currentTickdata.values.ContainsKey(indicatorID) == false)
                {
                    currentTickdata.values.Add(indicatorID, indicator.getIndicator().value);
                    currentTickdata.changed = true;

                    doneWriteOperation();
                }
            }

            progress.remove(name);
        }

        public void addMetaIndicatorSum(string[] ids, double[] weights, string fieldName, string instrument)
        {
            List<DataminingTickdata> inRamList = dataInRam[instrument];

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

                        DataminingTickdata currentTickdata = inRamList[currentId];

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
            
            List<DataminingTickdata> inRamList = dataInRam[instrument];

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

                        DataminingTickdata currentTickdata = inRamList[currentId];

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

        //Not tested ???
        public string getSuccessRate(int outcomeTimeframeSeconds, string indicator, double min, double max, string instrument, double tpPercent, double slPercent, bool buy)
        {
            List<DataminingTickdata> inRamList = dataInRam[instrument];

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

                        DataminingTickdata currentTickdata = inRamList[currentId];

                        try
                        {
                            double onePercent = currentTickdata.values["mid"] / 100d;

                            double maxDiff = currentTickdata.values["outcomeMax_" + outcomeTimeframeSeconds] / onePercent - 100; //calculate percent difference
                            double minDiff = currentTickdata.values["outcomeMin_" + outcomeTimeframeSeconds] / onePercent - 100;
                            double actualDiff = currentTickdata.values["outcomeActual_" + outcomeTimeframeSeconds] / onePercent - 100;

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

            return "outcomeTimeframeSeconds:" + outcomeTimeframeSeconds + " Indicator:" + indicator + " min:" + min + " max:" + max + " instrument:" + instrument + " tp:" + tpPercent + " sl:" + slPercent + " buy:" + buy + Environment.NewLine
                    + "Sucesses" + seperator + "Count" + seperator + "SucessRate" + seperator + "Result" + seperator + "Percent gained" + Environment.NewLine
                    + successes + seperator + count + seperator + successRate + seperator + (successRate * slTpRatio) + seperator + result;
        }

        public void getCorrelation(string indicatorId, int outcomeTimeframe, CorrelationCondition condition)
        {
            throw new NotImplementedException();
        }

        public void getCorrelationTable()
        {
            throw new NotImplementedException();
        }

        public void addDataToLearningComponent(string[] inputFields, string outcomeField, string instrument, IMachineLearning learningComponent)
        {
            progress.setProgress("Training", 0);

            double dataCount = dataInRam[instrument].Count();
            double doneData = 0;

            foreach (DataminingTickdata data in dataInRam[instrument])
            {
                bool validTick = true;
                double[] input = new double[inputFields.Length];

                if (data.values.ContainsKey(outcomeField) == false)
                    continue;

                double outcome = data.values[outcomeField];

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
                    learningComponent.addData(input, outcome);
                    doneWriteOperation();
                }

                doneData++;

                progress.setProgress("Training", Convert.ToInt32(doneData / dataCount * 100d));
            }

            progress.remove("Training");
        }
    }
}
