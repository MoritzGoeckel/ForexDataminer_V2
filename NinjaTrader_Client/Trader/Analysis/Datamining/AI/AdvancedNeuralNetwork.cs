using Accord.Neuro;
using Accord.Neuro.Learning;
using NinjaTrader_Client.Trader.Analysis.Datamining.AI;
using NinjaTrader_Client.Trader.Streaming.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining.AI
{
    class AdvancedNeuralNetwork : IMachineLearning
    {
        private int[] neuronsCount;
        private double learningRate, sigmoidAlphaValue;
        private bool useRegularization, useNguyenWidrow, useSameWeights;
        private JacobianMethod method;

        private string[] inputFieldNames;
        private string outputFieldName;

        private ActivationNetwork theNetwork;
        private LevenbergMarquardtLearning teacher;
        
        private static Random rand = new Random();
        public static AdvancedNeuralNetwork getRandom(string[] inputFieldNames, string outputFieldName)
        {
            int[] randomArchitecture = new int[3 + rand.Next(0, 10)];
            
            for (int i = 0; i < randomArchitecture.Length; i++)
                randomArchitecture[i] = rand.Next(2, inputFieldNames.Length * 2);

            randomArchitecture[0] = inputFieldNames.Length;
            randomArchitecture[randomArchitecture.Length] = 1;
            
            return new AdvancedNeuralNetwork(inputFieldNames, outputFieldName, randomArchitecture, rand.Next(1, 50) * 0.02, rand.Next(1, 10), rand.Next(0, 1) == 1, rand.Next(0, 1) == 1, rand.Next(0, 1) == 1, (rand.Next(0, 1) == 1 ? JacobianMethod.ByBackpropagation : JacobianMethod.ByFiniteDifferences));
        }

        public AdvancedNeuralNetwork(string[] inputFieldNames, string outputFieldName, int[] neuronsCount, double learningRate = 0.1, double sigmoidAlphaValue = 2, bool useRegularization = false, bool useNguyenWidrow = false, bool useSameWeights = false, JacobianMethod method = JacobianMethod.ByBackpropagation)
        {
            this.neuronsCount = neuronsCount;
            this.learningRate = learningRate;
            this.useRegularization = useRegularization;
            this.useNguyenWidrow = useNguyenWidrow;
            this.useSameWeights = useSameWeights;
            this.method = method;
            this.sigmoidAlphaValue = sigmoidAlphaValue;

            this.inputFieldNames = inputFieldNames;
            this.outputFieldName = outputFieldName;

            // create multi-layer neural network
            theNetwork = new ActivationNetwork(
                new BipolarSigmoidFunction(sigmoidAlphaValue), //Andere Function möglich???
                inputFieldNames.Length, neuronsCount);

            if (useNguyenWidrow)
            {
                if (useSameWeights)
                    Accord.Math.Random.Generator.Seed = 0;

                NguyenWidrow initializer = new NguyenWidrow(theNetwork);
                initializer.Randomize();
            }
            
            // create teacher
            teacher = new LevenbergMarquardtLearning(theNetwork, useRegularization, method);

            // set learning rate and momentum
            teacher.LearningRate = learningRate;
        }

        StrategySignal IMachineLearning.getPrediction(double[] input)
        {
            return new StrategySignal(theNetwork.Compute(input));
        }

        void IMachineLearning.load(string path)
        {
            theNetwork = ActivationNetwork.Load(path) as ActivationNetwork;
        }

        void IMachineLearning.save(string path)
        {
            theNetwork.Save(path);
        }

        private double error = -1;
        private double lastError = -1;
        void IMachineLearning.train(double[][] input, double[][] output, int epochs)
        {
            double[][] usedInput = new double[][] { }, usedOutput = new double[][] { };

            for (int i = 0; i < epochs; i++)
            {
                error = teacher.RunEpoch(usedInput.ToArray(), usedOutput.ToArray());

                if (lastError == error)
                    break;
                else
                    lastError = error;
            }
        }

        double IMachineLearning.getError()
        {
            return error;
        }

        public string getInfoString()
        {
            string nl = Environment.NewLine;

            string neuronsCountString = "";
            foreach (int i in neuronsCount)
                neuronsCountString += i + "|";

            string fieldsString = "";
            foreach (string f in inputFieldNames)
                fieldsString += f + "|";

            string outputStr = "";
            outputStr += "InputCount: " + inputFieldNames.Length + nl;
            outputStr += "NeuronsCount: " + neuronsCountString + nl;
            outputStr += "LearningRate: " + learningRate + nl;
            outputStr += "Regularization: " + useRegularization.ToString() + nl;
            outputStr += "NguyenWidrow: " + useNguyenWidrow.ToString() + nl;
            outputStr += "SameWeights: " + useSameWeights.ToString() + nl;
            outputStr += "sigmoidAlphaValue" + sigmoidAlphaValue + nl;
            outputStr += "Method: " + method.ToString() + nl;
            outputStr += "Inputs: " + fieldsString + nl;
            outputStr += "Output: " + outputFieldName;
            outputStr += "Error: " + this.error;

            return outputStr;
        }

        public static string[] getExcelHeader()
        {
            return new string[] { "InputCount", "NeuronsCount",
                "LearningRate", "Regularization", "NguyenWidrow", "SameWeights", "sigmoidAlphaValue",
                "Method", "Inputs", "Output", "Error" };
        }

        public string[] getInfo(string[] inputFieldName, string outputFieldName)
        {
            string nl = Environment.NewLine;

            string neuronsCountString = "";
            foreach (int i in neuronsCount)
                neuronsCountString += i + "|";

            string fieldsString = "";
            foreach (string f in inputFieldName)
                fieldsString += f + "|";

            string[] values = new string[] { inputFieldNames.Length.ToString(), neuronsCountString, learningRate.ToString(), useRegularization.ToString(),
                useNguyenWidrow.ToString(), useSameWeights.ToString(), sigmoidAlphaValue.ToString(), method.ToString(), fieldsString, outputFieldName, this.error.ToString() };

            return values;
        }

        public double getPredictionErrorFromData(double[][] input, double[][] output)
        {
            return teacher.ComputeError(input, output);
        }
    }
}
