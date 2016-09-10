using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NinjaTrader_Client.Trader.UIs.Forms
{
    public partial class DataRecordingForm : Form
    {
        private SSI_Downloader ssiApi;
        private FXCMRatesDownloader webApi;
        
        private Dictionary<string, double> prices = new Dictionary<string, double>();
        private string lastUpdatedPair = null;
        int insertedSets = 0;
        
        private SQLiteDatabase priceHistoryDatabase;

        public DataRecordingForm()
        {
            InitializeComponent();
            
            priceHistoryDatabase = new SQLiteDatabase(Application.StartupPath + "//priceHistorySQLite.s3db");
        }

        private void recordDataBtn_Click(object sender, EventArgs e)
        {
            ssiApi = new SSI_Downloader(AvailableInstruments.allInstruments);
            ssiApi.sourceDataArrived += sourceDataArrived;
            ssiApi.start();

            webApi = new FXCMRatesDownloader();
            webApi.sourceDataArrived += tickdataArrived;
            webApi.start();

            recordDataBtn.Enabled = false;
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
        }

        private void updateLabelTimer_Tick(object sender, EventArgs e)
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

            label1.Text = output;
        }
    }
}
