using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Analysis.Datamining
{
    [Serializable]
    public class DistributionRange
    {
        public double min, max;
        public int droppedPercent;
        public int datasets;

        public DistributionRange(double min, double max, int droppedPercent = 0, int datasetsCount = 0)
        {
            this.min = min;
            this.max = max;
            this.droppedPercent = droppedPercent;
            this.datasets = datasetsCount;
        }

        public DistributionRange()
        {
            min = double.MaxValue;
            max = double.MinValue;
            droppedPercent = 0;
            datasets = 0;
        }

        public int getDatasetsCount()
        {
            return datasets;
        }

        public double getMax()
        {
            if (max == double.MinValue)
                throw new Exception("Not set! " + datasets);

            return max;
        }

        public double getMin()
        {
            if (min == double.MaxValue)
                throw new Exception("Not set! " + datasets);

            return min;
        }

        public void setDatasetsCount(int datasets)
        {
            this.datasets = datasets;
        }

        public int getDroppedPercent()
        {
            return droppedPercent;
        }

        public void checkValue(double value)
        {
            if (value > max)
                max = value;

            if (value < min)
                min = value;

            datasets++;
        }

        public DistributionRange copy()
        {
            return new DistributionRange(this.min, this.max, this.droppedPercent, datasets);
        }
    }
}
