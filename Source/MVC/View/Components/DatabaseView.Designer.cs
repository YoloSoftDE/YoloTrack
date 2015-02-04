namespace YoloTrack.MVC.View.Components
{
    partial class DatabaseView
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label_count = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_merge = new System.Windows.Forms.Button();
            this.control_container = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.button_merge);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(312, 40);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label_count);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(115, 15);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.panel2.Size = new System.Drawing.Size(197, 19);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(110, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "records.";
            // 
            // label_count
            // 
            this.label_count.AutoSize = true;
            this.label_count.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_count.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_count.Location = new System.Drawing.Point(96, 0);
            this.label_count.Name = "label_count";
            this.label_count.Size = new System.Drawing.Size(14, 13);
            this.label_count.TabIndex = 6;
            this.label_count.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Database contains";
            // 
            // button_merge
            // 
            this.button_merge.BackColor = System.Drawing.SystemColors.Control;
            this.button_merge.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_merge.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_merge.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.button_merge.Location = new System.Drawing.Point(6, 5);
            this.button_merge.Name = "button_merge";
            this.button_merge.Size = new System.Drawing.Size(100, 32);
            this.button_merge.TabIndex = 0;
            this.button_merge.Text = "Merge";
            this.button_merge.UseVisualStyleBackColor = false;
            this.button_merge.Click += new System.EventHandler(this.button_merge_Click);
            // 
            // control_container
            // 
            this.control_container.AutoScroll = true;
            this.control_container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.control_container.Location = new System.Drawing.Point(0, 40);
            this.control_container.Name = "control_container";
            this.control_container.Size = new System.Drawing.Size(312, 212);
            this.control_container.TabIndex = 2;
            // 
            // DatabaseView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.control_container);
            this.Controls.Add(this.panel1);
            this.Name = "DatabaseView";
            this.Size = new System.Drawing.Size(312, 252);
            this.Resize += new System.EventHandler(this.DatabaseView_Resize);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel control_container;
        private System.Windows.Forms.Button button_merge;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_count;
        private System.Windows.Forms.Label label2;





    }
}
