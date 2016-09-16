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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataminingForm));
            this.updateUI_timer = new System.Windows.Forms.Timer(this.components);
            this.progress_label = new System.Windows.Forms.Label();
            this.create_ann_button = new System.Windows.Forms.Button();
            this.delete_btn = new System.Windows.Forms.Button();
            this.button_start_q = new System.Windows.Forms.Button();
            this.state_label = new System.Windows.Forms.Label();
            this.outcome_sampling_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.unload_btn = new System.Windows.Forms.Button();
            this.addData_btn = new System.Windows.Forms.Button();
            this.load_btn = new System.Windows.Forms.Button();
            this.save_btn = new System.Windows.Forms.Button();
            this.import_btn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.outcome_code_sampling_btn = new System.Windows.Forms.Button();
            this.outcomeCode_btn = new System.Windows.Forms.Button();
            this.outcome_btn = new System.Windows.Forms.Button();
            this.metaIndicatorSum_btn = new System.Windows.Forms.Button();
            this.indicator_stoch_btn = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.create_log_regression_btn = new System.Windows.Forms.Button();
            this.optimizeParametersNN_btn = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.dataInfoTextbox = new System.Windows.Forms.TextBox();
            this.updateInfo_Btn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.indicator_ma_btn = new System.Windows.Forms.Button();
            this.indicator_volume_btn = new System.Windows.Forms.Button();
            this.indicator_time_btn = new System.Windows.Forms.Button();
            this.indicator_deviation_btn = new System.Windows.Forms.Button();
            this.indicator_range_btn = new System.Windows.Forms.Button();
            this.getCodeDistributionBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // updateUI_timer
            // 
            this.updateUI_timer.Interval = 3000;
            this.updateUI_timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progress_label
            // 
            this.progress_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progress_label.Location = new System.Drawing.Point(8, 47);
            this.progress_label.Name = "progress_label";
            this.progress_label.Size = new System.Drawing.Size(289, 478);
            this.progress_label.TabIndex = 2;
            this.progress_label.Text = "no info yet";
            // 
            // create_ann_button
            // 
            this.create_ann_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.create_ann_button.Location = new System.Drawing.Point(5, 19);
            this.create_ann_button.Name = "create_ann_button";
            this.create_ann_button.Size = new System.Drawing.Size(55, 23);
            this.create_ann_button.TabIndex = 9;
            this.create_ann_button.Text = "ANN";
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
            this.button_start_q.Location = new System.Drawing.Point(5, 19);
            this.button_start_q.Name = "button_start_q";
            this.button_start_q.Size = new System.Drawing.Size(122, 23);
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
            this.state_label.Size = new System.Drawing.Size(291, 25);
            this.state_label.TabIndex = 15;
            this.state_label.Text = "Idle";
            // 
            // outcome_sampling_button
            // 
            this.outcome_sampling_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.outcome_sampling_button.Location = new System.Drawing.Point(6, 49);
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
            this.groupBox1.Location = new System.Drawing.Point(397, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 535);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.unload_btn);
            this.groupBox2.Controls.Add(this.addData_btn);
            this.groupBox2.Controls.Add(this.load_btn);
            this.groupBox2.Controls.Add(this.save_btn);
            this.groupBox2.Controls.Add(this.import_btn);
            this.groupBox2.Controls.Add(this.delete_btn);
            this.groupBox2.Location = new System.Drawing.Point(12, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(243, 109);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dangerous";
            // 
            // unload_btn
            // 
            this.unload_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.unload_btn.Location = new System.Drawing.Point(6, 77);
            this.unload_btn.Name = "unload_btn";
            this.unload_btn.Size = new System.Drawing.Size(109, 23);
            this.unload_btn.TabIndex = 15;
            this.unload_btn.Text = "Unload";
            this.unload_btn.UseVisualStyleBackColor = true;
            this.unload_btn.Click += new System.EventHandler(this.unload_btn_Click);
            // 
            // addData_btn
            // 
            this.addData_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addData_btn.Location = new System.Drawing.Point(121, 76);
            this.addData_btn.Name = "addData_btn";
            this.addData_btn.Size = new System.Drawing.Size(109, 23);
            this.addData_btn.TabIndex = 14;
            this.addData_btn.Text = "Add Data";
            this.addData_btn.UseVisualStyleBackColor = true;
            this.addData_btn.Click += new System.EventHandler(this.addData_btn_Click);
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
            this.groupBox3.Controls.Add(this.getCodeDistributionBtn);
            this.groupBox3.Controls.Add(this.outcome_code_sampling_btn);
            this.groupBox3.Controls.Add(this.outcome_sampling_button);
            this.groupBox3.Controls.Add(this.outcomeCode_btn);
            this.groupBox3.Controls.Add(this.outcome_btn);
            this.groupBox3.Location = new System.Drawing.Point(12, 127);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(243, 114);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Outcome";
            // 
            // outcome_code_sampling_btn
            // 
            this.outcome_code_sampling_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.outcome_code_sampling_btn.Location = new System.Drawing.Point(121, 49);
            this.outcome_code_sampling_btn.Name = "outcome_code_sampling_btn";
            this.outcome_code_sampling_btn.Size = new System.Drawing.Size(109, 23);
            this.outcome_code_sampling_btn.TabIndex = 17;
            this.outcome_code_sampling_btn.Text = "Sample Outc. Code";
            this.outcome_code_sampling_btn.UseVisualStyleBackColor = true;
            this.outcome_code_sampling_btn.Click += new System.EventHandler(this.outcome_code_sampling_btn_Click);
            // 
            // outcomeCode_btn
            // 
            this.outcomeCode_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.outcomeCode_btn.Location = new System.Drawing.Point(121, 20);
            this.outcomeCode_btn.Name = "outcomeCode_btn";
            this.outcomeCode_btn.Size = new System.Drawing.Size(109, 23);
            this.outcomeCode_btn.TabIndex = 17;
            this.outcomeCode_btn.Text = "Add outc. Label";
            this.outcomeCode_btn.UseVisualStyleBackColor = true;
            this.outcomeCode_btn.Click += new System.EventHandler(this.outcomeCode_btn_Click);
            // 
            // outcome_btn
            // 
            this.outcome_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.outcome_btn.Location = new System.Drawing.Point(7, 20);
            this.outcome_btn.Name = "outcome_btn";
            this.outcome_btn.Size = new System.Drawing.Size(109, 23);
            this.outcome_btn.TabIndex = 13;
            this.outcome_btn.Text = "Add Outcome";
            this.outcome_btn.UseVisualStyleBackColor = true;
            this.outcome_btn.Click += new System.EventHandler(this.outcome_btn_Click);
            // 
            // metaIndicatorSum_btn
            // 
            this.metaIndicatorSum_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.metaIndicatorSum_btn.Location = new System.Drawing.Point(5, 192);
            this.metaIndicatorSum_btn.Name = "metaIndicatorSum_btn";
            this.metaIndicatorSum_btn.Size = new System.Drawing.Size(124, 23);
            this.metaIndicatorSum_btn.TabIndex = 15;
            this.metaIndicatorSum_btn.Text = "Meta";
            this.metaIndicatorSum_btn.UseVisualStyleBackColor = true;
            this.metaIndicatorSum_btn.Click += new System.EventHandler(this.metaIndicatorSum_btn_Click);
            // 
            // indicator_stoch_btn
            // 
            this.indicator_stoch_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.indicator_stoch_btn.Location = new System.Drawing.Point(5, 19);
            this.indicator_stoch_btn.Name = "indicator_stoch_btn";
            this.indicator_stoch_btn.Size = new System.Drawing.Size(124, 23);
            this.indicator_stoch_btn.TabIndex = 18;
            this.indicator_stoch_btn.Text = "Stoch";
            this.indicator_stoch_btn.UseVisualStyleBackColor = true;
            this.indicator_stoch_btn.Click += new System.EventHandler(this.indicator_stoch_btn_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.create_log_regression_btn);
            this.groupBox5.Controls.Add(this.optimizeParametersNN_btn);
            this.groupBox5.Controls.Add(this.create_ann_button);
            this.groupBox5.Location = new System.Drawing.Point(12, 247);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(243, 52);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "AI";
            // 
            // create_log_regression_btn
            // 
            this.create_log_regression_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.create_log_regression_btn.Location = new System.Drawing.Point(62, 19);
            this.create_log_regression_btn.Name = "create_log_regression_btn";
            this.create_log_regression_btn.Size = new System.Drawing.Size(53, 23);
            this.create_log_regression_btn.TabIndex = 11;
            this.create_log_regression_btn.Text = "LogReg";
            this.create_log_regression_btn.UseVisualStyleBackColor = true;
            // 
            // optimizeParametersNN_btn
            // 
            this.optimizeParametersNN_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optimizeParametersNN_btn.Location = new System.Drawing.Point(121, 19);
            this.optimizeParametersNN_btn.Name = "optimizeParametersNN_btn";
            this.optimizeParametersNN_btn.Size = new System.Drawing.Size(109, 23);
            this.optimizeParametersNN_btn.TabIndex = 10;
            this.optimizeParametersNN_btn.Text = "Optimize ANN";
            this.optimizeParametersNN_btn.UseVisualStyleBackColor = true;
            this.optimizeParametersNN_btn.Click += new System.EventHandler(this.optimizeParametersNN_btn_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.button_start_q);
            this.groupBox6.Location = new System.Drawing.Point(259, 247);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(133, 52);
            this.groupBox6.TabIndex = 22;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Misc";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.dataInfoTextbox);
            this.groupBox8.Controls.Add(this.updateInfo_Btn);
            this.groupBox8.Location = new System.Drawing.Point(705, 8);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(375, 535);
            this.groupBox8.TabIndex = 24;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Data";
            // 
            // dataInfoTextbox
            // 
            this.dataInfoTextbox.Location = new System.Drawing.Point(9, 15);
            this.dataInfoTextbox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataInfoTextbox.Multiline = true;
            this.dataInfoTextbox.Name = "dataInfoTextbox";
            this.dataInfoTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.dataInfoTextbox.Size = new System.Drawing.Size(355, 484);
            this.dataInfoTextbox.TabIndex = 12;
            // 
            // updateInfo_Btn
            // 
            this.updateInfo_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateInfo_Btn.Location = new System.Drawing.Point(9, 502);
            this.updateInfo_Btn.Name = "updateInfo_Btn";
            this.updateInfo_Btn.Size = new System.Drawing.Size(353, 23);
            this.updateInfo_Btn.TabIndex = 11;
            this.updateInfo_Btn.Text = "Update info";
            this.updateInfo_Btn.UseVisualStyleBackColor = true;
            this.updateInfo_Btn.Click += new System.EventHandler(this.updateInfo_Btn_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 305);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(381, 239);
            this.panel1.TabIndex = 25;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.indicator_ma_btn);
            this.groupBox9.Controls.Add(this.metaIndicatorSum_btn);
            this.groupBox9.Controls.Add(this.indicator_volume_btn);
            this.groupBox9.Controls.Add(this.indicator_time_btn);
            this.groupBox9.Controls.Add(this.indicator_deviation_btn);
            this.groupBox9.Controls.Add(this.indicator_range_btn);
            this.groupBox9.Controls.Add(this.indicator_stoch_btn);
            this.groupBox9.Location = new System.Drawing.Point(259, 11);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox9.Size = new System.Drawing.Size(133, 229);
            this.groupBox9.TabIndex = 26;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Indicators";
            // 
            // indicator_ma_btn
            // 
            this.indicator_ma_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.indicator_ma_btn.Location = new System.Drawing.Point(5, 165);
            this.indicator_ma_btn.Name = "indicator_ma_btn";
            this.indicator_ma_btn.Size = new System.Drawing.Size(124, 23);
            this.indicator_ma_btn.TabIndex = 23;
            this.indicator_ma_btn.Text = "Moving Average";
            this.indicator_ma_btn.UseVisualStyleBackColor = true;
            this.indicator_ma_btn.Click += new System.EventHandler(this.indicator_ma_btn_Click);
            // 
            // indicator_volume_btn
            // 
            this.indicator_volume_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.indicator_volume_btn.Location = new System.Drawing.Point(5, 136);
            this.indicator_volume_btn.Name = "indicator_volume_btn";
            this.indicator_volume_btn.Size = new System.Drawing.Size(124, 23);
            this.indicator_volume_btn.TabIndex = 22;
            this.indicator_volume_btn.Text = "Volume at Price";
            this.indicator_volume_btn.UseVisualStyleBackColor = true;
            this.indicator_volume_btn.Click += new System.EventHandler(this.indicator_volume_btn_Click);
            // 
            // indicator_time_btn
            // 
            this.indicator_time_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.indicator_time_btn.Location = new System.Drawing.Point(5, 107);
            this.indicator_time_btn.Name = "indicator_time_btn";
            this.indicator_time_btn.Size = new System.Drawing.Size(124, 23);
            this.indicator_time_btn.TabIndex = 21;
            this.indicator_time_btn.Text = "Time";
            this.indicator_time_btn.UseVisualStyleBackColor = true;
            this.indicator_time_btn.Click += new System.EventHandler(this.indicator_time_btn_Click);
            // 
            // indicator_deviation_btn
            // 
            this.indicator_deviation_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.indicator_deviation_btn.Location = new System.Drawing.Point(5, 77);
            this.indicator_deviation_btn.Name = "indicator_deviation_btn";
            this.indicator_deviation_btn.Size = new System.Drawing.Size(124, 23);
            this.indicator_deviation_btn.TabIndex = 20;
            this.indicator_deviation_btn.Text = "Deviation";
            this.indicator_deviation_btn.UseVisualStyleBackColor = true;
            this.indicator_deviation_btn.Click += new System.EventHandler(this.indicator_deviation_btn_Click);
            // 
            // indicator_range_btn
            // 
            this.indicator_range_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.indicator_range_btn.Location = new System.Drawing.Point(5, 48);
            this.indicator_range_btn.Name = "indicator_range_btn";
            this.indicator_range_btn.Size = new System.Drawing.Size(124, 23);
            this.indicator_range_btn.TabIndex = 19;
            this.indicator_range_btn.Text = "Range";
            this.indicator_range_btn.UseVisualStyleBackColor = true;
            this.indicator_range_btn.Click += new System.EventHandler(this.indicator_range_btn_Click);
            // 
            // getCodeDistributionBtn
            // 
            this.getCodeDistributionBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.getCodeDistributionBtn.Location = new System.Drawing.Point(121, 78);
            this.getCodeDistributionBtn.Name = "getCodeDistributionBtn";
            this.getCodeDistributionBtn.Size = new System.Drawing.Size(109, 23);
            this.getCodeDistributionBtn.TabIndex = 18;
            this.getCodeDistributionBtn.Text = "Code distribution";
            this.getCodeDistributionBtn.UseVisualStyleBackColor = true;
            this.getCodeDistributionBtn.Click += new System.EventHandler(this.getCodeDistributionBtn_Click);
            // 
            // DataminingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1086, 549);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
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
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer updateUI_timer;
        private System.Windows.Forms.Label progress_label;
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
        private System.Windows.Forms.Button indicator_stoch_btn;
        private System.Windows.Forms.Button outcomeCode_btn;
        private System.Windows.Forms.Button metaIndicatorSum_btn;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button optimizeParametersNN_btn;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button save_btn;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button load_btn;
        private System.Windows.Forms.Button updateInfo_Btn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button unload_btn;
        private System.Windows.Forms.Button outcome_code_sampling_btn;
        private System.Windows.Forms.Button create_log_regression_btn;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button indicator_ma_btn;
        private System.Windows.Forms.Button indicator_volume_btn;
        private System.Windows.Forms.Button indicator_time_btn;
        private System.Windows.Forms.Button indicator_deviation_btn;
        private System.Windows.Forms.Button indicator_range_btn;
        private System.Windows.Forms.TextBox dataInfoTextbox;
        private System.Windows.Forms.Button getCodeDistributionBtn;
    }
}