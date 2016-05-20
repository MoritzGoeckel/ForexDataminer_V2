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

namespace NinjaTrader_Client.Trader
{
    public class InRamDatamining : DataminingDatabase
    {
        MongoFacade mongodb;

        int threadsCount = 20;

        ProgressDict progress = new ProgressDict();
        Random z = new Random();

        Dictionary<string, List<DataminingTickdata>> dataInRam = new Dictionary<string, List<DataminingTickdata>>();

        public InRamDatamining(MongoFacade mongoDbFacade)
        {
            mongodb = mongoDbFacade;
        }

        private static void waitForThreads(List<Thread> threads)
        {
            foreach (Thread thread in threads)
                thread.Join();
        }

        public void loadPair(string pair)
        {
            if (dataInRam.ContainsKey(pair))
                dataInRam.Remove(pair);

            dataInRam.Add(pair, new List<DataminingTickdata>());

            List<DataminingTickdata> list = dataInRam[pair];

            var cursors = mongodb.getDB().GetCollection("pair_" + pair).FindAllAs<DataminingTickdata>().SetSortOrder(SortBy.Ascending("timestamp"));
            double max = cursors.Count();
            double current = 0;

            foreach (DataminingTickdata tickdata in cursors)
            {
                list.Add(tickdata);
                current++;

                progress.setProgress("Loading " + pair, Convert.ToInt32((max / current) * 100d));
            }

            progress.remove("Loading " + pair);
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
                int indexBeginning = start + (indexFrame * threadId);
                int indexEnd = indexBeginning + indexFrame;

                Thread thread = new Thread(delegate ()
                {
                    string name = "saving " + instrument + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        currentId++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        DataminingTickdata currentTickdata = inRamList[currentId];

                        if(currentTickdata.changed == true)
                        {
                            mongodb.getDB().GetCollection("pair_" + instrument).Update(Query.EQ("_id", currentTickdata._id), Update.Replace(currentTickdata.ToBsonDocument()));
                            currentTickdata.changed = false;
                        }
                    }

                    progress.remove(name);

                });

                thread.Start();
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        void DataminingDatabase.deleteAll()
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

        string DataminingDatabase.getInfo()
        {
            throw new NotImplementedException();
        }

        public List<string> getLoadedPairs()
        {
            List<string> pairs = new List<string>();
            foreach (KeyValuePair<string, List<DataminingTickdata>> pair in dataInRam)
                pairs.Add(pair.Key);

            return pairs;
        }

        void DataminingDatabase.importPair(string pair, long start, long end, Database otherDatabase)
        {
            List<Thread> threads = new List<Thread>();

            var collection = mongodb.getDB().GetCollection("pair_" + pair);

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

                    List<Tickdata> data = otherDatabase.getPrices(threadBeginning, threadEnd, pair);
                    long count = data.Count();

                    foreach (Tickdata d in data)
                    {
                        collection.Insert(d.ToBsonDocument());

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

        void DataminingDatabase.addOutcome(long timeframeSeconds, string instrument)
        {
            //Do in walker? Faster? Todo...
            //{ outcome_max_1800: { $exists: true } }

            throw new Exception("Create the walker!");
                        
            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            while (threadId < threadsCount)
            {
                int indexBeginning = start + (indexFrame * threadId);
                int indexEnd = indexBeginning + indexFrame;

                int outcomeIndex = 0;

                double min = double.MaxValue, max = double.MinValue;

                Thread thread = new Thread(delegate ()
                {
                    string name = "outcome " + timeframeSeconds + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        currentId++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        DataminingTickdata currentTickdata = dataInRam[instrument][currentId];

                        if (currentTickdata.values.ContainsKey(dataname) == false)
                        {
                            //Walker action ????
                            while (outcomeIndex < dataInRam[instrument].Count() && (dataInRam[instrument][outcomeIndex].timestamp - currentTickdata.timestamp) * 1000 < timeframeSeconds)
                            {
                                outcomeIndex++;
                                double last = dataInRam[instrument][outcomeIndex].values["last"];
                                if (last > max)
                                    max = last;

                                if (last < min)
                                    min = last;

                                agadaagagagagsd

                                //Stimmt nicht... bin betrunken und wir brauchen min und max. geht das überhautp? ???
                            }

                            DataminingTickdata outcomeData = dataInRam[instrument][outcomeIndex];
                                
                            currentTickdata.values.Add("outcome_min_" + timeframeSeconds, ...);
                            currentTickdata.values.Add("outcome_max_" + timeframeSeconds, ...);
                            currentTickdata.values.Add("outcome_actual_" + timeframeSeconds, outcomeData.values["last"]);
                            currentTickdata.changed = true;
                        }
                    }

                    progress.remove(name);

                });

                thread.Start();
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        void DataminingDatabase.addData(string dataname, Database database, string instrument)
        {
            List<DataminingTickdata> inRamList = dataInRam[instrument];
            
            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            while (threadId < threadsCount)
            {
                int indexBeginning = start + (indexFrame * threadId);
                int indexEnd = indexBeginning + indexFrame;

                Thread thread = new Thread(delegate ()
                {
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
                            currentTickdata.values.Add(dataname, data.value);
                            currentTickdata.changed = true;

                        }
                    }

                    progress.remove(name);

                });

                thread.Start();
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
        void DataminingDatabase.getOutcomeIndicatorSampling(DataminingExcelGenerator excel, double min, double max, int steps, string indicatorId, int outcomeTimeframeSeconds, string instrument)
        {
            //Min und Max wird nicht mehr verwendet... ???

            Dictionary<double, OutcomeCountPair> valueCounts = new Dictionary<double, OutcomeCountPair>();

            List<DataminingTickdata> inRamList = dataInRam[instrument];

            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            while (threadId < threadsCount)
            {
                int indexBeginning = start + (indexFrame * threadId);
                int indexEnd = indexBeginning + indexFrame;

                Thread thread = new Thread(delegate ()
                {
                    string name = "indicator sampling " + indicatorId + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        currentId++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        DataminingTickdata currentTickdata = inRamList[currentId];

                        double value = currentTickdata.values[indicatorId];

                        if (valueCounts.ContainsKey(value) == false)
                            valueCounts.Add(value, new OutcomeCountPair { Count = 1, MinSum = currentTickdata.values["outcome_min_" + outcomeTimeframeSeconds], MaxSum = currentTickdata.values["outcome_max_" + outcomeTimeframeSeconds], ActualSum = currentTickdata.values["outcome_actual_" + outcomeTimeframeSeconds] });
                        else
                        {
                            valueCounts[value].Count++;

                            valueCounts[value].MinSum += currentTickdata.values["outcome_min_" + outcomeTimeframeSeconds];
                            valueCounts[value].MaxSum += currentTickdata.values["outcome_max_" + outcomeTimeframeSeconds];
                            valueCounts[value].ActualSum += currentTickdata.values["outcome_actual_" + outcomeTimeframeSeconds];
                        }
                    }

                    progress.remove(name);

                });

                thread.Start();
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

                excel.addRow(sheetName, pair.Key, pair.Key, Convert.ToInt32(pair.Value.Count), maxAvg, minAvg, minVsMax, actualAvg, (maxAvg + minAvg));
            }

            excel.FinishSheet(sheetName);
        }

        ProgressDict DataminingDatabase.getProgress()
        {
            return progress;
        }

        void DataminingDatabase.addIndicator(WalkerIndicator indicator, string instrument, string fieldId)
        {
            string name = "Indicator " + indicator.getName() + " " + instrument + " " + fieldId;
            progress.setProgress(name, 0);
            int done = 0;

            string indicatorID = indicator.getName() + "_" + fieldId;

            foreach (DataminingTickdata currentTickdata in dataInRam[instrument])
            {
                progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(done) / Convert.ToDouble(dataInRam[instrument].Count()) * 100d));
                done++;

                indicator.setNextData(currentTickdata.timestamp, currentTickdata.values[fieldId]);

                if (currentTickdata.values.ContainsKey(indicatorID) == false)
                {
                    currentTickdata.values.Add(indicatorID, indicator.getIndicator().value);
                    currentTickdata.changed = true;

                }
            }

            progress.remove(name);
        }

        void DataminingDatabase.addMetaIndicatorSum(string[] ids, double[] weights, string fieldName, string instrument)
        {
            List<DataminingTickdata> inRamList = dataInRam[instrument];

            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            while (threadId < threadsCount)
            {
                int indexBeginning = start + (indexFrame * threadId);
                int indexEnd = indexBeginning + indexFrame;

                Thread thread = new Thread(delegate ()
                {
                    string name = "indicator sum " + fieldName + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        currentId++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        DataminingTickdata currentTickdata = inRamList[currentId];

                        if (currentTickdata.values.ContainsKey(fieldName) == false)
                        {
                            double value = 0;
                            for (int i = 0; i < ids.Length; i++)
                                value += currentTickdata.values[ids[i]] * weights[i];

                            currentTickdata.values.Add(fieldName, value);
                            currentTickdata.changed = true;

                        }
                    }

                    progress.remove(name);

                });

                thread.Start();
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        void DataminingDatabase.addMetaIndicatorDifference(string id, string id_subtract, string fieldName, string instrument)
        {
            List<DataminingTickdata> inRamList = dataInRam[instrument];

            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            while (threadId < threadsCount)
            {
                int indexBeginning = start + (indexFrame * threadId);
                int indexEnd = indexBeginning + indexFrame;

                Thread thread = new Thread(delegate ()
                {
                    string name = "indicator sum " + fieldName + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        currentId++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        DataminingTickdata currentTickdata = inRamList[currentId];

                        if (currentTickdata.values.ContainsKey(fieldName) == false)
                        {
                            double value = currentTickdata.values[id] - currentTickdata.values[id_subtract];
                            currentTickdata.values.Add(fieldName, value);
                            currentTickdata.changed = true;

                        }
                    }

                    progress.remove(name);

                });

                thread.Start();
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        void DataminingDatabase.addOutcomeCode(double percentDifference, int outcomeTimeframeSeconds, string instrument)
        {
            string outcomeCodeFieldName = "outcome_code_" + outcomeTimeframeSeconds + "_" + percentDifference;
            
            List<DataminingTickdata> inRamList = dataInRam[instrument];

            List<Thread> threads = new List<Thread>();

            int start = 0;
            int end = dataInRam[instrument].Count();

            int indexFrame = (end - start) / threadsCount;
            int threadId = 0;

            while (threadId < threadsCount)
            {
                int indexBeginning = start + (indexFrame * threadId);
                int indexEnd = indexBeginning + indexFrame;

                Thread thread = new Thread(delegate ()
                {
                    string name = "add outcome code " + outcomeCodeFieldName + " ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        currentId++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        DataminingTickdata currentTickdata = inRamList[currentId];

                        if (currentTickdata.values.ContainsKey(outcomeCodeFieldName + "_buy") == false)
                        {
                            double onePercent = currentTickdata.values["last"] / 100d;

                            double maxDiff = currentTickdata.values["outcome_max_" + outcomeTimeframeSeconds] / onePercent - 100; //calculate percent difference
                            double minDiff = currentTickdata.values["outcome_min_" + outcomeTimeframeSeconds] / onePercent - 100;

                            currentTickdata.values.Add(outcomeCodeFieldName + "_buy", (maxDiff >= percentDifference ? 1 : 0));
                            currentTickdata.values.Add(outcomeCodeFieldName + "_sell", (minDiff <= -percentDifference ? 1 : 0));
                            currentTickdata.changed = true;

                        }
                    }

                    progress.remove(name);

                });

                thread.Start();
                threads.Add(thread);

                threadId++;
            }

            waitForThreads(threads);
        }

        string DataminingDatabase.getSuccessRate(int outcomeTimeframeSeconds, string indicator, double min, double max, string instrument, double tpPercent, double slPercent, bool buy)
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
                int indexBeginning = start + (indexFrame * threadId);
                int indexEnd = indexBeginning + indexFrame;

                Thread thread = new Thread(delegate ()
                {
                    string name = "successrate ID_" + indexBeginning + ":" + indexEnd;
                    progress.setProgress(name, 0);
                    int currentId = indexBeginning;

                    while (currentId <= indexEnd)
                    {
                        currentId++;
                        progress.setProgress(name, Convert.ToInt32(Convert.ToDouble(currentId - indexBeginning) / Convert.ToDouble(indexFrame) * 100d));

                        DataminingTickdata currentTickdata = inRamList[currentId];

                        try
                        {
                            double onePercent = currentTickdata.values["last"] / 100d;

                            double maxDiff = currentTickdata.values["outcome_max_" + outcomeTimeframeSeconds] / onePercent - 100; //calculate percent difference
                            double minDiff = currentTickdata.values["outcome_min_" + outcomeTimeframeSeconds] / onePercent - 100;
                            double actualDiff = currentTickdata.values["outcome_actual_" + outcomeTimeframeSeconds] / onePercent - 100;

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
                    }

                    progress.remove(name);

                });

                thread.Start();
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

        void DataminingDatabase.getCorrelation(string indicatorId, int outcomeTimeframe, CorrelationCondition condition)
        {
            throw new NotImplementedException();
        }

        void DataminingDatabase.getCorrelationTable()
        {
            throw new NotImplementedException();
        }

        void DataminingDatabase.doMachineLearning(string[] inputFields, string outcomeField, string instrument, string savePath = null)
        {
            throw new Exception("Not implemented");

            /*string name = "ANN";

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

            progress.setProgress(name, "Finished with successRate of " + successRate + " failRate of " + failRate);*/
        }
    }
}
