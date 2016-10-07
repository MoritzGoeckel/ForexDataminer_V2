using NinjaTrader_Client.Trader.BacktestBase;
using NinjaTrader_Client.Trader.Indicators;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining
{
    public interface IDataminingDatabase
    {
        void importPair(string pair, long start, long end, SQLiteDatabase database);
        void addOutcome(long timeframe, string instrument);
        void addData(string dataname, SQLiteDatabase database, string instrument);
        void addMetaIndicatorSum(string[] ids, double[] weights, string fieldName, string instrument);
        void addMetaIndicatorDifference(string id, string id_subtract, string fieldName, string instrument);
        void addOutcomeCode(double percentDifference, int outcomeTimeframe, string instrument);

        void deleteAll();

        void addIndicator(WalkerIndicator indicator, string instrument, string fieldId);
        void getOutcomeIndicatorSampling(SampleOutcomeExcelGenerator excel, string indicatorId, int outcomeTimeframe, string instument = null); //double min, double max, int steps, 
        void getOutcomeIndicatorSampling(SampleOutcomeExcelGenerator excel, double min, double max, int steps, string indicatorId, int outcomeTimeframe, string instument = null);
        string getSuccessRate(int outcomeTimeframe, string indicator, double min, double max, string instrument, double tpPercent, double slPercent, bool buy);

        //Trivial
        ProgressDict getProgress();

        void doMachineLearning(string[] inputFields, string outcomeField, string instrument, string savePath = null);

        List<DatasetInfo> getInfo();
    }
}
