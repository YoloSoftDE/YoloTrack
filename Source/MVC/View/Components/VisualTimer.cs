using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace YoloTrack.Source.MVC.View.Components
{
    /// <summary>
    /// Beautiful visual timer control
    /// </summary>
    public partial class VisualTimer : UserControl
    {
        /// <summary>
        /// Fired, when this countdown timeout occurs
        /// </summary>
        public event EventHandler Timeout;

        /// <summary>
        /// Get the current step value
        /// </summary>
        [DefaultValue(0)]
        public double Step { get; protected set; }

        /// <summary>
        /// Width of the animated arc
        /// </summary>
        [DefaultValue(10)]
        public int ArcWidth { get; set; }

        /// <summary>
        /// Timeout in seconds
        /// </summary>
        public int TimeoutValue { get; set; }

        /// <summary>
        /// (internal) Stores the starttime
        /// </summary>
        private DateTime m_start;

        /// <summary>
        /// Default constructor
        /// </summary>
        public VisualTimer()
        {
            this.InitializeComponent();
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Reset and start the Timer
        /// </summary>
        public void Start()
        {
            this.m_timer.Start();
            this.Step = 0;
            this.m_start = DateTime.Now;
        }

        /// <summary>
        /// Start the timer
        /// </summary>
        /// <param name="Seconds">Timeoutvalue in seconds</param>
        public void Start(int Seconds)
        {
            this.TimeoutValue = Seconds;
            this.Start();
        }

        /// <summary>
        /// EventHandler for timer-tick (force redraw layout)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_timer_Tick(object sender, EventArgs e)
        {
            /* Check if timout is reached */
            if (this.TimeoutValue - (DateTime.Now - this.m_start).TotalSeconds <= 0)
            {
                this.m_timer.Stop();
                return;
            }
            /* Calculate current step value */
            this.Step = (DateTime.Now - this.m_start).TotalMilliseconds;

            /* Force redrawing the whole control */
            this.Invalidate();

            /* Cast to int because Math.Round() is for faggets */
            this.label_time.Text = ((int)(this.TimeoutValue - (DateTime.Now - this.m_start).TotalSeconds)).ToString();
        }

        /// <summary>
        /// OnPaint Method for drawing the layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualTimer_Paint(object sender, PaintEventArgs e)
        {
            /* Clear previous drawn stuff */
            e.Graphics.Clear(this.BackColor);
            
            /* Set Font */
            this.label_time.Font = this.Font;
            this.label_time.ForeColor = this.ForeColor;

            /* Set the SmoothingMode */
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            /* Skip first step (div / 0 fail) */
            if (this.Step == 0)
            {
                return;
            }

            /* Declare paths for the arc */
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.GraphicsPath path_back = new System.Drawing.Drawing2D.GraphicsPath();

            /* Draw the inner and the outer border using arc */
            path.AddArc(new Rectangle(1, 1, Width - 2, Height - 2), -90, _calc_angle(Step));
            path_back.AddArc(new Rectangle(1 + ArcWidth, 1 + ArcWidth, Width - 2 - ArcWidth * 2, Height - 2 - ArcWidth * 2), -90, _calc_angle(Step));
            
            /* Reverse on path for correct drawing */
            path_back.Reverse();
            path.AddPath(path_back, true);

            /* Fill the Path */
            e.Graphics.FillPath(new SolidBrush(this.ForeColor), path);

            e.Dispose();
        }

        /// <summary>
        /// Shorthand-function
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private float _calc_angle(double a)
        {
            return (float)(Step / (TimeoutValue * 1000) * 360);
        }
    }
}
