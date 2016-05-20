using NinjaTrader_Client.Trader.Backtest;
using NinjaTrader_Client.Trader.BacktestBase.Visualization;
using NinjaTrader_Client.Trader.Indicators;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Strategies
{
    public class StochStrategy_OneSide : Strategy
    {
        int version = 0;

        private double percentTakeprofit, percentStoploss;

        double minBuy, maxBuy, minSell, maxSell;

        private int stochTimeframe;
        private int hitSL = 0, hitTP = 0;

        private WalkerIndicator stochIndicator;
        private WalkerIndicator tradingTime;

        private string instrument;
        
        public StochStrategy_OneSide(Database database, string instrument, double percentStoploss, double percentTakeprofit, int stochTimeframe, double minBuy, double maxBuy, double minSell, double maxSell)
            : base(database)
        {
            this.instrument = instrument;
            this.minBuy = minBuy;
            this.maxBuy = maxBuy;
            this.minSell = minSell;
            this.maxSell = maxSell;

            this.percentStoploss = percentStoploss;
            this.percentTakeprofit = percentTakeprofit;
            this.stochTimeframe = stochTimeframe;

            stochIndicator = new StochIndicator(stochTimeframe);
            tradingTime = new TradingTimeIndicator();

            setupVisualizationData();
        }

        public StochStrategy_OneSide(Database database, Dictionary<string, string> parameters) 
            : base(database)
        {
            percentStoploss = Double.Parse(parameters["sl"]);
            percentTakeprofit = Double.Parse(parameters["tp"]);
            stochTimeframe = Convert.ToInt32(parameters["time"]);

            minBuy = Double.Parse(parameters["minBuy"]);
            maxBuy = Double.Parse(parameters["maxBuy"]);

            minSell = Double.Parse(parameters["minSell"]);
            maxSell = Double.Parse(parameters["maxSell"]);

            instrument = parameters["instrument"];
            
            stochIndicator = new StochIndicator(stochTimeframe);

            setupVisualizationData();
        }

        private BacktestVisualizationDataComponent stochUpperBorder_vi_buy, stochLowerBorder_vi_buy, stochUpperBorder_vi_sell, stochLowerBorder_vi_sell, price_vi, uptodate_vi, tradingTimeCode_vi, stoch_vi, sl_vi, tp_vi;

        public override void setupVisualizationData()
        {
            stochUpperBorder_vi_buy = visualizationData.addComponent(new BacktestVisualizationDataComponent("stochUpperBorderBuy", BacktestVisualizationDataComponent.VisualizationType.ZeroToOne, 1, maxBuy));
            stochLowerBorder_vi_buy = visualizationData.addComponent(new BacktestVisualizationDataComponent("stochLowerBorderBuy", BacktestVisualizationDataComponent.VisualizationType.ZeroToOne, 1, minBuy));

            stochUpperBorder_vi_sell = visualizationData.addComponent(new BacktestVisualizationDataComponent("stochUpperBorderSell", BacktestVisualizationDataComponent.VisualizationType.ZeroToOne, 1, maxSell));
            stochLowerBorder_vi_sell = visualizationData.addComponent(new BacktestVisualizationDataComponent("stochLowerBorderSell", BacktestVisualizationDataComponent.VisualizationType.ZeroToOne, 1, minSell));

            price_vi = visualizationData.addComponent(new BacktestVisualizationDataComponent("price", BacktestVisualizationDataComponent.VisualizationType.OnChart, 0));
            uptodate_vi = visualizationData.addComponent(new BacktestVisualizationDataComponent("uptodate", BacktestVisualizationDataComponent.VisualizationType.TrafficLight, 2));
            tradingTimeCode_vi = visualizationData.addComponent(new BacktestVisualizationDataComponent("tradingTimeCode", BacktestVisualizationDataComponent.VisualizationType.TrafficLight, 3));
            stoch_vi = visualizationData.addComponent(new BacktestVisualizationDataComponent("stoch", BacktestVisualizationDataComponent.VisualizationType.ZeroToOne, 1));

            //sl_vi = visualizationData.addComponent(new BacktestVisualizationDataComponent("sl", BacktestVisualizationDataComponent.VisualizationType.OnChart, 0)); //??? Wird nicht gesetzt ???
            //tp_vi = visualizationData.addComponent(new BacktestVisualizationDataComponent("tp", BacktestVisualizationDataComponent.VisualizationType.OnChart, 0));
        }

        public override Strategy copy()
        {
            return new StochStrategy_OneSide(database, instrument, percentStoploss, percentTakeprofit, stochTimeframe, minBuy, maxBuy, minSell, maxSell);
        }

        public override string getName()
        {
            return "StochStrategy_OneSide" + "_V" + version;
        }

        public override Dictionary<string, string> getParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("tp", percentTakeprofit.ToString());
            parameters.Add("sl", percentStoploss.ToString());
            parameters.Add("time", stochTimeframe.ToString());

            parameters.Add("minBuy", minBuy.ToString());
            parameters.Add("maxBuy", maxBuy.ToString());
            parameters.Add("minSell", minSell.ToString());
            parameters.Add("maxSell", maxSell.ToString());

            parameters.Add("instrument", instrument);

            parameters.Add("name", getName());

            return parameters;
        }

        public override Dictionary<string, string> getResult()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("hitSl", hitSL.ToString());
            result.Add("hitTp", hitTP.ToString());

            return result;
        }

        public override void doTick()
        {
            Tickdata nowTick = new Tickdata(api.getNow(), 0, api.getBid(instrument), api.getAsk(instrument));
            price_vi.value = nowTick.getAvgPrice();
            
            bool isUptodate = api.isUptodate(instrument);
            uptodate_vi.value = isUptodate ? 1 : 0;
            if (isUptodate == false)
                return;

            double tradingTimeCode = tradingTime.getIndicator(api.getNow(), 0).value;
            tradingTimeCode_vi.value = tradingTimeCode;
            if (tradingTimeCode == 0)
            {
                //Flatten everything
                api.closePositions(instrument);
                return;
            }

            double takeprofit = api.getAvgPrice(instrument) * percentTakeprofit / 100d;
            double stoploss = api.getAvgPrice(instrument) * percentStoploss / 100d;

            TimeValueData stochTick = stochIndicator.getIndicator(api.getNow(), nowTick.getAvgPrice());
            stoch_vi.value = stochTick.value;

            double stochValue = stochTick.value;

            if(tradingTimeCode == 1)
            {
                if (stochValue >= minSell && stochValue <= maxSell && api.getShortPosition(instrument) == null)
                {
                    api.openShort(instrument);
                }

                if (stochValue >= minBuy && stochValue <= maxBuy && api.getLongPosition(instrument) == null)
                {
                    api.openLong(instrument);
                }
            }
            

            if (api.getShortPosition(instrument) != null && api.getShortPosition(instrument).getDifference(nowTick) > takeprofit)
            {
                api.closeShort(instrument);
                hitTP++;
            }

            if (api.getLongPosition(instrument) != null && api.getLongPosition(instrument).getDifference(nowTick) > takeprofit)
            {
                api.closeLong(instrument);
                hitTP++;
            }

            if (api.getShortPosition(instrument) != null && api.getShortPosition(instrument).getDifference(nowTick) < -stoploss)
            {
                api.closeShort(instrument);
                hitSL++;
            }

            if (api.getLongPosition(instrument) != null && api.getLongPosition(instrument).getDifference(nowTick) < -stoploss)
            {
                api.closeLong(instrument);
                hitSL++;
            }
        }

        public override List<string> getUsedPairs()
        {
            return new List<string> { instrument };
        }

        public override long getCatchUpTime()
        {
            return stochTimeframe;
        }
    }
}
