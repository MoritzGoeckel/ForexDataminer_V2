﻿using NinjaTrader_Client.Trader;
using NinjaTrader_Client.Trader.Backtest;
using NinjaTrader_Client.Trader.BacktestBase;
using NinjaTrader_Client.Trader.BacktestBase.Visualization;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Strategies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NinjaTrader_Client
{
    public abstract partial class BacktestForm : Form
    {
        protected abstract void getNextStrategyToTest(ref Strategy strategy, ref long resolutionInSeconds, ref bool continueBacktesting);
        protected abstract void backtestResultArrived(Dictionary<string, string> parameters, Dictionary<string, string> result);

        private Backtester backtester;
        protected Database database;

        private long endTimestamp, startTimestamp;

        private int testsCount = 0;
        private Dictionary<string, BacktestData> results = new Dictionary<string, BacktestData>();

        private int errorTests = 0;
        private int maxThreads = Environment.ProcessorCount; // Threads Count

        private int chartResolution = 3000;

        public BacktestForm(Database database, int backtestHours)
        {
            InitializeComponent();
            this.database = database;

            this.endTimestamp = database.getLastTimestamp();
            this.startTimestamp = endTimestamp - (backtestHours * 60L * 60L * 1000L);
        }

        private void BacktestForm_Load(object sender, EventArgs e)
        {
            backtester = new Backtester(database, startTimestamp, endTimestamp, 10);
            backtester.backtestResultArrived += backtester_backtestResultArrived;

            timer1.Start();
            timer1.Interval = 1000;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void checkStartNewBacktest()
        {
            while (backtester.getThreadsCount() < maxThreads)
            {
                bool continueTesting = false;
                Strategy strategy = null;

                long resolutionInSeconds = -1;

                getNextStrategyToTest(ref strategy, ref resolutionInSeconds, ref continueTesting);

                if ((strategy == null || resolutionInSeconds == -1) && continueTesting == true)
                    throw new Exception("Strategy eq NULL ->" + " S: " + strategy);

                if (continueTesting)
                    backtester.startBacktest(strategy, resolutionInSeconds, chartResolution);
                else
                    break;
            }
        }

        private void backtester_backtestResultArrived(BacktestData result)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => backtester_backtestResultArrived(result)));
                return;
            }

            if (result != null)
            {
                //Output to interface
                int i = 1;
                string name = result.getParameters()["strategy"] + "_" + result.getParameters()["pair"];
                while (results.ContainsKey(name))
                {
                    name = result.getParameters()["strategy"] + "_" + result.getParameters()["pair"] + "_" + i;
                    i++;
                }

                results.Add(name, result);
                listBox_results.Items.Add(name);

                int threads = backtester.getThreadsCount();
                this.Text = "Tests: " + (++testsCount).ToString() + " | Sec/Test: " + Math.Round(backtester.getMillisecondsPerTest() / 1000, 4) + " | Threads: " + threads + " | = " + Math.Round(backtester.getMillisecondsPerTest() / 1000 / threads) + " Seconds" + " | API:" + Math.Round(backtester.getMillisecondsAPITimePerTick(), 2) + " | Strategy:" + Math.Round(backtester.getMillisecondsStrategyTimePerTick(), 2);

                //Output to file
                if (Directory.Exists(Application.StartupPath + "/backtestes/") == false)
                    Directory.CreateDirectory(Application.StartupPath + "/backtestes/");

                string path = Application.StartupPath + "/backtestes/backtest-" + result.getParameters()["strategy"] + ".csv";

                if (File.Exists(path) == false)
                    File.WriteAllText(path, BacktestFormatter.getCSVHeader(result) + Environment.NewLine);
                File.AppendAllText(path, BacktestFormatter.getCSVLine(result) + Environment.NewLine);

                this.backtestResultArrived(result.getParameters(), result.getResult());
            }
            else
            {
                errorTests++;
            }
        }

        private void outputBacktestState()
        {
            threadsLabel.Text = "Backteststate:" + Environment.NewLine +
                "Threads: " + backtester.getThreadsCount() + Environment.NewLine +
                "Error: " + errorTests + Environment.NewLine +
                Environment.NewLine +
                backtester.getProgressText() +
                Environment.NewLine +
                Environment.NewLine;
        }

        //UI stuff
        private void listBox_results_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBox_results.SelectedItem != null)
            {
                BacktestData result = results[listBox_results.SelectedItem.ToString()];

                listBox_trades.Items.Clear();
                listBox_trades.Items.AddRange(BacktestFormatter.getPositionsText(result).Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

                label_parameters.Text = BacktestFormatter.getParameterText(result);
                label_result.Text = BacktestFormatter.getResultText(result);
            }
        }

        private void openChartBtn_Click(object sender, EventArgs e)
        {
            BacktestData result = results[listBox_results.SelectedItem.ToString()];
            //ChartingForm chartingForm = new ChartingForm(database, result.getPositions(), startTimestamp, endTimestamp);
            //chartingForm.Show(); //caching! ???

            Image img = BacktestDataVisualizer.getImageFromBacktestData(result, 3000, 150);
            img.Save(Config.startupPath + "/tmp.png");

            Process.Start(Config.startupPath + "/tmp.png");

            //ShowImageForm form = new ShowImageForm();
            //form.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            outputBacktestState();
            checkStartNewBacktest();
        }
    }
}