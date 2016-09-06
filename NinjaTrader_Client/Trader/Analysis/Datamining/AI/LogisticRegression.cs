using NinjaTrader_Client.Trader.Datamining.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Analysis.Datamining.AI
{
    //Todo
    class LogisticRegression : IMachineLearning
    {
        public double getError()
        {
            throw new NotImplementedException();
        }

        public string[] getInfo(string[] inputFieldName, string outputFieldName)
        {
            throw new NotImplementedException();
        }

        public string getInfoString()
        {
            throw new NotImplementedException();
        }

        public double getPrediction(double[] input)
        {
            throw new NotImplementedException();
        }

        public void load(string path)
        {
            throw new NotImplementedException();
        }

        public void save(string path)
        {
            throw new NotImplementedException();
        }

        public void train(double[][] input, double[] output, int epochs = 1)
        {
            throw new NotImplementedException();
        }

        public double validateOnData(double[][] input, double[] output)
        {
            throw new NotImplementedException();
        }
    }
}
