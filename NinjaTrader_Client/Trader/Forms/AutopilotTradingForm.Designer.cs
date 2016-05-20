namespace NinjaTrader_Client
{
    partial class AutopilotTradingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pairInfoLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.strategyInfoLabel = new System.Windows.Forms.Label();
            this.apiStateLabel = new System.Windows.Forms.Label();
            this.updateUITimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // pairInfoLabel
            // 
            this.pairInfoLabel.AutoSize = true;
            this.pairInfoLabel.Location = new System.Drawing.Point(13, 13);
            this.pairInfoLabel.Name = "pairInfoLabel";
            this.pairInfoLabel.Size = new System.Drawing.Size(35, 13);
            this.pairInfoLabel.TabIndex = 0;
            this.pairInfoLabel.Text = "label1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(672, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "EURUSD";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.addEURUSDStrategyButton_Clicked);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(672, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Trade TXT";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.addTxtStrategyButton_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(672, 428);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "STOP";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.stopTradingButton_Clicked);
            // 
            // strategyInfoLabel
            // 
            this.strategyInfoLabel.AutoSize = true;
            this.strategyInfoLabel.Location = new System.Drawing.Point(176, 13);
            this.strategyInfoLabel.Name = "strategyInfoLabel";
            this.strategyInfoLabel.Size = new System.Drawing.Size(35, 13);
            this.strategyInfoLabel.TabIndex = 4;
            this.strategyInfoLabel.Text = "label1";
            // 
            // apiStateLabel
            // 
            this.apiStateLabel.AutoSize = true;
            this.apiStateLabel.Location = new System.Drawing.Point(339, 13);
            this.apiStateLabel.Name = "apiStateLabel";
            this.apiStateLabel.Size = new System.Drawing.Size(35, 13);
            this.apiStateLabel.TabIndex = 5;
            this.apiStateLabel.Text = "label1";
            // 
            // updateUITimer
            // 
            this.updateUITimer.Tick += new System.EventHandler(this.updateUITimer_Tick);
            // 
            // AutopilotTradingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 463);
            this.Controls.Add(this.apiStateLabel);
            this.Controls.Add(this.strategyInfoLabel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pairInfoLabel);
            this.Name = "AutopilotTradingForm";
            this.Text = "AutopilotTradingForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label pairInfoLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label strategyInfoLabel;
        private System.Windows.Forms.Label apiStateLabel;
        private System.Windows.Forms.Timer updateUITimer;
    }
}