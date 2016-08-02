﻿using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class RangeIndicator : WalkerIndicator
    {
        private long timeframe;
        private List<TimestampValuePair> history = new List<TimestampValuePair>();

        public RangeIndicator(long timeframe)
        {
            this.timeframe = timeframe;
        }

        private void getMinMaxInPrices(ref double min, ref double max, List<TimestampValuePair> data)
        {
            min = double.MaxValue;
            max = double.MinValue;

            foreach (TimestampValuePair tick in data)
            {
                if (tick.value > max)
                    max = tick.value;

                if (tick.value < min)
                    min = tick.value;
            }

            if (data.Count == 0 || min == double.MaxValue || max == double.MinValue)
                throw new Exception("StochIndicator getMinMaxInData<TickData>: data.Count == " + data.Count + " or min->" + (min == double.MaxValue ? "ns" : "okay") + " or max->" + (max == double.MinValue ? "ns" : "okay"));
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

            history.Add(new TimestampValuePair { timestamp = _timestamp, value = _value });
            timestampNow = _timestamp;
            valueNow = _value;

            if (valueNow > cachedMax)
                cachedMax = valueNow;

            if (valueNow < cachedMin)
                cachedMin = valueNow;
        }

        double cachedMin = double.MaxValue, cachedMax = double.MinValue;
        public override TimeValueData getIndicator()
        {
            while (history.Count != 0 && history[0].timestamp < timestampNow - timeframe)
            {
                if (history[0].value == cachedMin)
                    cachedMin = double.MaxValue;

                if (history[0].value == cachedMax)
                    cachedMax = double.MinValue;

                history.RemoveAt(0);
            }

            if(cachedMax == double.MinValue || cachedMin == double.MaxValue && history.Count != 0)
            {
                getMinMaxInPrices(ref cachedMin, ref cachedMax, history);
            }

            double max = cachedMax;
            double min = cachedMin;

            double output;

            if (history.Count == 0 || max == min)
                output = -1; //Invalid
            else
                output = (max - min) / valueNow;

            return new TimeValueData(timestampNow, output);
        }

        public override string getName()
        {
            return "Range_" + timeframe;
        }
    }
}
