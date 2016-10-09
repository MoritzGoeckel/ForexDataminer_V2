using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Utils;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class TimeDayOfWeekIndicator : WalkerIndicator
    {
        public TimeDayOfWeekIndicator()
        {
            
        }

        long currentTime = 0;

        public override TimeValueData getIndicator()
        {
            DateTime dt = Timestamp.getDate(currentTime);
            return new TimeValueData(currentTime, (dt.DayOfWeek != 0 ? Convert.ToDouble(dt.DayOfWeek) / 7d : 0));
        }

        public override void setNextData(long timestamp, double value)
        {
            currentTime = timestamp;
        }

        public override string getName()
        {
            return "TimeDayOfWeek";
        }

        public override bool isValid(long timestamp)
        {
            return true;
        }
    }
}
