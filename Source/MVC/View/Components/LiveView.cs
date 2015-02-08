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
            InitializeComponent(); /* Great code inside here */

            /* Turn on double buffering */
            this.DoubleBuffered = true;

            /* Set default alignment mode */
            this.ImageAlign = ContentAlignment.TopLeft;
        }

        /// <summary>
        /// Get or set the image aligment mode
        /// </summary>
        public ContentAlignment ImageAlign { get; set; }

        /// <summary>
        /// Get or set the image displayed
        /// </summary>
        public Image Image
        {
            get
            {
                return m_image;
            }
            set
            {
                /* Dispose no longer used image
                 * Will result in a memoryleak otherwise!
                 */
                if (m_image != null)
                    m_image.Dispose();

                /* Store new image */
                m_image = value;



                /* We're using Refresh() since Invalidate() will
                 * cause a random unavoidable CrossThreadAccessViolation */
                try
                {
                    this.Refresh();
                }
                catch (InvalidOperationException)
                {
                    /* We're avoiding the CrossThreadAccessViolation but
                     * keep getting an InvalidOperationException pure randomly.
                     * At least we can catch that safely ...
                     */
                }


            }
        }

        /// <summary>
        /// Refreshes the LiveViews Layout
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            /* Skip first draw */
            if (this.Image == null)
            {
                return;
            }

            /* Calculate AspectRatio */
            float ar = (float)this.Image.Width / (float)this.Image.Height;
            float nheight, nwidth;
            int xpos = 0;
            int ypos = 0;

            /* Calculate new dimensions based on the longer/shorter edge
             * and matching the aspect-ratio.
             */
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

            /* Calculate Offset-values */
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
                    /* This error should NEVER appear */
                    throw new NotImplementedException("Invalid ImageAlign Mode");
            }

            /* Fill Background */
            e.Graphics.Clear(this.BackColor);

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
