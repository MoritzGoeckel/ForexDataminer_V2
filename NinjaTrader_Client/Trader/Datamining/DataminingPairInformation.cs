using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining
{
    class DataminingPairInformation
    {
        public string Pair;
        public Dictionary<string, DataminingDataComponentInfo> Components = new Dictionary<string, DataminingDataComponentInfo>();

        public DataminingPairInformation(string name)
        {
            this.Pair = name;
        }

        public void checkTickdata(DataminingTickdata tickdata)
        {
            foreach(KeyValuePair<string, double> v in tickdata.values)
            {
                if (Components.ContainsKey(v.Key) == false)
                    Components.Add(v.Key, new DataminingDataComponentInfo(v.Key));

                Components[v.Key].occurences++;
            }
        }
    }
}
