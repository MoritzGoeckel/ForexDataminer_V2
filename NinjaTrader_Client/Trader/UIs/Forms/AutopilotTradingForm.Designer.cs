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
            this.startTradingBtn = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.updateUITimer = new System.Windows.Forms.Timer(this.components);
            this.streamerInfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pairInfoLabel
            // 
            this.pairInfoLabel.AutoSize = true;
            this.pairInfoLabel.Location = new System.Drawing.Point(20, 20);
            this.pairInfoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pairInfoLabel.Name = "pairInfoLabel";
            this.pairInfoLabel.Size = new System.Drawing.Size(51, 20);
            this.pairInfoLabel.TabIndex = 0;
            this.pairInfoLabel.Text = "label1";
            // 
            // startTradingBtn
            // 
            this.startTradingBtn.Location = new System.Drawing.Point(1008, 20);
            this.startTradingBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.startTradingBtn.Name = "startTradingBtn";
            this.startTradingBtn.Size = new System.Drawing.Size(112, 35);
            this.startTradingBtn.TabIndex = 1;
            this.startTradingBtn.Text = "Start";
            this.startTradingBtn.UseVisualStyleBackColor = true;
            this.startTradingBtn.Click += new System.EventHandler(this.startTradingBtn_Clicked);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1008, 658);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 35);
            this.button3.TabIndex = 3;
            this.button3.Text = "STOP";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.stopTradingButton_Clicked);
            // 
            // updateUITimer
            // 
            this.updateUITimer.Tick += new System.EventHandler(this.updateUITimer_Tick);
            // 
            // streamerInfoLabel
            // 
            this.streamerInfoLabel.AutoSize = true;
            this.streamerInfoLabel.Location = new System.Drawing.Point(440, 20);
            this.streamerInfoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.streamerInfoLabel.Name = "streamerInfoLabel";
            this.streamerInfoLabel.Size = new System.Drawing.Size(51, 20);
            this.streamerInfoLabel.TabIndex = 4;
            this.streamerInfoLabel.Text = "label1";
            // 
            // AutopilotTradingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 712);
            this.Controls.Add(this.streamerInfoLabel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.startTradingBtn);
            this.Controls.Add(this.pairInfoLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AutopilotTradingForm";
            this.Text = "AutopilotTradingForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label pairInfoLabel;
        private System.Windows.Forms.Button startTradingBtn;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Timer updateUITimer;
        private System.Windows.Forms.Label streamerInfoLabel;
    }
}