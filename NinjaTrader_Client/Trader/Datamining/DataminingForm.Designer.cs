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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.create_ann_button = new System.Windows.Forms.Button();
            this.delete_btn = new System.Windows.Forms.Button();
            this.button_start_q = new System.Windows.Forms.Button();
            this.state_label = new System.Windows.Forms.Label();
            this.outcome_sampling_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.unload_btn = new System.Windows.Forms.Button();
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.optimizeParametersNN_btn = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.data_textbox = new System.Windows.Forms.TextBox();
            this.updateInfo_Btn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
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
            this.updateUI_timer.Interval = 3000;
            this.updateUI_timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progress_label
            // 
            this.progress_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progress_label.Location = new System.Drawing.Point(12, 72);
            this.progress_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.progress_label.Name = "progress_label";
            this.progress_label.Size = new System.Drawing.Size(434, 877);
            this.progress_label.TabIndex = 2;
            this.progress_label.Text = "no info yet";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(9, 23);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(606, 919);
            this.textBox1.TabIndex = 6;
            // 
            // create_ann_button
            // 
            this.create_ann_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.create_ann_button.Location = new System.Drawing.Point(8, 29);
            this.create_ann_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.create_ann_button.Name = "create_ann_button";
            this.create_ann_button.Size = new System.Drawing.Size(164, 35);
            this.create_ann_button.TabIndex = 9;
            this.create_ann_button.Text = "Create ANN";
            this.create_ann_button.UseVisualStyleBackColor = true;
            this.create_ann_button.Click += new System.EventHandler(this.create_ann_button_click);
            // 
            // delete_btn
            // 
            this.delete_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.delete_btn.Location = new System.Drawing.Point(9, 29);
            this.delete_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.delete_btn.Name = "delete_btn";
            this.delete_btn.Size = new System.Drawing.Size(164, 35);
            this.delete_btn.TabIndex = 11;
            this.delete_btn.Text = "Delete";
            this.delete_btn.UseVisualStyleBackColor = true;
            this.delete_btn.Click += new System.EventHandler(this.button_deleteAll_Click);
            // 
            // button_start_q
            // 
            this.button_start_q.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_start_q.Location = new System.Drawing.Point(8, 29);
            this.button_start_q.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_start_q.Name = "button_start_q";
            this.button_start_q.Size = new System.Drawing.Size(164, 35);
            this.button_start_q.TabIndex = 14;
            this.button_start_q.Text = "Start big operation";
            this.button_start_q.UseVisualStyleBackColor = true;
            this.button_start_q.Click += new System.EventHandler(this.button_start_q_Click);
            // 
            // state_label
            // 
            this.state_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.state_label.Location = new System.Drawing.Point(9, 25);
            this.state_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.state_label.Name = "state_label";
            this.state_label.Size = new System.Drawing.Size(436, 38);
            this.state_label.TabIndex = 15;
            this.state_label.Text = "Idle";
            // 
            // outcome_sampling_button
            // 
            this.outcome_sampling_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.outcome_sampling_button.Location = new System.Drawing.Point(9, 29);
            this.outcome_sampling_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.outcome_sampling_button.Name = "outcome_sampling_button";
            this.outcome_sampling_button.Size = new System.Drawing.Size(164, 35);
            this.outcome_sampling_button.TabIndex = 16;
            this.outcome_sampling_button.Text = "Sample Outcome";
            this.outcome_sampling_button.UseVisualStyleBackColor = true;
            this.outcome_sampling_button.Click += new System.EventHandler(this.outcome_sampling_button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.state_label);
            this.groupBox1.Controls.Add(this.progress_label);
            this.groupBox1.Location = new System.Drawing.Point(392, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(454, 954);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.unload_btn);
            this.groupBox2.Controls.Add(this.load_btn);
            this.groupBox2.Controls.Add(this.save_btn);
            this.groupBox2.Controls.Add(this.import_btn);
            this.groupBox2.Controls.Add(this.delete_btn);
            this.groupBox2.Location = new System.Drawing.Point(18, 18);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(364, 168);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dangerous";
            // 
            // unload_btn
            // 
            this.unload_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.unload_btn.Location = new System.Drawing.Point(9, 118);
            this.unload_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.unload_btn.Name = "unload_btn";
            this.unload_btn.Size = new System.Drawing.Size(164, 35);
            this.unload_btn.TabIndex = 15;
            this.unload_btn.Text = "Unload";
            this.unload_btn.UseVisualStyleBackColor = true;
            this.unload_btn.Click += new System.EventHandler(this.unload_btn_Click);
            // 
            // load_btn
            // 
            this.load_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.load_btn.Location = new System.Drawing.Point(182, 72);
            this.load_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.load_btn.Name = "load_btn";
            this.load_btn.Size = new System.Drawing.Size(164, 35);
            this.load_btn.TabIndex = 14;
            this.load_btn.Text = "Load";
            this.load_btn.UseVisualStyleBackColor = true;
            this.load_btn.Click += new System.EventHandler(this.load_btn_Click);
            // 
            // save_btn
            // 
            this.save_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.save_btn.Location = new System.Drawing.Point(9, 74);
            this.save_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.save_btn.Name = "save_btn";
            this.save_btn.Size = new System.Drawing.Size(164, 35);
            this.save_btn.TabIndex = 13;
            this.save_btn.Text = "Save";
            this.save_btn.UseVisualStyleBackColor = true;
            this.save_btn.Click += new System.EventHandler(this.save_btn_Click);
            // 
            // import_btn
            // 
            this.import_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.import_btn.Location = new System.Drawing.Point(182, 29);
            this.import_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.import_btn.Name = "import_btn";
            this.import_btn.Size = new System.Drawing.Size(164, 35);
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
            this.groupBox3.Location = new System.Drawing.Point(18, 195);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(364, 175);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Building";
            // 
            // indicator_btn
            // 
            this.indicator_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.indicator_btn.Location = new System.Drawing.Point(9, 29);
            this.indicator_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.indicator_btn.Name = "indicator_btn";
            this.indicator_btn.Size = new System.Drawing.Size(164, 35);
            this.indicator_btn.TabIndex = 18;
            this.indicator_btn.Text = "Add Indicator";
            this.indicator_btn.UseVisualStyleBackColor = true;
            this.indicator_btn.Click += new System.EventHandler(this.indicator_btn_Click);
            // 
            // outcomeCode_btn
            // 
            this.outcomeCode_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.outcomeCode_btn.Location = new System.Drawing.Point(182, 118);
            this.outcomeCode_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.outcomeCode_btn.Name = "outcomeCode_btn";
            this.outcomeCode_btn.Size = new System.Drawing.Size(164, 35);
            this.outcomeCode_btn.TabIndex = 17;
            this.outcomeCode_btn.Text = "Add outc. Label";
            this.outcomeCode_btn.UseVisualStyleBackColor = true;
            this.outcomeCode_btn.Click += new System.EventHandler(this.outcomeCode_btn_Click);
            // 
            // metaIndicatorSum_btn
            // 
            this.metaIndicatorSum_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.metaIndicatorSum_btn.Location = new System.Drawing.Point(9, 74);
            this.metaIndicatorSum_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.metaIndicatorSum_btn.Name = "metaIndicatorSum_btn";
            this.metaIndicatorSum_btn.Size = new System.Drawing.Size(164, 35);
            this.metaIndicatorSum_btn.TabIndex = 15;
            this.metaIndicatorSum_btn.Text = "Add Sum";
            this.metaIndicatorSum_btn.UseVisualStyleBackColor = true;
            this.metaIndicatorSum_btn.Click += new System.EventHandler(this.metaIndicatorSum_btn_Click);
            // 
            // addData_btn
            // 
            this.addData_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addData_btn.Location = new System.Drawing.Point(182, 29);
            this.addData_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.addData_btn.Name = "addData_btn";
            this.addData_btn.Size = new System.Drawing.Size(164, 35);
            this.addData_btn.TabIndex = 14;
            this.addData_btn.Text = "Add Data";
            this.addData_btn.UseVisualStyleBackColor = true;
            this.addData_btn.Click += new System.EventHandler(this.addData_btn_Click);
            // 
            // outcome_btn
            // 
            this.outcome_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.outcome_btn.Location = new System.Drawing.Point(9, 118);
            this.outcome_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.outcome_btn.Name = "outcome_btn";
            this.outcome_btn.Size = new System.Drawing.Size(164, 35);
            this.outcome_btn.TabIndex = 13;
            this.outcome_btn.Text = "Add Outcome";
            this.outcome_btn.UseVisualStyleBackColor = true;
            this.outcome_btn.Click += new System.EventHandler(this.outcome_btn_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.outcome_sampling_button);
            this.groupBox4.Location = new System.Drawing.Point(18, 380);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(364, 80);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Analysis";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.optimizeParametersNN_btn);
            this.groupBox5.Controls.Add(this.create_ann_button);
            this.groupBox5.Location = new System.Drawing.Point(20, 471);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox5.Size = new System.Drawing.Size(363, 80);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "AI";
            // 
            // optimizeParametersNN_btn
            // 
            this.optimizeParametersNN_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optimizeParametersNN_btn.Location = new System.Drawing.Point(180, 29);
            this.optimizeParametersNN_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.optimizeParametersNN_btn.Name = "optimizeParametersNN_btn";
            this.optimizeParametersNN_btn.Size = new System.Drawing.Size(164, 35);
            this.optimizeParametersNN_btn.TabIndex = 10;
            this.optimizeParametersNN_btn.Text = "Optimize ANN";
            this.optimizeParametersNN_btn.UseVisualStyleBackColor = true;
            this.optimizeParametersNN_btn.Click += new System.EventHandler(this.optimizeParametersNN_btn_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.button_start_q);
            this.groupBox6.Location = new System.Drawing.Point(20, 549);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox6.Size = new System.Drawing.Size(363, 77);
            this.groupBox6.TabIndex = 22;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Misc";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.textBox1);
            this.groupBox7.Location = new System.Drawing.Point(1206, 18);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox7.Size = new System.Drawing.Size(626, 954);
            this.groupBox7.TabIndex = 23;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Output";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.data_textbox);
            this.groupBox8.Controls.Add(this.updateInfo_Btn);
            this.groupBox8.Location = new System.Drawing.Point(855, 18);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox8.Size = new System.Drawing.Size(342, 954);
            this.groupBox8.TabIndex = 24;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Data";
            // 
            // data_textbox
            // 
            this.data_textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.data_textbox.Location = new System.Drawing.Point(14, 23);
            this.data_textbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.data_textbox.Multiline = true;
            this.data_textbox.Name = "data_textbox";
            this.data_textbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.data_textbox.Size = new System.Drawing.Size(318, 875);
            this.data_textbox.TabIndex = 12;
            this.data_textbox.Text = "no data yet";
            // 
            // updateInfo_Btn
            // 
            this.updateInfo_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateInfo_Btn.Location = new System.Drawing.Point(14, 909);
            this.updateInfo_Btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.updateInfo_Btn.Name = "updateInfo_Btn";
            this.updateInfo_Btn.Size = new System.Drawing.Size(320, 35);
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
            this.panel1.Location = new System.Drawing.Point(18, 635);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(364, 336);
            this.panel1.TabIndex = 25;
            // 
            // DataminingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1848, 991);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            this.groupBox8.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button optimizeParametersNN_btn;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button save_btn;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button load_btn;
        private System.Windows.Forms.Button updateInfo_Btn;
        private System.Windows.Forms.TextBox data_textbox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button unload_btn;
    }
}