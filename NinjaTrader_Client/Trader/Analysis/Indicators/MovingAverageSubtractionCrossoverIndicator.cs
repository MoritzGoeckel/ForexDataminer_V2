using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class MovingAverageSubtractionCrossoverIndicator : WalkerIndicator
    {
        private long timeframeOne, timeframeTwo;
        private MovingAverageSubtractionIndicator maSub;
        private double lastDifference = double.NaN;
        public MovingAverageSubtractionCrossoverIndicator(long timeframeOne, long timeframeTwo)
        {
            this.timeframeOne = timeframeOne;
            this.timeframeTwo = timeframeTwo;
            maSub = new MovingAverageSubtractionIndicator(timeframeOne, timeframeTwo);
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

            double diffTmp = maSub.getIndicator().value;
            if(diffTmp != 0d)
                lastDifference = diffTmp;

            maSub.setNextData(_timestamp, _value);
        }

        public override TimeValueData getIndicator()
        {
            double differenceNow = maSub.getIndicator().value;

            double output;
            if (differenceNow > 0 && lastDifference < 0) //Ist positiv war negativ -> 1
                output = 1;
            else if (differenceNow < 0 && lastDifference > 0) //Ist negativ war positiv -> 0
                output = 0;
            else
                output = 0.5; //War und ist positiv oder war und ist negativ

            return new TimeValueData(timestampNow, output);
        }

        public override string getName()
        {
            return "MASubCross_" + timeframeOne + "_" + timeframeTwo;
        }

        public override bool isValid(long timestamp)
        {
            return maSub.isValid(timestamp) && double.IsNaN(lastDifference) == false;
        }
    }
}
