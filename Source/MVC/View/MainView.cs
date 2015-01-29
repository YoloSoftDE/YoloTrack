﻿using System;
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
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.AutoScroll = true;
        }

        void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            ColorImageFrame frame = e.OpenColorImageFrame();
            byte[] buffer = new byte[frame.PixelDataLength];
            frame.CopyPixelDataTo(buffer);
            Bitmap bmp = new Bitmap(1280, 960, 1280 * frame.BytesPerPixel, System.Drawing.Imaging.PixelFormat.Format32bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0));
            //pb_liveview.DrawToBitmap(bmp, new Rectangle(new Point(0, 0), new Size(1280, 960)));
            //pb_liveview.Image = bmp;
            //pb_liveview.Invalidate();

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

        
        public void Observe(Model.TrackingModel model)
        {
            // Register event handlers
            KinectSensor sensor = model.Kinect;
            sensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(sensor_ColorFrameReady);

            model.MainDatabase.PersonAdded += new EventHandler(MainDatabase_Changed);
            model.MainDatabase.PersonRemoved += new EventHandler(MainDatabase_Changed);
        }

        void MainDatabase_Changed(object sender, EventArgs e)
        {
            Model.Storage.MainDatabase db = (Model.Storage.MainDatabase)sender;

            foreach (Model.Storage.Person p in db.People)
            {
                YoloTrack.MVC.View.Components.ProfileCard card = new YoloTrack.MVC.View.Components.ProfileCard()
                {
                    FullName = p.Name,
                    Picture = p.Picture,
                    TrackedCount = p.RTInfo.TrackedCount,
                    RecognizedCount = p.RTInfo.RecognizedCount,
                    LearnedAt = p.Learned
                };

                flowLayoutPanel1.Controls.Add(card);
            }
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
