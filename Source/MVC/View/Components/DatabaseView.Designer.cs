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
            this.databaseViewHeader1 = new YoloTrack.MVC.View.Components.DatabaseViewHeader();
            this.SuspendLayout();
            // 
            // control_container
            // 
            this.control_container.AutoScroll = true;
            this.control_container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.control_container.Location = new System.Drawing.Point(0, 40);
            this.control_container.Name = "control_container";
            this.control_container.Padding = new System.Windows.Forms.Padding(3);
            this.control_container.Size = new System.Drawing.Size(312, 212);
            this.control_container.TabIndex = 4;
            this.control_container.Click += new System.EventHandler(this.control_container_Click);
            this.control_container.Paint += new System.Windows.Forms.PaintEventHandler(this.control_container_Paint);
            // 
            // databaseViewHeader1
            // 
            this.databaseViewHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.databaseViewHeader1.HasBottomLine = false;
            this.databaseViewHeader1.Location = new System.Drawing.Point(0, 0);
            this.databaseViewHeader1.Margin = new System.Windows.Forms.Padding(0);
            this.databaseViewHeader1.Name = "databaseViewHeader1";
            this.databaseViewHeader1.Size = new System.Drawing.Size(312, 40);
            this.databaseViewHeader1.TabIndex = 3;
            this.databaseViewHeader1.MergeClick += new System.EventHandler(this.databaseViewHeader1_MergeClick);
            // 
            // DatabaseView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.control_container);
            this.Controls.Add(this.databaseViewHeader1);
            this.Name = "DatabaseView";
            this.Size = new System.Drawing.Size(312, 252);
            this.Resize += new System.EventHandler(this.DatabaseView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private DatabaseViewHeader databaseViewHeader1;
        private System.Windows.Forms.FlowLayoutPanel control_container;






    }
}
