using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Analysis.Datamining
{
    public class DistributionRange
    {
        public double min { get; }
        public double max { get; }

        public DistributionRange(double min, double max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
