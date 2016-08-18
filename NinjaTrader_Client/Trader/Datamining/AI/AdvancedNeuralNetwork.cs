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
        private int inputCount;
        private int[] neuronsCount, dropedFields;
        private double learningRate, sigmoidAlphaValue;
        private bool useRegularization, useNguyenWidrow, useSameWeights;
        private JacobianMethod method;

        private ActivationNetwork theNetwork;
        private LevenbergMarquardtLearning teacher;

        //Todo: testen
        private static Random rand = new Random();
        public static AdvancedNeuralNetwork getRandom(string[] inputFieldNames, string outputFieldName, bool dropFields)
        {
            int[] randomArchitecture = new int[3 + rand.Next(0, 10)];

            for (int i = 0; i < randomArchitecture.Length; i++)
                randomArchitecture[i] = rand.Next(2, inputFieldNames.Length * 2);

            randomArchitecture[0] = inputFieldNames.Length;
            randomArchitecture[randomArchitecture.Length] = 1;

            int[] droppedFields = new int[rand.Next(0, inputFieldNames.Length - 1)];
            if(dropFields)
            {
                List<int> availableDrops = new List<int>();
                for (int i = 0; i < inputFieldNames.Length; i++)
                    availableDrops.Add(i);

                for (int i = 0; i < droppedFields.Length; i++)
                {
                    int choosenAvailableDropId = rand.Next(0, availableDrops.Count);
                    droppedFields[i] = availableDrops[choosenAvailableDropId];
                    availableDrops.RemoveAt(choosenAvailableDropId);
                }
            }

            return new AdvancedNeuralNetwork(inputFieldNames.Length - droppedFields.Length, randomArchitecture, rand.Next(1, 50) * 0.02, rand.Next(1, 10), rand.Next(0, 1) == 1, rand.Next(0, 1) == 1, rand.Next(0, 1) == 1, (rand.Next(0, 1) == 1 ? JacobianMethod.ByBackpropagation : JacobianMethod.ByFiniteDifferences));
        }

        public AdvancedNeuralNetwork(int inputCount, int[] neuronsCount, double learningRate = 0.1, double sigmoidAlphaValue = 2, bool useRegularization = false, bool useNguyenWidrow = false, bool useSameWeights = false, JacobianMethod method = JacobianMethod.ByBackpropagation, int[] dropedFields = null)
        {
            this.inputCount = inputCount;
            this.neuronsCount = neuronsCount;
            this.learningRate = learningRate;
            this.useRegularization = useRegularization;
            this.useNguyenWidrow = useNguyenWidrow;
            this.useSameWeights = useSameWeights;
            this.method = method;
            this.dropedFields = dropedFields;

            // create multi-layer neural network
            theNetwork = new ActivationNetwork(
                new BipolarSigmoidFunction(sigmoidAlphaValue), //Andere Function möglich???
                inputCount, neuronsCount);

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

        List<double[]> inputs = new List<double[]>();
        List<double[]> outputs = new List<double[]>();

        void IMachineLearning.addData(double[] input, double output)
        {
            if(input.Length != inputCount)
                throw new Exception("Wrong input count! Length: " + input.Length + " should be " + inputCount);

            foreach (double d in input)
                if (double.IsInfinity(d) || double.IsNegativeInfinity(d) || double.IsNaN(d))
                    throw new Exception("Invalid value!");

            if (double.IsInfinity(output) || double.IsNegativeInfinity(output) || double.IsNaN(output))
                throw new Exception("Invalid value!");

            double[] inputUsed = input;

            //Todo: Test
            if (dropedFields != null)
            {
                inputUsed = new double[input.Length - dropedFields.Length];
                for (int i = 0, b = 0; i < input.Length; i++) //Iterates thorugh input (i)
                    if (dropedFields.Contains<int>(i) == false) //Checks weather currentInput is dropped
                    {
                        inputUsed[b] = input[i]; //If not, add it to next inputVectorUsed
                        b++;
                    }
            }

            inputs.Add(inputUsed);
            outputs.Add(new double[] { output } );
        }

        private double error = -1;
        void IMachineLearning.train()
        {
            if(inputs.Count != 0)
                error = teacher.RunEpoch(inputs.ToArray(), outputs.ToArray());
        }

        void IMachineLearning.clearData()
        {
            inputs.Clear();
            outputs.Clear();
        }

        double IMachineLearning.getError()
        {
            return error;
        }

        public string getInfoString(string[] inputFieldName, string outputFieldName)
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

            string outputStr = "";
            outputStr += "InputCount: " + inputCount + nl;
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

        public GeneralExcelGenerator addRowToExcel(string[] inputFieldName, string outputFieldName, GeneralExcelGenerator excel, string sheet)
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

            string[] values = new string[] { inputCount.ToString(), neuronsCountString, learningRate.ToString(), useRegularization.ToString(),
                useNguyenWidrow.ToString(), useSameWeights.ToString(), method.ToString(), droppedString, fieldsString, outputFieldName, this.error.ToString() };

            excel.addRow(sheet, values);
            return excel;
        }
    }
}
