namespace YoloTrack.MVC.View.Components
{
    partial class DatabaseViewItem
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.picture_box_image = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_name = new System.Windows.Forms.Label();
            this.label_id = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_text_counters = new System.Windows.Forms.Label();
            this.label_learned_at = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_box_image)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.picture_box_image, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(298, 64);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // picture_box_image
            // 
            this.picture_box_image.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.picture_box_image.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picture_box_image.ErrorImage = null;
            this.picture_box_image.Location = new System.Drawing.Point(3, 3);
            this.picture_box_image.Name = "picture_box_image";
            this.picture_box_image.Size = new System.Drawing.Size(60, 58);
            this.picture_box_image.TabIndex = 1;
            this.picture_box_image.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(66, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.625F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59.375F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(232, 64);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_name);
            this.panel1.Controls.Add(this.label_id);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 20);
            this.panel1.TabIndex = 3;
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_name.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_name.Location = new System.Drawing.Point(0, 0);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(0, 19);
            this.label_name.TabIndex = 8;
            // 
            // label_id
            // 
            this.label_id.AutoSize = true;
            this.label_id.BackColor = System.Drawing.SystemColors.Highlight;
            this.label_id.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_id.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_id.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_id.Location = new System.Drawing.Point(0, 0);
            this.label_id.Name = "label_id";
            this.label_id.Size = new System.Drawing.Size(0, 19);
            this.label_id.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label_text_counters);
            this.panel2.Controls.Add(this.label_learned_at);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(226, 32);
            this.panel2.TabIndex = 4;
            // 
            // label_text_counters
            // 
            this.label_text_counters.AutoSize = true;
            this.label_text_counters.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_text_counters.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label_text_counters.Location = new System.Drawing.Point(0, 15);
            this.label_text_counters.Name = "label_text_counters";
            this.label_text_counters.Size = new System.Drawing.Size(26, 13);
            this.label_text_counters.TabIndex = 8;
            this.label_text_counters.Text = "test";
            // 
            // label_learned_at
            // 
            this.label_learned_at.AutoSize = true;
            this.label_learned_at.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_learned_at.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label_learned_at.Location = new System.Drawing.Point(0, 0);
            this.label_learned_at.Name = "label_learned_at";
            this.label_learned_at.Size = new System.Drawing.Size(26, 13);
            this.label_learned_at.TabIndex = 7;
            this.label_learned_at.Text = "test";
            // 
            // DatabaseViewItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DatabaseViewItem";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(300, 66);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DatabaseViewItem_Paint);
            this.MouseEnter += new System.EventHandler(this.DatabaseViewItem_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.DatabaseViewItem_MouseLeave);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picture_box_image)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox picture_box_image;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_id;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label_text_counters;
        private System.Windows.Forms.Label label_learned_at;
        private System.Windows.Forms.Label label_name;
    }
}
