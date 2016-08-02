using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining
{
    public class DataminingPairInformation
    {
        public string Pair;
        public Dictionary<string, DataminingDataComponentInfo> Components = new Dictionary<string, DataminingDataComponentInfo>();

        public int Datasets = 0;

        public DataminingPairInformation(string name)
        {
            this.Pair = name;
        }

        public void checkTickdata(DataminingTickdata tickdata)
        {
            Datasets++;

            foreach (KeyValuePair<string, double> v in tickdata.values)
            {
                if (Components.ContainsKey(v.Key) == false)
                    Components.Add(v.Key, new DataminingDataComponentInfo(v.Key));

                Components[v.Key].occurences++;
            }
        }
    }
}
