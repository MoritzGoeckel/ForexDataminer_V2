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
        public double droppedPercent = 0;

        public DistributionRange(double min, double max, int droppedPercent = 0)
        {
            this.min = min;
            this.max = max;
            this.droppedPercent = droppedPercent;
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
