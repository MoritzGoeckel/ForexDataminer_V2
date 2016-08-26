using NinjaTrader_Client.Trader.Analysis.IndicatorCollections;
using NinjaTrader_Client.Trader.Indicators;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Streaming.Strategies;
using NinjaTrader_Client.Trader.TradingAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader
{
    class TradingDataStreamProcessor
    {
        Strategy strategy;
        ITradingAPI api;

        public TradingDataStreamProcessor(Strategy strategy, ITradingAPI api)
        {
            this.strategy = strategy;
            this.api = api;
        }

        public void setNewestData(DataminingTickdata data)
        {
            //Calculate indicators
            data = strategy.getIndicatorCollection().doCalculations(data);

            //Apply strategy
            strategy.doTick(data, api);
        }

        public void setNewestData(Tickdata data)
        {
            setNewestData(data.ToDataminingTickdata());
        }
    }
}
