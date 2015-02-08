using System;
using System.Windows.Forms;

using Sensor = YoloTrack.MVC.Model.Sensor.Model;
using StateMachine = YoloTrack.MVC.Model.StateMachine.Model;
using Configuration = YoloTrack.MVC.Model.Configuration.Model;
using IdentificationData = YoloTrack.MVC.Model.IdentificationData.Model;
using RuntimeDatabase = YoloTrack.MVC.Model.RuntimeDatabase.Model;
using Database = YoloTrack.MVC.Model.Database.Model;

using DebugView = YoloTrack.MVC.View.Debug.View;
using ApplicationView = YoloTrack.MVC.View.Application.View;

using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Kinect;
using System.Collections.Generic;
using YoloTrack.MVC.View.Components;
using YoloTrack.MVC.Controller;

namespace YoloTrack.MVC.View.Application
{
    public partial class View : Form,
        IBindable<Sensor>,
        IBindable<RuntimeDatabase>,
        IBindable<Database>,
        YoloTrack.MVC.Controller.IObserver<Sensor>,
        YoloTrack.MVC.Controller.IObserver<RuntimeDatabase>,
        YoloTrack.MVC.Controller.IObserver<StateMachine>,
        YoloTrack.MVC.Controller.IObserver<IdentificationData>,
        YoloTrack.MVC.Controller.IObserver<Database>
    {
        public event EventHandler RepeatInitTimeout;

        public event EventHandler<DatabaseItemSelectedEventArgs> DatabaseItemSelected;

        public event EventHandler<DatabaseItemFirstNameChangedEventArgs> DatabaseItemFirstNameChanged;

        public event EventHandler<DatabaseItemLastNameChangedEventArgs> DatabaseItemLastNameChanged;

        public event EventHandler<DatabaseItemImageChangedEventArgs> DatabaseItemImageChanged;

        public event EventHandler<DatabaseMergeEventArgs> DatabaseMergeRequested;

        public event EventHandler<DatabaseItemDeleteRequestEventArgs> DatabaseItemDeleteRequested;

        Sensor m_sensor;

        RuntimeDatabase m_runtime_database;

        Database m_database;

        public View()
        {
            InitializeComponent();

            liveView1.BackColor = Color.Black;
            //visualTimer1.Start(30);
        }

        public void Bind(Sensor Sensor)
        {
            m_sensor = Sensor;
        }

        public void Bind(RuntimeDatabase RuntimeDatabase)
        {
            m_runtime_database = RuntimeDatabase;
        }

        public void Bind(Database Database)
        {
            m_database = Database;

            foreach (KeyValuePair<int, Model.Database.Record> record in m_database.ContainerCopy)
            {
                _add_database_record(record.Value);
                record.Value.RecordChanged += Database_RecordChanged;
            }
        }

        #region Observer registration

        public void Observe(StateMachine StateMachine)
        {
            StateMachine.StateChange += new EventHandler<Model.StateMachine.StateChangeEventArgs>(StateMachine_StateChange);
        }

        public void Observe(Sensor Sensor)
        {
            Sensor.ColorFrameAvailable += new EventHandler<Model.Sensor.ColorImageFrameEventArgs>(Sensor_ColorFrameAvailable);
            liveView1.BackColor = Color.Black;
        }

        public void Observe(RuntimeDatabase RuntimeDatabase)
        {
            RuntimeDatabase.RecordAdded += new EventHandler<Model.RuntimeDatabase.RecordAddedEventArgs>(RuntimeDatabase_RecordAdded);
            RuntimeDatabase.RecordRemoved += new EventHandler<Model.RuntimeDatabase.RecordRemovedEventArgs>(RuntimeDatabase_RecordRemoved);
        }

        public void Observe(IdentificationData IdentificationData)
        {
            //throw new NotImplementedException();
        }

        public void Observe(Database Database)
        {
            Database.RecordAdded += new EventHandler<Model.Database.RecordAddedEventArgs>(Database_RecordAdded);
            Database.RecordRemoved += new EventHandler<Model.Database.RecordRemovedEventArgs>(Database_RecordRemoved);
        }

        #endregion

        #region Event handlers for the state machine

        void StateMachine_StateChange(object sender, Model.StateMachine.StateChangeEventArgs e)
        {
            toolStripStatusLabel1.Text = e.NextState.GetType().Name;
        }

        #endregion

        #region Event handlers for sensor

        void Sensor_ColorFrameAvailable(object sender, Model.Sensor.ColorImageFrameEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { _show_liveview_frame(e.Frame); }));
            }
            else
            {
                _show_liveview_frame(e.Frame);
            }
        }

        void _show_liveview_frame(ColorImageFrame Frame)
        {
            byte[] buffer = new byte[Frame.PixelDataLength];
            Frame.CopyPixelDataTo(buffer);

            //buffer = Frame.GetRawPixelData();

            GCHandle pinned = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr arrayPtr = pinned.AddrOfPinnedObject();

            Bitmap bmp = new Bitmap(
                Frame.Width, Frame.Height,
                Frame.Width * Frame.BytesPerPixel, System.Drawing.Imaging.PixelFormat.Format32bppRgb,
                pinned.AddrOfPinnedObject());

            Graphics g = Graphics.FromImage(bmp);
            Pen pen = new Pen(Color.Blue);
            Pen pen_red = new Pen(Color.Red);
            foreach (KeyValuePair<int, Model.RuntimeDatabase.Record> p in (Dictionary<int, Model.RuntimeDatabase.Record>)m_runtime_database.ContainerCopy)
            {
                g.DrawRectangle(pen, p.Value.KinectResource.HeadRectangle);
                ColorImagePoint begin, end;
                if (p.Value.KinectResource.Skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    begin = m_sensor.CoordinateMapper.MapSkeletonPointToColorPoint(p.Value.KinectResource.Skeleton.Joints[JointType.Head].Position);
                    end = m_sensor.CoordinateMapper.MapSkeletonPointToColorPoint(p.Value.KinectResource.Skeleton.Joints[JointType.ShoulderCenter].Position);
                    g.DrawLine(pen_red, new Point(begin.X, begin.Y), new Point(end.X, end.Y));
                    g.DrawString("Id=" + p.Value.KinectResource.Skeleton.TrackingId.ToString() + " Attempts=" + p.Value.IdentifyAttempts.ToString(),
                                SystemFonts.DefaultFont, Brushes.Black,
                                (float)begin.X, (float)begin.Y);
                }
            }

            pinned.Free();
            
            liveView1.Image = bmp;
        }

        #endregion

        #region Event handlers for runtime database

        private void RuntimeDatabase_RecordAdded(object sender, Model.RuntimeDatabase.RecordAddedEventArgs e)
        {
            _update_rt_info();
        }

        private void RuntimeDatabase_RecordRemoved(object sender, Model.RuntimeDatabase.RecordRemovedEventArgs e)
        {
            _update_rt_info();
        }

        private void _update_rt_info()
        {
            if (m_runtime_database.Count == 0)
            {
                toolStripStatusLabel5.Text = "Runtime Database is empty";
                return;
            }

            toolStripStatusLabel5.Text = m_runtime_database.Count.ToString();
            if (m_runtime_database.Count == 1)
            {
                toolStripStatusLabel5.Text += " Record";
            }
            else
            {
                toolStripStatusLabel5.Text += " Records";
            }
            toolStripStatusLabel5.Text += " in Runtime Database";
        }

        #endregion

        #region Event handlers for database

        void Database_RecordAdded(object sender, Model.Database.RecordAddedEventArgs e)
        {
            e.Record.RecordChanged += Database_RecordChanged;

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { _add_database_record(e.Record); }));
            }
            else
            {
                _add_database_record(e.Record);
            }
        }

        void Database_RecordRemoved(object sender, Model.Database.RecordRemovedEventArgs e)
        {
            e.Record.RecordChanged -= Database_RecordChanged;

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { _remove_database_record(e.Record); }));
            }
            else
            {
                _remove_database_record(e.Record);
            }
        }

        void Database_RecordChanged(object sender, Model.Database.RecordChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { _update_database_record((Model.Database.Record)sender); }));
            }
            else
            {
                _update_database_record((Model.Database.Record)sender);
            }
        }

        private void _add_database_record(Model.Database.Record record)
        {
            databaseView1.Items.Add(new Components.DatabaseViewItem()
            {
                Id = record.Id,
                FirstName = record.FirstName,
                LastName = record.LastName,
                LearnedAt = record.LearnedAt,
                TimesRecognized = record.TimesRecognized,
                TimesTracked = record.TimesTracked
            });
        }

        private void _remove_database_record(Model.Database.Record record)
        {
            DatabaseViewItem item = databaseView1.Items.Find(r => r.Id == record.Id);
            if (item == null)
            {
                return;
            }
            databaseView1.Items.Remove(item);
        }

        private void _update_database_record(Model.Database.Record record)
        {
            DatabaseViewItem item = databaseView1.Items.Find(r => r.Id == record.Id);
            if (item == null)
            {
                return;
            }
            item.Id = record.Id;
            item.FirstName = record.FirstName;
            item.LastName = record.LastName;
            item.LearnedAt = record.LearnedAt;
            item.TimesRecognized = record.TimesRecognized;
            item.TimesTracked = record.TimesTracked;
        }

        #endregion

        private void View_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        public void ShowFailure(string Message, Controller.FailureType Type)
        {
            switch (Type)
            {
                case FailureType.Sensor:
                    failure_header.Text = "Sensor Failure";
                    break;

                case FailureType.IdentificationAPI:
                    failure_header.Text = "Identification API Failure";
                    break;
            }

            failure_message.Text = Message;
            visualTimer1.Start(30);
            panel_failure_message.Visible = true;
            liveView1.BackColor = SystemColors.ControlDarkDark;

            visualTimer1.Timeout += visualTimer1_Timeout;
        }

        public void HideFailureMessage()
        {
            panel_failure_message.Visible = false;
        }

        void visualTimer1_Timeout(object sender, EventArgs e)
        {
            if (RepeatInitTimeout != null)
            {
                RepeatInitTimeout(this, null);
            }
        }

        private void databaseView1_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            if (DatabaseItemSelected != null)
            {
                DatabaseItemSelected(this, new DatabaseItemSelectedEventArgs()
                {
                    DatabaseId = e.DatabaseId
                });
            }
        }

        public void ShowRecordDetail(Model.Database.Record Record)
        {
            detailEditView1.Id = Record.Id;
            detailEditView1.FirstName = Record.FirstName;
            detailEditView1.LastName = Record.LastName;
            detailEditView1.IsTracked = Record.IsTarget;
            detailEditView1.LearnedAt = Record.LearnedAt;
            detailEditView1.TimesRecognized = Record.TimesRecognized;
            detailEditView1.TimesTracked = Record.TimesTracked;
            detailEditView1.Image = Record.Image;
            if (Record.RuntimeRecord != null)
            {
                detailEditView1.TrackingId = Record.RuntimeRecord.KinectResource.Skeleton.TrackingId;
                detailEditView1.IdentifyAttempts = Record.RuntimeRecord.IdentifyAttempts;
            }else
            {
                detailEditView1.TrackingId = 0;
            }
        }

        private void detailEditView1_LastNameChanged(object sender, LastNameChangedEventArgs e)
        {
            if (DatabaseItemLastNameChanged != null)
            {
                DatabaseItemLastNameChanged(this, new DatabaseItemLastNameChangedEventArgs()
                {
                    DatabaseId = detailEditView1.Id,
                    LastName = e.LastName
                });
            }
        }

        private void detailEditView1_FirstNameChanged(object sender, FirstNameChangedEventArgs e)
        {
            if (DatabaseItemFirstNameChanged != null)
            {
                DatabaseItemFirstNameChanged(this, new DatabaseItemFirstNameChangedEventArgs()
                {
                    DatabaseId = detailEditView1.Id,
                    FirstName = e.FirstName
                });
            }
        }

        private void detailEditView1_ImageChanged(object sender, ImageChangedEventArgs e)
        {
            if (DatabaseItemImageChanged != null)
            {
                DatabaseItemImageChanged(this, new DatabaseItemImageChangedEventArgs()
                {
                    DatabaseId = detailEditView1.Id,
                    Image = e.Image
                });
            }
        }

        private void databaseView1_MergeRequest(object sender, MergeEventArgs e)
        {
            if (DatabaseMergeRequested != null)
            {
                int[] id_list = new int[e.Items.Length];
                for (int i = 0; i < e.Items.Length; i++)
                {
                    id_list[i] = e.Items[i].Id;
                }
                DatabaseMergeRequested(this, new DatabaseMergeEventArgs()
                {
                    DatabaseIdList = id_list
                });
            }
        }

        private void detailEditView1_DeleteRequest(object sender, DeleteRequestEventArgs e)
        {
            if (DatabaseItemDeleteRequested != null)
            {
                DatabaseItemDeleteRequested(this, new DatabaseItemDeleteRequestEventArgs()
                {
                    DatabaseId = e.DatabaseId
                });
            }
        }
    }

    public class DatabaseMergeEventArgs : EventArgs
    {
        public int[] DatabaseIdList;
    }

    public class DatabaseItemDeleteRequestEventArgs : EventArgs
    {
        public int DatabaseId;
    }

    public class DatabaseItemSelectedEventArgs : EventArgs
    {
        public int DatabaseId;
    }
    
    public class DatabaseItemFirstNameChangedEventArgs : EventArgs
    {
        public int DatabaseId;
        public string FirstName;
    }

    public class DatabaseItemLastNameChangedEventArgs : EventArgs
    {
        public int DatabaseId;
        public string LastName;

    }

    public class DatabaseItemImageChangedEventArgs : EventArgs
    {
        public int DatabaseId;
        public Image Image;
    }
}
