namespace YoloTrack.MVC.View.Components
{
    partial class DatabaseViewHeader
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
            this.panel1.Size = new System.Drawing.Size(314, 40);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label_count);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(142, 15);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.panel2.Size = new System.Drawing.Size(170, 19);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(115, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "records.";
            // 
            // label_count
            // 
            this.label_count.AutoSize = true;
            this.label_count.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_count.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_count.Location = new System.Drawing.Point(102, 0);
            this.label_count.Margin = new System.Windows.Forms.Padding(0);
            this.label_count.Name = "label_count";
            this.label_count.Size = new System.Drawing.Size(13, 13);
            this.label_count.TabIndex = 6;
            this.label_count.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Database contains";
            // 
            // button_merge
            // 
            this.button_merge.BackColor = System.Drawing.SystemColors.Control;
            this.button_merge.Enabled = false;
            this.button_merge.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_merge.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_merge.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.button_merge.Location = new System.Drawing.Point(3, 5);
            this.button_merge.Name = "button_merge";
            this.button_merge.Size = new System.Drawing.Size(133, 32);
            this.button_merge.TabIndex = 0;
            this.button_merge.Text = "Select items to merge";
            this.button_merge.UseVisualStyleBackColor = false;
            this.button_merge.Click += new System.EventHandler(this.button_merge_Click);
            // 
            // DatabaseViewHeader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "DatabaseViewHeader";
            this.Size = new System.Drawing.Size(314, 40);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_count;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_merge;
    }
}
