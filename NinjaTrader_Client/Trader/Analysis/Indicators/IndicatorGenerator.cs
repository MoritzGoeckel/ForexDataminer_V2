using NinjaTrader_Client.Trader.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Analysis.Indicators
{
    public static class IndicatorGenerator
    {
        private static Random z = new Random();
        public static WalkerIndicator getRandomIndicator()
        {
            WalkerIndicator theIndicator = null;

            long timeframeOne = z.Next(1, 2880) * 60 * 1000;
            long timeframeTwo = z.Next(1, 2880) * 60 * 1000;
            long timeFrameThree = z.Next(1, 2880) * 60 * 1000;

            long timeFrameSmaller = z.Next(1, 2880) * 60 * 1000;

            switch (z.Next(0, 6))
            {
                case 0:
                    theIndicator = new BolingerBandsIndicator(timeframeOne, getRanDouble(1d, 5d));
                    break;

                case 1:
                    theIndicator = new MACDContinousIndicator(timeframeOne, timeframeTwo, timeFrameSmaller);
                    break;

                case 2:
                    theIndicator = new MACDIndicator(timeframeOne, timeframeTwo, timeFrameSmaller);
                    break;

                    //Todo: add the other indicators

                default:
                    throw new Exception("Fired a unexpected random value");
            }

            return theIndicator;
        }

        private static double getRanDouble(double min, double max)
        {
            return min + z.NextDouble() * (max - min); 
        }
    }
}
