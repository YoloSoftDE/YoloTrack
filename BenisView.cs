using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YoloTrack
{
    public partial class BenisView : UserControl
    {
        protected Image m_image = null;
        protected int m_count = 0;

        public BenisView()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public Image Image
        {
            get
            {
                return m_image;
            }
            set
            {
                m_image = value;

                /* Invalidate SUCKS!!!!
                 * PictureBox SUCKS!!!!
                 */
                this.Refresh();

                
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Image == null)
            {
                return;
            }
            

            e.Graphics.DrawImage(this.Image, new Point(0,0));
            
        }

    }
}
