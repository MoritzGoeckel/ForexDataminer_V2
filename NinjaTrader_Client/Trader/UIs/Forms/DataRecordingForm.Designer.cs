﻿namespace NinjaTrader_Client.Trader.UIs.Forms
{
    partial class DataRecordingForm
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
            this.recordDataBtn = new System.Windows.Forms.Button();
            this.updateLabelTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // recordDataBtn
            // 
            this.recordDataBtn.Location = new System.Drawing.Point(56, 74);
            this.recordDataBtn.Name = "recordDataBtn";
            this.recordDataBtn.Size = new System.Drawing.Size(147, 72);
            this.recordDataBtn.TabIndex = 0;
            this.recordDataBtn.Text = "Start recording";
            this.recordDataBtn.UseVisualStyleBackColor = true;
            this.recordDataBtn.Click += new System.EventHandler(this.recordDataBtn_Click);
            // 
            // updateLabelTimer
            // 
            this.updateLabelTimer.Tick += new System.EventHandler(this.updateLabelTimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(259, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "No Info";
            // 
            // DataRecordingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 303);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.recordDataBtn);
            this.Name = "DataRecordingForm";
            this.Text = "DataRecordingForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button recordDataBtn;
        private System.Windows.Forms.Timer updateLabelTimer;
        private System.Windows.Forms.Label label1;
    }
}