using NinjaTrader_Client.Trader.Datamining;
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

            MongoFacade facade = new MongoFacade(Application.StartupPath + "\\MongoDB\\mongod.exe", Application.StartupPath + "\\MongoDB\\data", "traderDataminingDb");
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
            state_label.Text = stateMessage + "\t" + Math.Round(dataminingDb.getOperationsPerSecond(), 2);

            //Render dataInfo
            StringBuilder dataInfoB = new StringBuilder("");
            foreach(KeyValuePair<string, DataminingPairInformation> pair in dataminingDb.getInfo())
            {
                dataInfoB.Append(pair.Key + " (" + pair.Value.Datasets + ")" + Environment.NewLine);

                foreach (KeyValuePair<string, DataminingDataComponentInfo> compInf in pair.Value.Components)
                {
                    dataInfoB.Append("\t" + compInf.Key + "\t" + compInf.Value.getOccurencesRatio(pair.Value.Datasets) + Environment.NewLine);
                }

                dataInfoB.Append(Environment.NewLine);
            }

            dataInfo_label.Text = dataInfoB.ToString();
        }

        private void button_deleteAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Really delete all data from the datamining database?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                dataminingDb.deleteAll();
        }

        private void save_btn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save data?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                dataminingDb.savePair("EURUSD");
        }

        private void import_btn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Import data?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                dataminingDb.importPair("EURUSD", 0, Timestamp.getNow(), sourceDatabase);
        }

        private void create_ann_button_click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button_start_q_Click(object sender, EventArgs e)
        {
            button_start_q.Enabled = false;

            Thread thread = new Thread(delegate ()
            {
                doData("USDCHF");
            });

            thread.Start();
        }

        public void doData(string pair)
        {      
            setState("Importing");
            dataminingDb.importPair(pair, sourceDatabase.getFirstTimestamp(), sourceDatabase.getLastTimestamp(), sourceDatabase);

            setState("Adding Indicator MA");
            dataminingDb.addIndicator(new MovingAverageIndicator(1000 * 60 * 60 * 1), pair, "last");
            dataminingDb.addIndicator(new MovingAverageIndicator(1000 * 60 * 60 * 3), pair, "last");
            dataminingDb.addIndicator(new MovingAverageIndicator(1000 * 60 * 60 * 6), pair, "last");
            dataminingDb.addIndicator(new MovingAverageIndicator(1000 * 60 * 60 * 9), pair, "last");
            dataminingDb.addIndicator(new MovingAverageIndicator(1000 * 60 * 60 * 12), pair, "last");

            setState("Adding IndicatorDifference");
            dataminingDb.addMetaIndicatorDifference("MA_" + 1000 * 60 * 60 * 1 + "_last", "MA_" + 1000 * 60 * 60 * 3 + "_last", "MA1-MA3", pair);
            dataminingDb.addMetaIndicatorDifference("MA_" + 1000 * 60 * 60 * 3 + "_last", "MA_" + 1000 * 60 * 60 * 6 + "_last", "MA3-MA6", pair);
            dataminingDb.addMetaIndicatorDifference("MA_" + 1000 * 60 * 60 * 6 + "_last", "MA_" + 1000 * 60 * 60 * 9 + "_last", "MA6-MA9", pair);
            dataminingDb.addMetaIndicatorDifference("MA_" + 1000 * 60 * 60 * 9 + "_last", "MA_" + 1000 * 60 * 60 * 12 + "_last", "MA9-MA12", pair);

            setState("Adding Indicator STOCH");
            dataminingDb.addIndicator(new StochIndicator(1000 * 60 * 30), pair, "last");
            dataminingDb.addIndicator(new StochIndicator(1000 * 60 * 60 * 1), pair, "last");
            dataminingDb.addIndicator(new StochIndicator(1000 * 60 * 60 * 3), pair, "last");
            dataminingDb.addIndicator(new StochIndicator(1000 * 60 * 60 * 6), pair, "last");
            dataminingDb.addIndicator(new StochIndicator(1000 * 60 * 60 * 12), pair, "last");

            setState("Adding SSI-Win");
            dataminingDb.addData("ssi-win-mt", sourceDatabase, pair);
            setState("Adding SSI");
            dataminingDb.addData("ssi-mt4", sourceDatabase, pair);

            setState("Adding SSI-Stoch");
            dataminingDb.addIndicator(new StochIndicator(1000 * 60 * 20), pair, "ssi-mt4");
            dataminingDb.addIndicator(new StochIndicator(1000 * 60 * 60 * 1), pair, "ssi-mt4");
            dataminingDb.addIndicator(new StochIndicator(1000 * 60 * 60 * 3), pair, "ssi-mt4");
            dataminingDb.addIndicator(new StochIndicator(1000 * 60 * 60 * 6), pair, "ssi-mt4");
            dataminingDb.addIndicator(new StochIndicator(1000 * 60 * 60 * 12), pair, "ssi-mt4");

            setState("Adding Outcome");
            dataminingDb.addOutcome(60 * 60 * 2, pair);
            dataminingDb.addOutcome(60 * 60, pair);
            dataminingDb.addOutcome(60 * 30, pair);
            dataminingDb.addOutcome(60 * 10, pair);

            setState("Adding Outcome Code");
            dataminingDb.addOutcomeCode(0.20, 60 * 30, pair);
            dataminingDb.addOutcomeCode(0.10, 60 * 10, pair);
            dataminingDb.addOutcomeCode(0.30, 60 * 60, pair);
            dataminingDb.addOutcomeCode(0.10, 60 * 30, pair);

            setState("Done! :)");
        }

        private void outcome_sampling_button_Click(object sender, EventArgs e)
        {
            outcome_sampling_button.Enabled = false;

            Thread thread = new Thread(delegate ()
            {
                doSample("EURUSD");
                //EURUSD
                //GPBUSD
                //USDJPY
                //USDCHF
            });

            thread.Start();
        }

        public void doSample(string pair)
        {
            /*if (Directory.Exists(Application.StartupPath + @"\Analysis\") == false)
                Directory.CreateDirectory(Application.StartupPath + @"\Analysis\");

            DataminingExcelGenerator excel = new DataminingExcelGenerator(Application.StartupPath + @"\Analysis\" + DateTime.Now.ToString("yyyy_dd_mm") + "_" + pair + ".xls");

            setState("SSI 1");
            dataminingDb.getOutcomeIndicatorSampling(excel, -1, 1, 20, "ssi-mt4", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -1, 1, 20, "ssi-win-mt", 60 * 60, pair);

            setState("Done! :)");

            excel.FinishDoc();
            excel.ShowDocument();*/
        }
    }
}
