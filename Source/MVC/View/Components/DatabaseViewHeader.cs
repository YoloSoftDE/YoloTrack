using System;
using System.Windows.Forms;
using System.Drawing;

namespace YoloTrack.MVC.View.Components
{
    /// <summary>
    /// Some buttons on a control, we love controls
    /// </summary>
    public partial class DatabaseViewHeader : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler MergeClick;

        /// <summary>
        /// Set the number of selected items to change the button-face
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
        /// Set the number of total items
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
        /// Get or set the visibility of the bottom-line
        /// </summary>
        public bool HasBottomLine { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DatabaseViewHeader()
        {
            InitializeComponent();

            DoubleBuffered = false;
        }

        /// <summary>
        /// Eventhandler for button-click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_merge_Click(object sender, EventArgs e)
        {
            if (this.MergeClick != null)
            {
                this.MergeClick(this, e);
            }
        }

        /// <summary>
        /// EventHandler for OnPaint (Draws a line)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Pen blackpen = new Pen(SystemColors.ActiveBorder, 1);
            Graphics g = e.Graphics;
            g.DrawLine(blackpen, 0, panel1.Height - 1, Width, panel1.Height - 1);

            // @todo: this can't be right ... :/

            g.Dispose();
        }
    }
}
