using NinjaTrader_Client.Trader.Analysis.Datamining.AI;
using NinjaTrader_Client.Trader.Datamining.AI;
using NinjaTrader_Client.Trader.Utils;
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
        /// <summary>
        /// Used with the "getOutcomeSampling" function
        /// </summary>
        public static double[] getPredictivePower(ConcurrentDictionary<double, OutcomeCountPair> dict)
        {
            Logger.log("Start", "getPredictivePower");

            double minSell = double.MaxValue, maxSell = double.MinValue;
            double minBuy = double.MaxValue, maxBuy = double.MinValue;

            double maxBuySellDistance = double.MinValue;

            Logger.log("Start foreach", "getPredictivePower");
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

            Logger.log("Done", "getPredictivePower");

            double curveScore = (maxBuy - minBuy) + (maxSell - minSell);
            return new double[] { curveScore, maxBuySellDistance };
        }

        /// <summary>
        /// Used with the "getOutcomeCodeSampling" function
        ///--> 1. Point over 0.5
        ///--> 2. Difference between buy and sell at any point
        ///--> 3. Comparing heighest with lowest point in curve
        /// </summary>
        public static double[] getPredictivePower(ConcurrentDictionary<double, OutcomeCodeCountPair> dict)
        {
            Logger.log("Start", "getPredictivePower");
            double theorem1Score = 0, theoreme2Score = 0, theoreme3Score = 0;

            double minSell = double.MaxValue, maxSell = double.MinValue;
            double minBuy = double.MaxValue, maxBuy = double.MinValue;

            double maxBuySellDistance = double.MinValue;

            Logger.log("First foreach", "getPredictivePower");
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
            Logger.log("Success", "getPredictivePower");
            return new double[] { theorem1Score, theoreme2Score, theoreme3Score };
        }

        public enum MLMethodForPPAnalysis { LogRegression, LinearRegression };

        public static double getPredictivePowerWithMl(double[][] trainingInput, double[][] trainingOutput, double[][] testInput, double[][] testOutput, MLMethodForPPAnalysis method)
        {
            Logger.log("Start", "getPredictivePowerWithMl " + method);

            IMachineLearning ml;

            if (method == MLMethodForPPAnalysis.LinearRegression)
                ml = new MyRegression(1);
            else if (method == MLMethodForPPAnalysis.LogRegression)
                ml = new MyLogisticRegression(1);
            else
                throw new Exception("No method given...");

            Logger.log("Train", "getPredictivePowerWithMl " + method);
            ml.train(trainingInput, trainingOutput);

            Logger.log("Get error", "getPredictivePowerWithMl " + method);
            return 1d / ml.getPredictionErrorFromData(testInput, testOutput);
        }
    }
}
