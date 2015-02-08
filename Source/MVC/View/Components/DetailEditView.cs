using System;
using System.Drawing;
using System.Windows.Forms;

namespace YoloTrack.MVC.View.Components
{
    /// <summary>
    /// Great summary control
    /// </summary>
    public partial class DetailEditView : UserControl
    {
        /// <summary>
        /// Get or set the UserImage
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DetailEditView()
        {
            InitializeComponent();
            DoubleBuffered = true;

            m_editLabel_First.Text = "";
            m_editLabel_Last.Text = "";
        }

        /// <summary>
        /// Hook to the OnPaint-Method. Draw a separator-line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailEditView_Paint(object sender, PaintEventArgs e)
        {
            /* Draw a separator line */
            e.Graphics.DrawLine(
                new Pen(SystemColors.ActiveBorder, 1), 
                0, 
                0, 
                this.Width,
                0
            );
        }

        /// <summary>
        /// UserImage-Clickevent opens a filebrowser for a customized image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_userImage_Click(object sender, EventArgs e)
        {
            /* Open the dialog */
            this.openFileDialog.Multiselect = false;
            DialogResult user_input = openFileDialog.ShowDialog();

            /* Check if the user pressed the correct buttons */
            if (user_input == DialogResult.OK)
            {
                /* Apply Image */
                this.Image = Image.FromFile(this.openFileDialog.FileName);
                this.m_userImage.Image = this.Image;
            }
        }

        /// <summary>
        /// Add some visual feedback for the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_userImage_MouseEnter(object sender, EventArgs e)
        {
            /* Change BG-Color if no image present */
            if (this.Image == null)
            {
                this.m_userImage.BackColor = SystemColors.ControlDarkDark;
            }

            /* Show tooltip */
            this.toolTip.Show("Click to select another image for this record", this.m_userImage);
        }

        /// <summary>
        /// Add some visual feedback for the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_userImage_MouseLeave(object sender, EventArgs e)
        {
            /* Change BG-Color if no image present */
            if (this.Image == null)
            {
                this.m_userImage.BackColor = SystemColors.ControlDark;
            }
        }
    }
}
