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
    public partial class LiveView : UserControl
    {
        protected Image m_image = null;
        protected int m_count = 0;

        public LiveView()
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

        protected override void OnPaint (PaintEventArgs e)
		{
			if (this.Image == null) {
				return;
			}
            

			//e.Graphics.DrawImage (this.Image, new Point (0, 0)); /* Old Code */
			
			int new_height, new_width;
			
			if (this.ClientRectangle.Width > this.ClientRectangle.Height) {
				new_height = this.ClientRectangle.Width * this.Image.Height / this.ClientRectangle.Height;
				new_width = this.ClientRectangle.Height * this.Image.Height / this.ClientRectangle.Height;
			} else {
				new_height = this.ClientRectangle.Width * this.Image.Width / this.ClientRectangle.Width;
				new_width = this.ClientRectangle.Height * this.Image.Width / this.ClientRectangle.Width;	
			}
			
			e.Graphics.DrawImage (
				this.Image, 					/* Image instance */
				0, 								/* Target X */
				0, 								/* Target Y */
				new_width,						/* Target Width  */
				new_height						/* Target Height */
			);
            
        }
    }
}
