using NinjaTrader_Client.Trader;
using NinjaTrader_Client.Trader.BacktestBase;
using NinjaTrader_Client.Trader.BacktestBase.Visualization;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Streaming.Strategies;
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
        bool continueLiveTradingThread = true;

        public AutopilotTradingForm(SQLiteDatabase database)
        {
            new Thread(delegate ()
            {
                while (continueLiveTradingThread)
                {
                    
                }
            }).Start();

            InitializeComponent();
        }
                
        private void addEURUSDStrategyButton_Clicked(object sender, EventArgs e)
        {
            if (MessageBox.Show("Wirklich traden?", "", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                
            }
        }

        private void addTxtStrategyButton_Click(object sender, EventArgs e)
        {
        
        }

        private void stopTradingButton_Clicked(object sender, EventArgs e)
        {
            continueLiveTradingThread = false;
        }

        private void updateUITimer_Tick(object sender, EventArgs e)
        {
            apiStateLabel.Text = "";
        }
    }
}
