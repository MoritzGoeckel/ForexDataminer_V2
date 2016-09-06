using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NinjaTrader_Client.Trader;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.TradingAPIs;
using NinjaTrader_Client.Trader.Analysis;
using NinjaTrader_Client.Trader.BacktestBase;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Utils;
using NinjaTrader_Client.Trader.Indicators;
using System.Threading;

namespace NinjaTrader_Client
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private SQLiteDatabase priceHistoryDatabase;

        private LowLevelNinjaTraderAPI ntApi;
        private SSI_Downloader ssiApi;
        private FXCMRatesDownloader webApi;

        private List<string> allInstruments = new List<string>();
        private List<string> minorsInstruments = new List<string>();
        private List<string> majorsInstruments = new List<string>();

        private bool recordData = false;

        private Dictionary<string, double> prices = new Dictionary<string, double>();
        private string lastUpdatedPair = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            update_info_timer.Start();

            majorsInstruments.Add("EURUSD");
            majorsInstruments.Add("GBPUSD");
            majorsInstruments.Add("USDJPY");
            majorsInstruments.Add("USDCHF");

            minorsInstruments.Add("AUDCAD");
            minorsInstruments.Add("AUDJPY");
            minorsInstruments.Add("AUDUSD");
            minorsInstruments.Add("CHFJPY");
            minorsInstruments.Add("EURCHF");
            minorsInstruments.Add("EURGBP");
            minorsInstruments.Add("EURJPY");
            minorsInstruments.Add("GBPCHF");
            minorsInstruments.Add("GBPJPY");
            minorsInstruments.Add("NZDUSD");
            minorsInstruments.Add("USDCAD");

            allInstruments.AddRange(majorsInstruments);
            allInstruments.AddRange(minorsInstruments);

            Config.startConfig(Application.StartupPath);
            priceHistoryDatabase = new SQLiteDatabase(Application.StartupPath + "//priceHistorySQLite.s3db");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(ntApi != null)
                ntApi.stop();

            if(ssiApi != null)
                ssiApi.stop();

            if(webApi != null)
                webApi.stop();

            /*if(priceHistoryDatabase != null)
                priceHistoryDatabase.shutdown();*/
        }

        private void chart_btn_Click(object sender, EventArgs e)
        {
            ChartingForm cf = new ChartingForm(priceHistoryDatabase, null, priceHistoryDatabase.getLastTimestamp() - 1000 * 60 * 60, priceHistoryDatabase.getLastTimestamp());
            cf.Show();
        }

        private void exp_imp_btn_Click(object sender, EventArgs e)
        {
            ExportImportForm eiForm = new ExportImportForm(priceHistoryDatabase);
            eiForm.Show();
        }

        private void backtest_btn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void open_trade_form_btn_Click(object sender, EventArgs e)
        {
            LiveTradingForm form = new LiveTradingForm(NTLiveTradingAPI.getTheInstace());
            form.Show();
        }

        private void density_per_day_btn_Click(object sender, EventArgs e)
        {
            DataDensityPerDayForm ddForm = new DataDensityPerDayForm(priceHistoryDatabase);
            ddForm.Show();
        }

        private void backtest_rndm_btn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void correlation_btn_Click(object sender, EventArgs e)
        {
            CorrelationAnalysisForm cf = new CorrelationAnalysisForm(priceHistoryDatabase, 1000, 31, allInstruments);
            cf.Show();
        }

        private void position_chart_btn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void density_btn_Click(object sender, EventArgs e)
        {
            DataDensityForm f = new DataDensityForm(priceHistoryDatabase, 1000 * 60 * 60, "EURUSD");
            f.Show();
        }

        private void raw_backtestdata_renderer_btn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void count_btn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Main: " + priceHistoryDatabase.getSetsCount());
        }

        private void test_btn_Click(object sender, EventArgs e)
        {
            double tradingTimeCode = new TradingTimeIndicator().getIndicator(Timestamp.getNow(), 0).value;
            DateTime dt = Timestamp.getDate(Timestamp.getNow());
            MessageBox.Show(dt.ToString());
        }

        private void generate_strategy_btn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void datamining_btn_Click(object sender, EventArgs e)
        {
            DataminingForm df = new DataminingForm(priceHistoryDatabase);
            df.ShowDialog();
        }

        private void start_autopilot_btn_Click(object sender, EventArgs e)
        {
            AutopilotTradingForm atf = new AutopilotTradingForm(priceHistoryDatabase);
            atf.Show();
        }

        private void connect_nt_btn_Click(object sender, EventArgs e)
        {
            ntApi = new LowLevelNinjaTraderAPI(allInstruments, "Sim101");
            NTLiveTradingAPI.createInstace(ntApi, 125); //125 per position * 11 strategies = 1375 investement
        }

        private void sourceDataArrived(double value, long timestamp, string sourceName, string instrument)
        {
            if (recordData)
            {
                priceHistoryDatabase.setData(new TimeValueData(timestamp, value), sourceName, instrument);
                insertedSets++;
            }
        }

        int insertedSets = 0;
        private void tickdataArrived(TickData data)
        {
            if (recordData)
            {
                priceHistoryDatabase.setPrice(data);
                insertedSets++;
            }

            if (prices.ContainsKey(data.instrument) == false)
                prices.Add(data.instrument, data.getAvgPrice());
            else
                prices[data.instrument] = data.getAvgPrice();

            lastUpdatedPair = data.instrument;
        }

        private void connect_web_btn_Click(object sender, EventArgs e)
        {
            ssiApi = new SSI_Downloader(allInstruments);
            ssiApi.sourceDataArrived += sourceDataArrived;
            ssiApi.start();

            webApi = new FXCMRatesDownloader();
            webApi.sourceDataArrived += tickdataArrived;
            webApi.start();

            connect_web_btn.Enabled = false;
        }

        private void record_web_btn_Click(object sender, EventArgs e)
        {
            record_web_btn.Enabled = false;
            recordData = true;
        }

        private void update_info_timer_Tick(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => {
                string output = "Data collected: " + insertedSets + Environment.NewLine;

                if (NTLiveTradingAPI.getTheInstace() != null)
                {
                    output += Environment.NewLine +
                        "Positionsize: " + NTLiveTradingAPI.getTheInstace().getPositionSize() + Environment.NewLine +
                        "Cash value: " + NTLiveTradingAPI.getTheInstace().getCashValue() + Environment.NewLine +
                        "Buying power: " + NTLiveTradingAPI.getTheInstace().getBuyingPower() + Environment.NewLine;
                }

                output += Environment.NewLine +
                    (ntApi != null ? "NT CONNECTED!" : "no nt connection") + Environment.NewLine +
                    (recordData ? "RECORDING!" : "not recording") + Environment.NewLine +
                    (ssiApi != null && webApi != null ? "DOWNLOADING!" : "not downloading") + Environment.NewLine;

                if(prices.Count > 0)
                {
                    output += Environment.NewLine;
                    foreach(string pair in majorsInstruments)
                    {
                        if(prices.ContainsKey(pair))
                            output += pair + " " + Math.Round(prices[pair], 5) + Environment.NewLine;
                    }
                }

                if(lastUpdatedPair != null)
                {
                    output += Environment.NewLine + "Last update: " + lastUpdatedPair + Environment.NewLine;
                }

                info_label.Text = output;
            }));
        }
    }
}
