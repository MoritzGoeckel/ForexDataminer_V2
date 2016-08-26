using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Analysis.IndicatorCollections
{
    public abstract class IndicatorCollection
    {
        private Dictionary<string, double> indicatorValues;
        public abstract DataminingTickdata doCalculations(DataminingTickdata data);
        public bool existsIndicator(string name)
        {
            return indicatorValues.ContainsKey(name);
        }

        public double getIndicator(string name)
        {
            return indicatorValues[name];
        }
    }
}
