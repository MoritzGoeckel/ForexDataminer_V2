using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class VolumeAtPriceIndicator : WalkerIndicator
    {
        private long timeframe, samplingRate;
        private double stepsize;
        private List<TimestampValuePair> history = new List<TimestampValuePair>();

        private Dictionary<int, int> priceOcurrencesDict = new Dictionary<int, int>();
        int entriesInOcurrencesDict = 0;
        private void modifyOccurancesAtValue(double value, int amount)
        {
            int nValue = Convert.ToInt32(value / stepsize);
            if (priceOcurrencesDict.ContainsKey(nValue) == false)
                priceOcurrencesDict.Add(nValue, 0);

            priceOcurrencesDict[nValue] += amount;
            entriesInOcurrencesDict += amount;

            if (priceOcurrencesDict[nValue] <= 0)
                priceOcurrencesDict.Remove(nValue);
        }

        private int getOccurancesAtValue(double value)
        {
            int nValue = Convert.ToInt32(value / stepsize);

            if (priceOcurrencesDict.ContainsKey(nValue))
                return priceOcurrencesDict[nValue];
            else
                return 0;
        }

        public VolumeAtPriceIndicator(long timeframe, double stepsize, long samplingRate)
        {
            this.timeframe = timeframe;
            this.stepsize = stepsize;
            this.samplingRate = samplingRate;
        }
            
        long timestampNow;
        double lastAddedValue;
        public override void setNextData(long _timestamp, double _value)
        {
            if (_timestamp < timestampNow)
                throw new Exception("Cant add older data here!");

            if (_timestamp == timestampNow && _value != lastAddedValue)
                throw new Exception("Same timestamp different value!");

            if (_timestamp == timestampNow && _value == lastAddedValue)
                throw new Exception("Already added");

            if (_timestamp - timestampNow < samplingRate) //Only add values every sampling rate ticks
                return;

            history.Add(new TimestampValuePair { timestamp = _timestamp, value = _value });
            modifyOccurancesAtValue(_value, 1);

            timestampNow = _timestamp;
            lastAddedValue = _value;
        }

        public override TimeValueData getIndicator()
        {
            while (history.Count != 0 && history[0].timestamp < timestampNow - timeframe)
            {
                modifyOccurancesAtValue(history[0].value, -1);
                history.RemoveAt(0);
            }

            double value = Convert.ToDouble(getOccurancesAtValue(lastAddedValue)) / Convert.ToDouble(entriesInOcurrencesDict);
            if (value < 0 || value > 1)
                throw new Exception("Value hast to be between 0 and 1. = " + getOccurancesAtValue(lastAddedValue) + " / " + entriesInOcurrencesDict + " " + value);

            if (entriesInOcurrencesDict != history.Count)
                throw new Exception("entriesInOcurrencesDict != history.count " + entriesInOcurrencesDict + " != " + history.Count + " should be same!");

            return new TimeValueData(timestampNow, value);
        }

        public override string getName()
        {
            return "VolumeAtPrice_" + timeframe + "_" + stepsize;
        }
    }
}
