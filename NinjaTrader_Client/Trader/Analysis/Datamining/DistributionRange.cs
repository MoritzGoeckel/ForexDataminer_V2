using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Analysis.Datamining
{
    public class DistributionRange
    {
        public double min, max;

        public DistributionRange(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        internal void checkValue(double value)
        {
            if (value > max)
                max = value;

            if (value < min)
                min = value;
        }
    }
}
