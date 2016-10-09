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

            switch (z.Next(0, 18)) //Todo: set max value for choosing the indicator
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

                case 3:
                    theIndicator = new MovingAveragePriceSubtractionIndicator(timeframeOne);
                    break;

                case 4:
                    theIndicator = new MovingAverageSubtractionCrossoverIndicator(timeframeOne, timeframeTwo);
                    break;

                case 5:
                    theIndicator = new MovingAverageSubtractionIndicator(timeframeOne, timeframeTwo);
                    break;

                case 6:
                    theIndicator = new RangeIndicator(timeframeOne);
                    break;

                case 7:
                    theIndicator = new RSIBorderCrossoverIndicator(timeframeOne, getRanDouble(0.1, 0.4));
                    break;

                case 8:
                    theIndicator = new RSIBorderIndicator(timeframeOne, getRanDouble(0.1, 0.4));
                    break;

                case 9:
                    theIndicator = new RSIIndicator(timeframeOne);
                    break;

                case 10:
                    theIndicator = new RSIMACrossoverContinousIndicator(timeframeOne, timeFrameSmaller);
                    break;

                case 11:
                    theIndicator = new RSIMACrossoverIndicator(timeframeOne, timeFrameSmaller);
                    break;

                case 12:
                    theIndicator = new StandartDeviationIndicator(timeframeOne);
                    break;

                case 13:
                    theIndicator = new StochBorderCrossoverIndicator(timeframeOne, getRanDouble(0.1, 0.4));
                    break;

                case 14:
                    theIndicator = new StochBorderIndicator(timeframeOne, getRanDouble(0.1, 0.4));
                    break;

                case 15:
                    theIndicator = new StochIndicator(timeframeOne);
                    break;

                case 16:
                    theIndicator = new VolumeAtPriceIndicator(timeframeOne, getRanDouble(0.0003, 0.002), z.Next(1000 * 30, 1000 * 60 * 10));
                    break; //Not sure about stepsize todo

                case 17:
                    theIndicator = new TimeOfDayIndicator(); //Only once?
                    break;

                case 18:
                    theIndicator = new TimeDayOfWeekIndicator(); //Todo: Only once?
                    break;

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
