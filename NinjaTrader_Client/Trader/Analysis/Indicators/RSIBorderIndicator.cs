using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class RSIBorderIndicator : WalkerIndicator
    {
        private double border;
        private long timeframe;
        private RSIIndicator rsi;

        public RSIBorderIndicator(long timeframe, double border)
        {
            this.timeframe = timeframe;
            this.border = border;
            rsi = new RSIIndicator(timeframe);
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

            rsi.setNextData(_timestamp, _value);

            timestampNow = _timestamp;
            valueNow = _value;
        }

        public override TimeValueData getIndicator()
        {
            double output;
            if (rsi.getIndicator().value < border)
                output = 0;
            else if (rsi.getIndicator().value > 1 - border)
                output = 1;
            else
                output = 0.5;

            return new TimeValueData(timestampNow, output);
        }

        public override string getName()
        {
            return "RSIBorder_" + timeframe + "_" + border;
        }

        public override bool isValid(long timestamp)
        {
            return rsi.isValid(timestamp);
        }
    }
}
