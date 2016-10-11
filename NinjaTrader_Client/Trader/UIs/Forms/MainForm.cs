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
using NinjaTrader_Client.Trader.UIs.Forms;

namespace NinjaTrader_Client
{
    //Todo: Autopilot form bauen, altes überarbeiten
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private LowLevelNinjaTraderAPI ntApi;
        private SQLiteDatabase priceHistoryDatabase;

        private void Form1_Load(object sender, EventArgs e)
        {
            update_info_timer.Start();
            
            Config.startConfig(Application.StartupPath);
            priceHistoryDatabase = new SQLiteDatabase(Application.StartupPath + "//priceHistorySQLite.s3db");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(ntApi != null)
                ntApi.stop();
        }

        private void chart_btn_Click(object sender, EventArgs e)
        {
            
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
            CorrelationAnalysisForm cf = new CorrelationAnalysisForm(priceHistoryDatabase, 1000, 31, AvailableInstruments.allInstruments);
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
            /*double tradingTimeCode = new TimeOpeningHoursIndicator().setNextDataAndGetIndicator(Timestamp.getNow(), 0).value;
            DateTime dt = Timestamp.getDate(Timestamp.getNow());
            MessageBox.Show(dt.ToString());*/
            Logger.sendImportantMessage("test :)");
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
            //AutopilotTradingForm atf = new AutopilotTradingForm(priceHistoryDatabase, new FakeTradingAPI(), );
            //atf.Show();
        }

        private void connect_nt_btn_Click(object sender, EventArgs e)
        {
            ntApi = new LowLevelNinjaTraderAPI(AvailableInstruments.allInstruments, "Sim101");
            NTLiveTradingAPI.createInstace(ntApi, 125); //125 per position * 11 strategies = 1375 investement
        }

        private void update_info_timer_Tick(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => {
                string output = "";

                if (NTLiveTradingAPI.getTheInstace() != null)
                {
                    output += Environment.NewLine +
                        "Positionsize: " + NTLiveTradingAPI.getTheInstace().getPositionSize() + Environment.NewLine +
                        "Cash value: " + NTLiveTradingAPI.getTheInstace().getCashValue() + Environment.NewLine +
                        "Buying power: " + NTLiveTradingAPI.getTheInstace().getBuyingPower() + Environment.NewLine;
                }

                output += Environment.NewLine + (ntApi != null ? "NT CONNECTED!" : "no nt connection");
                
                info_label.Text = output;
            }));
        }

        private void openDataRecordingFormBtn_Click(object sender, EventArgs e)
        {
            DataRecordingForm drf = new DataRecordingForm();
            drf.ShowDialog();
        }
    }
}
