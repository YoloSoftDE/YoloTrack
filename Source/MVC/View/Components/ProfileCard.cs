using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YoloTrack.MVC.View.Components
{
    public partial class ProfileCard : UserControl
    {
        public event EventHandler DeleteButtonClicked;

        public ProfileCard()
        {
            InitializeComponent();
        }

        private void ProfileCard_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, SystemColors.ControlDark, ButtonBorderStyle.Solid);
        }

        public string FirstName
        {
            set { lbl_first_name.Text = value; }
        }

        public string LastName
        {
            set { lbl_last_name.Text = value; }
        }

        public int TrackedCount
        {
            set { lbl_tracked_count.Text = value.ToString(); }
        }

        public int RecognizedCount
        {
            set { lbl_recognized_count.Text = value.ToString(); }
        }

        int m_id;
        public int Id
        {
            set { m_id = value;  lv_details.Items[0].SubItems[1].Text = value.ToString(); }
            get { return m_id;  }
        }

        public DateTime LearnedAt
        {
            set { lv_details.Items[1].SubItems[1].Text = value.ToShortDateString() + " " + value.ToShortTimeString(); }
        }

        public Image Picture
        {
            set { pb_profile_picture.Image = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DeleteButtonClicked != null)
            {
                DeleteButtonClicked(this, new EventArgs());
            }
        }
    }
}