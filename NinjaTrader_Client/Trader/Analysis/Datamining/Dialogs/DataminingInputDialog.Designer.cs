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
            this.hoursTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.infoTxbox);
            this.groupBox1.Location = new System.Drawing.Point(272, 8);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(337, 345);
            this.groupBox1.TabIndex = 120;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // infoTxbox
            // 
            this.infoTxbox.Location = new System.Drawing.Point(5, 17);
            this.infoTxbox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.infoTxbox.Multiline = true;
            this.infoTxbox.Name = "infoTxbox";
            this.infoTxbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.infoTxbox.Size = new System.Drawing.Size(329, 326);
            this.infoTxbox.TabIndex = 110;
            // 
            // inputGroupBox
            // 
            this.inputGroupBox.Location = new System.Drawing.Point(8, 8);
            this.inputGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.inputGroupBox.Name = "inputGroupBox";
            this.inputGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.inputGroupBox.Size = new System.Drawing.Size(260, 320);
            this.inputGroupBox.TabIndex = 0;
            this.inputGroupBox.TabStop = false;
            this.inputGroupBox.Text = "Input";
            // 
            // submitBtn
            // 
            this.submitBtn.Location = new System.Drawing.Point(8, 331);
            this.submitBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.submitBtn.Name = "submitBtn";
            this.submitBtn.Size = new System.Drawing.Size(260, 21);
            this.submitBtn.TabIndex = 10;
            this.submitBtn.Text = "Submit";
            this.submitBtn.UseVisualStyleBackColor = true;
            this.submitBtn.Click += new System.EventHandler(this.submitBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.hoursTextBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.msTxBox);
            this.groupBox2.Controls.Add(this.secondsTxbox);
            this.groupBox2.Controls.Add(this.minutesTxbox);
            this.groupBox2.Location = new System.Drawing.Point(613, 8);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(249, 345);
            this.groupBox2.TabIndex = 130;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Utils";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "MS                           Sec                   Min";
            // 
            // msTxBox
            // 
            this.msTxBox.Location = new System.Drawing.Point(13, 42);
            this.msTxBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.msTxBox.Name = "msTxBox";
            this.msTxBox.Size = new System.Drawing.Size(86, 20);
            this.msTxBox.TabIndex = 101;
            this.msTxBox.TextChanged += new System.EventHandler(this.msTxBox_TextChanged);
            // 
            // secondsTxbox
            // 
            this.secondsTxbox.Location = new System.Drawing.Point(101, 42);
            this.secondsTxbox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.secondsTxbox.Name = "secondsTxbox";
            this.secondsTxbox.Size = new System.Drawing.Size(68, 20);
            this.secondsTxbox.TabIndex = 102;
            this.secondsTxbox.TextChanged += new System.EventHandler(this.secondsTxbox_TextChanged);
            // 
            // minutesTxbox
            // 
            this.minutesTxbox.Location = new System.Drawing.Point(172, 42);
            this.minutesTxbox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.minutesTxbox.Name = "minutesTxbox";
            this.minutesTxbox.Size = new System.Drawing.Size(68, 20);
            this.minutesTxbox.TabIndex = 103;
            this.minutesTxbox.TextChanged += new System.EventHandler(this.minutesTxbox_TextChanged);
            // 
            // hoursTextBox
            // 
            this.hoursTextBox.Location = new System.Drawing.Point(90, 162);
            this.hoursTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.hoursTextBox.Name = "hoursTextBox";
            this.hoursTextBox.Size = new System.Drawing.Size(68, 20);
            this.hoursTextBox.TabIndex = 104;
            this.hoursTextBox.TextChanged += new System.EventHandler(this.hoursTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 105;
            this.label2.Text = "hour";
            // 
            // DataminingInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 361);
            this.Controls.Add(this.submitBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.inputGroupBox);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "DataminingInputDialog";
            this.Text = "Inputdialog";
            this.Load += new System.EventHandler(this.Inputdialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox hoursTextBox;
    }
}