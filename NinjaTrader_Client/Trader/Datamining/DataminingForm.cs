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
            state_label.Text = "Op/s" + (ops != 0.0 ? ops.ToString() : "Idle") + " " + stateMessage;

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

            dataInfo_label.Text = dataInfoB.ToString();
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
            /*AdvancedNeuralNetwork network = new AdvancedNeuralNetwork();
            dataminingDb.addDataToLearningComponent(, network);
            network.train();*/
        }

        private void button_start_q_Click(object sender, EventArgs e)
        {
            
        }

        private void outcome_sampling_button_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Application.StartupPath + @"\Analysis\") == false)
                Directory.CreateDirectory(Application.StartupPath + @"\Analysis\");

            DataminingExcelGenerator excel = new DataminingExcelGenerator(Application.StartupPath + @"\Analysis\" + DateTime.Now.ToString("yyyy_dd_mm") + "_" + "EURUSD" + ".xls");

            setState("SSI 1");
            dataminingDb.getOutcomeIndicatorSampling(excel, new DataminingDataComponent("Stoch", 1000 * 60 * 60).getID(), 60 * 60 * 1000, "EURUSD");

            excel.FinishDoc();
            excel.ShowDocument();
        }

        private void updateInfo_Btn_Click(object sender, EventArgs e)
        {
            dataminingDb.updateInfo("EURUSD");
        }

        private void indicator_btn_Click(object sender, EventArgs e)
        {
            dataminingDb.addIndicator(new StochIndicator(60 * 60 * 1000), "EURUSD", "mid");
        }

        private void addData_btn_Click(object sender, EventArgs e)
        {
            dataminingDb.addData("ssi-win-mt4", sourceDatabase, "EURUSD");
            dataminingDb.addData("ssi-mt4", sourceDatabase, "EURUSD");
        }

        private void metaIndicatorSum_btn_Click(object sender, EventArgs e)
        {
            dataminingDb.addMetaIndicatorSum(new string[] { "bid", "ask" }, new double[] { -1, 1 }, "spread", "EURUSD");
        }

        private void outcome_btn_Click(object sender, EventArgs e)
        {
            dataminingDb.addOutcome(1000 * 60 * 15, "EURUSD");
        }

        private void outcomeCode_btn_Click(object sender, EventArgs e)
        {
            dataminingDb.addOutcomeCode(0.005, 1000 * 60 * 15, "EURUSD");
        }

        private void successRate_btn_Click(object sender, EventArgs e)
        {

        }
    }
}
