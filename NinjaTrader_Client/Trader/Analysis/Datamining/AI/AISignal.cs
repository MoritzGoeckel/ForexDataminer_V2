using NinjaTrader_Client.Trader.Streaming.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Analysis.Datamining.AI
{
    public class AISignal
    {
        private double buy, sell;
        public AISignal(double buy, double sell)
        {
            this.buy = buy;
            this.sell = sell;
        }
        public AISignal(double[] buySell)
        {
            this.buy = buySell[0];
            this.sell = buySell[1];
        }
        public double getBuy()
        {
            return buy;
        }
        public double getSell()
        {
            return sell;
        }

        public StrategySignal getSignal()
        {
            return new StrategySignal(getBuy() - getSell());
        }
    }
}
