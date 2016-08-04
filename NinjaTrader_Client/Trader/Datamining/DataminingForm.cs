using NinjaTrader_Client.Trader.Datamining;
using NinjaTrader_Client.Trader.Datamining.AI;
using NinjaTrader_Client.Trader.Indicators;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NinjaTrader_Client.Trader.Analysis
{
    public partial class DataminingForm : Form
    {
        InRamDatamining dataminingDb;
        Database sourceDatabase;

        string stateMessage = "";

        private void setState(string msg)
        {
            stateMessage = msg;
        }

        public DataminingForm(Database sourceDatabase)
        {
            InitializeComponent();

            MongoFacade facade = new MongoFacade(@"E:\\Programmieren\\C# Neu\\NinjaTrader_Client\\NoGIT\\Data\\MongoDB-Datamining\\mongod.exe",
                "E:\\Programmieren\\C# Neu\\NinjaTrader_Client\\NoGIT\\Data\\MongoDB-Datamining\\data\\",
                "dataminingDb"); //Geht nicht ???

            dataminingDb = new InRamDatamining(facade);

            this.sourceDatabase = sourceDatabase;
        }

        private void DataminingForm_Load(object sender, EventArgs e)
        {
            updateUI_timer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Progress Text
            progress_label.Text = dataminingDb.getProgress().getString();

            //State text
            double ops = Math.Round(dataminingDb.getOperationsPerSecond(), 2);
            state_label.Text = "Op/s " + (ops != 0.0 ? ops.ToString() : "Idle") + " " + stateMessage;

            //Render dataInfo
            StringBuilder dataInfoB = new StringBuilder("");
            foreach(KeyValuePair<string, DataminingPairInformation> pair in dataminingDb.getInfo())
            {
                dataInfoB.Append(pair.Key + " (" + pair.Value.AllDatasets + ")" + Environment.NewLine);

                foreach (KeyValuePair<string, DataminingDataComponentInfo> compInf in pair.Value.Components)
                {
                    dataInfoB.Append("  " + compInf.Key + " ~" + Math.Round(compInf.Value.getOccurencesRatio(pair.Value.Datasets), 3) + Environment.NewLine);
                }

                dataInfoB.Append(Environment.NewLine);
            }

            if(data_textbox.Text != dataInfoB.ToString())
                data_textbox.Text = dataInfoB.ToString();
        }

        private void button_deleteAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Really delete all data from the datamining database?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                new Thread(delegate () {
                    dataminingDb.deleteAll();
                }).Start();
            
        }

        private void save_btn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save data?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                new Thread(delegate () {
                    dataminingDb.savePair("EURUSD");
                }).Start();
        }

        private void import_btn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Import data?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)            
                new Thread(delegate () {
                    dataminingDb.importPair("EURUSD", 0, Timestamp.getNow(), sourceDatabase);
                }).Start();
        }

        private void load_btn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Load data?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)          
                new Thread(delegate () {
                    dataminingDb.loadPair("EURUSD");
                }).Start();
        }

        private void create_ann_button_click(object sender, EventArgs e)
        {
            /*int epochs = 8;
            new Thread(delegate () {
                setState("Gathering data");

                IMachineLearning network = new AdvancedNeuralNetwork(,);
                dataminingDb.addDataToLearningComponent(new string[] { "ssi-win-mt4", "ssi-mt4", "Stoch_3600000_mid", "spread" }, "buy-outcomeCode-0,005_900000", "EURUSD", network);

                for (int i = 0; i < epochs; i++)
                {
                    setState("Training Epoch " + i + "/" + epochs);
                    network.train();
                }

                setState("Saving NN");

                string path = Application.StartupPath + "/AI/";
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);

                network.save(path + DateTime.Now.ToString("yyyy_dd_mm") + "_" + "EURUSD"+ ".network");

                setState("");
            }).Start();*/
        }

        private void outcome_sampling_button_Click(object sender, EventArgs e)
        {
            new Thread(delegate () {
                if (Directory.Exists(Application.StartupPath + @"\Analysis\") == false)
                    Directory.CreateDirectory(Application.StartupPath + @"\Analysis\");

                DataminingExcelGenerator excel = new DataminingExcelGenerator(Application.StartupPath + @"\Analysis\" + DateTime.Now.ToString("yyyy_dd_mm") + "_" + "EURUSD" + ".xls");

                setState("SSI 1");
                dataminingDb.getOutcomeIndicatorSampling(excel, "mid-" + new DataminingDataComponent("Stoch", 1000 * 60 * 60).getID(), 15 * 60 * 1000, 0.05, "EURUSD");

                excel.FinishDoc();
                excel.ShowDocument();
            }).Start();
        }

        private void updateInfo_Btn_Click(object sender, EventArgs e)
        {
            new Thread(delegate () {
                dataminingDb.updateInfo("EURUSD");
            }).Start();
        }

        private void indicator_btn_Click(object sender, EventArgs e)
        {
            new Thread(delegate () {
                dataminingDb.addIndicator(new StochIndicator(60 * 60 * 1000), "EURUSD", "mid");
            }).Start();
        }

        private void addData_btn_Click(object sender, EventArgs e)
        {
            new Thread(delegate () {
                dataminingDb.addData("ssi-win-mt4", sourceDatabase, "EURUSD");
                dataminingDb.addData("ssi-mt4", sourceDatabase, "EURUSD");
            }).Start();
        }

        private void metaIndicatorSum_btn_Click(object sender, EventArgs e)
        {
            new Thread(delegate () {
                dataminingDb.addMetaIndicatorSum(new string[] { "bid", "ask" }, new double[] { -1, 1 }, "spread", "EURUSD");
            }).Start();
        }

        private void outcome_btn_Click(object sender, EventArgs e)
        {
            new Thread(delegate () {
                dataminingDb.addOutcome(1000 * 60 * 30, "EURUSD");
            }).Start();
        }

        private void outcomeCode_btn_Click(object sender, EventArgs e)
        {
            new Thread(delegate () {
                dataminingDb.addOutcomeCode(0.005, 1000 * 60 * 30, "EURUSD");
            }).Start();
        }

        private void button_start_q_Click(object sender, EventArgs e)
        {
            /*new Thread(delegate () {
                setState("Data 1");
                dataminingDb.addData("ssi-win-mt4", sourceDatabase, "EURUSD");

                setState("Data 2");
                dataminingDb.addData("ssi-mt4", sourceDatabase, "EURUSD");

                setState("Save");
                dataminingDb.savePair("EURUSD");

                setState("Indicators 1");
                dataminingDb.addIndicator(new StochIndicator(10 * 60 * 1000), "EURUSD", "mid");
                dataminingDb.addIndicator(new StochIndicator(30 * 60 * 1000), "EURUSD", "mid");
                dataminingDb.addIndicator(new StochIndicator(60 * 60 * 1000), "EURUSD", "mid");
                dataminingDb.addIndicator(new StochIndicator(2 * 60 * 60 * 1000), "EURUSD", "mid");
                dataminingDb.addIndicator(new StochIndicator(4 * 60 * 60 * 1000), "EURUSD", "mid");
                dataminingDb.addIndicator(new StochIndicator(6 * 60 * 60 * 1000), "EURUSD", "mid");

                setState("Indicators 2");
                dataminingDb.addIndicator(new MovingAverageIndicator(10 * 60 * 1000), "EURUSD", "mid");
                dataminingDb.addIndicator(new MovingAverageIndicator(30 * 60 * 1000), "EURUSD", "mid");
                dataminingDb.addIndicator(new MovingAverageIndicator(60 * 60 * 1000), "EURUSD", "mid");
                dataminingDb.addIndicator(new MovingAverageIndicator(2 * 60 * 60 * 1000), "EURUSD", "mid");
                dataminingDb.addIndicator(new MovingAverageIndicator(4 * 60 * 60 * 1000), "EURUSD", "mid");
                dataminingDb.addIndicator(new MovingAverageIndicator(6 * 60 * 60 * 1000), "EURUSD", "mid");

                setState("Indicators 3");
                dataminingDb.addIndicator(new TradingTimeIndicator(), "EURUSD", "mid");

                setState("Indicators 4");
                dataminingDb.addIndicator(new RangeIndicator(30 * 60 * 1000), "EURUSD", "mid");
                dataminingDb.addIndicator(new RangeIndicator(60 * 60 * 1000), "EURUSD", "mid");
                dataminingDb.addIndicator(new RangeIndicator(2 * 60 * 60 * 1000), "EURUSD", "mid");

                setState("Meta indicators");
                dataminingDb.addMetaIndicatorSum(new string[] { "bid", "ask" }, new double[] { 1, -1 }, "spread", "EURUSD");

                dataminingDb.addMetaIndicatorSum(new string[] { "mid-" + new MovingAverageIndicator(10 * 60 * 1000).getName(),
                    "mid-" + new MovingAverageIndicator(30 * 60 * 1000).getName() }, new double[] { 1, -1 }, "spread", "EURUSD");

                dataminingDb.addMetaIndicatorSum(new string[] { "mid-" + new MovingAverageIndicator(30 * 60 * 1000).getName(),
                    "mid-" + new MovingAverageIndicator(60 * 60 * 1000).getName() }, new double[] { 1, -1 }, "spread", "EURUSD");

                dataminingDb.addMetaIndicatorSum(new string[] { "mid-" + new MovingAverageIndicator(60 * 60 * 1000).getName(),
                    "mid-" + new MovingAverageIndicator(2 * 60 * 60 * 1000).getName() }, new double[] { 1, -1 }, "spread", "EURUSD");

                dataminingDb.addMetaIndicatorSum(new string[] { "mid-" + new MovingAverageIndicator(2 * 60 * 60 * 1000).getName(),
                    "mid-" + new MovingAverageIndicator(4 * 60 * 60 * 1000).getName() }, new double[] { 1, -1 }, "spread", "EURUSD");

                dataminingDb.addMetaIndicatorSum(new string[] { "mid", "mid-" + new MovingAverageIndicator(10 * 60 * 1000).getName() }, new double[] { 1, -1 }, "spread", "EURUSD");
                dataminingDb.addMetaIndicatorSum(new string[] { "mid", "mid-" + new MovingAverageIndicator(30 * 60 * 1000).getName() }, new double[] { 1, -1 }, "spread", "EURUSD");
                dataminingDb.addMetaIndicatorSum(new string[] { "mid", "mid-" + new MovingAverageIndicator(60 * 60 * 60 * 1000).getName() }, new double[] { 1, -1 }, "spread", "EURUSD");
                dataminingDb.addMetaIndicatorSum(new string[] { "mid", "mid-" + new MovingAverageIndicator(2 * 60 * 60 * 1000).getName() }, new double[] { 1, -1 }, "spread", "EURUSD");
                dataminingDb.addMetaIndicatorSum(new string[] { "mid", "mid-" + new MovingAverageIndicator(4 * 60 * 60 * 1000).getName() }, new double[] { 1, -1 }, "spread", "EURUSD");

                setState("Save");
                dataminingDb.savePair("EURUSD");

                setState("Outcomes");
                dataminingDb.addOutcome(1000 * 60 * 10, "EURUSD");
                dataminingDb.addOutcome(1000 * 60 * 30, "EURUSD");
                dataminingDb.addOutcome(1000 * 60 * 60, "EURUSD");

                setState("Outcome labels");
                dataminingDb.addOutcomeCode(0.001, 1000 * 60 * 10, "EURUSD");
                dataminingDb.addOutcomeCode(0.002, 1000 * 60 * 30, "EURUSD");
                dataminingDb.addOutcomeCode(0.005, 1000 * 60 * 60, "EURUSD");

                setState("Save");
                dataminingDb.savePair("EURUSD");
            }).Start();*/

            new Thread(delegate () {

                setState("Outcome labels");
                dataminingDb.addOutcomeCode(0.0005, 1000 * 60 * 10, "EURUSD");
                dataminingDb.addOutcomeCode(0.001, 1000 * 60 * 30, "EURUSD");
                dataminingDb.addOutcomeCode(0.001, 1000 * 60 * 60, "EURUSD");
                dataminingDb.addOutcomeCode(0.002, 1000 * 60 * 60, "EURUSD");

                setState("Save");
                dataminingDb.savePair("EURUSD");
            }).Start();
        }
    }
}
