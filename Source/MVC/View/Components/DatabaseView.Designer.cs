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
            this.control_container = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // control_container
            // 
            this.control_container.AutoScroll = true;
            this.control_container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.control_container.Location = new System.Drawing.Point(0, 0);
            this.control_container.Name = "control_container";
            this.control_container.Size = new System.Drawing.Size(312, 252);
            this.control_container.TabIndex = 1;
            // 
            // DatabaseView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.control_container);
            this.Name = "DatabaseView";
            this.Size = new System.Drawing.Size(312, 252);
            this.Resize += new System.EventHandler(this.DatabaseView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel control_container;




    }
}
