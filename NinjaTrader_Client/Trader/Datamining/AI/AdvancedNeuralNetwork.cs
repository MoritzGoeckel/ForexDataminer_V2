using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining.AI
{
    class AdvancedNeuralNetwork
    {
        public AdvancedNeuralNetwork()
        {
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
