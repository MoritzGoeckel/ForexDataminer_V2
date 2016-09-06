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
        public abstract AdvancedTickData doCalculations(AdvancedTickData data);

        public AdvancedTickData doCalculations(TickData data)
        {
            return doCalculations(data.toAdvancedTickData());
        }

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
