using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class StochBorderCrossoverIndicator : WalkerIndicator
    {
        private double border;
        private long timeframe;
        private StochIndicator stoch;

        private double lastSeenStochValue = double.NaN;

        /// <summary>
        /// Will return 0 on crossover from upper bound to middle and 1 on crossover from lower bound to middle, its 0.5 every other time
        /// </summary>
        /// <param name="timeframe">In Seconds</param>
        /// <param name="border">Needs to be 0 << border << 1 </param>
        public StochBorderCrossoverIndicator(long timeframe, double border)
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

            lastSeenStochValue = stoch.getIndicator().value;
            stoch.setNextData(_timestamp, _value);

            timestampNow = _timestamp;
            valueNow = _value;
        }

        public override TimeValueData getIndicator()
        {
            double indicatorValueNow = stoch.getIndicator().value;

            double output;
            if (double.IsNaN(lastSeenStochValue) == false && indicatorValueNow > border && indicatorValueNow < 1 - border) // In mitte
            {
                if (lastSeenStochValue <= border) //Kommt von unten
                    output = 1d;
                else if (lastSeenStochValue >= 1 - border) //Kommt von oben
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
            return "StochBorderCrossover_" + timeframe + "_" + border;
        }

        public override bool isValid(long timestamp)
        {
            return stoch.isValid(timestamp) && double.IsNaN(lastSeenStochValue) != false;
        }
    }
}
