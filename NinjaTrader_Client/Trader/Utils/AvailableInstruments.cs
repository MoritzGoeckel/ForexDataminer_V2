using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Utils
{
    class AvailableInstruments
    {
        public static List<string> allInstruments = new List<string>();
        public static List<string> minorsInstruments = new List<string>();
        public static List<string> majorsInstruments = new List<string>();

        static AvailableInstruments()
        {
            majorsInstruments.Add("EURUSD");
            majorsInstruments.Add("GBPUSD");
            majorsInstruments.Add("USDJPY");
            majorsInstruments.Add("USDCHF");

            minorsInstruments.Add("AUDCAD");
            minorsInstruments.Add("AUDJPY");
            minorsInstruments.Add("AUDUSD");
            minorsInstruments.Add("CHFJPY");
            minorsInstruments.Add("EURCHF");
            minorsInstruments.Add("EURGBP");
            minorsInstruments.Add("EURJPY");
            minorsInstruments.Add("GBPCHF");
            minorsInstruments.Add("GBPJPY");
            minorsInstruments.Add("NZDUSD");
            minorsInstruments.Add("USDCAD");

            allInstruments.AddRange(majorsInstruments);
            allInstruments.AddRange(minorsInstruments);

        }
}
}
