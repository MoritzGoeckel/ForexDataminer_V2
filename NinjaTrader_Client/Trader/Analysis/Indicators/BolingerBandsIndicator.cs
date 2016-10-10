using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class BolingerBandsIndicator : WalkerIndicator
    {
        private long timeframe;

        private MovingAverageIndicator ma;
        private StandartDeviationIndicator std;
        private double stdMultiplicator;

        public BolingerBandsIndicator(long timeframe, double stdMultiplicator)
        {
            this.stdMultiplicator = stdMultiplicator;
            this.timeframe = timeframe;
            this.ma = new MovingAverageIndicator(timeframe);
            this.std = new StandartDeviationIndicator(timeframe);
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
            std.setNextData(_timestamp, _value);
        }

        public override TimeValueData getIndicator()
        {
            double upperBond = (ma.getIndicator().value * valueNow) + (std.getIndicator().value * stdMultiplicator);
            double lowerBond = (ma.getIndicator().value * valueNow) - (std.getIndicator().value * stdMultiplicator);

            double value = (valueNow - lowerBond) / (upperBond - lowerBond);
            if (value > 1)
                value = 1;

            if (value < 0)
                value = 0;

            return new TimeValueData(timestampNow, value);
        }

        public override string getName()
        {
            return "BOLINGERBANDS_" + timeframe + "_" + stdMultiplicator;
        }

        public override bool isValid(long timestamp)
        {
            return ma.isValid(timestamp) && std.isValid(timestamp) && timestamp - timestampNow < 5 * 60; //Last data not older then 5 min
        }
    }
}
