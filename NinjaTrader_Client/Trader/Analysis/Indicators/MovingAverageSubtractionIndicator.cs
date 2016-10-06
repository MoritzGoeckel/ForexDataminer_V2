using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class MovingAverageSubtractionIndicator : WalkerIndicator
    {
        private long timeframeOne, timeframeTwo;
        private MovingAverageIndicator maOne, maTwo;
        public MovingAverageSubtractionIndicator(long timeframeOne, long timeframeTwo)
        {
            maOne = new MovingAverageIndicator(timeframeOne);
            this.timeframeOne = timeframeOne;

            maTwo = new MovingAverageIndicator(timeframeTwo);
            this.timeframeTwo = timeframeTwo;
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

            maOne.setNextData(_timestamp, _value);
            maTwo.setNextData(_timestamp, _value);
        }

        public override TimeValueData getIndicator()
        {
            return new TimeValueData(timestampNow, maOne.getIndicator().value - maTwo.getIndicator().value);
        }

        public override string getName()
        {
            return "MASub_" + timeframeOne + "_" + timeframeTwo;
        }

        public override bool isValid(long timestamp)
        {
            return maOne.isValid(timestamp) && maTwo.isValid(timestamp);
        }
    }
}
