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

        public int Id { get; set; }

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

        public DateTime LearnedAt
        {
            set {  }
        }

        public Image Picture
        {
            set { pb_profile_picture.Image = value; }
        }

        private void lbl_first_name_Click(object sender, EventArgs e)
        {
            TextBox tb_first_name = new TextBox() 
            {
                Text = lbl_first_name.Text,
                Left = lbl_first_name.Left,
                Top = lbl_first_name.Top
            };

            lbl_first_name.Visible = false;
            lbl_first_name.Parent.Controls.Add(tb_first_name);

            tb_first_name.Leave += new EventHandler(tb_first_name_Leave);
        }

        void tb_first_name_Leave(object sender, EventArgs e)
        {
            TextBox tb_first_name = (TextBox)sender;
            lbl_first_name.Text = tb_first_name.Text;
            lbl_first_name.Parent.Controls.Remove(tb_first_name);
            lbl_first_name.Visible = true;
        }
    }
}