using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace YoloTrack.Source.MVC.View.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class VisualTimer : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Timeout;

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        public double Step { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(10)]
        public int ArcWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TimeoutValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private DateTime m_start;

        /// <summary>
        /// 
        /// </summary>
        public VisualTimer()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            timer1.Start();
            Step = 0;
            m_start = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start(int Seconds)
        {
            TimeoutValue = Seconds;
            Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (TimeoutValue - (DateTime.Now - m_start).TotalSeconds <= 0)
            {
                timer1.Stop();
                return;
            }

            Step = (DateTime.Now - m_start).TotalMilliseconds;

            Invalidate();
            label_time.Text = ((int)(TimeoutValue - (DateTime.Now - m_start).TotalSeconds)).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualTimer_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            label_time.Font = Font;
            label_time.ForeColor = ForeColor;

            if (Step == 0)
            {
                return;
            }

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.GraphicsPath path_back = new System.Drawing.Drawing2D.GraphicsPath();

            path.AddArc(new Rectangle(1, 1, Width - 2, Height - 2), -90, _calc_angle(Step));
            path_back.AddArc(new Rectangle(1 + ArcWidth, 1 + ArcWidth, Width - 2 - ArcWidth * 2, Height - 2 - ArcWidth * 2), -90, _calc_angle(Step));
            
            path_back.Reverse();
            path.AddPath(path_back, true);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillPath(new SolidBrush(ForeColor), path);
            
            e.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private float _calc_angle(double a)
        {
            return (float)(Step / (TimeoutValue * 1000) * 360);
        }
    }
}
