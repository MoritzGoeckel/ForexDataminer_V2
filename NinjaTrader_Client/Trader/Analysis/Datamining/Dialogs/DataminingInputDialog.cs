using NinjaTrader_Client.Trader.Datamining;
using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NinjaTrader_Client.Trader.Analysis.Datamining
{
    public partial class DataminingInputDialog : Form
    {
        private string[] inputFields;
        private Dictionary<string, TextBox> outputs = new Dictionary<string, TextBox>();
        private List<DatasetInfo> infos;

        private static Dictionary<string, string> historicNameValues = new Dictionary<string, string>();

        public DataminingInputDialog(string[] inputFields, List<DatasetInfo> infos)
        {
            this.infos = infos;
            this.inputFields = inputFields;

            InitializeComponent();
        }

        private void Inputdialog_Load(object sender, EventArgs e)
        {
            TextBox firstTxbox = null;
            int tabIndex = 0;

            int y = 30;
            foreach(string s in inputFields)
            {
                Label l = new Label();
                l.Location = new Point(5, y);
                l.Text = s;

                TextBox t = new TextBox();
                t.Location = new Point(l.Width + 10, y);

                if (tabIndex == 0)
                    firstTxbox = t;

                t.TabIndex = tabIndex++;

                if (historicNameValues.ContainsKey(s))
                    t.Text = historicNameValues[s];

                inputGroupBox.Controls.Add(l);
                inputGroupBox.Controls.Add(t);

                y += 35;

                outputs.Add(s, t);
            }

            submitBtn.TabIndex = tabIndex;

            if (infos != null)
            {
                StringBuilder dataInfoB = new StringBuilder("");

                foreach (DatasetInfo info in infos)
                {
                    dataInfoB.Append(info.id.getID() + " O:" + Math.Round(compInf.Value.getOccurencesRatio(pair.Value.Datasets), 3) + " V:" + Math.Round(compInf.Value.min, 5) + "~" + Math.Round(compInf.Value.max, 5) + Environment.NewLine);
                }

                infoTxbox.Text = dataInfoB.ToString();
            }

            firstTxbox.Focus();
        }

        public bool isValidResult()
        {
            return validOutput;
        }

        bool validOutput = false;
        private void submitBtn_Click(object sender, EventArgs e)
        {
            validOutput = true;
            foreach (KeyValuePair<string, TextBox> pair in outputs)
            {
                if (pair.Value.Text == null || pair.Value.Text == "" || pair.Value.Text == " ")
                {
                    validOutput = false;
                    pair.Value.BackColor = Color.Red;
                }
                else
                {
                    pair.Value.BackColor = Color.White;

                    if (historicNameValues.ContainsKey(pair.Key) == false)
                        historicNameValues.Add(pair.Key, pair.Value.Text);
                    else
                        historicNameValues[pair.Key] = pair.Value.Text;
                }
            }

            if (validOutput)
                this.Close();
            else
                validOutput = false;
        }

        public Dictionary<string, string> getResult()
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            foreach (KeyValuePair<string, TextBox> pair in outputs)
                output.Add(pair.Key, pair.Value.Text);

            return output;
        }

        private void secondsTxbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                minutesTxbox.Text = (Convert.ToInt32(secondsTxbox.Text) / 60).ToString();
                msTxBox.Text = (Convert.ToInt32(secondsTxbox.Text) * 1000).ToString();
            }
            catch { }
        }

        private void minutesTxbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                secondsTxbox.Text = (Convert.ToInt32(minutesTxbox.Text) * 60).ToString();
                msTxBox.Text = (Convert.ToInt32(minutesTxbox.Text) * 60 * 1000).ToString();
                hoursTextBox.Text = (Convert.ToInt32(minutesTxbox.Text) / 60).ToString();

            }
            catch { }
        }

        private void msTxBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                minutesTxbox.Text = (Convert.ToInt32(msTxBox.Text) / 1000 / 60).ToString();
                secondsTxbox.Text = (Convert.ToInt32(msTxBox.Text) / 1000).ToString();
                hoursTextBox.Text = (Convert.ToInt32(msTxBox.Text) / 60 / 60 / 1000).ToString();

            }
            catch { }
        }

        private void hoursTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                minutesTxbox.Text = (Convert.ToInt32(hoursTextBox.Text) * 60).ToString();
                secondsTxbox.Text = (Convert.ToInt32(hoursTextBox.Text) * 60 * 60).ToString();
                msTxBox.Text = (Convert.ToInt32(hoursTextBox.Text) * 60 * 60 * 1000).ToString();

            }
            catch { }
        }
    }
}
