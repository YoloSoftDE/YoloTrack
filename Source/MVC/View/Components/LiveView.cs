using System;
using System.Drawing;
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
            this.ImageAlign = ContentAlignment.TopLeft;
        }

        public ContentAlignment ImageAlign
        {
            get;
            set;
        }

        public Image Image
        {
            get
            {
                return m_image;
            }
            set
            {
                if (m_image != null)
                    m_image.Dispose();
                m_image = value;

                /* Invalidate SUCKS!!!!
                 * PictureBox SUCKS!!!!
                 */
                this.Refresh(); // FIXME: may throw InOpEx


            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Image == null)
            {
                return;
            }


            float ar = (float)this.Image.Width / (float)this.Image.Height;
            float nheight, nwidth;
            int xpos = 0;
            int ypos = 0;

            if ((float)this.ClientRectangle.Height * ar < (float)ClientRectangle.Width)
            {
                nheight = (float)this.ClientRectangle.Height;
                nwidth = (float)this.ClientRectangle.Height * ar;
            }
            else
            {
                nheight = (float)this.ClientRectangle.Width * 1 / ar;
                nwidth = (float)this.ClientRectangle.Width;
            }

            switch (this.ImageAlign)
            {
                #region Alignment Top
                case ContentAlignment.TopLeft:
                    break;


                case ContentAlignment.TopRight:
                    if (nwidth < this.ClientRectangle.Width)
                    {
                        xpos = this.ClientRectangle.Width - (int)nwidth;
                    }
                    break;

                case ContentAlignment.TopCenter:
                    if (nwidth < this.ClientRectangle.Width)
                    {
                        xpos = (int)((float)(this.ClientRectangle.Width - (int)nwidth) / 2);
                    }
                    break;
                #endregion
                #region Alignment Middle
                case ContentAlignment.MiddleLeft:
                    if (nheight < this.ClientRectangle.Height)
                    {
                        ypos = (int)((float)(this.ClientRectangle.Height - (int)nheight) / 2);
                    }
                    break;

                case ContentAlignment.MiddleCenter:
                    if (nwidth < this.ClientRectangle.Width)
                    {
                        xpos = (int)((float)(this.ClientRectangle.Width - (int)nwidth) / 2);
                    }

                    if (nheight < this.ClientRectangle.Height)
                    {
                        ypos = (int)((float)(this.ClientRectangle.Height - (int)nheight) / 2);
                    }
                    break;

                case ContentAlignment.MiddleRight:

                    if (nwidth < this.ClientRectangle.Width)
                    {
                        xpos = this.ClientRectangle.Width - (int)nwidth;
                    }

                    if (nheight < this.ClientRectangle.Height)
                    {
                        ypos = (int)((float)(this.ClientRectangle.Height - (int)nheight) / 2);
                    }
                    break;
                #endregion
                #region Alignment Bottom
                case ContentAlignment.BottomLeft:
                    if (nheight < this.ClientRectangle.Height)
                    {
                        ypos = this.ClientRectangle.Height - (int)nheight;
                    }
                    break;

                case ContentAlignment.BottomCenter:
                    if (nheight < this.ClientRectangle.Height)
                    {
                        ypos = this.ClientRectangle.Height - (int)nheight;
                    }
                    if (nwidth < this.ClientRectangle.Width)
                    {
                        xpos = (int)((float)(this.ClientRectangle.Width - (int)nwidth) / 2);
                    }
                    break;

                case ContentAlignment.BottomRight:
                    if (nheight < this.ClientRectangle.Height)
                    {
                        ypos = this.ClientRectangle.Height - (int)nheight;
                    }
                    if (nwidth < this.ClientRectangle.Width)
                    {
                        xpos = this.ClientRectangle.Width - (int)nwidth;
                    }
                    break;

                #endregion

                default:
                    throw new NotImplementedException("FAILERRORFAIL");
            }

            e.Graphics.Clear(BackColor);

            e.Graphics.DrawImage(
                this.Image, /* Image instance */
                xpos, /* Target X */
                ypos, /* Target Y */
                nwidth, /* Target Width */
                nheight /* Target Height */
            );
        }
    }
}
