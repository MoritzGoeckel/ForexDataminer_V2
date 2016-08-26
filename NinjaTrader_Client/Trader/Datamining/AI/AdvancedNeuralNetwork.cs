using Accord.Neuro;
using Accord.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining.AI
{
    class AdvancedNeuralNetwork : IMachineLearning
    {
        private int[] neuronsCount, dropedFields;
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

        public AdvancedNeuralNetwork(string[] inputFieldNames, string outputFieldName, int[] neuronsCount, double learningRate = 0.1, double sigmoidAlphaValue = 2, bool useRegularization = false, bool useNguyenWidrow = false, bool useSameWeights = false, JacobianMethod method = JacobianMethod.ByBackpropagation, int[] dropedFields = null)
        {
            this.neuronsCount = neuronsCount;
            this.learningRate = learningRate;
            this.useRegularization = useRegularization;
            this.useNguyenWidrow = useNguyenWidrow;
            this.useSameWeights = useSameWeights;
            this.method = method;
            this.dropedFields = dropedFields;

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

        double IMachineLearning.getPrediction(double[] input)
        {
            return theNetwork.Compute(input)[0];
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
        void IMachineLearning.train(double[][] input, double[] output, int epochs = 1)
        {
            double[][] usedInput = new double[][] { }, usedOutput = new double[][] { };
            convertData(input, output, ref usedInput, ref usedOutput);

            for (int i = 0; i < epochs; i++)
            {
                error = teacher.RunEpoch(usedInput.ToArray(), usedOutput.ToArray());

                if (lastError == error)
                    break;
                else
                    lastError = error;
            }
        }

        //Do we really need to convert it again??? todo!
        private void convertData(double[][] input, double[] output, ref double[][] properInput, ref double[][] properOutput)
        {
            if (input.Length != inputFieldNames.Length)
                throw new Exception("Wrong input count! Length: " + input.Length + " should be " + inputFieldNames.Length);

            properInput = new double[input.Length][];
            properOutput = new double[input.Length][];

            for (int row = 0; row < input.Length; row++)
            {
                for (int column = 0; column < input[row].Length; column++)
                {
                    double inputValue = input[row][column];
                    if (double.IsInfinity(inputValue) || double.IsNegativeInfinity(inputValue) || double.IsNaN(inputValue))
                        throw new Exception("Invalid value!");

                    double[] tmpInput = input[row];
                    if (dropedFields != null)
                    {
                        tmpInput = new double[input.Length - dropedFields.Length];
                        for (int i = 0, b = 0; i < input.Length; i++) //Iterates thorugh input (i)
                            if (dropedFields.Contains<int>(i) == false) //Checks weather currentInput is dropped
                            {
                                tmpInput[b] = input[row][i]; //If not, add it to next inputVectorUsed
                                b++;
                            }
                    }

                    //Check dropped field
                    properInput[row] = tmpInput;
                }

                double d = output[row];
                if (double.IsInfinity(d) || double.IsNegativeInfinity(d) || double.IsNaN(d))
                    throw new Exception("Invalid value!");

                properOutput[row] = new double[] { d };
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

            string droppedString = "";
            foreach (int i in dropedFields)
                droppedString += i + "|";

            string outputStr = "";
            outputStr += "InputCount: " + inputFieldNames.Length + nl;
            outputStr += "NeuronsCount: " + neuronsCountString + nl;
            outputStr += "LearningRate: " + learningRate + nl;
            outputStr += "Regularization: " + useRegularization.ToString() + nl;
            outputStr += "NguyenWidrow: " + useNguyenWidrow.ToString() + nl;
            outputStr += "SameWeights: " + useSameWeights.ToString() + nl;
            outputStr += "Method: " + method.ToString() + nl;
            outputStr += "Dropped: " + droppedString + nl;
            outputStr += "Inputs: " + fieldsString + nl;
            outputStr += "Output: " + outputFieldName;
            outputStr += "Error: " + this.error;

            return outputStr;
        }

        public static string[] getExcelHeader()
        {
            return new string[] { "InputCount", "NeuronsCount",
                "LearningRate", "Regularization", "NguyenWidrow", "SameWeights",
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

            string droppedString = "";
            foreach (int i in dropedFields)
                droppedString += i + "|";

            string[] values = new string[] { inputFieldNames.Length.ToString(), neuronsCountString, learningRate.ToString(), useRegularization.ToString(),
                useNguyenWidrow.ToString(), useSameWeights.ToString(), method.ToString(), droppedString, fieldsString, outputFieldName, this.error.ToString() };

            return values;
        }

        public double validateOnData(double[][] input, double[] output)
        {
            double[][] usedInput = new double[][] { }, usedOutput = new double[][] { };
            convertData(input, output, ref usedInput, ref usedOutput);
            return teacher.ComputeError(usedInput.ToArray(), usedOutput.ToArray());
        }
    }
}
