using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class MACDContinousIndicator : WalkerIndicator
    {
        private long timeframeOne, timeframeTwo, signalTimeframe;
        private MovingAverageSubtractionIndicator maSub;
        private MovingAverageIndicator signalMa;

        public MACDContinousIndicator(long timeframeOne, long timeframeTwo, long signalTimeframe)
        {
            this.timeframeOne = timeframeOne;
            this.timeframeTwo = timeframeTwo;
            this.signalTimeframe = signalTimeframe;
            maSub = new MovingAverageSubtractionIndicator(timeframeOne, timeframeTwo);
            signalMa = new MovingAverageIndicator(signalTimeframe);
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

            maSub.setNextData(_timestamp, _value);
            signalMa.setNextData(_timestamp, maSub.getIndicator().value);
        }

        public override TimeValueData getIndicator()
        {
            return new TimeValueData(timestampNow, maSub.getIndicator().value - signalMa.getIndicator().value);
        }

        public override string getName()
        {
            return "MACDContinous_" + timeframeOne + "_" + timeframeTwo + "_" + signalTimeframe;
        }

        public override bool isValid(long timestamp)
        {
            return maSub.isValid(timestamp) && signalMa.isValid(timestamp);
        }
    }
}
