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
        public double min = double.MaxValue, max = double.MinValue;

        public double getOccurencesRatio(int total)
        {
            return (double)occurences / (double)total;
        }

        public DataminingDataComponentInfo(string name, int timeframeSeconds) : base(name, timeframeSeconds)
        {
            
        }

        public DataminingDataComponentInfo(string nameAndTimeframe) : base(nameAndTimeframe)
        {
            
        }

        public void checkHeighLow(double value)
        {
            if (value > max)
                max = value;

            if (value < min)
                min = value;
        }
    }
}
