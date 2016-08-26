using NinjaTrader_Client.Trader.Analysis.IndicatorCollections;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.TradingAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Streaming.Strategies
{
    public abstract class Strategy
    {
        public abstract void doTick(DataminingTickdata data, ITradingAPI api);
        public abstract IndicatorCollection getIndicatorCollection();
        public abstract string getName();
        public abstract Dictionary<string, string> getResult();
        public abstract Dictionary<string, string> getParameters();
    }
}
