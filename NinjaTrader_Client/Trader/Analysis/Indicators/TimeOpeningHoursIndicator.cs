﻿using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Utils;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class TimeOpeningHoursIndicator : WalkerIndicator
    {
        public TimeOpeningHoursIndicator()
        {
            
        }

        long currentTime = 0;

        public override TimeValueData getIndicator()
        {
            DateTime dt = Timestamp.getDate(currentTime);

            if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                return new TimeValueData(currentTime, 0); //Kein trading

            if (dt.DayOfWeek == DayOfWeek.Friday && dt.Hour >= 21)
                return new TimeValueData(currentTime, 0); //Kein trading

            if (dt.DayOfWeek == DayOfWeek.Friday && dt.Hour >= 19)
                return new TimeValueData(currentTime, 0.5); //Do not open positions

            return new TimeValueData(currentTime, 1); //Happy trading :)
            // Trading: 0 = No, 0.5 = Dont open, 1 = Yes
        }

        public override void setNextData(long timestamp, double value)
        {
            currentTime = timestamp;
        }

        public override string getName()
        {
            return "TradingTime";
        }

        public override bool isValid(long timestamp)
        {
            return true;
        }
    }
}
