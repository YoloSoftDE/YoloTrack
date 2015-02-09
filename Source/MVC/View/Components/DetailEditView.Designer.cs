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
            this.button_delete = new System.Windows.Forms.Button();
            this.label_counters = new System.Windows.Forms.Label();
            this.label_learned_at = new System.Windows.Forms.Label();
            this.m_label_trackingid = new System.Windows.Forms.Label();
            this.label_id = new System.Windows.Forms.Label();
            this.button_track = new System.Windows.Forms.CheckBox();
            this.m_userImage = new Components.CrashSafePictureBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_label_last_name = new YoloTrack.Source.MVC.View.Components.EditLabel();
            this.m_label_first_name = new YoloTrack.Source.MVC.View.Components.EditLabel();
            this.m_label_identify_attempts = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_userImage)).BeginInit();
            this.SuspendLayout();
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
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // label_counters
            // 
            this.label_counters.AutoSize = true;
            this.label_counters.Location = new System.Drawing.Point(5, 192);
            this.label_counters.Name = "label_counters";
            this.label_counters.Size = new System.Drawing.Size(168, 13);
            this.label_counters.TabIndex = 12;
            this.label_counters.Text = "4 times seen, 1 time tracked????";
            // 
            // label_learned_at
            // 
            this.label_learned_at.AutoSize = true;
            this.label_learned_at.Location = new System.Drawing.Point(5, 163);
            this.label_learned_at.Name = "label_learned_at";
            this.label_learned_at.Size = new System.Drawing.Size(153, 13);
            this.label_learned_at.TabIndex = 13;
            this.label_learned_at.Text = "Listed since 07.02.2015 20:39";
            // 
            // m_label_trackingid
            // 
            this.m_label_trackingid.AutoSize = true;
            this.m_label_trackingid.Location = new System.Drawing.Point(5, 221);
            this.m_label_trackingid.Name = "m_label_trackingid";
            this.m_label_trackingid.Size = new System.Drawing.Size(242, 13);
            this.m_label_trackingid.TabIndex = 14;
            this.m_label_trackingid.Text = "17666214 Identify attempts, TrackingId is 1337";
            // 
            // label_id
            // 
            this.label_id.AutoSize = true;
            this.label_id.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_id.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_id.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_id.Location = new System.Drawing.Point(14, 108);
            this.label_id.Name = "label_id";
            this.label_id.Size = new System.Drawing.Size(37, 30);
            this.label_id.TabIndex = 16;
            this.label_id.Text = "42";
            // 
            // button_track
            // 
            this.button_track.Appearance = System.Windows.Forms.Appearance.Button;
            this.button_track.Location = new System.Drawing.Point(164, 73);
            this.button_track.Name = "button_track";
            this.button_track.Size = new System.Drawing.Size(85, 32);
            this.button_track.TabIndex = 17;
            this.button_track.Text = "Track";
            this.button_track.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_track.UseVisualStyleBackColor = true;
            this.button_track.CheckedChanged += new System.EventHandler(this.button_track_CheckedChanged);
            this.button_track.Click += new System.EventHandler(this.button_track_Click);
            // 
            // m_userImage
            // 
            this.m_userImage.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.m_userImage.ErrorImage = null;
            this.m_userImage.Location = new System.Drawing.Point(8, 10);
            this.m_userImage.Name = "m_userImage";
            this.m_userImage.Size = new System.Drawing.Size(142, 133);
            this.m_userImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.m_userImage.TabIndex = 5;
            this.m_userImage.TabStop = false;
            this.m_userImage.Click += new System.EventHandler(this.m_userImage_Click);
            this.m_userImage.MouseEnter += new System.EventHandler(this.m_userImage_MouseEnter);
            this.m_userImage.MouseLeave += new System.EventHandler(this.m_userImage_MouseLeave);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // m_label_last_name
            // 
            this.m_label_last_name.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.m_label_last_name.DefaultText = "LastName";
            this.m_label_last_name.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_label_last_name.Location = new System.Drawing.Point(159, 33);
            this.m_label_last_name.Margin = new System.Windows.Forms.Padding(6);
            this.m_label_last_name.Mode = YoloTrack.Source.MVC.View.Components.EditLabel.EditLabelMode.LabelMode;
            this.m_label_last_name.Name = "m_label_last_name";
            this.m_label_last_name.Size = new System.Drawing.Size(123, 33);
            this.m_label_last_name.TabIndex = 11;
            this.m_label_last_name.TextChanged += new System.EventHandler(this.label_last_name_TextChanged);
            // 
            // m_label_first_name
            // 
            this.m_label_first_name.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.m_label_first_name.DefaultText = "FirstName";
            this.m_label_first_name.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_label_first_name.Location = new System.Drawing.Point(159, 6);
            this.m_label_first_name.Margin = new System.Windows.Forms.Padding(6);
            this.m_label_first_name.Mode = YoloTrack.Source.MVC.View.Components.EditLabel.EditLabelMode.LabelMode;
            this.m_label_first_name.Name = "m_label_first_name";
            this.m_label_first_name.Size = new System.Drawing.Size(123, 32);
            this.m_label_first_name.TabIndex = 10;
            this.m_label_first_name.TextChanged += new System.EventHandler(this.label_first_name_TextChanged);
            // 
            // m_label_identify_attempts
            // 
            this.m_label_identify_attempts.AutoSize = true;
            this.m_label_identify_attempts.Location = new System.Drawing.Point(7, 250);
            this.m_label_identify_attempts.Name = "m_label_identify_attempts";
            this.m_label_identify_attempts.Size = new System.Drawing.Size(242, 13);
            this.m_label_identify_attempts.TabIndex = 18;
            this.m_label_identify_attempts.Text = "17666214 Identify attempts, TrackingId is 1337";
            // 
            // DetailEditView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_label_identify_attempts);
            this.Controls.Add(this.button_track);
            this.Controls.Add(this.label_id);
            this.Controls.Add(this.m_label_trackingid);
            this.Controls.Add(this.label_learned_at);
            this.Controls.Add(this.label_counters);
            this.Controls.Add(this.m_label_last_name);
            this.Controls.Add(this.m_label_first_name);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.m_userImage);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DetailEditView";
            this.Size = new System.Drawing.Size(293, 296);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DetailEditView_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.m_userImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_delete;
        private Source.MVC.View.Components.EditLabel m_label_first_name;
        private Source.MVC.View.Components.EditLabel m_label_last_name;
        private System.Windows.Forms.Label label_counters;
        private System.Windows.Forms.Label label_learned_at;
        private System.Windows.Forms.Label m_label_trackingid;
        private System.Windows.Forms.Label label_id;
        private System.Windows.Forms.CheckBox button_track;
        private Components.CrashSafePictureBox m_userImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label m_label_identify_attempts;
    }
}
