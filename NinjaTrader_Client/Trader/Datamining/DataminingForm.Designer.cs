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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.create_ann_button = new System.Windows.Forms.Button();
            this.button_deleteAll = new System.Windows.Forms.Button();
            this.button_start_q = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.outcome_sampling_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(646, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(148, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(477, 169);
            this.textBox1.TabIndex = 6;
            // 
            // create_ann_button
            // 
            this.create_ann_button.Location = new System.Drawing.Point(263, 187);
            this.create_ann_button.Name = "create_ann_button";
            this.create_ann_button.Size = new System.Drawing.Size(109, 23);
            this.create_ann_button.TabIndex = 9;
            this.create_ann_button.Text = "Create ANN";
            this.create_ann_button.UseVisualStyleBackColor = true;
            this.create_ann_button.Click += new System.EventHandler(this.create_ann_button_click);
            // 
            // button_deleteAll
            // 
            this.button_deleteAll.Location = new System.Drawing.Point(148, 187);
            this.button_deleteAll.Name = "button_deleteAll";
            this.button_deleteAll.Size = new System.Drawing.Size(109, 23);
            this.button_deleteAll.TabIndex = 11;
            this.button_deleteAll.Text = "Delete all";
            this.button_deleteAll.UseVisualStyleBackColor = true;
            this.button_deleteAll.Click += new System.EventHandler(this.button_deleteAll_Click);
            // 
            // button_start_q
            // 
            this.button_start_q.Location = new System.Drawing.Point(263, 299);
            this.button_start_q.Name = "button_start_q";
            this.button_start_q.Size = new System.Drawing.Size(109, 23);
            this.button_start_q.TabIndex = 14;
            this.button_start_q.Text = "Start big operation";
            this.button_start_q.UseVisualStyleBackColor = true;
            this.button_start_q.Click += new System.EventHandler(this.button_start_q_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 420);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "label2";
            // 
            // outcome_sampling_button
            // 
            this.outcome_sampling_button.Location = new System.Drawing.Point(516, 299);
            this.outcome_sampling_button.Name = "outcome_sampling_button";
            this.outcome_sampling_button.Size = new System.Drawing.Size(109, 23);
            this.outcome_sampling_button.TabIndex = 16;
            this.outcome_sampling_button.Text = "Sample";
            this.outcome_sampling_button.UseVisualStyleBackColor = true;
            this.outcome_sampling_button.Click += new System.EventHandler(this.outcome_sampling_button_Click);
            // 
            // DataminingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1233, 442);
            this.Controls.Add(this.outcome_sampling_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_start_q);
            this.Controls.Add(this.button_deleteAll);
            this.Controls.Add(this.create_ann_button);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "DataminingForm";
            this.Text = "DataminingForm";
            this.Load += new System.EventHandler(this.DataminingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button create_ann_button;
        private System.Windows.Forms.Button button_deleteAll;
        private System.Windows.Forms.Button button_start_q;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button outcome_sampling_button;
    }
}