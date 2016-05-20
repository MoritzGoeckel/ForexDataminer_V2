using NinjaTrader_Client.Trader.Datamining;
using NinjaTrader_Client.Trader.Indicators;
using NinjaTrader_Client.Trader.MainAPIs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NinjaTrader_Client.Trader.Analysis
{
    public partial class DataminingForm : Form
    {
        DataminingDatabase dataminingDb;
        Database sourceDatabase;

        string stateMessage = "";

        private void setState(string msg)
        {
            stateMessage = msg;
        }

        public DataminingForm(DataminingDatabase dataminingDb, Database sourceDatabase)
        {
            InitializeComponent();

            this.dataminingDb = dataminingDb;
            this.sourceDatabase = sourceDatabase;
        }

        private void DataminingForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = dataminingDb.getProgress().getString();
            label2.Text = stateMessage;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void create_ann_button_click(object sender, EventArgs e)
        {
            dataminingDb.doMachineLearning(
                new string[] {
                    "MA1-MA3",
                    "MA3-MA6",
                    "MA9-MA12",
                    "Stoch_" + 1000 * 60 * 60 * 3 + "_last",
                    "Stoch_" + 1000 * 60 * 60 * 6 + "_last",
                    "Stoch_" + 1000 * 60 * 60 * 12 + "_last",
                    "ssi-mt4",
                    "ssi-win-mt"},
                "outcome_code_" + 60 * 30 + "_" + 0.10,
                "EURUSD", 
                Application.StartupPath + "/ann.NeuralNetwork");
        }

        private void button_deleteAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Echt?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                dataminingDb.deleteAll();
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

                //USDCHF not yet
            });

            thread.Start();
        }

        public void doSample(string pair)
        {
            if (Directory.Exists(Application.StartupPath + @"\Analysis\") == false)
                Directory.CreateDirectory(Application.StartupPath + @"\Analysis\");

            DataminingExcelGenerator excel = new DataminingExcelGenerator(Application.StartupPath + @"\Analysis\" + DateTime.Now.ToString("yyyy_dd_mm") + "_" + pair + ".xls");

            setState("SSI 1");
            dataminingDb.getOutcomeIndicatorSampling(excel, -1, 1, 20, "ssi-mt4", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -1, 1, 20, "ssi-win-mt", 60 * 60, pair);

            setState("SSI 2");
            dataminingDb.getOutcomeIndicatorSampling(excel, -1, 1, 20, "ssi-mt4", 60 * 30, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -1, 1, 20, "ssi-win-mt", 60 * 30, pair);

            setState("SSI 3");
            dataminingDb.getOutcomeIndicatorSampling(excel, -1, 1, 20, "ssi-mt4", 60 * 10, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -1, 1, 20, "ssi-win-mt", 60 * 10, pair);

            setState("SSI 4");
            dataminingDb.getOutcomeIndicatorSampling(excel, -1, 1, 20, "ssi-mt4", 60 * 60 * 2, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -1, 1, 20, "ssi-win-mt", 60 * 60 * 2, pair);

            setState("SSI-Stoch 1");
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 20 + "_" + "ssi-mt4", 60 * 10, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 1 + "_" + "ssi-mt4", 60 * 10, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 3 + "_" + "ssi-mt4", 60 * 10, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 6 + "_" + "ssi-mt4", 60 * 10, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 12 + "_" + "ssi-mt4", 60 * 10, pair);

            setState("SSI-Stoch 2");
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 20 + "_" + "ssi-mt4", 60 * 30, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 1 + "_" + "ssi-mt4", 60 * 30, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 3 + "_" + "ssi-mt4", 60 * 30, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 6 + "_" + "ssi-mt4", 60 * 30, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 12 + "_" + "ssi-mt4", 60 * 30, pair);

            setState("SSI-Stoch 3");
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 20 + "_" + "ssi-mt4", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 1 + "_" + "ssi-mt4", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 3 + "_" + "ssi-mt4", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 6 + "_" + "ssi-mt4", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 12 + "_" + "ssi-mt4", 60 * 60, pair);

            setState("SSI-Stoch 4");
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 20 + "_" + "ssi-mt4", 60 * 60 * 2, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 1 + "_" + "ssi-mt4", 60 * 60 * 2, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 3 + "_" + "ssi-mt4", 60 * 60 * 2, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 6 + "_" + "ssi-mt4", 60 * 60 * 2, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 12 + "_" + "ssi-mt4", 60 * 60 * 2, pair);

            setState("MA-MA 1");
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.4, 0.4, 20, "MA1-MA3", 60 * 10, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.4, 0.4, 20, "MA3-MA6", 60 * 10, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.4, 0.4, 20, "MA6-MA9", 60 * 10, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.4, 0.4, 20, "MA9-MA12", 60 * 10, pair);

            setState("MA-MA 2");
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.4, 0.4, 20, "MA1-MA3", 60 * 30, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.4, 0.4, 20, "MA3-MA6", 60 * 30, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.4, 0.4, 20, "MA6-MA9", 60 * 30, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.4, 0.4, 20, "MA9-MA12", 60 * 30, pair);

            setState("MA-MA 3");
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.4, 0.4, 20, "MA1-MA3", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.4, 0.4, 20, "MA3-MA6", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.4, 0.4, 20, "MA6-MA9", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.4, 0.4, 20, "MA9-MA12", 60 * 60, pair);

            setState("MA-MA 4");
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.6, 0.6, 20, "MA1-MA3", 60 * 60 * 2, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.6, 0.6, 20, "MA3-MA6", 60 * 60 * 2, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.6, 0.6, 20, "MA6-MA9", 60 * 60 * 2, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, -0.6, 0.6, 20, "MA9-MA12", 60 * 60 * 2, pair);


            setState("Stoch 1");
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 30 + "_last", 60 * 10, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 1 + "_last", 60 * 10, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 3 + "_last", 60 * 10, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 6 + "_last", 60 * 10, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 12 + "_last", 60 * 10, pair);

            setState("Stoch 2");
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 30 + "_last", 60 * 30, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 1 + "_last", 60 * 30, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 3 + "_last", 60 * 30, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 6 + "_last", 60 * 30, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 12 + "_last", 60 * 30, pair);

            setState("Stoch 3");
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 30 + "_last", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 1 + "_last", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 3 + "_last", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 6 + "_last", 60 * 60, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 12 + "_last", 60 * 60, pair);

            setState("Stoch 4");
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 30 + "_last", 60 * 60 * 2, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 1 + "_last", 60 * 60 * 2, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 3 + "_last", 60 * 60 * 2, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 6 + "_last", 60 * 60 * 2, pair);
            dataminingDb.getOutcomeIndicatorSampling(excel, 0, 100, 20, "Stoch_" + 1000 * 60 * 60 * 12 + "_last", 60 * 60 * 2, pair);

            setState("Done! :)");

            excel.FinishDoc();
            excel.ShowDocument();
        }
    }
}
