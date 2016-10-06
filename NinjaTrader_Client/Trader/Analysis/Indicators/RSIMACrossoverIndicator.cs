﻿using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.Indicators
{
    class RSIMACrossoverIndicator : WalkerIndicator
    {
        private long rsiTimeframe, signalTimeframe;
        private RSIIndicator rsi;
        private MovingAverageIndicator signalMa;

        private double lastDifference = double.NaN;

        public RSIMACrossoverIndicator(long rsiTimeframe, long signalTimeframe)
        {
            this.rsiTimeframe = rsiTimeframe;
            this.signalTimeframe = signalTimeframe;
            rsi = new RSIIndicator(rsiTimeframe);
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

            double tmpDiff = rsi.getIndicator().value - signalMa.getIndicator().value;
            if (tmpDiff != 0d)
                lastDifference = tmpDiff;

            rsi.setNextData(_timestamp, _value);
            signalMa.setNextData(_timestamp, rsi.getIndicator().value);
        }

        public override TimeValueData getIndicator()
        {
            double differenceNow = rsi.getIndicator().value - signalMa.getIndicator().value;

            double output;
            if (double.IsNaN(lastDifference) == false)
            {
                if (differenceNow > 0 && lastDifference < 0) //Ist positiv war negativ -> 1
                    output = 1;
                else if (differenceNow < 0 && lastDifference > 0) //Ist negativ war positiv -> 0
                    output = 0;
                else
                    output = 0.5; //War und ist positiv oder war und ist negativ
            }
            else
                output = 0.5;

            return new TimeValueData(timestampNow, output);
        }

        public override string getName()
        {
            return "RSIMACrossover_" + rsiTimeframe + "_" + signalTimeframe;
        }

        public override bool isValid(long timestamp)
        {
            return rsi.isValid(timestamp) && signalMa.isValid(timestamp) && double.IsNaN(lastDifference) == false;
        }
    }
}
