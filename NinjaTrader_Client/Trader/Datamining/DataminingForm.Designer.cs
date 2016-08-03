namespace NinjaTrader_Client.Trader.Analysis
{
    partial class DataminingForm
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
            this.updateUI_timer = new System.Windows.Forms.Timer(this.components);
            this.progress_label = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.create_ann_button = new System.Windows.Forms.Button();
            this.delete_btn = new System.Windows.Forms.Button();
            this.button_start_q = new System.Windows.Forms.Button();
            this.state_label = new System.Windows.Forms.Label();
            this.outcome_sampling_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.load_btn = new System.Windows.Forms.Button();
            this.save_btn = new System.Windows.Forms.Button();
            this.import_btn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.indicator_btn = new System.Windows.Forms.Button();
            this.outcomeCode_btn = new System.Windows.Forms.Button();
            this.metaIndicatorSum_btn = new System.Windows.Forms.Button();
            this.addData_btn = new System.Windows.Forms.Button();
            this.outcome_btn = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.successRate_btn = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.updateInfo_Btn = new System.Windows.Forms.Button();
            this.dataInfo_label = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // updateUI_timer
            // 
            this.updateUI_timer.Interval = 1000;
            this.updateUI_timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progress_label
            // 
            this.progress_label.Location = new System.Drawing.Point(8, 47);
            this.progress_label.Name = "progress_label";
            this.progress_label.Size = new System.Drawing.Size(361, 342);
            this.progress_label.TabIndex = 2;
            this.progress_label.Text = "no info yet";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 15);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(405, 374);
            this.textBox1.TabIndex = 6;
            // 
            // create_ann_button
            // 
            this.create_ann_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.create_ann_button.Location = new System.Drawing.Point(5, 19);
            this.create_ann_button.Name = "create_ann_button";
            this.create_ann_button.Size = new System.Drawing.Size(109, 23);
            this.create_ann_button.TabIndex = 9;
            this.create_ann_button.Text = "Create ANN";
            this.create_ann_button.UseVisualStyleBackColor = true;
            this.create_ann_button.Click += new System.EventHandler(this.create_ann_button_click);
            // 
            // delete_btn
            // 
            this.delete_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.delete_btn.Location = new System.Drawing.Point(6, 19);
            this.delete_btn.Name = "delete_btn";
            this.delete_btn.Size = new System.Drawing.Size(109, 23);
            this.delete_btn.TabIndex = 11;
            this.delete_btn.Text = "Delete";
            this.delete_btn.UseVisualStyleBackColor = true;
            this.delete_btn.Click += new System.EventHandler(this.button_deleteAll_Click);
            // 
            // button_start_q
            // 
            this.button_start_q.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_start_q.Location = new System.Drawing.Point(65, 27);
            this.button_start_q.Name = "button_start_q";
            this.button_start_q.Size = new System.Drawing.Size(109, 23);
            this.button_start_q.TabIndex = 14;
            this.button_start_q.Text = "Start big operation";
            this.button_start_q.UseVisualStyleBackColor = true;
            this.button_start_q.Click += new System.EventHandler(this.button_start_q_Click);
            // 
            // state_label
            // 
            this.state_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.state_label.Location = new System.Drawing.Point(6, 16);
            this.state_label.Name = "state_label";
            this.state_label.Size = new System.Drawing.Size(363, 25);
            this.state_label.TabIndex = 15;
            this.state_label.Text = "Idle";
            // 
            // outcome_sampling_button
            // 
            this.outcome_sampling_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.outcome_sampling_button.Location = new System.Drawing.Point(6, 19);
            this.outcome_sampling_button.Name = "outcome_sampling_button";
            this.outcome_sampling_button.Size = new System.Drawing.Size(109, 23);
            this.outcome_sampling_button.TabIndex = 16;
            this.outcome_sampling_button.Text = "Sample Outcome";
            this.outcome_sampling_button.UseVisualStyleBackColor = true;
            this.outcome_sampling_button.Click += new System.EventHandler(this.outcome_sampling_button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.state_label);
            this.groupBox1.Controls.Add(this.progress_label);
            this.groupBox1.Location = new System.Drawing.Point(261, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(375, 395);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.load_btn);
            this.groupBox2.Controls.Add(this.save_btn);
            this.groupBox2.Controls.Add(this.import_btn);
            this.groupBox2.Controls.Add(this.delete_btn);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(243, 79);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dangerous";
            // 
            // load_btn
            // 
            this.load_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.load_btn.Location = new System.Drawing.Point(121, 47);
            this.load_btn.Name = "load_btn";
            this.load_btn.Size = new System.Drawing.Size(109, 23);
            this.load_btn.TabIndex = 14;
            this.load_btn.Text = "Load";
            this.load_btn.UseVisualStyleBackColor = true;
            this.load_btn.Click += new System.EventHandler(this.load_btn_Click);
            // 
            // save_btn
            // 
            this.save_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.save_btn.Location = new System.Drawing.Point(6, 48);
            this.save_btn.Name = "save_btn";
            this.save_btn.Size = new System.Drawing.Size(109, 23);
            this.save_btn.TabIndex = 13;
            this.save_btn.Text = "Save";
            this.save_btn.UseVisualStyleBackColor = true;
            this.save_btn.Click += new System.EventHandler(this.save_btn_Click);
            // 
            // import_btn
            // 
            this.import_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.import_btn.Location = new System.Drawing.Point(121, 19);
            this.import_btn.Name = "import_btn";
            this.import_btn.Size = new System.Drawing.Size(109, 23);
            this.import_btn.TabIndex = 12;
            this.import_btn.Text = "Import";
            this.import_btn.UseVisualStyleBackColor = true;
            this.import_btn.Click += new System.EventHandler(this.import_btn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.indicator_btn);
            this.groupBox3.Controls.Add(this.outcomeCode_btn);
            this.groupBox3.Controls.Add(this.metaIndicatorSum_btn);
            this.groupBox3.Controls.Add(this.addData_btn);
            this.groupBox3.Controls.Add(this.outcome_btn);
            this.groupBox3.Location = new System.Drawing.Point(12, 97);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(243, 114);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Building";
            // 
            // indicator_btn
            // 
            this.indicator_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.indicator_btn.Location = new System.Drawing.Point(6, 19);
            this.indicator_btn.Name = "indicator_btn";
            this.indicator_btn.Size = new System.Drawing.Size(109, 23);
            this.indicator_btn.TabIndex = 18;
            this.indicator_btn.Text = "Add Indicator";
            this.indicator_btn.UseVisualStyleBackColor = true;
            this.indicator_btn.Click += new System.EventHandler(this.indicator_btn_Click);
            // 
            // outcomeCode_btn
            // 
            this.outcomeCode_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.outcomeCode_btn.Location = new System.Drawing.Point(121, 77);
            this.outcomeCode_btn.Name = "outcomeCode_btn";
            this.outcomeCode_btn.Size = new System.Drawing.Size(109, 23);
            this.outcomeCode_btn.TabIndex = 17;
            this.outcomeCode_btn.Text = "Add outc. Label";
            this.outcomeCode_btn.UseVisualStyleBackColor = true;
            this.outcomeCode_btn.Click += new System.EventHandler(this.outcomeCode_btn_Click);
            // 
            // metaIndicatorSum_btn
            // 
            this.metaIndicatorSum_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.metaIndicatorSum_btn.Location = new System.Drawing.Point(6, 48);
            this.metaIndicatorSum_btn.Name = "metaIndicatorSum_btn";
            this.metaIndicatorSum_btn.Size = new System.Drawing.Size(109, 23);
            this.metaIndicatorSum_btn.TabIndex = 15;
            this.metaIndicatorSum_btn.Text = "Add Sum";
            this.metaIndicatorSum_btn.UseVisualStyleBackColor = true;
            this.metaIndicatorSum_btn.Click += new System.EventHandler(this.metaIndicatorSum_btn_Click);
            // 
            // addData_btn
            // 
            this.addData_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addData_btn.Location = new System.Drawing.Point(121, 19);
            this.addData_btn.Name = "addData_btn";
            this.addData_btn.Size = new System.Drawing.Size(109, 23);
            this.addData_btn.TabIndex = 14;
            this.addData_btn.Text = "Add Data";
            this.addData_btn.UseVisualStyleBackColor = true;
            this.addData_btn.Click += new System.EventHandler(this.addData_btn_Click);
            // 
            // outcome_btn
            // 
            this.outcome_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.outcome_btn.Location = new System.Drawing.Point(6, 77);
            this.outcome_btn.Name = "outcome_btn";
            this.outcome_btn.Size = new System.Drawing.Size(109, 23);
            this.outcome_btn.TabIndex = 13;
            this.outcome_btn.Text = "Add Outcome";
            this.outcome_btn.UseVisualStyleBackColor = true;
            this.outcome_btn.Click += new System.EventHandler(this.outcome_btn_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.successRate_btn);
            this.groupBox4.Controls.Add(this.outcome_sampling_button);
            this.groupBox4.Location = new System.Drawing.Point(12, 217);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(243, 52);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Analysis";
            // 
            // successRate_btn
            // 
            this.successRate_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.successRate_btn.Location = new System.Drawing.Point(121, 19);
            this.successRate_btn.Name = "successRate_btn";
            this.successRate_btn.Size = new System.Drawing.Size(109, 23);
            this.successRate_btn.TabIndex = 17;
            this.successRate_btn.Text = "Success Rate";
            this.successRate_btn.UseVisualStyleBackColor = true;
            this.successRate_btn.Click += new System.EventHandler(this.successRate_btn_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button1);
            this.groupBox5.Controls.Add(this.create_ann_button);
            this.groupBox5.Location = new System.Drawing.Point(13, 276);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(242, 52);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "AI";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(120, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Create SVM";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.button_start_q);
            this.groupBox6.Location = new System.Drawing.Point(13, 335);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(242, 72);
            this.groupBox6.TabIndex = 22;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Misc";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.textBox1);
            this.groupBox7.Location = new System.Drawing.Point(804, 12);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(417, 395);
            this.groupBox7.TabIndex = 23;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Output";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.updateInfo_Btn);
            this.groupBox8.Controls.Add(this.dataInfo_label);
            this.groupBox8.Location = new System.Drawing.Point(642, 12);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(156, 395);
            this.groupBox8.TabIndex = 24;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Data";
            // 
            // updateInfo_Btn
            // 
            this.updateInfo_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateInfo_Btn.Location = new System.Drawing.Point(9, 366);
            this.updateInfo_Btn.Name = "updateInfo_Btn";
            this.updateInfo_Btn.Size = new System.Drawing.Size(141, 23);
            this.updateInfo_Btn.TabIndex = 11;
            this.updateInfo_Btn.Text = "Update info";
            this.updateInfo_Btn.UseVisualStyleBackColor = true;
            this.updateInfo_Btn.Click += new System.EventHandler(this.updateInfo_Btn_Click);
            // 
            // dataInfo_label
            // 
            this.dataInfo_label.Location = new System.Drawing.Point(6, 18);
            this.dataInfo_label.Name = "dataInfo_label";
            this.dataInfo_label.Size = new System.Drawing.Size(144, 345);
            this.dataInfo_label.TabIndex = 0;
            this.dataInfo_label.Text = "no data yet";
            // 
            // DataminingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1232, 415);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DataminingForm";
            this.Text = "Datamining";
            this.Load += new System.EventHandler(this.DataminingForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer updateUI_timer;
        private System.Windows.Forms.Label progress_label;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button create_ann_button;
        private System.Windows.Forms.Button delete_btn;
        private System.Windows.Forms.Button button_start_q;
        private System.Windows.Forms.Label state_label;
        private System.Windows.Forms.Button outcome_sampling_button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button import_btn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button addData_btn;
        private System.Windows.Forms.Button outcome_btn;
        private System.Windows.Forms.Button indicator_btn;
        private System.Windows.Forms.Button outcomeCode_btn;
        private System.Windows.Forms.Button metaIndicatorSum_btn;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button successRate_btn;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button save_btn;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label dataInfo_label;
        private System.Windows.Forms.Button load_btn;
        private System.Windows.Forms.Button updateInfo_Btn;
    }
}