using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class RSIBorderCrossoverIndicator : WalkerIndicator
    {
        private double border;
        private long timeframe;
        private RSIIndicator rsi;

        private double lastSeenValue = double.NaN;

        public RSIBorderCrossoverIndicator(long timeframe, double border)
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

            lastSeenValue = rsi.getIndicator().value;
            rsi.setNextData(_timestamp, _value);

            timestampNow = _timestamp;
            valueNow = _value;
        }

        public override TimeValueData getIndicator()
        {
            double indicatorValueNow = rsi.getIndicator().value;

            double output;
            if (double.IsNaN(lastSeenValue) == false && indicatorValueNow > border && indicatorValueNow < 1 - border) // In mitte
            {
                if (lastSeenValue <= border) //Kommt von unten
                    output = 1d;
                else if (lastSeenValue >= 1 - border) //Kommt von oben
                    output = 0d;
                else
                    output = 0.5;
            }
            else //Nicht in mitte
                output = 0.5;

            return new TimeValueData(timestampNow, output);
        }

        public override string getName()
        {
            return "RSIBorderCrossover_" + timeframe + "_" + border;
        }

        public override bool isValid(long timestamp)
        {
            return rsi.isValid(timestamp) && double.IsNaN(lastSeenValue) != false;
        }
    }
}
