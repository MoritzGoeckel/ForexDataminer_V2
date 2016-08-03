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
        private double learningRate = 0.1;
        private double sigmoidAlphaValue = 2;
        private int iterations = 100;

        private bool useRegularization = false;
        private bool useNguyenWidrow = false;
        private bool useSameWeights = false;

        private int inputCount;

        private ActivationNetwork theNetwork;
        private LevenbergMarquardtLearning teacher;

        public AdvancedNeuralNetwork(int inputCount, int[] neuronsCount, double learningRate, double sigmoidAlphaValue, int iterations, bool useRegularization, bool useNguyenWidrow, bool useSameWeights, JacobianMethod method)
        {
            this.learningRate = learningRate;
            this.sigmoidAlphaValue = sigmoidAlphaValue;
            this.iterations = iterations;
            this.useRegularization = useRegularization;
            this.useNguyenWidrow = useNguyenWidrow;
            this.useSameWeights = useSameWeights;

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
            foreach(double d in input)
                if (double.IsInfinity(d) || double.IsNegativeInfinity(d) || double.IsNaN(d))
                    throw new Exception("Invalid value!");

            if (double.IsInfinity(output) || double.IsNegativeInfinity(output) || double.IsNaN(output))
                throw new Exception("Invalid value!");

            inputs.Add(input);
            outputs.Add(new double[] { output } );
        }

        double error;
        void IMachineLearning.train()
        {
            error = teacher.RunEpoch(inputs.ToArray(), outputs.ToArray());
        }

        void IMachineLearning.clearData()
        {
            inputs.Clear();
            outputs.Clear();
        }
    }
}
