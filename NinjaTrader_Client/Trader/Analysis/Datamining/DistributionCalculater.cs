using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Analysis.Datamining
{
    public class DistributionCalculater
    {
        private Dictionary<double, int> distribution = new Dictionary<double, int>();
        private DistributionRange cachedRange = new DistributionRange(double.MaxValue, double.MinValue, 0, 0);
        
        public void addValue(double value)
        {
            if (distribution.ContainsKey(value) == false)
            {
                distribution.Add(value, 0);
                cachedRange.checkValue(value);
            }

            datasetsCount++;

            distribution[value]++;
        }

        public Dictionary<double, int> getValues()
        {
            return distribution;
        }

        public DistributionRange calculateRange()
        {
            double min = double.MaxValue, max = double.MinValue;
            foreach (KeyValuePair<double, int> pair in distribution)
            {
                if (pair.Key > max)
                    max = pair.Key;

                if (pair.Key < min)
                    min = pair.Key;
            }

            return cachedRange = new DistributionRange(min, max, droppedPercent, datasetsCount);
        }

        internal DistributionRange getRange()
        {
            return cachedRange;
        }

        private int droppedPercent = 0;
        private int datasetsCount = 0;

        public void dropPercent(int percent)
        {
            double min = double.MaxValue, max = double.MinValue;
            List<double> values = new List<double>();
            int valuesInDistribution = 0;

            //Find min max
            foreach (KeyValuePair<double, int> pair in distribution)
            {
                if (pair.Key > max)
                    max = pair.Key;

                if (pair.Key < min)
                    min = pair.Key;

                values.Add(pair.Key);
                valuesInDistribution += pair.Value;
            }

            values.Sort();

            int valuesToDrop = Convert.ToInt32(Convert.ToDouble(valuesInDistribution) / 100d * Convert.ToDouble(percent));

            while (valuesToDrop > 0 && values.Count >= 2)
            {
                double upperValue = values[0];
                double lowerValue = values[values.Count - 1];

                //Check whether to remove the upper
                if (distribution[upperValue] <= valuesToDrop && (distribution[upperValue] < distribution[lowerValue] || distribution[lowerValue] > valuesToDrop))
                {
                    valuesToDrop -= distribution[upperValue];
                    datasetsCount -= distribution[upperValue];
                    distribution.Remove(upperValue);
                    values.RemoveAt(0);
                }
                //Check whether to remove the lower
                else if (distribution[lowerValue] <= valuesToDrop && (distribution[lowerValue] < distribution[upperValue] || distribution[upperValue] > valuesToDrop))
                {
                    valuesToDrop -= distribution[lowerValue];
                    datasetsCount -= distribution[lowerValue];
                    distribution.Remove(lowerValue);
                    values.RemoveAt(values.Count - 1);
                }
                else break;
            }

            droppedPercent += percent;

            calculateRange();
        }
    }
}
