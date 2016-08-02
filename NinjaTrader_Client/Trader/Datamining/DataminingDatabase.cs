using NinjaTrader_Client.Trader.BacktestBase;
using NinjaTrader_Client.Trader.Indicators;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining
{
    public interface DataminingDatabase
    {
        void importPair(string pair, long start, long end, Database database);
        void addOutcome(long timeframeSeconds, string instrument);
        void addData(string dataname, Database database, string instrument);
        void addMetaIndicatorSum(string[] ids, double[] weights, string fieldName, string instrument);
        void addMetaIndicatorDifference(string id, string id_subtract, string fieldName, string instrument);
        void addOutcomeCode(double percentDifference, int outcomeTimeframeSeconds, string instrument);

        void deleteAll();

        void addIndicator(WalkerIndicator indicator, string instrument, string fieldId);
        void getOutcomeIndicatorSampling(DataminingExcelGenerator excel, string indicatorId, int outcomeTimeframeSeconds, string instument = null); //double min, double max, int steps, 
        void getOutcomeIndicatorSampling(DataminingExcelGenerator excel, double min, double max, int steps, string indicatorId, int outcomeTimeframeSeconds, string instument = null);
        string getSuccessRate(int outcomeTimeframeSeconds, string indicator, double min, double max, string instrument, double tpPercent, double slPercent, bool buy);

        //Not yet implemented
        void getCorrelation(string indicatorId, int outcomeTimeframe, CorrelationCondition condition);
        void getCorrelationTable();

        //Trivial
        ProgressDict getProgress();

        void doMachineLearning(string[] inputFields, string outcomeField, string instrument, string savePath = null);

        string getInfo();
        //void trainNeuralNetwork(ActivationNetwork ann, int iterations)
    }
}
