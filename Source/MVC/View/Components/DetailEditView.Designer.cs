namespace YoloTrack.MVC.View.Components
{
    partial class DetailEditView
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
            this.components = new System.ComponentModel.Container();
            this.button_track = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.editLabel2 = new YoloTrack.Source.MVC.View.Components.EditLabel();
            this.editLabel1 = new YoloTrack.Source.MVC.View.Components.EditLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_track
            // 
            this.button_track.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_track.Location = new System.Drawing.Point(164, 75);
            this.button_track.Name = "button_track";
            this.button_track.Size = new System.Drawing.Size(85, 30);
            this.button_track.TabIndex = 9;
            this.button_track.Text = "Track";
            this.button_track.UseVisualStyleBackColor = true;
            // 
            // button_delete
            // 
            this.button_delete.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_delete.Location = new System.Drawing.Point(164, 111);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(85, 32);
            this.button_delete.TabIndex = 8;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(8, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(142, 133);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // editLabel2
            // 
            this.editLabel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.editLabel2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editLabel2.Location = new System.Drawing.Point(159, 33);
            this.editLabel2.Margin = new System.Windows.Forms.Padding(6);
            this.editLabel2.Name = "editLabel2";
            this.editLabel2.Size = new System.Drawing.Size(123, 33);
            this.editLabel2.TabIndex = 11;
            // 
            // editLabel1
            // 
            this.editLabel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.editLabel1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editLabel1.Location = new System.Drawing.Point(159, 6);
            this.editLabel1.Margin = new System.Windows.Forms.Padding(6);
            this.editLabel1.Name = "editLabel1";
            this.editLabel1.Size = new System.Drawing.Size(123, 32);
            this.editLabel1.TabIndex = 10;
            // 
            // DetailEditView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.editLabel2);
            this.Controls.Add(this.editLabel1);
            this.Controls.Add(this.button_track);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.pictureBox1);
            this.Name = "DetailEditView";
            this.Size = new System.Drawing.Size(293, 296);
            this.Load += new System.EventHandler(this.DetailEditView_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DetailEditView_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_track;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        private Source.MVC.View.Components.EditLabel editLabel1;
        private Source.MVC.View.Components.EditLabel editLabel2;
    }
}
