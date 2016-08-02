using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Datamining
{
    public class DataminingDataComponent
    {
        private string name;
        private int timeframe;

        public string getName()
        {
            return name;
        }

        public int getTimeframe()
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

        public void setTimeframeHours(int hours)
        {
            timeframe = hours * 60 * 60;
        }

        public DataminingDataComponent(string name, int timeframeSeconds)
        {
            this.timeframe = timeframeSeconds;
            this.name = name;
        }

        public DataminingDataComponent(string nameAndTimeframe)
        {
            string[] nameAndTimeframeArray = nameAndTimeframe.Split('_');

            this.timeframe = Convert.ToInt32(nameAndTimeframeArray[1]);
            this.name = nameAndTimeframeArray[0];
        }
    }
}
