using Accord.Math.Optimization.Losses;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using Accord.Statistics.Models.Regression.Linear;
using NinjaTrader_Client.Trader.Datamining.AI;
using NinjaTrader_Client.Trader.Streaming.Strategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NinjaTrader_Client.Trader.Analysis.Datamining.AI
{
    class MyRegression : IMachineLearning
    {
        MultipleLinearRegression logisticBuy;
        OrdinaryLeastSquares teacherBuy;

        MultipleLinearRegression logisticSell;
        OrdinaryLeastSquares teacherSell;

        int inputsCount;

        public MyRegression(int inputsCount)
        {
            this.inputsCount = inputsCount;
            logisticBuy = new MultipleLinearRegression();
            logisticBuy.NumberOfInputs = inputsCount;
            logisticBuy.NumberOfOutputs = 1;
            teacherBuy = new OrdinaryLeastSquares() { UseIntercept = true };

            logisticSell = new MultipleLinearRegression();
            logisticSell.NumberOfInputs = inputsCount;
            logisticSell.NumberOfOutputs = 1;
            teacherSell = new OrdinaryLeastSquares() { UseIntercept = true };
        }

        public double getError()
        {
            return error;
        }

        public string[] getInfo(string[] inputFieldName, string outputFieldName)
        {
            throw new Exception("No info for the logistic regression. They are all the same :)");
        }

        public string getInfoString()
        {
            throw new Exception("No info for the logistic regression. They are all the same :)");
        }

        public StrategySignal getPrediction(double[] input)
        {
            return new StrategySignal(logisticBuy.Transform(input), logisticSell.Transform(input));
        }

        private class BuySellRegressionPair
        {
            public MultipleLinearRegression buy, sell;
        }

        public void load(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            string xmlString = xmlDocument.OuterXml;

            using (StringReader read = new StringReader(xmlString))
            {
                Type outType = typeof(BuySellRegressionPair);

                XmlSerializer serializer = new XmlSerializer(outType);
                using (XmlReader reader = new XmlTextReader(read))
                {
                    BuySellRegressionPair pair = (BuySellRegressionPair)serializer.Deserialize(reader);

                    logisticBuy = pair.buy;
                    logisticSell = pair.sell;

                    reader.Close();
                }

                read.Close();
            }
        }

        public void save(string path)
        {
            BuySellRegressionPair pair = new BuySellRegressionPair();
            pair.buy = logisticBuy;
            pair.sell = logisticSell;

            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(pair.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, pair);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(path);
                stream.Close();
            }
        }

        double error;
        public void train(double[][] input, double[][] output, int epochs = 1)
        {
            if (input[0].Length != inputsCount)
                throw new Exception("Input has a unexpected length: " + input[0].Length + "!=" + inputsCount);

            double[] buyOutput = new double[output.Length];
            double[] sellOutput = new double[output.Length];

            for(int i = 0; i < output.Length; i++)
            {
                buyOutput[i] = output[i][0];
                sellOutput[i] = output[i][1];
            }

            for (int i = 0; i < epochs; i++)
            {
                teacherBuy.Learn(input, buyOutput);
                teacherSell.Learn(input, sellOutput);
            }
        }

        public double getPredictionErrorFromData(double[][] input, double[][] output)
        {
            //Auseinander friemeln
            double[] buyOutput = new double[output.Length];
            double[] sellOutput = new double[output.Length];

            for (int i = 0; i < output.Length; i++)
            {
                buyOutput[i] = output[i][0];
                sellOutput[i] = output[i][1];
            }

            //Predict
            double[] predictedBuys = logisticBuy.Transform(input);
            double[] predictedSells = logisticSell.Transform(input);

            //Zusammenbauen
            double[][] prediction = new double[predictedBuys.Length][];
            for (int i = 0; i < prediction.Length; i++)
            {
                prediction[i] = new double[] { predictedBuys[i], predictedSells[i] };
            }
            
            return new SquareLoss(output).Loss(prediction);
        }
    }
}
