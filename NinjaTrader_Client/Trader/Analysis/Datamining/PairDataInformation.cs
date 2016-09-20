using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining
{
    public class PairDataInformation
    {
        public string Pair;
        public Dictionary<string, IndicatorDataInfo> Components = new Dictionary<string, IndicatorDataInfo>();

        public int AllDatasets = 0;
        public int Datasets = 0;

        public PairDataInformation(string name)
        {
            this.Pair = name;
        }

        public void checkTickdata(AdvancedTickData tickdata)
        {
            Datasets++;

            foreach (KeyValuePair<string, double> v in tickdata.values)
            {
                if (Components.ContainsKey(v.Key) == false)
                    Components.Add(v.Key, new IndicatorDataInfo(v.Key));

                Components[v.Key].occurences++;
                Components[v.Key].checkDistribution(v.Value);
            }
        }
    }
}
