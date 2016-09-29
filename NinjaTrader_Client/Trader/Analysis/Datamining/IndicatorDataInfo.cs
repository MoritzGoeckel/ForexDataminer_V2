using NinjaTrader_Client.Trader.Analysis.Datamining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining
{
    public class IndicatorDataInfo
    {
        public IndicatorID id;
        public int occurences = 0;
        public Distribution distribution = new Distribution();

        public IndicatorDataInfo(IndicatorID id)
        {
            this.id = id;
        }

        public double getOccurencesRatio(int total)
        {
            return (double)occurences / (double)total;
        }

        public DistributionRange dropPercentFromDistribution(int percent)
        {
            distribution.dropPercent(percent);
            return distribution.getRange();
        }

        public DistributionRange getRange()
        {
            //Maybe drop the entire distribution after that? (because of ram)
            //todo:
            return distribution.getRange();
        }

        public IndicatorDataInfo(string name, int timeframeSeconds)
        {
            this.id = new IndicatorID(name, timeframeSeconds);
        }

        public IndicatorDataInfo(string nameAndTimeframe)
        {
            this.id = new IndicatorID(nameAndTimeframe);
        }

        public void checkDistribution(double value)
        {
            distribution.addValue(value);
        }
    }
}
