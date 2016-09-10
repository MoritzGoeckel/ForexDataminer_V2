using NinjaTrader_Client.Trader.TradingAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Streaming.Strategies
{
    public abstract class ExecutionStrategy
    {
        public abstract void doTrading(StrategySignal currentSignal, ITradingAPI api);
        public abstract string getName();
    }
}
