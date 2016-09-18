using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Analysis.Datamining
{
    public class Distribution
    {
        private Dictionary<double, int> distribution = new Dictionary<double, int>();
        private bool droppedData = false;

        public void addValue(double value)
        {
            if(distribution.ContainsKey(value) == false)
                distribution.Add(value, 0);

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

            return new DistributionRange(min, max);
        }

        public void dropPercentOnce(int percent)
        {
            if (droppedData == false)
                dropPercent(percent);

            droppedData = true;
        }

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

                //Check whether some dropping is possible
                if (distribution[upperValue] > valuesToDrop && distribution[lowerValue] > valuesToDrop)
                    break;

                //Check whether to remove the upper
                if (distribution[upperValue] <= valuesToDrop && (distribution[upperValue] < distribution[lowerValue] || distribution[lowerValue] > valuesToDrop))
                {
                    valuesToDrop -= distribution[upperValue];
                    distribution.Remove(upperValue);
                    values.RemoveAt(0);
                }

                //Check whether to remove the lower
                if (distribution[lowerValue] <= valuesToDrop && (distribution[lowerValue] < distribution[upperValue] || distribution[upperValue] > valuesToDrop))
                {
                    valuesToDrop -= distribution[lowerValue];
                    distribution.Remove(lowerValue);
                    values.RemoveAt(values.Count - 1);
                }
            }
        }
    }
}
