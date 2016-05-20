using NinjaTrader_Client.Trader;
using NinjaTrader_Client.Trader.BacktestBase;
using NinjaTrader_Client.Trader.BacktestBase.Visualization;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Strategies;
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
        Database database;
        NinjaTraderAPI api;

        List<Strategy> runningStrategies = new List<Strategy>();

        private bool continueLiveTradingThread = true;
        private int tradingTick = 0;
        int strategyRunnerErrors = 0;
        long strategiesTickTime = -1;

        public AutopilotTradingForm(Database database)
        {
            this.database = database;
            this.api = NTLiveTradingAPI.getTheInstace().getAPI();
            api.tickdataArrived += AutopilotTradingForm_tickdataArrived;

            new Thread(delegate ()
            {
                while (continueLiveTradingThread)
                {
                    tradingTick++;
                    if (tradingTick > 1000)
                        tradingTick = 0;


                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    try
                    {
                        foreach (Strategy strat in runningStrategies)
                            try { strat.doTick(); }
                            catch { strategyRunnerErrors++; }
                    }
                    catch { }
                    sw.Stop();

                    strategiesTickTime = sw.ElapsedMilliseconds;

                    if(strategiesTickTime < 500)
                        Thread.Sleep(500 - Convert.ToInt32(strategiesTickTime));
                }
            }).Start();

            InitializeComponent();
        }

        Dictionary<string, string> pairInfoString = new Dictionary<string, string>();
        private void AutopilotTradingForm_tickdataArrived(Tickdata data, string instrument)
        {
            if (pairInfoString.ContainsKey(instrument) == false)
                pairInfoString.Add(instrument, "");

            pairInfoString[instrument] = "Position: " + api.getMarketPosition("EURUSD") + Environment.NewLine
                    + "Price: " + data.last + " -> " + data.bid + "/t" + data.ask;
        }

        private void startTradingLive(Strategy strat)
        {
            strat.setAPI(NTLiveTradingAPI.getTheInstace());
            strat.letIndicatorsCatchUp(Timestamp.getNow());

            runningStrategies.Add(strat);
        }

        private void addEURUSDStrategyButton_Clicked(object sender, EventArgs e)
        {
            if (MessageBox.Show("Wirklich traden?", "", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                //Add EURUSD Strat
                startTradingLive(new StochStrategy(database, "EURUSD", 0.2, 0.2, 21600000, 96, false));
            }
        }

        private void addTxtStrategyButton_Click(object sender, EventArgs e)
        {
            //Add TXT Strat

            if (MessageBox.Show("Wirklich traden?", "", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Config.startupPath;

                while (ofd.ShowDialog() != DialogResult.OK) ;

                List<string> stratStrings = BacktestFormatter.getParametersFromFile(ofd.FileName);

                foreach (string stratStr in stratStrings)
                {
                    Strategy strat = null;

                    BacktestFormatter.getStrategyFromString(database, stratStr, ref strat);
                    startTradingLive(strat);
                }

                button1.Enabled = false;
            }
        }

        private void stopTradingButton_Clicked(object sender, EventArgs e)
        {
            continueLiveTradingThread = false;
        }

        private void updateUITimer_Tick(object sender, EventArgs e)
        {
            apiStateLabel.Text = "Connected: " + api.isConnected() + Environment.NewLine
                    + "Trading: " + continueLiveTradingThread + Environment.NewLine
                    + "Tick time: " + strategiesTickTime + Environment.NewLine
                    + "Strategy errors: " + strategyRunnerErrors + Environment.NewLine;

            string strategyInfo = "";
            foreach (Strategy strat in runningStrategies)
            {
                strategyInfo += strat.getName() + " ";
                foreach (string pair in strat.getUsedPairs())
                    strategyInfo += pair;

                strategyInfo += Environment.NewLine + Environment.NewLine;

                foreach (KeyValuePair<string, BacktestVisualizationDataComponent> comp in strat.getVisualizationData().components)
                    strategyInfo += comp.Value.getName() + ": " + comp.Value.value + Environment.NewLine;

                strategyInfo += Environment.NewLine + Environment.NewLine;

                foreach (KeyValuePair<string, string> para in strat.getParameters())
                    strategyInfo += para.Key + ": " + para.Value;
            }
            strategyInfoLabel.Text = strategyInfo;

            pairInfoLabel.Text = "";
            foreach (KeyValuePair<string, string> pair in pairInfoString)
            {
                pairInfoLabel.Text += pair.Key + Environment.NewLine + pair.Value + Environment.NewLine + Environment.NewLine;
            }
        }
    }
}
