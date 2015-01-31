using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace YoloTrack.MVC.View
{
    public partial class MainView : Form, IObserver
    {
        Components.LiveView pb_liveview;
        Thread m_th;

        public MainView()
        {
            InitializeComponent();
            pb_liveview = new Components.LiveView();
            pb_liveview.Name = "LiveView";
            pb_liveview.Dock = DockStyle.Fill;
            panel1.Controls.Add(pb_liveview);
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.AutoScroll = true;
        }

        void OnNextColorFrame(ColorImageFrame frame)
        {
            byte[] buffer = new byte[frame.PixelDataLength];
            frame.CopyPixelDataTo(buffer);
            Bitmap bmp = new Bitmap(1280, 960, 1280 * frame.BytesPerPixel, System.Drawing.Imaging.PixelFormat.Format32bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0));
            //pb_liveview.DrawToBitmap(bmp, new Rectangle(new Point(0, 0), new Size(1280, 960)));
            //pb_liveview.Image = bmp;
            //pb_liveview.Invalidate();

            Graphics g = Graphics.FromImage(bmp);
            Pen pen = new Pen(Color.Blue);
            Pen pen_red = new Pen(Color.Red);

            Model.Storage.RuntimeDatabase db = Model.TrackingModel.Instance().RuntimeDatabase;
            db.Use();
            foreach (KeyValuePair<int, Model.Storage.RuntimeInfo> entry in db) // FIXME: may throw due to change of db
            {
                g.DrawRectangle(pen, entry.Value.HeadRect);

                SkeletonPoint pt = new SkeletonPoint() 
                {
                    X = entry.Value.Skeleton.BoneOrientations[JointType.Head].AbsoluteRotation.Quaternion.X,
                    Y = entry.Value.Skeleton.BoneOrientations[JointType.Head].AbsoluteRotation.Quaternion.Y,
                    Z = entry.Value.Skeleton.BoneOrientations[JointType.Head].AbsoluteRotation.Quaternion.Z,
                };
                ColorImagePoint begin, end;
                begin = Model.TrackingModel.Instance().Kinect.MapSkeletonPointToColor(entry.Value.Skeleton.Joints[JointType.Head].Position, ColorImageFormat.RgbResolution1280x960Fps12);
                end = Model.TrackingModel.Instance().Kinect.MapSkeletonPointToColor(pt, ColorImageFormat.RgbResolution1280x960Fps12);

                g.DrawLine(pen_red, new Point(begin.X, begin.Y), new Point(end.X, end.Y));
            }
            db.UnUse();
            g.Dispose();
            DrawLiveviewBitmap(bmp);
        }

        private void DrawLiveviewBitmap(Bitmap bmp)
        {
            // Invoke nötig?
            if (pb_liveview.InvokeRequired)
            {
                // Invoke nötig
                pb_liveview.Invoke((MethodInvoker)delegate {
                    //pb_liveview.DrawToBitmap(bmp, new Rectangle(new Point(0, 0), new Size(1280, 960)));
                    //pb_liveview.Refresh();

                    pb_liveview.Image = (bmp);
                });
            }
            else
            {
                // Kein Invoke nötig - Vorgang sicher durchführbar
                //pb_liveview.DrawToBitmap(bmp, new Rectangle(new Point(0, 0), new Size(1280, 960)));
                //pb_liveview.Refresh();
                pb_liveview.Image = (bmp);
            }
        }

        void PollFrame()
        {
            KinectSensor sensor = Model.TrackingModel.Instance().Kinect;
            while (true)
            {
                ColorImageFrame frame = sensor.ColorStream.OpenNextFrame(500);
                if (frame == null)
                    continue;
                OnNextColorFrame(frame);
                Thread.Sleep(30);
            }
        }
        
        public void Observe(Model.TrackingModel model)
        {
            // Register event handlers
            model.MainDatabase.PersonAdded += new EventHandler(MainDatabase_Changed);
            model.MainDatabase.PersonRemoved += new EventHandler(MainDatabase_Changed);
            model.MainDatabase.PersonChanged += new EventHandler(MainDatabase_Changed);

            m_th = new Thread(new ThreadStart(PollFrame));
            m_th.Start();
        }

        void MainDatabase_Changed(object sender, EventArgs e)
        {
            Model.Storage.MainDatabase db = (Model.Storage.MainDatabase)sender;

            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { flowLayoutPanel1.Controls.Clear(); ; });
            else
                flowLayoutPanel1.Controls.Clear();
            

            foreach (Model.Storage.Person p in db.People)
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate { AddProfileCard(p); });
                }
                else
                {
                    AddProfileCard(p);
                }
            }
        }

        void AddProfileCard(Model.Storage.Person p)
        {
            YoloTrack.MVC.View.Components.ProfileCard card = new YoloTrack.MVC.View.Components.ProfileCard()
            {
                FullName = p.Name,
                Picture = p.Picture,
                TrackedCount = p.TrackedCount,
                RecognizedCount = p.RecognizedCount,
                LearnedAt = p.Learned
            };

            flowLayoutPanel1.Controls.Add(card);
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

        private void MainView_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_th.Abort();
        }
    }
}
