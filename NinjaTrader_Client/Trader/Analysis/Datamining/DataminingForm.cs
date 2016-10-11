using NinjaTrader_Client.Trader.Analysis.Datamining;
using NinjaTrader_Client.Trader.Analysis.Indicators;
using NinjaTrader_Client.Trader.Datamining;
using NinjaTrader_Client.Trader.Datamining.AI;
using NinjaTrader_Client.Trader.Exceptions;
using NinjaTrader_Client.Trader.Indicators;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static NinjaTrader_Client.Trader.Analysis.Datamining.PredictivePowerAnalyzer;

namespace NinjaTrader_Client.Trader.Analysis
{
    public partial class DataminingForm : Form
    {
        InRamDatamining dataminingDb;
        SQLiteDatabase sourceDatabase;

        string stateMessage = "";

        private void setState(string msg)
        {
            stateMessage = msg;
        }

        public DataminingForm(SQLiteDatabase sourceDatabase)
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
            double ops = Math.Round(dataminingDb.getOperationsPerSecond(), 2);
            
            //Progress Text
            progress_label.Text = "Op/s " + (ops != 0.0 ? ops.ToString() : "Idle") + Environment.NewLine +
                dataminingDb.getProgress().getString();

            //State text
            state_label.Text = stateMessage;

            //Render dataInfo
            string infoString = DatasetInfo.renderInfoList(dataminingDb.getInfo());

            if (dataInfoTextbox.Text != infoString)
                dataInfoTextbox.Text = infoString;
        }

        private string formatDouble(double d)
        {
            return Math.Round(d, 3).ToString();
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
            {
                DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument" }, dataminingDb.getInfo());
                id.ShowDialog();

                if (id.isValidResult())
                    new Thread(delegate ()
                    {
                        dataminingDb.savePair(id.getResult()["instrument"]);
                    }).Start();
            }
        }

        private void import_btn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Import data?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument" }, dataminingDb.getInfo());
                id.ShowDialog();

                if (id.isValidResult())
                    new Thread(delegate ()
                    {
                        dataminingDb.importPair(id.getResult()["instrument"], 0, Timestamp.getNow(), sourceDatabase);
                    }).Start();
            }
        }

        private void load_btn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Load data?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument" }, dataminingDb.getInfo());
                id.ShowDialog();

                if (id.isValidResult())
                    new Thread(delegate ()
                    {
                        dataminingDb.loadPair(id.getResult()["instrument"]);
                    }).Start();
            }
        }

        private void outcome_sampling_button_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument", "steps", "indicatorid", "outcometimeframe" }, dataminingDb.getInfo());
            id.ShowDialog();

            if (id.isValidResult())
            {
                new Thread(delegate ()
                {
                    Dictionary<string, string> parameters = id.getResult();

                    if (Directory.Exists(Application.StartupPath + @"\Analysis\") == false)
                        Directory.CreateDirectory(Application.StartupPath + @"\Analysis\");

                    SampleOutcomeExcelGenerator excel = new SampleOutcomeExcelGenerator(Application.StartupPath + @"\Analysis\" + DateTime.Now.ToString("yyyy_dd_mm") + "_" + parameters["instrument"] + ".xls");

                    setState("Outcomesampling");                                                                                                                              
                    dataminingDb.getOutcomeIndicatorSampling(excel, parameters["indicatorid"], Convert.ToInt32(parameters["outcometimeframe"]), Convert.ToInt32(parameters["steps"]), dataminingDb.getInfo(parameters["indicatorid"]).getDecentRange(), parameters["instrument"]);

                    excel.FinishDoc();
                    excel.ShowDocument();
                }).Start();
            }
        }

        private void updateInfo_Btn_Click(object sender, EventArgs e)
        {
            
        }

        private void indicator_stoch_btn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument", "timeframe" }, dataminingDb.getInfo());
            id.ShowDialog();

            if (id.isValidResult())
                new Thread(delegate () {
                    Dictionary<string, string> parameters = id.getResult();
                    dataminingDb.addIndicator(new StochIndicator(Convert.ToInt64(parameters["timeframe"])), parameters["instrument"], "mid");
                }).Start();
        }

        private void indicator_range_btn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument", "timeframe" }, dataminingDb.getInfo());
            id.ShowDialog();

            if (id.isValidResult())
                new Thread(delegate () {
                    Dictionary<string, string> parameters = id.getResult();
                    dataminingDb.addIndicator(new RangeIndicator(Convert.ToInt64(parameters["timeframe"])), parameters["instrument"], "mid");
                }).Start();
        }

        private void indicator_deviation_btn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument", "timeframe" }, dataminingDb.getInfo());
            id.ShowDialog();

            if (id.isValidResult())
                new Thread(delegate () {
                    Dictionary<string, string> parameters = id.getResult();
                    dataminingDb.addIndicator(new StandartDeviationIndicator(Convert.ToInt64(parameters["timeframe"])), parameters["instrument"], "mid");
                }).Start();
        }

        private void indicator_time_btn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument" }, dataminingDb.getInfo());
            id.ShowDialog();

            if (id.isValidResult())
                new Thread(delegate () {
                    Dictionary<string, string> parameters = id.getResult();
                    dataminingDb.addIndicator(new TimeOpeningHoursIndicator(), parameters["instrument"], "mid");
                }).Start();
        }

        private void indicator_volume_btn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument", "timeframe", "samplingrate", "stepsize" }, dataminingDb.getInfo());
            id.ShowDialog();

            if (id.isValidResult())
                new Thread(delegate () {
                    Dictionary<string, string> parameters = id.getResult();
                    dataminingDb.addIndicator(new VolumeAtPriceIndicator(Convert.ToInt64(parameters["timeframe"]), double.Parse(parameters["stepsize"], CultureInfo.InvariantCulture), Convert.ToInt64(parameters["samplingrate"])), parameters["instrument"], "mid");
                }).Start();
        }

        private void indicator_ma_btn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument", "timeframe" }, dataminingDb.getInfo());
            id.ShowDialog();

            if (id.isValidResult())
                new Thread(delegate () {
                    Dictionary<string, string> parameters = id.getResult();
                    dataminingDb.addIndicator(new MovingAverageIndicator(Convert.ToInt64(parameters["timeframe"])), parameters["instrument"], "mid");
                }).Start();
        }

        private void metaIndicatorSum_btn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument", "fieldsArrayBy|", "wightArrayBy|", "fieldname" }, dataminingDb.getInfo());
            id.ShowDialog();

            if(id.isValidResult())
                new Thread(delegate () {
                    Dictionary<string, string> parameters = id.getResult();

                    List<double> wights = new List<double>();
                    foreach (string s in parameters["wightArrayBy|"].Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                        wights.Add(double.Parse(s, CultureInfo.InvariantCulture));

                    string[] fieldnames = parameters["fieldsArrayBy|"].Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    dataminingDb.addMetaIndicatorSum(fieldnames, wights.ToArray(), parameters["fieldname"], parameters["instrument"]);
                }).Start();
        }

        private void outcome_btn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument", "timeframe"}, dataminingDb.getInfo());
            id.ShowDialog();

            if (id.isValidResult())
                new Thread(delegate () {
                    Dictionary<string, string> parameters = id.getResult();
                    dataminingDb.addOutcome(Convert.ToInt32(parameters["timeframe"]), parameters["instrument"]);
                }).Start();
        }

        private void outcomeCode_btn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument", "timeframe", "normalizedDifference" }, dataminingDb.getInfo());
            id.ShowDialog();

            if (id.isValidResult())
                new Thread(delegate () {
                    Dictionary<string, string> parameters = id.getResult();
                    dataminingDb.addOutcomeCode(double.Parse(parameters["normalizedDifference"], CultureInfo.InvariantCulture), Convert.ToInt32(parameters["timeframe"]), parameters["instrument"]);
                }).Start();
        }

        private void unload_btn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument" }, dataminingDb.getInfo());
            id.ShowDialog();

            if (id.isValidResult())
                dataminingDb.unloadPair(id.getResult()["instrument"]);
        }

        private void getCodeDistributionBtn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument", "timeframe", "normalizedDifference" }, dataminingDb.getInfo());
            id.ShowDialog();

            Dictionary<string, string> parameters = id.getResult();

            if (id.isValidResult())
            {
                string msg = dataminingDb.getOutcomeCodeDistribution(double.Parse(parameters["normalizedDifference"], CultureInfo.InvariantCulture), Convert.ToInt32(parameters["timeframe"]), parameters["instrument"]);
                MessageBox.Show(msg);
            }
        }

        private void outcome_code_sampling_btn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument", "indicatorId", "normalizedDifference", "outcomeTimeframe", "steps" }, dataminingDb.getInfo());
            id.ShowDialog();

            if (id.isValidResult())
                new Thread(delegate () {
                    Dictionary<string, string> parameters = id.getResult();
                    
                    if (Directory.Exists(Application.StartupPath + @"\Analysis\") == false)
                        Directory.CreateDirectory(Application.StartupPath + @"\Analysis\");

                    SampleOutcomeCodeExcelGenerator excel = new SampleOutcomeCodeExcelGenerator(Application.StartupPath + @"\Analysis\" + DateTime.Now.ToString("yyyy_dd_mm") + "_" + parameters["instrument"] + ".xls");

                    setState("OutcomeCodeSampling");                                                                                 
                    dataminingDb.getOutcomeCodeIndicatorSampling(excel, parameters["indicatorId"], Convert.ToInt32(parameters["steps"]), dataminingDb.getInfo(parameters["indicatorId"]).getDecentRange(), double.Parse(parameters["normalizedDifference"], CultureInfo.InvariantCulture), Convert.ToInt32(parameters["outcomeTimeframe"]), parameters["instrument"]);

                    excel.FinishDoc();
                    excel.ShowDocument();
                }).Start();
        }

        private void button_start_q_Click(object sender, EventArgs e)
        {

        }

        //Todo: Create dialog
        private void optimizeParametersNN_btn_Click(object sender, EventArgs e)
        {
            new Thread(delegate () {

                //Todo: Drop fields random

                string[] inputFields = new string[] { "ssi-mt4", "spread", "mid-TradingTime", "mid-Stoch_600000", "mid-Stoch_1800000", "mid-Stoch_3600000", "mid-Stoch_7200000", "mid-Stoch_14400000", "mid-Stoch_21600000", "mid-MA_600000", "mid-MA_1800000", "mid-MA_3600000", "mid-MA_7200000", "mid-MA_14400000", "mid-MA_21600000", "mid-Range_1800000", "mid-Range_3600000", "mid-Range_7200000" };
                string outputField = "buy-outcomeCode-0,001_600000";
                int epochs = 10;
                int networksCount = 500;

                string path = Application.StartupPath + "/AI/";
                GeneralExcelGenerator excel = new GeneralExcelGenerator(path + "excel.xlsx");
                string sheetName = "OptimizeNN-E10";

                string[] header = new string[2 + AdvancedNeuralNetwork.getExcelHeader().Length];
                AdvancedNeuralNetwork.getExcelHeader().CopyTo(header, 2);
                header[0] = "ValidationE";
                header[1] = "TrainingE";

                excel.CreateSheet(sheetName, header);

                double[][] inputsTraining = new double[][] { };
                double[][] outputsTraining = new double[][] { };
                dataminingDb.getInputOutputArrays(inputFields, outputField, "EURUSD", ref inputsTraining, ref outputsTraining, DataGroup.All, 100 * 1000, 0);

                double[][] inputsValidation = new double[][] { };
                double[][] outputsValidation = new double[][] { };
                dataminingDb.getInputOutputArrays(inputFields, outputField, "EURUSD", ref inputsValidation, ref outputsValidation, DataGroup.All, 10 * 1000, 1);

                dataminingDb.unloadPair("EURUSD");

                for (int ns = 0; ns < networksCount; ns++)
                {
                    IMachineLearning network = AdvancedNeuralNetwork.getRandom(inputFields, outputField);
                    network.train(inputsTraining, outputsTraining, epochs);

                    double trainingError = network.getError();
                    double validationError = network.getPredictionErrorFromData(inputsValidation, outputsValidation);

                    string[] excelRow = new string[2 + network.getInfo(inputFields, outputField).Length];
                    network.getInfo(inputFields, outputField).CopyTo(excelRow, 2);
                    excelRow[0] = validationError.ToString();
                    excelRow[1] = trainingError.ToString();

                    excel.addRow(sheetName, excelRow);

                    setState("Optimizing NN " + ns + "/" + networksCount);
                }

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);

                excel.FinishSheet(sheetName);
                excel.FinishDoc();
                excel.save();

            }).Start();
        }

        //Todo: create dialog
        private void addData_btn_Click(object sender, EventArgs e)
        {
            new Thread(delegate () {
                dataminingDb.addData("ssi-win-mt4", sourceDatabase, "EURUSD");
                dataminingDb.addData("ssi-mt4", sourceDatabase, "EURUSD");
            }).Start();
        }

        //Todo: Dialog, Training, Testing, Validation
        private void create_ann_button_click(object sender, EventArgs e)
        {
            new Thread(delegate () {

                string path = Application.StartupPath + "/AI/";

                int maxEpochs = int.MaxValue;
                string[] inputFields = new string[] { "ssi-mt4", "spread", "mid-TradingTime", "mid-Stoch_600000", "mid-Stoch_1800000", "mid-Stoch_3600000", "mid-Stoch_7200000", "mid-Stoch_14400000", "mid-Stoch_21600000", "mid-MA_600000", "mid-MA_1800000", "mid-MA_3600000", "mid-MA_7200000", "mid-MA_14400000", "mid-MA_21600000", "mid-Range_1800000", "mid-Range_3600000", "mid-Range_7200000" };
                int[] neuronsCount = new int[] { inputFields.Length, 18, 12, 1 };
                string outputField = "buy-outcomeCode-0,001_600000";

                IMachineLearning network = new AdvancedNeuralNetwork(inputFields, outputField, neuronsCount, 0.1, 2, false, false, false, Accord.Neuro.Learning.JacobianMethod.ByBackpropagation);

                double[][] inputs = new double[][] { };
                double[][] outputs = new double[][] { };
                dataminingDb.getInputOutputArrays(inputFields, outputField, "EURUSD", ref inputs, ref outputs, DataGroup.Training);
                dataminingDb.unloadPair("EURUSD");

                int epochsDone = 0;
                double lastError = -1;
                while (epochsDone < maxEpochs)
                {
                    setState("e" + Math.Round(network.getError(), 4).ToString());
                    network.train(inputs, outputs, 1);

                    if (lastError == network.getError())
                        break;
                    else
                        lastError = network.getError();

                    if (Directory.Exists(path) == false)
                        Directory.CreateDirectory(path);

                    network.save(path + "E" + epochsDone + "_" + "e" + Math.Round(network.getError(), 4).ToString().Replace(',', '-').Replace('.', '-') + "_" + "EURUSD" + ".network"); //DateTime.Now.ToString("yyyy_dd_mm")
                    epochsDone++;
                }

                setState("Done learning");
            }).Start();
        }

        private void maxPpOutcomeCodeBtn_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument", "outcomeCodeId" }, dataminingDb.getInfo());
            id.ShowDialog();

            long operationSum = 0;
            int operationsCount = 0;

            if (id.isValidResult())
            {
                string instrument = id.getResult()["instrument"];
                string outcomeId = id.getResult()["outcomeCodeId"];

                string filename = Config.startupPath + "/ppForIndicators-" + outcomeId + ".csv";
                if (File.Exists(filename) == false)
                    writeTextToFile(filename, "Method Vanilla;Method LinRegr;Method LogRegr;Indicator" + Environment.NewLine);

                //Start some threads for 
                for (int threadId = 0; threadId < 1; threadId++) //Todo: Do 4 threads
                    new Thread(delegate ()
                    {
                        while (true)
                        {
                            try
                            {
                                Stopwatch watch = new Stopwatch();
                                watch.Start();

                                WalkerIndicator indicator = IndicatorGenerator.getRandomIndicator();
                                string indicatorId = "mid-" + indicator.getName();

                                setState("Max pp: avg" + Math.Round(operationSum / 1000d / (operationsCount != 0 ? operationsCount : 1)) + "s" + " n" + operationsCount);

                                try {
                                    dataminingDb.addIndicator(indicator, instrument, "mid");
                                } catch (IndicatorNeverValidException)
                                {
                                    writeTextToFile(filename, "x;x;x;" + indicatorId + Environment.NewLine);
                                    Debug.WriteLine("##### INVALID INDICATOR: " + indicatorId);
                                    continue;
                                }

                                DistributionRange range = dataminingDb.getInfo(indicatorId).getDecentRange();
                                double ppMethod1 = dataminingDb.getOutcomeCodeIndicatorSampling(null, indicatorId, 20, range, outcomeId, instrument);

                                /*double[][] inputsTraining = new double[0][], outputsTraining = new double[0][];
                                dataminingDb.getInputOutputArrays(new string[] { indicatorId }, outcomeId, instrument, ref inputsTraining, ref outputsTraining, DataGroup.All, 1000 * 20, 0);

                                double[][] inputsTest = new double[0][], outputsTest = new double[0][];
                                dataminingDb.getInputOutputArrays(new string[] { indicatorId }, outcomeId, instrument, ref inputsTest, ref outputsTest, DataGroup.All, 5000, 1);

                                double ppMethod2 = PredictivePowerAnalyzer.getPredictivePowerWithMl(inputsTraining, outputsTraining, inputsTest, outputsTest, MLMethodForPPAnalysis.LinearRegression);

                                double ppMethod3 = PredictivePowerAnalyzer.getPredictivePowerWithMl(inputsTraining, outputsTraining, inputsTest, outputsTest, MLMethodForPPAnalysis.LogRegression);*/

                                writeTextToFile(filename, ppMethod1 + ";" + "ni" + ";" + "ni" + ";" + indicatorId + Environment.NewLine);

                                dataminingDb.removeDataset(indicatorId, instrument);

                                watch.Stop();
                                operationSum += watch.ElapsedMilliseconds;
                                operationsCount++;
                            }
                            catch { }
                        }
                    }).Start();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void writeTextToFile(string path, string content)
        {
            Debug.WriteLine(content);
            File.AppendAllText(path, content);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataminingInputDialog id = new DataminingInputDialog(new string[] { "instrument" }, dataminingDb.getInfo());
            id.ShowDialog();
            
            if(id.isValidResult())
                dataminingDb.setDataGroups(id.getResult()["instrument"]);
        }
    }
}
