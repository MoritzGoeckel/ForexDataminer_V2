using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Streaming.Strategies
{
    public class StrategySignal
    {
        private double buy, sell;
        public StrategySignal(double buy, double sell)
        {
            this.buy = buy;
            this.sell = sell;
        }
        public StrategySignal(double[] buySell)
        {
            this.buy = buySell[0];
            this.sell = buySell[1];
        }

        private void checkBuySellValue(double d)
        {
            if (d > 1 || d < 0)
                throw new Exception("Buy / sell signal has wrong value: " + d);
        }

        public double getBuy()
        {
            return buy;
        }
        public double getSell()
        {
            return sell;
        }

        [Obsolete("This method is destroying information. Use getBuy() and getSell() instead")]
        public double getSignal()
        {
            return getBuy() - getSell(); //Todo: Not really helpfull because info gets destroyed...
        }
    }
}
