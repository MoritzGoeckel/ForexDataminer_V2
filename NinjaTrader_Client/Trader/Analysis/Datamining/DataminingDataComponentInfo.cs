using NinjaTrader_Client.Trader.Analysis.Datamining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining
{
    public class DataminingDataComponentInfo : DataminingDataComponent
    {
        public int occurences = 0;

        public Distribution distribution = new Distribution();

        public double getOccurencesRatio(int total)
        {
            return (double)occurences / (double)total;
        }

        private DistributionRange cachedRange = null;
        public DistributionRange getRange()
        {
            //Maybe drop the entire distribution after that? (because of ram)
            //todo:
            distribution.dropPercentOnce(10);

            if (cachedRange == null)
                cachedRange = distribution.calculateRange();

            return cachedRange;
        }

        public DataminingDataComponentInfo(string name, int timeframeSeconds) : base(name, timeframeSeconds)
        {
            
        }

        public DataminingDataComponentInfo(string nameAndTimeframe) : base(nameAndTimeframe)
        {
            
        }

        public void checkDistribution(double value)
        {
            distribution.addValue(value);
        }
    }
}
