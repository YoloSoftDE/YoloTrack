using System;
using System.Windows.Forms;
using System.Drawing;

namespace YoloTrack.MVC.View.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DatabaseViewHeader : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler MergeClick;

        /// <summary>
        /// 
        /// </summary>
        public int SelectedItems
        {
            set
            {
                if (value >= 2)
                {
                    button_merge.Text = "Merge " + value + " items";
                    button_merge.Enabled = true;
                }
                else
                {
                    button_merge.Text = "Select items to merge";
                    button_merge.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ItemCount
        {
            set
            {
                label_count.Text = value.ToString();
                if (value == 1)
                {
                    label1.Text = "record.";
                }
                else
                {
                    label1.Text = "records.";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasBottomLine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DatabaseViewHeader()
        {
            InitializeComponent();

            DoubleBuffered = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_merge_Click(object sender, EventArgs e)
        {
            if (MergeClick != null)
            {
                MergeClick(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Pen blackpen = new Pen(SystemColors.ActiveBorder, 1);
            Graphics g = e.Graphics;
            g.DrawLine(blackpen, 0, panel1.Height - 1, Width, panel1.Height - 1);
            g.Dispose();
        }
    }
}
