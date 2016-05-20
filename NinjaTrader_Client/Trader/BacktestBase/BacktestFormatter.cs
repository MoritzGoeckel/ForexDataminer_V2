﻿using NinjaTrader_Client.Trader.Backtest;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Strategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NinjaTrader_Client.Trader.BacktestBase
{
    class BacktestFormatter
    {
        public static string getCSVHeader(BacktestData data)
        {
            return getCSVHeader(data.getParameters(), data.getResult());
        }

        public static string getCSVHeader(Dictionary<string, string> parameterSet, Dictionary<string, string> resultSet)
        {
            string output = "";
            foreach (KeyValuePair<string, string> pair in parameterSet)
                output += pair.Key + ";";

            foreach (KeyValuePair<string, string> pair in resultSet)
                output += pair.Key + ";";

            output += "stringCodedParams;stringCodedPositions;stringCodedResult;";

            return output;
        }

        public static string getCSVLine(BacktestData data)
        {
            StringBuilder output = new StringBuilder("");

            foreach (KeyValuePair<string, string> pair in data.getParameters())
                output.Append(pair.Value + ";");

            foreach (KeyValuePair<string, string> pair in data.getResult())
                output.Append(pair.Value + ";");

            output.Append(BacktestFormatter.getDictStringCoded(data.getParameters()) + ";");
            output.Append(BacktestFormatter.getStringCodedPositionHistory(data.getPositions()) + ";");
            output.Append(BacktestFormatter.getDictStringCoded(data.getResult()) + ";");

            return output.ToString();
        }

        public static string getStringCodedPositionHistory(List<TradePosition> positions)
        {
            StringBuilder output = new StringBuilder("");

            if (positions.Count < 1000)
            {
                foreach (TradePosition pos in positions)
                    output.Append((pos.type == TradePosition.PositionType.longPosition ? "L" : "S") + ":" + pos.timestampOpen + ":" + pos.priceOpen + ":" + pos.timestampClose + ":" + pos.priceClose + ":" + pos.instrument + "|");
            }
            else
                output.Append("More than 1000");

            return output.ToString();
        }

        public static List<TradePosition> getPositionHistoryFromCodedString(string str)
        {
            List<string> positions = new List<string>();
            positions.AddRange(str.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

            List<TradePosition> positionList = new List<TradePosition>();

            foreach (string posString in positions)
            {
                try
                {
                    string[] posArray = posString.Split(':');
                    positionList.Add(new TradePosition((posArray[0] == "L" ? TradePosition.PositionType.longPosition : TradePosition.PositionType.shortPosition), long.Parse(posArray[1]), double.Parse(posArray[2]), long.Parse(posArray[3]), double.Parse(posArray[4]), posArray[5]));
                }
                catch (Exception){ }
            }

            return positionList;
        }

        public static string getResultText(BacktestData data)
        {
            return getResultText(data.getResult());
        }

        public static string getResultText(Dictionary<string, string> resultSet)
        {
            string output = "";
            foreach (KeyValuePair<string, string> pair in resultSet)
                output += pair.Key + ": " + pair.Value + Environment.NewLine;
            return output;
        }

        public static string getPositionsText(BacktestData data)
        {
            return getPositionsText(data.getPositions());
        }

        public static string getPositionsText(List<TradePosition> trades)
        {
            string output = "";
            foreach (TradePosition position in trades)
                output += Math.Round(position.getDifference(), 4) + Environment.NewLine;

            return output;
        }

        public static string getParameterText(BacktestData data)
        {
            return getParameterText(data.getParameters());
        }

        public static string getParameterText(Dictionary<string, string> parameterSet)
        {
            string output = "";
            foreach (KeyValuePair<string, string> pair in parameterSet)
                output += pair.Key + ": " + pair.Value + Environment.NewLine;
            return output;
        }

        public static string getParameterStringCoded(BacktestData data)
        {
            return getDictStringCoded(data.getParameters());
        }

        public static string getDictStringCoded(Dictionary<string, string> parameterSet)
        {
            string output = "";
            foreach (KeyValuePair<string, string> pair in parameterSet)
                output += pair.Key + ":" + pair.Value + "|";
            return output.Substring(0, output.Length - 1);
        }

        public static List<string> getParametersFromFile(string path)
        {
            List<string> parameterList = new List<string>();
            string[] file = File.ReadAllText(path).Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in file)
            {
                if (line.StartsWith("//") == false && line != "")
                    parameterList.Add(line);
            }

            //Test if file is valid
            foreach (string parameter in parameterList)
            {
                try
                {
                    convertStringCodedToParameters(parameter);
                }
                catch { throw new Exception("DedicatedStrategyBacktestForm Constructor: Invalid parameter->" + parameter); }
            }
            return parameterList;
        }

        public static void getStrategyFromString(Database database, string parameters, ref Strategy strategy)
        {
            Dictionary<string, string> parameterDict = convertStringCodedToParameters(parameters);

            string name = null;
            if (parameterDict.ContainsKey("strategy"))
                name = parameterDict["strategy"];
            else
                name = parameterDict["name"];

            if (name.StartsWith("SSIStochStrategy"))
                strategy = new SSIStochStrategy(database, parameterDict);

            if (name.StartsWith("MomentumStrategy"))
                strategy = new MomentumStrategy(database, parameterDict);

            if (name.StartsWith("SSIStrategy"))
                strategy = new SSIStrategy(database, parameterDict);

            if (name.StartsWith("StochStrategy"))
                strategy = new StochStrategy(database, parameterDict);

            if (strategy == null)
                throw new Exception("Strategy not deserializable: " + name);
        }

        public static Dictionary<string, string> convertStringCodedToParameters(string str)
        {
            List<string> parameters = new List<string>();
            parameters.AddRange(str.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

            Dictionary<string, string> paramDict = new Dictionary<string,string>();

            foreach(string parameter in parameters)
            {
                string[] pair = parameter.Split(':');
                paramDict.Add(pair[0], pair[1]);
            }

            return paramDict;
        }
    }
}
