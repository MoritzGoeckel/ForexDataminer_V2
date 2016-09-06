using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Streaming.Strategies
{
    public class StrategySignal
    {
        private double signal;
        public StrategySignal(double signal)
        {
            if (signal > 1 || signal < -1)
                throw new Exception("StrategySignal has to be between 1 for buy to -1 for sell");

            this.signal = signal;
        }

        public double getSignal()
        {
            return signal;
        }
    }
}
