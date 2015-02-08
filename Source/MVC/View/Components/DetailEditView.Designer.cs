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
            this.m_userImage = new System.Windows.Forms.PictureBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_editLabel_Last = new YoloTrack.Source.MVC.View.Components.EditLabel();
            this.m_editLabel_First = new YoloTrack.Source.MVC.View.Components.EditLabel();
            ((System.ComponentModel.ISupportInitialize)(this.m_userImage)).BeginInit();
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
            this.m_userImage.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.m_userImage.ErrorImage = null;
            this.m_userImage.Location = new System.Drawing.Point(8, 10);
            this.m_userImage.Name = "pictureBox1";
            this.m_userImage.Size = new System.Drawing.Size(142, 133);
            this.m_userImage.TabIndex = 5;
            this.m_userImage.TabStop = false;
            this.m_userImage.Click += new System.EventHandler(this.m_userImage_Click);
            this.m_userImage.MouseEnter += new System.EventHandler(this.m_userImage_MouseEnter);
            this.m_userImage.MouseLeave += new System.EventHandler(this.m_userImage_MouseLeave);
            // 
            // openFileDialog1
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // editLabel2
            // 
            this.m_editLabel_Last.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.m_editLabel_Last.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_editLabel_Last.Location = new System.Drawing.Point(159, 33);
            this.m_editLabel_Last.Margin = new System.Windows.Forms.Padding(6);
            this.m_editLabel_Last.Name = "editLabel2";
            this.m_editLabel_Last.Size = new System.Drawing.Size(123, 33);
            this.m_editLabel_Last.TabIndex = 11;
            // 
            // editLabel1
            // 
            this.m_editLabel_First.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.m_editLabel_First.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_editLabel_First.Location = new System.Drawing.Point(159, 6);
            this.m_editLabel_First.Margin = new System.Windows.Forms.Padding(6);
            this.m_editLabel_First.Name = "editLabel1";
            this.m_editLabel_First.Size = new System.Drawing.Size(123, 32);
            this.m_editLabel_First.TabIndex = 10;
            // 
            // DetailEditView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_editLabel_Last);
            this.Controls.Add(this.m_editLabel_First);
            this.Controls.Add(this.button_track);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.m_userImage);
            this.Name = "DetailEditView";
            this.Size = new System.Drawing.Size(293, 296);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DetailEditView_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.m_userImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_track;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.PictureBox m_userImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolTip toolTip;
        private Source.MVC.View.Components.EditLabel m_editLabel_First;
        private Source.MVC.View.Components.EditLabel m_editLabel_Last;
    }
}
