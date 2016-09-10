namespace NinjaTrader_Client.Trader.Analysis.Datamining
{
    partial class DataminingInputDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.infoTxbox = new System.Windows.Forms.TextBox();
            this.inputGroupBox = new System.Windows.Forms.GroupBox();
            this.submitBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.msTxBox = new System.Windows.Forms.TextBox();
            this.secondsTxbox = new System.Windows.Forms.TextBox();
            this.minutesTxbox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.inputGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.infoTxbox);
            this.groupBox1.Location = new System.Drawing.Point(408, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(505, 531);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // infoTxbox
            // 
            this.infoTxbox.Location = new System.Drawing.Point(7, 26);
            this.infoTxbox.Multiline = true;
            this.infoTxbox.Name = "infoTxbox";
            this.infoTxbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.infoTxbox.Size = new System.Drawing.Size(492, 499);
            this.infoTxbox.TabIndex = 0;
            // 
            // inputGroupBox
            // 
            this.inputGroupBox.Controls.Add(this.submitBtn);
            this.inputGroupBox.Location = new System.Drawing.Point(12, 12);
            this.inputGroupBox.Name = "inputGroupBox";
            this.inputGroupBox.Size = new System.Drawing.Size(390, 525);
            this.inputGroupBox.TabIndex = 1;
            this.inputGroupBox.TabStop = false;
            this.inputGroupBox.Text = "Input";
            // 
            // submitBtn
            // 
            this.submitBtn.Location = new System.Drawing.Point(296, 486);
            this.submitBtn.Name = "submitBtn";
            this.submitBtn.Size = new System.Drawing.Size(88, 33);
            this.submitBtn.TabIndex = 0;
            this.submitBtn.Text = "Submit";
            this.submitBtn.UseVisualStyleBackColor = true;
            this.submitBtn.Click += new System.EventHandler(this.submitBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.msTxBox);
            this.groupBox2.Controls.Add(this.secondsTxbox);
            this.groupBox2.Controls.Add(this.minutesTxbox);
            this.groupBox2.Location = new System.Drawing.Point(919, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(374, 531);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Calc";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "MS                           Sec                   Min";
            // 
            // msTxBox
            // 
            this.msTxBox.Location = new System.Drawing.Point(19, 64);
            this.msTxBox.Name = "msTxBox";
            this.msTxBox.Size = new System.Drawing.Size(127, 26);
            this.msTxBox.TabIndex = 2;
            this.msTxBox.TextChanged += new System.EventHandler(this.msTxBox_TextChanged);
            // 
            // secondsTxbox
            // 
            this.secondsTxbox.Location = new System.Drawing.Point(152, 64);
            this.secondsTxbox.Name = "secondsTxbox";
            this.secondsTxbox.Size = new System.Drawing.Size(100, 26);
            this.secondsTxbox.TabIndex = 1;
            this.secondsTxbox.TextChanged += new System.EventHandler(this.secondsTxbox_TextChanged);
            // 
            // minutesTxbox
            // 
            this.minutesTxbox.Location = new System.Drawing.Point(258, 64);
            this.minutesTxbox.Name = "minutesTxbox";
            this.minutesTxbox.Size = new System.Drawing.Size(100, 26);
            this.minutesTxbox.TabIndex = 0;
            this.minutesTxbox.TextChanged += new System.EventHandler(this.minutesTxbox_TextChanged);
            // 
            // DataminingInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1305, 555);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.inputGroupBox);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DataminingInputDialog";
            this.Text = "Inputdialog";
            this.Load += new System.EventHandler(this.Inputdialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.inputGroupBox.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox inputGroupBox;
        private System.Windows.Forms.Button submitBtn;
        private System.Windows.Forms.TextBox infoTxbox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox secondsTxbox;
        private System.Windows.Forms.TextBox minutesTxbox;
        private System.Windows.Forms.TextBox msTxBox;
        private System.Windows.Forms.Label label1;
    }
}