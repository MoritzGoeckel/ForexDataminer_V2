using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class RSIIndicator : WalkerIndicator
    {
        private long timeframe;

        private List<TimestampValuePair> positiveChanges = new List<TimestampValuePair>();
        private double positiveChangesSum = 0;

        private List<TimestampValuePair> negativeChanges = new List<TimestampValuePair>();
        private double negativeChangesSum = 0;

        private double lastSeenPrice;

        public RSIIndicator(long timeframe)
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

            double difference = _value - lastSeenPrice;
            if (difference > 0)
            {
                positiveChanges.Add(new TimestampValuePair { timestamp = _timestamp, value = difference });
                positiveChangesSum += difference;
            }

            if (difference < 0)
            {
                negativeChanges.Add(new TimestampValuePair { timestamp = _timestamp, value = difference });
                negativeChangesSum += difference;
            }

            lastSeenPrice = _value;

            timestampNow = _timestamp;
            valueNow = _value;
        }

        public override TimeValueData getIndicator()
        {
            while (positiveChanges.Count != 0 && positiveChanges[0].timestamp < timestampNow - timeframe)
            {
                positiveChangesSum -= positiveChanges[0].value;
                positiveChanges.RemoveAt(0);
            }

            while (negativeChanges.Count != 0 && negativeChanges[0].timestamp < timestampNow - timeframe)
            {
                negativeChangesSum -= negativeChanges[0].value;
                negativeChanges.RemoveAt(0);
            }

            int n = positiveChanges.Count + negativeChanges.Count;

            double nChangesAvg = negativeChangesSum / n;
            double pChangesAvg = positiveChangesSum / n;

            return new TimeValueData(timestampNow, pChangesAvg / (pChangesAvg + nChangesAvg)); 
        }

        public override string getName()
        {
            return "RSI_" + timeframe;
        }

        public override bool isValid(long timestamp)
        {
            if (positiveChanges.Count + negativeChanges.Count == 0)
                return false;

            TimestampValuePair oldestPair;
            if (positiveChanges.Count != 0)
                oldestPair = positiveChanges[0];
            else
                oldestPair = negativeChanges[0];

            if (negativeChanges[0].timestamp < oldestPair.timestamp)
                oldestPair = negativeChanges[0];

            if (timestamp - oldestPair.timestamp > timeframe - (timeframe * 0.2)) //Ältester Datensatz ist älter als die (timeframe - 10% Timeframe) 
                return true;
            else
                return false;
        }
    }
}
