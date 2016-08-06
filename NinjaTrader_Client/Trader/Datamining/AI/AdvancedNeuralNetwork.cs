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

        private ActivationNetwork theNetwork;
        private LevenbergMarquardtLearning teacher;

        public AdvancedNeuralNetwork(int inputCount, int[] neuronsCount, double learningRate = 0.1, double sigmoidAlphaValue = 2, bool useRegularization = false, bool useNguyenWidrow = false, bool useSameWeights = false, JacobianMethod method = JacobianMethod.ByBackpropagation)
        {
            this.inputCount = inputCount;

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

            inputs.Add(input);
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
    }
}
