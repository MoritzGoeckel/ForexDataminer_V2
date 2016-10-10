using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class StandartDeviationIndicator : WalkerIndicator
    {
        private long timeframe;
        private List<TimestampValuePair> history = new List<TimestampValuePair>();

        public StandartDeviationIndicator(long timeframe)
        {
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

            history.Add(new TimestampValuePair { timestamp = _timestamp, value = _value });
            timestampNow = _timestamp;
            valueNow = _value;

            sum += _value;
            sumSquared += Math.Pow(_value, 2);
        }

        double sum = 0;
        double sumSquared = 0;

        public override TimeValueData getIndicator()
        {
            while (history.Count != 0 && history[0].timestamp < timestampNow - timeframe)
            {
                sum -= history[0].value;
                sumSquared -= Math.Pow(history[0].value, 2);

                history.RemoveAt(0);
            }

            double count = history.Count;

            double standartDeviation = Math.Sqrt((1 / (count - 1)) * (sumSquared - (1 / count * Math.Pow(sum, 2))));
            double output = standartDeviation / valueNow;
            if (valueNow == 0)
                output = 0;

            return new TimeValueData(timestampNow, output);
        }

        public override string getName()
        {
            return "SD_" + timeframe;
        }

        public override bool isValid(long timestamp)
        {
            if (history.Count == 0)
                return false;

            TimestampValuePair pair = history[0];
            if (timestamp - pair.timestamp > timeframe - (timeframe * 0.2)) //Ältester Datensatz ist älter als die (timeframe - 10% Timeframe) 
                return true;
            else
                return false;
        }
    }
}
