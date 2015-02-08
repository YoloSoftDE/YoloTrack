namespace YoloTrack.Source.MVC.View.Components
{
    partial class EditLabel
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
            this.m_textbox = new System.Windows.Forms.TextBox();
            this.m_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_textbox
            // 
            this.m_textbox.Location = new System.Drawing.Point(0, 0);
            this.m_textbox.Margin = new System.Windows.Forms.Padding(0);
            this.m_textbox.Name = "m_textbox";
            this.m_textbox.Size = new System.Drawing.Size(53, 20);
            this.m_textbox.TabIndex = 1;
            this.m_textbox.Text = "test";
            this.m_textbox.Visible = false;
            this.m_textbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_textbox_KeyDown);
            // 
            // m_label
            // 
            this.m_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_label.Location = new System.Drawing.Point(0, 0);
            this.m_label.Margin = new System.Windows.Forms.Padding(0);
            this.m_label.Name = "m_label";
            this.m_label.Padding = new System.Windows.Forms.Padding(2, 1, 0, 0);
            this.m_label.Size = new System.Drawing.Size(217, 30);
            this.m_label.TabIndex = 2;
            this.m_label.Text = "label1";
            this.m_label.Click += new System.EventHandler(this.m_label_Click);
            // 
            // EditLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.m_label);
            this.Controls.Add(this.m_textbox);
            this.Name = "EditLabel";
            this.Size = new System.Drawing.Size(217, 110);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_textbox;
        private System.Windows.Forms.Label m_label;
    }
}
