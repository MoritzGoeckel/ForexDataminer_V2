using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining.AI
{
    public interface IMachineLearning
    {
        void clearData();
        void addData(double[] input, double output);
        void train();

        double getPrediction(double[] input);

        void save(string path);
        void load(string path);

        double getError();
        string getInfoString(string[] inputFieldName, string outputFieldName);
        GeneralExcelGenerator addRowToExcel(string[] inputFieldName, string outputFieldName, GeneralExcelGenerator excel, string sheet);

    }
}
