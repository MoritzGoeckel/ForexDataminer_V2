using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining
{
    public class DatasetId
    {
        private string name;
        private long timeframe;

        public string getName()
        {
            return name;
        }

        public long getTimeframeSeconds()
        {
            return timeframe;
        }

        public string getID()
        {
            return name + "_" + timeframe;
        }

        public void setTimeframeSeconds(int seconds)
        {
            timeframe = seconds;
        }

        public void setTimeframeMinutes(int minutes)
        {
            timeframe = minutes * 60;
        }

        public double getTimeframeMinutes()
        {
            return Convert.ToDouble(timeframe) / 60d;
        }

        public void setTimeframeHours(int hours)
        {
            timeframe = hours * 60 * 60;
        }

        public double getTimeframeHours()
        {
            return Convert.ToDouble(timeframe) / 60d / 60d;
        }

        public DatasetId(string name, long timeframeSeconds)
        {
            this.timeframe = timeframeSeconds;
            this.name = name;
        }

        public DatasetId(string nameAndTimeframe)
        {
            if (nameAndTimeframe.Contains('_'))
            {
                string[] nameAndTimeframeArray = nameAndTimeframe.Split('_');

                this.timeframe = Convert.ToInt32(nameAndTimeframeArray[1]);
                this.name = nameAndTimeframeArray[0];
            }
            else
                this.name = nameAndTimeframe;
        }
    }
}
