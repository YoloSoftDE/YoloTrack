using System;
using System.Drawing;
using System.Windows.Forms;

namespace YoloTrack.MVC.View.Components
{
    public partial class DetailEditView : UserControl
    {
        public Image Image { get; set; }

        public DetailEditView()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void DetailEditView_Load(object sender, EventArgs e)
        {

        }

        private void DetailEditView_Paint(object sender, PaintEventArgs e)
        {
            Pen blackpen = new Pen(SystemColors.ActiveBorder, 1);
            Graphics g = e.Graphics;
            g.DrawLine(blackpen, 0, 0, Width, 0);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            DialogResult user_input = openFileDialog1.ShowDialog();
            if (user_input == DialogResult.OK)
            {
                Image = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = Image;
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (Image == null)
            {
                pictureBox1.BackColor = SystemColors.ControlDarkDark;
            }
            toolTip1.Show("Click to select another image for this record", pictureBox1);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (Image == null)
            {
                pictureBox1.BackColor = SystemColors.ControlDark;
            }
        }
    }
}
