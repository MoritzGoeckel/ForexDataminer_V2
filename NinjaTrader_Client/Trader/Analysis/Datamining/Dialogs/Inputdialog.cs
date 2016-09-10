using NinjaTrader_Client.Trader.Datamining;
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
    public partial class Inputdialog : Form
    {
        string[] inputFields;
        Dictionary<string, TextBox> outputs = new Dictionary<string, TextBox>();
        Dictionary<string, DataminingPairInformation> dbInfo;

        public Inputdialog(string[] inputFields, Dictionary<string, DataminingPairInformation> dbInfo)
        {
            this.dbInfo = dbInfo;
            this.inputFields = inputFields;

            InitializeComponent();
        }

        private void Inputdialog_Load(object sender, EventArgs e)
        {
            int y = 10;
            foreach(string s in inputFields)
            {
                Label l = new Label();
                l.Location = new Point(0, y);
                l.Text = s;

                TextBox t = new TextBox();
                t.Location = new Point(l.Width, y);
                
                inputGroupBox.Controls.Add(l);
                inputGroupBox.Controls.Add(t);

                y += 30;

                outputs.Add(s, t);
            }

            if (dbInfo != null)
            {
                StringBuilder dataInfoB = new StringBuilder("");
                foreach (KeyValuePair<string, DataminingPairInformation> pair in dbInfo)
                {
                    dataInfoB.Append(pair.Key + " (" + pair.Value.AllDatasets + ")" + Environment.NewLine);

                    foreach (KeyValuePair<string, DataminingDataComponentInfo> compInf in pair.Value.Components)
                    {
                        dataInfoB.Append("  " + compInf.Key + " Ocurrence: " + Math.Round(compInf.Value.getOccurencesRatio(pair.Value.Datasets), 3) + " Values: " + compInf.Value.min + "~" + compInf.Value.max + Environment.NewLine);
                    }

                    dataInfoB.Append(Environment.NewLine);
                }

                infoTxbox.Text = dataInfoB.ToString();
            }
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
                if (pair.Value.Text == null || pair.Value.Text == "" || pair.Value.Text == " ")
                {
                    validOutput = false;
                    pair.Value.BackColor = Color.Red;
                }
                else
                    pair.Value.BackColor = Color.White;

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
            minutesTxbox.Text = (Convert.ToInt32(secondsTxbox.Text) / 60).ToString();
            msTxBox.Text = (Convert.ToInt32(secondsTxbox.Text) * 1000).ToString();
        }

        private void minutesTxbox_TextChanged(object sender, EventArgs e)
        {
            secondsTxbox.Text = (Convert.ToInt32(minutesTxbox.Text) * 60).ToString();
            msTxBox.Text = (Convert.ToInt32(minutesTxbox.Text) * 60 * 1000).ToString();
        }

        private void msTxBox_TextChanged(object sender, EventArgs e)
        {
            minutesTxbox.Text = (Convert.ToInt32(msTxBox.Text) / 1000 / 60).ToString();
            secondsTxbox.Text = (Convert.ToInt32(msTxBox.Text) / 1000).ToString();
        }
    }
}
