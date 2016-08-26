using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining.AI
{
    public interface IMachineLearning
    {
        void train(double[][] input, double[] output, int epochs = 1);
        double getPrediction(double[] input);

        void save(string path);
        void load(string path);

        double getError();
        string getInfoString();
        string[] getInfo(string[] inputFieldName, string outputFieldName);
        double validateOnData(double[][] input, double[] output);
    }
}
