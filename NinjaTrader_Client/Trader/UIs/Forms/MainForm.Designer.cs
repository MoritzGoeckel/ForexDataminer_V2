namespace NinjaTrader_Client
{
    partial class MainForm
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
            this.info_label = new System.Windows.Forms.Label();
            this.chart_btn = new System.Windows.Forms.Button();
            this.exp_imp_btn = new System.Windows.Forms.Button();
            this.backtest_btn = new System.Windows.Forms.Button();
            this.start_autopilot_btn = new System.Windows.Forms.Button();
            this.open_trade_form_btn = new System.Windows.Forms.Button();
            this.density_per_day_btn = new System.Windows.Forms.Button();
            this.backtest_rndm_btn = new System.Windows.Forms.Button();
            this.correlation_btn = new System.Windows.Forms.Button();
            this.position_chart_btn = new System.Windows.Forms.Button();
            this.density_btn = new System.Windows.Forms.Button();
            this.raw_backtestdata_renderer_btn = new System.Windows.Forms.Button();
            this.timer_migrate_update = new System.Windows.Forms.Timer(this.components);
            this.count_btn = new System.Windows.Forms.Button();
            this.test_btn = new System.Windows.Forms.Button();
            this.generate_strategy_btn = new System.Windows.Forms.Button();
            this.datamining_btn = new System.Windows.Forms.Button();
            this.connect_nt_btn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.openDataRecordingFormBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.update_info_timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // info_label
            // 
            this.info_label.AutoSize = true;
            this.info_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.info_label.Location = new System.Drawing.Point(9, 25);
            this.info_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.info_label.Name = "info_label";
            this.info_label.Size = new System.Drawing.Size(83, 29);
            this.info_label.TabIndex = 0;
            this.info_label.Text = "not yet";
            // 
            // chart_btn
            // 
            this.chart_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chart_btn.Location = new System.Drawing.Point(20, 74);
            this.chart_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chart_btn.Name = "chart_btn";
            this.chart_btn.Size = new System.Drawing.Size(237, 35);
            this.chart_btn.TabIndex = 8;
            this.chart_btn.Text = "Chart";
            this.chart_btn.UseVisualStyleBackColor = true;
            this.chart_btn.Click += new System.EventHandler(this.chart_btn_Click);
            // 
            // exp_imp_btn
            // 
            this.exp_imp_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exp_imp_btn.Location = new System.Drawing.Point(501, 74);
            this.exp_imp_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.exp_imp_btn.Name = "exp_imp_btn";
            this.exp_imp_btn.Size = new System.Drawing.Size(237, 35);
            this.exp_imp_btn.TabIndex = 9;
            this.exp_imp_btn.Text = "Ex / Imp";
            this.exp_imp_btn.UseVisualStyleBackColor = true;
            this.exp_imp_btn.Click += new System.EventHandler(this.exp_imp_btn_Click);
            // 
            // backtest_btn
            // 
            this.backtest_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backtest_btn.Location = new System.Drawing.Point(219, 29);
            this.backtest_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.backtest_btn.Name = "backtest_btn";
            this.backtest_btn.Size = new System.Drawing.Size(237, 35);
            this.backtest_btn.TabIndex = 4;
            this.backtest_btn.Text = "Backtest Single";
            this.backtest_btn.UseVisualStyleBackColor = true;
            this.backtest_btn.Click += new System.EventHandler(this.backtest_btn_Click);
            // 
            // start_autopilot_btn
            // 
            this.start_autopilot_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.start_autopilot_btn.Location = new System.Drawing.Point(9, 75);
            this.start_autopilot_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.start_autopilot_btn.Name = "start_autopilot_btn";
            this.start_autopilot_btn.Size = new System.Drawing.Size(237, 35);
            this.start_autopilot_btn.TabIndex = 12;
            this.start_autopilot_btn.Text = "Start Autopilot";
            this.start_autopilot_btn.UseVisualStyleBackColor = true;
            this.start_autopilot_btn.Click += new System.EventHandler(this.start_autopilot_btn_Click);
            // 
            // open_trade_form_btn
            // 
            this.open_trade_form_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.open_trade_form_btn.Location = new System.Drawing.Point(9, 29);
            this.open_trade_form_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.open_trade_form_btn.Name = "open_trade_form_btn";
            this.open_trade_form_btn.Size = new System.Drawing.Size(237, 35);
            this.open_trade_form_btn.TabIndex = 11;
            this.open_trade_form_btn.Text = "Open Trade Form";
            this.open_trade_form_btn.UseVisualStyleBackColor = true;
            this.open_trade_form_btn.Click += new System.EventHandler(this.open_trade_form_btn_Click);
            // 
            // density_per_day_btn
            // 
            this.density_per_day_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.density_per_day_btn.Location = new System.Drawing.Point(255, 29);
            this.density_per_day_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.density_per_day_btn.Name = "density_per_day_btn";
            this.density_per_day_btn.Size = new System.Drawing.Size(237, 35);
            this.density_per_day_btn.TabIndex = 1;
            this.density_per_day_btn.Text = "Analyse Datadensity Per Day";
            this.density_per_day_btn.UseVisualStyleBackColor = true;
            this.density_per_day_btn.Click += new System.EventHandler(this.density_per_day_btn_Click);
            // 
            // backtest_rndm_btn
            // 
            this.backtest_rndm_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backtest_rndm_btn.Location = new System.Drawing.Point(219, 74);
            this.backtest_rndm_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.backtest_rndm_btn.Name = "backtest_rndm_btn";
            this.backtest_rndm_btn.Size = new System.Drawing.Size(237, 35);
            this.backtest_rndm_btn.TabIndex = 5;
            this.backtest_rndm_btn.Text = "Backtest Random";
            this.backtest_rndm_btn.UseVisualStyleBackColor = true;
            this.backtest_rndm_btn.Click += new System.EventHandler(this.backtest_rndm_btn_Click);
            // 
            // correlation_btn
            // 
            this.correlation_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.correlation_btn.Location = new System.Drawing.Point(501, 29);
            this.correlation_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.correlation_btn.Name = "correlation_btn";
            this.correlation_btn.Size = new System.Drawing.Size(237, 35);
            this.correlation_btn.TabIndex = 6;
            this.correlation_btn.Text = "Correlation Analysis";
            this.correlation_btn.UseVisualStyleBackColor = true;
            this.correlation_btn.Click += new System.EventHandler(this.correlation_btn_Click);
            // 
            // position_chart_btn
            // 
            this.position_chart_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.position_chart_btn.Location = new System.Drawing.Point(20, 29);
            this.position_chart_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.position_chart_btn.Name = "position_chart_btn";
            this.position_chart_btn.Size = new System.Drawing.Size(237, 35);
            this.position_chart_btn.TabIndex = 7;
            this.position_chart_btn.Text = "Positions Chart";
            this.position_chart_btn.UseVisualStyleBackColor = true;
            this.position_chart_btn.Click += new System.EventHandler(this.position_chart_btn_Click);
            // 
            // density_btn
            // 
            this.density_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.density_btn.Location = new System.Drawing.Point(255, 74);
            this.density_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.density_btn.Name = "density_btn";
            this.density_btn.Size = new System.Drawing.Size(237, 35);
            this.density_btn.TabIndex = 2;
            this.density_btn.Text = "Analyse Datadensity";
            this.density_btn.UseVisualStyleBackColor = true;
            this.density_btn.Click += new System.EventHandler(this.density_btn_Click);
            // 
            // raw_backtestdata_renderer_btn
            // 
            this.raw_backtestdata_renderer_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.raw_backtestdata_renderer_btn.Location = new System.Drawing.Point(9, 29);
            this.raw_backtestdata_renderer_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.raw_backtestdata_renderer_btn.Name = "raw_backtestdata_renderer_btn";
            this.raw_backtestdata_renderer_btn.Size = new System.Drawing.Size(201, 35);
            this.raw_backtestdata_renderer_btn.TabIndex = 3;
            this.raw_backtestdata_renderer_btn.Text = "Analyse RawTestData";
            this.raw_backtestdata_renderer_btn.UseVisualStyleBackColor = true;
            this.raw_backtestdata_renderer_btn.Click += new System.EventHandler(this.raw_backtestdata_renderer_btn_Click);
            // 
            // timer_migrate_update
            // 
            this.timer_migrate_update.Interval = 1000;
            // 
            // count_btn
            // 
            this.count_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.count_btn.Location = new System.Drawing.Point(501, 74);
            this.count_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.count_btn.Name = "count_btn";
            this.count_btn.Size = new System.Drawing.Size(237, 35);
            this.count_btn.TabIndex = 14;
            this.count_btn.Text = "Count Data";
            this.count_btn.UseVisualStyleBackColor = true;
            this.count_btn.Click += new System.EventHandler(this.count_btn_Click);
            // 
            // test_btn
            // 
            this.test_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.test_btn.Location = new System.Drawing.Point(20, 29);
            this.test_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.test_btn.Name = "test_btn";
            this.test_btn.Size = new System.Drawing.Size(237, 35);
            this.test_btn.TabIndex = 16;
            this.test_btn.Text = "Test";
            this.test_btn.UseVisualStyleBackColor = true;
            this.test_btn.Click += new System.EventHandler(this.test_btn_Click);
            // 
            // generate_strategy_btn
            // 
            this.generate_strategy_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.generate_strategy_btn.Location = new System.Drawing.Point(9, 74);
            this.generate_strategy_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.generate_strategy_btn.Name = "generate_strategy_btn";
            this.generate_strategy_btn.Size = new System.Drawing.Size(201, 35);
            this.generate_strategy_btn.TabIndex = 17;
            this.generate_strategy_btn.Text = "Generate strategy";
            this.generate_strategy_btn.UseVisualStyleBackColor = true;
            this.generate_strategy_btn.Click += new System.EventHandler(this.generate_strategy_btn_Click);
            // 
            // datamining_btn
            // 
            this.datamining_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.datamining_btn.Location = new System.Drawing.Point(9, 29);
            this.datamining_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.datamining_btn.Name = "datamining_btn";
            this.datamining_btn.Size = new System.Drawing.Size(237, 35);
            this.datamining_btn.TabIndex = 18;
            this.datamining_btn.Text = "Datamining";
            this.datamining_btn.UseVisualStyleBackColor = true;
            this.datamining_btn.Click += new System.EventHandler(this.datamining_btn_Click);
            // 
            // connect_nt_btn
            // 
            this.connect_nt_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.connect_nt_btn.Location = new System.Drawing.Point(9, 29);
            this.connect_nt_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.connect_nt_btn.Name = "connect_nt_btn";
            this.connect_nt_btn.Size = new System.Drawing.Size(237, 35);
            this.connect_nt_btn.TabIndex = 19;
            this.connect_nt_btn.Text = "Connect to NinjaTrader";
            this.connect_nt_btn.UseVisualStyleBackColor = true;
            this.connect_nt_btn.Click += new System.EventHandler(this.connect_nt_btn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chart_btn);
            this.groupBox1.Controls.Add(this.position_chart_btn);
            this.groupBox1.Location = new System.Drawing.Point(650, 422);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(748, 120);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Charting";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.openDataRecordingFormBtn);
            this.groupBox2.Controls.Add(this.connect_nt_btn);
            this.groupBox2.Controls.Add(this.exp_imp_btn);
            this.groupBox2.Location = new System.Drawing.Point(650, 18);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(748, 125);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Collection";
            // 
            // openDataRecordingFormBtn
            // 
            this.openDataRecordingFormBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openDataRecordingFormBtn.Location = new System.Drawing.Point(9, 74);
            this.openDataRecordingFormBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.openDataRecordingFormBtn.Name = "openDataRecordingFormBtn";
            this.openDataRecordingFormBtn.Size = new System.Drawing.Size(237, 35);
            this.openDataRecordingFormBtn.TabIndex = 20;
            this.openDataRecordingFormBtn.Text = "Data recording";
            this.openDataRecordingFormBtn.UseVisualStyleBackColor = true;
            this.openDataRecordingFormBtn.Click += new System.EventHandler(this.openDataRecordingFormBtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.open_trade_form_btn);
            this.groupBox3.Controls.Add(this.start_autopilot_btn);
            this.groupBox3.Location = new System.Drawing.Point(650, 152);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(273, 126);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Controlling";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.density_per_day_btn);
            this.groupBox4.Controls.Add(this.datamining_btn);
            this.groupBox4.Controls.Add(this.density_btn);
            this.groupBox4.Controls.Add(this.count_btn);
            this.groupBox4.Controls.Add(this.correlation_btn);
            this.groupBox4.Location = new System.Drawing.Point(650, 291);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(748, 122);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Research";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.backtest_btn);
            this.groupBox5.Controls.Add(this.generate_strategy_btn);
            this.groupBox5.Controls.Add(this.backtest_rndm_btn);
            this.groupBox5.Controls.Add(this.raw_backtestdata_renderer_btn);
            this.groupBox5.Location = new System.Drawing.Point(932, 152);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox5.Size = new System.Drawing.Size(466, 126);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Backtest";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.test_btn);
            this.groupBox6.Location = new System.Drawing.Point(650, 551);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox6.Size = new System.Drawing.Size(748, 125);
            this.groupBox6.TabIndex = 27;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Misc";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.info_label);
            this.groupBox7.Location = new System.Drawing.Point(18, 18);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox7.Size = new System.Drawing.Size(622, 657);
            this.groupBox7.TabIndex = 28;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Info";
            // 
            // update_info_timer
            // 
            this.update_info_timer.Tick += new System.EventHandler(this.update_info_timer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1416, 689);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "Main Menu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label info_label;
        private System.Windows.Forms.Button chart_btn;
        private System.Windows.Forms.Button exp_imp_btn;
        private System.Windows.Forms.Button backtest_btn;
        private System.Windows.Forms.Button start_autopilot_btn;
        private System.Windows.Forms.Button open_trade_form_btn;
        private System.Windows.Forms.Button density_per_day_btn;
        private System.Windows.Forms.Button backtest_rndm_btn;
        private System.Windows.Forms.Button correlation_btn;
        private System.Windows.Forms.Button position_chart_btn;
        private System.Windows.Forms.Button density_btn;
        private System.Windows.Forms.Button raw_backtestdata_renderer_btn;
        private System.Windows.Forms.Timer timer_migrate_update;
        private System.Windows.Forms.Button count_btn;
        private System.Windows.Forms.Button test_btn;
        private System.Windows.Forms.Button generate_strategy_btn;
        private System.Windows.Forms.Button datamining_btn;
        private System.Windows.Forms.Button connect_nt_btn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Timer update_info_timer;
        private System.Windows.Forms.Button openDataRecordingFormBtn;
    }
}

