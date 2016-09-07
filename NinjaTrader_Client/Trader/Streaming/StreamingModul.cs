using NinjaTrader_Client.Trader.Analysis.IndicatorCollections;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Streaming.Strategies;
using NinjaTrader_Client.Trader.TradingAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Streaming
{
    class StreamingModul
    {
        IndicatorCollection indicators;
        Strategy strategy;
        ExecutionStrategy execStrat;
        ITradingAPI tradingApi;

        string instrument;

        public StreamingModul(IndicatorCollection indicators, Strategy strategy, ExecutionStrategy execStrat, ITradingAPI tradingApi, string instrument)
        {
            this.indicators = indicators;
            this.strategy = strategy;
            this.execStrat = execStrat;
            this.tradingApi = tradingApi;

            this.instrument = instrument;
        }

        long lastPushedTimestamp = 0;
        public void pushData(TickData newestTickdata)
        {
            if (newestTickdata.timestamp < lastPushedTimestamp)
                throw new Exception("Old data recieved!");

            lastPushedTimestamp = newestTickdata.timestamp;

            AdvancedTickData advancedTickdata = indicators.doCalculations(newestTickdata);

            StrategySignal signal = strategy.pushData(advancedTickdata);

            execStrat.doTrading(signal, tradingApi);
        }
    }
}
