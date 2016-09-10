using NinjaTrader_Client.Trader;
using NinjaTrader_Client.Trader.Analysis.IndicatorCollections;
using NinjaTrader_Client.Trader.BacktestBase;
using NinjaTrader_Client.Trader.BacktestBase.Visualization;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Streaming;
using NinjaTrader_Client.Trader.Streaming.Strategies;
using NinjaTrader_Client.Trader.TradingAPIs;
using NinjaTrader_Client.Trader.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NinjaTrader_Client
{
    public partial class AutopilotTradingForm : Form
    {
        private bool continueLiveTradingThread = false;

        private StreamingModul streamer;

        private SSI_Downloader ssiApi;
        private FXCMRatesDownloader webApi;

        private SQLiteDatabase priceHistoryDatabase;

        private Dictionary<string, double> prices = new Dictionary<string, double>();
        private string lastUpdatedPair = null;
        int insertedSets = 0;

        public AutopilotTradingForm(SQLiteDatabase db, ITradingAPI tradingApi, IndicatorCollection indicators, Strategy strat, ExecutionStrategy execStrat, string instrument)
        {
            priceHistoryDatabase = db;
            
            new Thread(delegate ()
            {
                streamer = new StreamingModul(indicators, strat, execStrat, tradingApi, instrument);

                //Prepare streamer
                List<TickData> tickdataToPrepare = priceHistoryDatabase.getPrices(Timestamp.getNow() - 24 * 60 * 60 * 1000, Timestamp.getNow(), instrument);
                foreach (TickData tick in tickdataToPrepare)
                    streamer.prepareDataWithoutTrading(tick);

                ssiApi = new SSI_Downloader(AvailableInstruments.allInstruments);
                ssiApi.sourceDataArrived += sourceDataArrived;
                ssiApi.start();

                webApi = new FXCMRatesDownloader();
                webApi.sourceDataArrived += tickdataArrived;
                webApi.start();

                while (true)
                {
                    if(continueLiveTradingThread)
                    {
                        streamer.doTradingTickWithoutNewData(Timestamp.getNow());
                        Thread.Sleep(1000 * 10);        
                    }            
                }
            }).Start();

            InitializeComponent();
        }
                
        private void startTradingBtn_Clicked(object sender, EventArgs e)
        {
            if (MessageBox.Show("Wirklich traden?", "", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                continueLiveTradingThread = true;       
            }
        }

        private void stopTradingButton_Clicked(object sender, EventArgs e)
        {
            continueLiveTradingThread = false;
        }

        private void updateUITimer_Tick(object sender, EventArgs e)
        {
            string output = "Data collected: " + insertedSets + Environment.NewLine;

            if (prices.Count > 0)
            {
                output += Environment.NewLine;
                foreach (string pair in AvailableInstruments.allInstruments)
                {
                    if (prices.ContainsKey(pair))
                        output += pair + " " + Math.Round(prices[pair], 5) + Environment.NewLine;
                }
            }

            if (lastUpdatedPair != null)
            {
                output += Environment.NewLine + "Last update: " + lastUpdatedPair + Environment.NewLine;
            }

            streamerInfoLabel.Text = streamer.getInfo();
            pairInfoLabel.Text = output;
        }

        private void sourceDataArrived(double value, long timestamp, string sourceName, string instrument)
        {
            priceHistoryDatabase.setData(new TimeValueData(timestamp, value), sourceName, instrument);
            insertedSets++;
        }

        private void tickdataArrived(TickData data)
        {
            priceHistoryDatabase.setPrice(data);
            insertedSets++;

            if (prices.ContainsKey(data.instrument) == false)
                prices.Add(data.instrument, data.getAvgPrice());
            else
                prices[data.instrument] = data.getAvgPrice();

            lastUpdatedPair = data.instrument;

            if(continueLiveTradingThread)
                streamer.pushDataAndTrade(data);
        }
    }
}
