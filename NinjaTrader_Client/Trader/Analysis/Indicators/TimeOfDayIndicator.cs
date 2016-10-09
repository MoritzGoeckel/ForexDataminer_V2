using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Utils;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class TimeOfDayIndicator : WalkerIndicator
    {
        public TimeOfDayIndicator()
        {
            
        }

        long currentTime = 0;

        public override TimeValueData getIndicator()
        {
            DateTime dt = Timestamp.getDate(currentTime);
            return new TimeValueData(currentTime, (dt.Hour != 0 ? Convert.ToDouble(dt.Hour) / 24d : 0));
        }

        public override void setNextData(long timestamp, double value)
        {
            currentTime = timestamp;
        }

        public override string getName()
        {
            return "TimeOfDay";
        }

        public override bool isValid(long timestamp)
        {
            return true;
        }
    }
}
