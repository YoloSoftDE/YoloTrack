using System;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace YoloTrack.MVC.View
{
    public partial class MainView : Form, IObserver
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            KinectSensor sensor = Model.TrackingModel.Instance().Kinect;
            sensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(sensor_ColorFrameReady);
        }

        void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            ColorImageFrame frame = e.OpenColorImageFrame();
            byte[] buffer = new byte[frame.PixelDataLength];
            frame.CopyPixelDataTo(buffer);
            Bitmap bmp = new Bitmap(1280, 800, 1280 * frame.BytesPerPixel, System.Drawing.Imaging.PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0));
            pb_liveview.DrawToBitmap(bmp, new Rectangle(new Point(0, 0), new Size(1280, 800)));
            //pb_liveview.Image = bmp;
        }

        public void Observe(Model.TrackingModel model)
        {
            // Register event handlers
        }

        public Model.Status Status
        {
            set
            {
                switch (value)
                {
                    case Model.Status.RUNNING:
                        lbl_state.Text = "Running";
                        break;
                    case Model.Status.IDLE:
                        lbl_state.Text = "Idle";
                        break;
                    case Model.Status.SENSOR_UNAVAILABLE:
                        lbl_state.Text = "No Sensor available";
                        break;
                    case Model.Status.STOPPED:
                        lbl_state.Text = "Stopped";
                        break;
                    case Model.Status.TRACKING:
                        lbl_state.Text = "Tracking";
                        break;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }
    }
}
