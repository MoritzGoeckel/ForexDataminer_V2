using NinjaTrader_Client.Trader.Analysis.IndicatorCollections;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Streaming.Strategies;
using NinjaTrader_Client.Trader.TradingAPIs;
using NinjaTrader_Client.Trader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Streaming
{
    //Todo: Handle Data like SSI
    class StreamingModul
    {
        IndicatorCollection indicators;
        Strategy strategy;
        ExecutionStrategy execStrat;
        ITradingAPI tradingApi;

        string instrument;

        TickData lastRecievedTickdata;

        public StreamingModul(IndicatorCollection indicators, Strategy strategy, ExecutionStrategy execStrat, ITradingAPI tradingApi, string instrument)
        {
            this.indicators = indicators;
            this.strategy = strategy;
            this.execStrat = execStrat;
            this.tradingApi = tradingApi;

            this.instrument = instrument;
        }

        public void doTradingTickWithoutNewData(long timestamp)
        {
            TickData fakeTickdata = new TickData(timestamp, lastRecievedTickdata.last, lastRecievedTickdata.bid, lastRecievedTickdata.ask, lastRecievedTickdata.instrument);

            StrategySignal signal = pushDataGetSignal(fakeTickdata);
            execStrat.doTrading(signal, tradingApi);
        }

        public void pushDataAndTrade(TickData newestTickdata)
        {
            StrategySignal signal = pushDataGetSignal(newestTickdata);
            execStrat.doTrading(signal, tradingApi);
        }

        public void prepareDataWithoutTrading(TickData newestTickdata)
        {
            pushDataGetSignal(newestTickdata);
        }

        private StrategySignal pushDataGetSignal(TickData newestTickdata)
        {
            if (lastRecievedTickdata != null && newestTickdata.timestamp < lastRecievedTickdata.timestamp)
                throw new Exception("Old data recieved!");

            if (newestTickdata.instrument != instrument)
                throw new Exception("Wrong instrument for this streamer: " + newestTickdata.instrument + " != " + instrument);

            lastRecievedTickdata = new TickData(newestTickdata.timestamp, newestTickdata.last, newestTickdata.bid, newestTickdata.ask, newestTickdata.instrument);

            AdvancedTickData advancedTickdata = indicators.doCalculations(newestTickdata);

            lastAdvancedTickdata = advancedTickdata;
            lastSignal = strategy.pushData(advancedTickdata);

            return lastSignal;
        }

        private AdvancedTickData lastAdvancedTickdata;
        private StrategySignal lastSignal;

        public string getInfo()
        {
            string output = "Strat: " + strategy.getName() + Environment.NewLine +
                "Exec: " + execStrat.getName() + Environment.NewLine + Environment.NewLine;

            output += "Indicators: " + Environment.NewLine;
            foreach(KeyValuePair<string, double> pair in lastAdvancedTickdata.values)
            {
                output += pair.Key + " -> " + pair.Value + Environment.NewLine;
            }

            output += Environment.NewLine;
            output += "Signal: " + lastSignal.getSignal();
            output += Timestamp.getNow() - lastAdvancedTickdata.timestamp + "S ago";

            return output;
        }
    }
}
