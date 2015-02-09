#region Usings

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

#endregion

namespace YoloTrack.MVC.View.Application
{
    /// <summary>
    /// The main application view / form
    /// </summary>
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
        /// <summary>
        /// Fired when the Repeat init on failure timer reached 0
        /// </summary>
        public event EventHandler RepeatInitTimeout;

        /// <summary>
        /// Fired after an item in the main database was selected
        /// </summary>
        public event EventHandler<DatabaseItemSelectedEventArgs> DatabaseItemSelected;

        /// <summary>
        /// Fired when the user requested to change the first name of the currently displayed database record
        /// </summary>
        public event EventHandler<DatabaseItemFirstNameChangedEventArgs> DatabaseItemFirstNameChanged;

        /// <summary>
        /// Fired when the user requested to change the last name of the currently displayed database record
        /// </summary>
        public event EventHandler<DatabaseItemLastNameChangedEventArgs> DatabaseItemLastNameChanged;

        /// <summary>
        /// Fired when the user requested to change the image of the currently displayed database record
        /// </summary>
        public event EventHandler<DatabaseItemImageChangedEventArgs> DatabaseItemImageChanged;

        /// <summary>
        /// Fired when the user requested to merge two or more database records into one
        /// </summary>
        public event EventHandler<DatabaseMergeEventArgs> DatabaseMergeRequested;

        /// <summary>
        /// Fired when the user requested to delete the currently displayed database record
        /// </summary>
        public event EventHandler<DatabaseItemDeleteRequestEventArgs> DatabaseItemDeleteRequested;

        /// <summary>
        /// Fired when the user requests a tracking change
        /// </summary>
        public event EventHandler<TrackingRequestedEventArgs> TrackingRequested;

        /// <summary>
        /// Fired when the user requests to stop tracking
        /// </summary>
        public event EventHandler HaltTrackingRequested;

        /// <summary>
        /// Instance of the Kinect sensor
        /// </summary>
        private Sensor m_sensor;

        /// <summary>
        /// Instance of the runtime database
        /// </summary>
        private RuntimeDatabase m_runtime_database;

        /// <summary>
        /// Instance of the main database
        /// </summary>
        private Database m_database;

        /// <summary>
        /// Default constructor
        /// </summary>
        public View()
        {
            InitializeComponent();
            _clear_detail_edit_view();
        }

        #region Foreign model binders

        /// <summary>
        /// Binder for the Sensor model
        /// </summary>
        /// <param name="Sensor"></param>
        public void Bind(Sensor Sensor)
        {
            m_sensor = Sensor;
        }

        /// <summary>
        /// Binder for the RuntimeDatabase model
        /// </summary>
        /// <param name="RuntimeDatabase"></param>
        public void Bind(RuntimeDatabase RuntimeDatabase)
        {
            m_runtime_database = RuntimeDatabase;
        }

        /// <summary>
        /// Binder for the Database model
        /// </summary>
        /// <param name="Database"></param>
        public void Bind(Database Database)
        {
            m_database = Database;

            foreach (KeyValuePair<int, Model.Database.Record> record in m_database.ContainerCopy)
            {
                _add_database_record(record.Value);
                record.Value.RecordChanged += Database_RecordChanged;
            }
        }

        #endregion

        #region Observer registration

        /// <summary>
        /// Event registration on the StateMachine model
        /// </summary>
        /// <param name="StateMachine"></param>
        public void Observe(StateMachine StateMachine)
        {
            StateMachine.StateChange += new EventHandler<Model.StateMachine.StateChangeEventArgs>(StateMachine_StateChange);
        }

        /// <summary>
        /// Event registration on the Sensor model
        /// </summary>
        /// <param name="Sensor"></param>
        public void Observe(Sensor Sensor)
        {
            Sensor.ColorFrameAvailable += Sensor_ColorFrameAvailable;
            liveView1.BackColor = Color.Black;
        }

        /// <summary>
        /// Event registration on the RuntimeDatabase model
        /// </summary>
        /// <param name="RuntimeDatabase"></param>
        public void Observe(RuntimeDatabase RuntimeDatabase)
        {
            RuntimeDatabase.RecordAdded += new EventHandler<Model.RuntimeDatabase.RecordAddedEventArgs>(RuntimeDatabase_RecordAdded);
            RuntimeDatabase.RecordRemoved += new EventHandler<Model.RuntimeDatabase.RecordRemovedEventArgs>(RuntimeDatabase_RecordRemoved);
        }

        /// <summary>
        /// Event registration on the IdentificationData model
        /// </summary>
        /// <param name="IdentificationData"></param>
        public void Observe(IdentificationData IdentificationData)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Event registration on the Database model
        /// </summary>
        /// <param name="Database"></param>
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

        /// <summary>
        /// Called when a new color frame becoms available and shoudl be displayed on screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Draws the head rectangle onto the new liveview frame and passes it to the live view component
        /// </summary>
        /// <param name="Frame"></param>
        void _show_liveview_frame(Model.Sensor.ColorImageFrame Frame)
        {
            Bitmap bmp = new Bitmap(
                Frame.Width, Frame.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(new Rectangle(new Point(0, 0), bmp.Size), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

            Marshal.Copy(Frame.PixelData, 0, data.Scan0, Frame.PixelData.Length);

            bmp.UnlockBits(data);

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

            liveView1.Image = bmp;
        }

        #endregion

        #region Event handlers for runtime database

        /// <summary>
        /// Called when a new record gets added to the runtime database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuntimeDatabase_RecordAdded(object sender, Model.RuntimeDatabase.RecordAddedEventArgs e)
        {
            _update_rt_info();
        }

        /// <summary>
        /// Called when a record got removed from the runtime database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuntimeDatabase_RecordRemoved(object sender, Model.RuntimeDatabase.RecordRemovedEventArgs e)
        {
            _update_rt_info();
        }

        /// <summary>
        /// Updates the runtime database information within the status bar.
        /// </summary>
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

        /// <summary>
        /// Called when a new database record was added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Called when a record was removed from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Called when a record of the database was changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Adds a new item to the databae view
        /// </summary>
        /// <param name="record"></param>
        private void _add_database_record(Model.Database.Record record)
        {
            m_database_view.Items.Add(new Components.DatabaseViewItem()
            {
                Id = record.Id,
                FirstName = record.FirstName,
                LastName = record.LastName,
                LearnedAt = record.LearnedAt,
                TimesRecognized = record.TimesRecognized,
                TimesTracked = record.TimesTracked,
                Image = record.Image
            });
        }

        /// <summary>
        /// Removed an obsolete item from the database view
        /// </summary>
        /// <param name="record"></param>
        private void _remove_database_record(Model.Database.Record record)
        {
            DatabaseViewItem item = m_database_view.Items.Find(r => r.Id == record.Id);
            if (item == null)
            {
                return;
            }
            m_database_view.Items.Remove(item);

            if (m_detail_edit_view.Id == record.Id)
            {
                _clear_detail_edit_view();
            }
        }

        /// <summary>
        /// Updates an item in the database view to fit the changed database record
        /// </summary>
        /// <param name="record"></param>
        private void _update_database_record(Model.Database.Record record)
        {
            DatabaseViewItem item = m_database_view.Items.Find(r => r.Id == record.Id);
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
            item.Image = record.Image;
        }

        #endregion

        /// <summary>
        /// Displays an visual error message on screen and starts a 30 second timer with a called attached
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Type"></param>
        public void DisplayFailure(string Message, Controller.FailureType Type)
        {
            switch (Type)
            {
                case FailureType.Sensor:
                    m_failure_header.Text = "Sensor Failure";
                    break;

                case FailureType.IdentificationAPI:
                    m_failure_header.Text = "Identification API Failure";
                    break;
            }

            m_failure_message.Text = Message;
            m_visual_timer.Start(30);
            m_panel_failure_message.Visible = true;
            liveView1.BackColor = SystemColors.ControlDarkDark;

            m_visual_timer.Timeout += visualTimer1_Timeout;
        }

        /// <summary>
        /// Hides the error message 
        /// </summary>
        public void HideFailureMessage()
        {
            m_panel_failure_message.Visible = false;
        }

        /// <summary>
        /// Propagates the timeout event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void visualTimer1_Timeout(object sender, EventArgs e)
        {
            if (RepeatInitTimeout != null)
            {
                RepeatInitTimeout(this, null);
            }
        }

        /// <summary>
        /// Propagates the item selected event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Displays detailed information about a database record in the edit detail view
        /// </summary>
        /// <param name="Record"></param>
        public void ShowRecordDetail(Model.Database.Record Record)
        {
            int previous_id = m_detail_edit_view.Id;
            if (m_database.ContainsKey(previous_id))
            {
                m_database[previous_id].RecordChanged -= Record_RecordChanged;
            }

            m_detail_edit_view.Id = Record.Id;
            m_detail_edit_view.FirstName = Record.FirstName;
            m_detail_edit_view.LastName = Record.LastName;
            m_detail_edit_view.IsTracked = Record.IsTarget;
            m_detail_edit_view.LearnedAt = Record.LearnedAt;
            m_detail_edit_view.TimesRecognized = Record.TimesRecognized;
            m_detail_edit_view.TimesTracked = Record.TimesTracked;
            m_detail_edit_view.Image = Record.Image;
            if (Record.RuntimeRecord != null)
            {
                m_detail_edit_view.TrackingId = Record.RuntimeRecord.KinectResource.Skeleton.TrackingId;
                m_detail_edit_view.IdentifyAttempts = Record.RuntimeRecord.IdentifyAttempts;
            }else
            {
                m_detail_edit_view.TrackingId = 0;
            }
            m_detail_edit_view.Visible = true;
            Record.RecordChanged += Record_RecordChanged;
        }

        /// <summary>
        /// Clears the detail edit view to a default state
        /// </summary>
        private void _clear_detail_edit_view()
        {
            m_detail_edit_view.Visible = false;
        }

        /// <summary>
        /// ¯\ (ツ) /¯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Record_RecordChanged(object sender, Model.Database.RecordChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate() { _update_edit_view((Model.Database.Record)sender); }));
            } else
            {
                _update_edit_view((Model.Database.Record)sender);
            }
        }

        /// <summary>
        /// whatever
        /// </summary>
        /// <param name="Record"></param>
        private void _update_edit_view(Model.Database.Record Record)
        {
            m_detail_edit_view.Id = Record.Id;
            m_detail_edit_view.FirstName = Record.FirstName;
            m_detail_edit_view.LastName = Record.LastName;
            m_detail_edit_view.IsTracked = Record.IsTarget;
            m_detail_edit_view.LearnedAt = Record.LearnedAt;
            m_detail_edit_view.TimesRecognized = Record.TimesRecognized;
            m_detail_edit_view.TimesTracked = Record.TimesTracked;
            m_detail_edit_view.Image = Record.Image;
            if (Record.RuntimeRecord != null)
            {
                m_detail_edit_view.TrackingId = Record.RuntimeRecord.KinectResource.Skeleton.TrackingId;
                m_detail_edit_view.IdentifyAttempts = Record.RuntimeRecord.IdentifyAttempts;
            }
            else
            {
                m_detail_edit_view.TrackingId = 0;
            }
        }

        /// <summary>
        /// Propagates the last name changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detailEditView1_LastNameChanged(object sender, LastNameChangedEventArgs e)
        {
            if (DatabaseItemLastNameChanged != null)
            {
                DatabaseItemLastNameChanged(this, new DatabaseItemLastNameChangedEventArgs()
                {
                    DatabaseId = m_detail_edit_view.Id,
                    LastName = e.LastName
                });
            }
        }

        /// <summary>
        /// Propagates the first name changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detailEditView1_FirstNameChanged(object sender, FirstNameChangedEventArgs e)
        {
            if (DatabaseItemFirstNameChanged != null)
            {
                DatabaseItemFirstNameChanged(this, new DatabaseItemFirstNameChangedEventArgs()
                {
                    DatabaseId = m_detail_edit_view.Id,
                    FirstName = e.FirstName
                });
            }
        }

        /// <summary>
        /// Propagates the image changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detailEditView1_ImageChanged(object sender, ImageChangedEventArgs e)
        {
            if (DatabaseItemImageChanged != null)
            {
                DatabaseItemImageChanged(this, new DatabaseItemImageChangedEventArgs()
                {
                    DatabaseId = m_detail_edit_view.Id,
                    Image = e.Image
                });
            }
        }

        /// <summary>
        /// Propagates the merge request event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Propagates the delete request event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Called when the user requests to track the selected user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_detail_edit_view_TrackingRequest(object sender, TrackingRequestEventArgs e)
        {
            if (TrackingRequested != null)
            {
                TrackingRequested(this, new TrackingRequestedEventArgs()
                {
                    DatabaseId = e.DatabaseId
                });
            }
        }

        /// <summary>
        /// Called when the user requests to stop tracking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_detail_edit_view_HaltTrackingRequest(object sender, HaltTrackingRequestEventArgs e)
        {
            if (HaltTrackingRequested != null)
            {
                HaltTrackingRequested(this, e);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox.View().ShowDialog();
        }

        private void View_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_sensor.ColorFrameAvailable -= Sensor_ColorFrameAvailable;
        }
    } // End class

    /// <summary>
    /// Provided on a MergeRequest event
    /// </summary>
    public class DatabaseMergeEventArgs : EventArgs
    {
        public int[] DatabaseIdList;
    } // End class

    /// <summary>
    /// Provided on a ItemDeleteRequest event
    /// </summary>
    public class DatabaseItemDeleteRequestEventArgs : EventArgs
    {
        public int DatabaseId;
    } // End class

    /// <summary>
    /// Provided on a ItemSelected event
    /// </summary>
    public class DatabaseItemSelectedEventArgs : EventArgs
    {
        public int DatabaseId;
    } // End class
    
    /// <summary>
    /// Provided on a FirstNameChanged event
    /// </summary>
    public class DatabaseItemFirstNameChangedEventArgs : EventArgs
    {
        public int DatabaseId;
        public string FirstName;
    } // End class

    /// <summary>
    /// Provided on a LastNameChanged event
    /// </summary>
    public class DatabaseItemLastNameChangedEventArgs : EventArgs
    {
        public int DatabaseId;
        public string LastName;

    } // End class

    /// <summary>
    /// Provided on a ImageChangedEvent
    /// </summary>
    public class DatabaseItemImageChangedEventArgs : EventArgs
    {
        public int DatabaseId;
        public Image Image;
    } // End class

    /// <summary>
    /// Provided on a TrackRequest event
    /// </summary>
    public class TrackingRequestedEventArgs : EventArgs
    {
        public int DatabaseId;
    } // End class

} // End namespace
