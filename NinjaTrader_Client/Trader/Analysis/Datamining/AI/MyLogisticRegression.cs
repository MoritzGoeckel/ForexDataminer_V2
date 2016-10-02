using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
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
    class MyLogisticRegression : IMachineLearning
    {
        LogisticRegression logistic;
        IterativeReweightedLeastSquares teacher;
        int inputsCount;

        public MyLogisticRegression(int inputsCount)
        {
            this.inputsCount = inputsCount;
            logistic = new LogisticRegression(inputs: inputsCount);
            teacher = new IterativeReweightedLeastSquares(logistic);
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

        public AISignal getPrediction(double[] input)
        {
            return new AISignal(logistic.Compute(input));
        }

        public void load(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            string xmlString = xmlDocument.OuterXml;

            using (StringReader read = new StringReader(xmlString))
            {
                Type outType = typeof(LogisticRegression);

                XmlSerializer serializer = new XmlSerializer(outType);
                using (XmlReader reader = new XmlTextReader(read))
                {
                    logistic = (LogisticRegression)serializer.Deserialize(reader);
                    reader.Close();
                }

                read.Close();
            }
        }

        public void save(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(logistic.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, logistic);
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

            for (int i = 0; i < epochs; i++)
                error = teacher.Run(input, output);
        }

        public double validateOnData(double[][] input, double[][] output)
        {
            return logistic.GetDeviance(input, output);
        }
    }
}
