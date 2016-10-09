using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining
{
    [Serializable]
    public class DatasetId
    {
        private string name;
        private long timeframe;
        private string id;

        public string getName()
        {
            return name;
        }

        public long getTimeframeSeconds()
        {
            return Convert.ToInt64(timeframe / 1000d);
        }

        public string getID()
        {
            return name + "_" + timeframe;
        }

        public long getTimeframe()
        {
            return timeframe;
        }

        public void setTimeframeSeconds(int seconds)
        {
            timeframe = seconds * 1000;
        }

        public void setTimeframeMinutes(int minutes)
        {
            timeframe = minutes * 60 * 1000;
        }

        public double getTimeframeMinutes()
        {
            return Convert.ToDouble(timeframe) / 60d / 1000;
        }

        public void setTimeframeHours(int hours)
        {
            timeframe = hours * 60 * 60 * 1000;
        }

        public double getTimeframeHours()
        {
            return Convert.ToDouble(timeframe) / 60d / 60d / 1000d;
        }

        public DatasetId(string name, long timeframe)
        {
            this.timeframe = timeframe;
            this.name = name;

            this.id = getID();
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

            id = getID();
        }
    }
}
