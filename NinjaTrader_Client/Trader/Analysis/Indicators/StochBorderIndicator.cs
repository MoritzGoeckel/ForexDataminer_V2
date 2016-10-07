using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class StochBorderIndicator : WalkerIndicator
    {
        private double border;
        private long timeframe;
        private StochIndicator stoch;

        /// <summary>
        /// Will return 0 in lower bound, 1 in upper bound and 0.5 in between
        /// </summary>
        /// <param name="border">Needs to be 0 << border << 1 </param>
        public StochBorderIndicator(long timeframe, double border)
        {
            this.timeframe = timeframe;
            this.border = border;
            stoch = new StochIndicator(timeframe);
        }

        long timestampNow;
        double valueNow;
        public override void setNextData(long _timestamp, double _value)
        {
            if (_timestamp < timestampNow)
                throw new Exception("Cant add older data here!");

            if (_timestamp == timestampNow && _value != valueNow)
                throw new Exception("Same timestamp different value!");

            if (_timestamp == timestampNow && _value == valueNow)
                return;

            stoch.setNextData(_timestamp, _value);

            timestampNow = _timestamp;
            valueNow = _value;
        }

        public override TimeValueData getIndicator()
        {
            double output;
            if (stoch.getIndicator().value < border)
                output = 0;
            else if (stoch.getIndicator().value > 1 - border)
                output = 1;
            else
                output = 0.5;

            return new TimeValueData(timestampNow, output);
        }

        public override string getName()
        {
            return "StochBorder_" + timeframe + "_" + border;
        }

        public override bool isValid(long timestamp)
        {
            return stoch.isValid(timestamp);
        }
    }
}
