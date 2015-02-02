using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

using Sensor = YoloTrack.MVC.Model.Sensor.Model;
using StateMachine = YoloTrack.MVC.Model.StateMachine.Model;
using Configuration = YoloTrack.MVC.Model.Configuration.Model;
using IdentificationData = YoloTrack.MVC.Model.IdentificationData.Model;
using RuntimeDatabase = YoloTrack.MVC.Model.RuntimeDatabase.Model;
using Database = YoloTrack.MVC.Model.Database.Model;
using Microsoft.Kinect;

namespace YoloTrack.MVC.View.Application
{
    /// <summary>
    /// Application main view class
    /// </summary>
    public partial class View : Form
    {
        /// <summary>
        /// The Liveview component
        /// </summary>
        private Components.LiveView m_pb_liveview;

        /// <summary>
        /// 
        /// </summary>
        protected RuntimeDatabase m_runtime_database;

        /// <summary>
        /// Sensor to be used
        /// </summary>
        protected Sensor m_sensor;

        /// <summary>
        /// Default constructor
        /// </summary>
        public View()
        {
            InitializeComponent();
            m_pb_liveview = new Components.LiveView();
            m_pb_liveview.Name = "LiveView";
            m_pb_liveview.Dock = DockStyle.Fill;
            panel1.Controls.Add(m_pb_liveview);
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.AutoScroll = true;
        }

        #region Database GUI methods

        /// <summary>
        /// Adds a new item to the flow panel
        /// </summary>
        /// <param name="p"></param>
        private void _add_profile_card(Model.Database.Record p)
        {
            YoloTrack.MVC.View.Components.ProfileCard card = new YoloTrack.MVC.View.Components.ProfileCard()
            {
                FullName = string.Format("{0} {1}", p.FirstName, p.LastName),
                Picture = p.Image,
                TrackedCount = p.TimesTracked,
                RecognizedCount = p.TimesRecognized,
                LearnedAt = p.LearnedAt
            };

            flowLayoutPanel1.Controls.Add(card);
        }

        /// <summary>
        /// Updates the profile card for the given record
        /// </summary>
        /// <param name="record"></param>
        private void _update_profile_card(Model.Database.Record record)
        {
            foreach (Components.ProfileCard profile_card in flowLayoutPanel1.Controls)
            {
                if (profile_card.Id == record.Id)
                {
                    profile_card.FullName = string.Format("{0} {1}", record.FirstName, record.LastName);
                    profile_card.Picture = record.Image;
                    profile_card.TrackedCount = record.TimesTracked;
                    profile_card.RecognizedCount = record.TimesRecognized;
                    profile_card.LearnedAt = record.LearnedAt;
                    break;
                }
            }
        }

        /// <summary>
        /// Removes the profile card for the given record
        /// </summary>
        /// <param name="record"></param>
        private void _remove_profile_card(Model.Database.Record record)
        {
            Components.ProfileCard to_remove = null;
            foreach (Components.ProfileCard profile_card in flowLayoutPanel1.Controls)
            {
                if (profile_card.Id == record.Id)
                {
                    to_remove = profile_card;
                    break;
                }
            }

            if (to_remove != null)
                flowLayoutPanel1.Controls.Remove(to_remove);
        }

        #endregion

        #region Sensor GUI method

        /// <summary>
        /// Updates the liveview with the last received frame
        /// </summary>
        /// <param name="colorImageFrame"></param>
        private void _show_liveframe(Microsoft.Kinect.ColorImageFrame Frame)
        {
            byte[] buffer = new byte[Frame.PixelDataLength];
            Frame.CopyPixelDataTo(buffer);

            Bitmap bmp = new Bitmap(
                Frame.Width, Frame.Height, 
                Frame.Width * Frame.BytesPerPixel, System.Drawing.Imaging.PixelFormat.Format32bppRgb, 
                Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0));

            Graphics g = Graphics.FromImage(bmp);
            Pen pen = new Pen(Color.Blue);
            Pen pen_red = new Pen(Color.Red);

            foreach (KeyValuePair<int, Model.RuntimeDatabase.Record> p in (Dictionary<int, Model.RuntimeDatabase.Record>)m_runtime_database.ContainerCopy)
            {
                g.DrawRectangle(pen, p.Value.KinectResource.HeadRectangle);
                ColorImagePoint begin, end;
                begin = m_sensor.CoordinateMapper.MapSkeletonPointToColorPoint(p.Value.KinectResource.Skeleton.Joints[JointType.Head].Position);
                end = m_sensor.CoordinateMapper.MapSkeletonPointToColorPoint(p.Value.KinectResource.Skeleton.Joints[JointType.ShoulderCenter].Position);
                g.DrawLine(pen_red, new Point(begin.X, begin.Y), new Point(end.X, end.Y));
            }

            m_pb_liveview.Image = bmp;
        }

        #endregion

        #region Observer event registration

        /// <summary>
        /// Observer-binder
        /// </summary>
        /// <param name="Sensor"></param>
        public void Observe(Sensor Sensor)
        {
            Sensor.ColorFrameAvailable += new EventHandler<Model.Sensor.ColorImageFrameEventArgs>(_sensor_colorframe_available);
        }

        /// <summary>
        /// Observer-binder
        /// </summary>
        /// <param name="Sensor"></param>
        public void Observe(StateMachine StateMachine)
        {
            
        }

        /// <summary>
        /// Observer-binder
        /// </summary>
        /// <param name="RuntimeDatabase"></param>
        public void Observe(RuntimeDatabase RuntimeDatabase)
        {
            
        }

        /// <summary>
        /// Observer-binder
        /// </summary>
        /// <param name="Database"></param>
        public void Observe(Database Database)
        {
            Database.RecordAdded += new EventHandler<Model.Database.RecordAddedEventArgs>(_on_database_record_added);
            Database.RecordRemoved += new EventHandler<Model.Database.RecordRemovedEventArgs>(_on_database_record_removed);
        }

        /// <summary>
        /// Observer-binder
        /// </summary>
        /// <param name="IdentificationData"></param>
        public void Observe(IdentificationData IdentificationAPI)
        {
            // Do not subscribe yet
        }

        #endregion

        #region Model event handlers

        private void _on_database_record_added(object sender, Model.Database.RecordAddedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { _add_profile_card(e.Record); });
            }
            else
            {
                _add_profile_card(e.Record);
            }

            e.Record.RecordChanged += new EventHandler<Model.Database.RecordChangedEventArgs>(_on_database_record_changed);
        }

        private void _on_database_record_changed(object sender, Model.Database.RecordChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { _update_profile_card((Model.Database.Record)sender); });
            }
            else
            {
                _update_profile_card((Model.Database.Record)sender);
            }
        }

        private void _on_database_record_removed(object sender, Model.Database.RecordRemovedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { _remove_profile_card(e.Record); });
            }
            else
            {
                _remove_profile_card(e.Record);
            }

            // TODO: Fix this.
            e.Record.RecordChanged -= new EventHandler<Model.Database.RecordChangedEventArgs>(_on_database_record_changed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _sensor_colorframe_available(object sender, Model.Sensor.ColorImageFrameEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { _show_liveframe(e.Frame); });
            }
            else
            {
                _show_liveframe(e.Frame);
            }
        }

        #endregion

        #region Foreign model bindings

        /// <summary>
        /// Binder to the runtime database
        /// </summary>
        /// <param name="IdentificationAPI"></param>
        public void Bind(RuntimeDatabase RuntimeDatabase)
        {
            m_runtime_database = RuntimeDatabase;
        }

        /// <summary>
        /// Binder to an instance of the sensor model
        /// </summary>
        /// <param name="Sensor"></param>
        public void Bind(Sensor Sensor)
        {
            m_sensor = Sensor;
        }

        #endregion
    } // End class
} // End namespace
