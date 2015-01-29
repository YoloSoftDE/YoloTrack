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
        public ProfileCard()
        {
            InitializeComponent();
        }

        private void ProfileCard_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }

        public string FullName
        {
            set { lbl_name.Text = value; }
        }

        public int TrackedCount
        {
            set { lbl_tracked.Text = value.ToString(); }
        }

        public int RecognizedCount
        {
            set { lbl_view_count.Text = value.ToString(); }
        }

        public DateTime LearnedAt
        {
            set { lbl_learned.Text = value.ToShortDateString(); }
        }

        public Image Picture
        {
            set { pictureBox1.Image = value; }
        }
    }
}