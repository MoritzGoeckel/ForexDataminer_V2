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
            switch(z.Next(1, 6))
            {
                case 1:
                    theIndicator = new MovingAverageIndicator(z.Next(1, 2880) * 60 * 1000);
                    break;
                case 2:
                    theIndicator = new RangeIndicator(z.Next(1, 2880) * 60 * 1000);
                    break;
                case 3:
                    theIndicator = new StandartDeviationIndicator(z.Next(1, 2880) * 60 * 1000);
                    break;
                case 4:
                    theIndicator = new StochIndicator(z.Next(1, 2880) * 60 * 1000);
                    break;
                case 5:
                    theIndicator = new TradingTimeIndicator(); //Not much variation... todo
                    break;
                case 6:
                    theIndicator = new VolumeAtPriceIndicator(z.Next(1, 2880) * 60 * 1000, z.NextDouble() * 0.01, z.Next(1, 10) * 30 * 1000);
                    break;

                    //Todo: More indicators :)

                default:
                    throw new Exception("Fired a unexpected random value");
            }

            return theIndicator;
        }
    }
}
