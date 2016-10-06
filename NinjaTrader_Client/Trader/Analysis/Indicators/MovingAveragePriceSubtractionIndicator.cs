using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class MovingAveragePriceSubtractionIndicator : WalkerIndicator
    {
        private long timeframe;
        private MovingAverageIndicator ma;
        public MovingAveragePriceSubtractionIndicator(long timeframe)
        {
            ma = new MovingAverageIndicator(timeframe);
            this.timeframe = timeframe;
        }

        double valueNow;
        long timestampNow;
        public override void setNextData(long _timestamp, double _value)
        {
            if (_timestamp < timestampNow)
                throw new Exception("Cant add older data here!");

            if (_timestamp == timestampNow && _value != valueNow)
                throw new Exception("Same timestamp different value!");

            if (_timestamp == timestampNow && _value == valueNow)
                return;

            timestampNow = _timestamp;
            valueNow = _value;

            ma.setNextData(_timestamp, _value);
        }

        public override TimeValueData getIndicator()
        {
            return new TimeValueData(timestampNow, (ma.getIndicator().value * valueNow) - valueNow);
        }

        public override string getName()
        {
            return "MAPriceSub_" + timeframe;
        }

        public override bool isValid(long timestamp)
        {
            return ma.isValid(timestamp) && timestamp - timestampNow < 60 * 5; //Neuster preis nicht älter als 5min
        }
    }
}
