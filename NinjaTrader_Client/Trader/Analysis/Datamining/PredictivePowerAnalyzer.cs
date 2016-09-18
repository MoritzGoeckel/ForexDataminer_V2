using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NinjaTrader_Client.Trader.InRamDatamining;

namespace NinjaTrader_Client.Trader.Analysis.Datamining
{
    public static class PredictivePowerAnalyzer
    {
        //--> 1. Difference between buy and sell at any point
        //--> 2. Comparing heighest with lowest point in curve
        public static double getPredictivePower(ConcurrentDictionary<double, OutcomeCountPair> dict)
        {
            double minSell = double.MaxValue, maxSell = double.MinValue;
            double minBuy = double.MaxValue, maxBuy = double.MinValue;

            double maxBuySellDistance = double.MinValue;

            foreach (KeyValuePair<double, OutcomeCountPair> pair in dict)
            {
                double buyAvg = pair.Value.MaxSum / pair.Value.Count;
                double sellAvg = pair.Value.MinSum / pair.Value.Count;

                //Condition 2 (Strong direction of curve)
                if (sellAvg < minSell)
                    minSell = sellAvg;

                if (sellAvg > maxSell)
                    maxSell = sellAvg;

                if (buyAvg < minBuy)
                    minBuy = buyAvg;

                if (buyAvg > maxBuy)
                    maxBuy = buyAvg;

                //Condition 1 (Max difference between buy and sell)
                double difference = Math.Abs(buyAvg - sellAvg);
                if (difference > maxBuySellDistance)
                    maxBuySellDistance = difference;
            }

            double curveScore = (maxBuy - minBuy) + (maxSell - minSell);
            return curveScore + maxBuySellDistance * 2; //wight distance heigher
        }

        //--> 1. Point over 0.5
        //--> 2. Difference between buy and sell at any point
        //--> 3. Comparing heighest with lowest point in curve
        public static double getPredictivePower(ConcurrentDictionary<double, OutcomeCodeCountPair> dict)
        {
            double theorem1Score = 0, theoreme2Score = 0, theoreme3Score = 0;

            double minSell = double.MaxValue, maxSell = double.MinValue;
            double minBuy = double.MaxValue, maxBuy = double.MinValue;

            double maxBuySellDistance = double.MinValue;

            foreach (KeyValuePair<double, OutcomeCodeCountPair> pair in dict)
            {
                double buyAvg = pair.Value.buySum / pair.Value.Count;
                double sellAvg = pair.Value.sellSum / pair.Value.Count;

                //Condition 1 (Any point over 0.5)
                if (buyAvg > 0.5)
                    theorem1Score += (buyAvg - 0.5) * 2;

                if (sellAvg > 0.5)
                    theorem1Score += (sellAvg - 0.5) * 2;

                //Condition 3 (Strong direction of curve)
                if (sellAvg < minSell)
                    minSell = sellAvg;

                if (sellAvg > maxSell)
                    maxSell = sellAvg;

                if (buyAvg < minBuy)
                    minBuy = buyAvg;

                if (buyAvg > maxBuy)
                    maxBuy = buyAvg;

                //Condition 2 (Max difference between buy and sell)
                double difference = Math.Abs(buyAvg - sellAvg);
                if (difference > maxBuySellDistance)
                    maxBuySellDistance = difference;
            }

            //Theorem 3 (Curve)
            theoreme3Score += (maxBuy - minBuy);
            theoreme3Score += (maxSell - minSell);

            //Theorem 2 (difference)
            theoreme2Score += maxBuySellDistance;

            //caculate result 
            return theorem1Score * 2 + theoreme2Score + theoreme3Score; //wight theo1 heigher
        }

    }
}
