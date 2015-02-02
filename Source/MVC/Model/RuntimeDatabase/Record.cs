using System;
using System.Drawing;
using Microsoft.Kinect;
using DatabaseRecord = YoloTrack.MVC.Model.Database.Record;

namespace YoloTrack.MVC.Model.RuntimeDatabase
{
    /// <summary>
    /// Kinect resource bundle
    /// </summary>
    public class KinectResource
    {
        /// <summary>
        /// Sensor to be used
        /// </summary>
        protected Sensor.Model m_sensor;

        /// <summary>
        /// Current skeleton instance.
        /// </summary>
        public Skeleton Skeleton { get; private set; }

        /// <summary>
        /// Obtains the guess for current head position (rectangle)
        /// depending on what information is available.
        /// </summary>
        public Rectangle HeadRectangle
        {
            get
            {
                Joint head_joint = Skeleton.Joints[JointType.Head];
                if (head_joint.TrackingState == JointTrackingState.NotTracked)
                    return new Rectangle(0, 0, 0, 0);

                ColorImagePoint center_point;
                try
                {
                    center_point = m_sensor.CoordinateMapper.MapSkeletonPointToColorPoint(head_joint.Position);
                    int radius;

                    // Button (ShoulderCenter) available?
                    if (Skeleton.Joints[JointType.ShoulderCenter].TrackingState == JointTrackingState.Tracked)
                    {
                        // Calculation by 2-Tracked-Points (more accurate)
                        ColorImagePoint button_point 
                            = m_sensor.CoordinateMapper.MapSkeletonPointToColorPoint(Skeleton.Joints[JointType.ShoulderCenter].Position);
                        radius = System.Math.Abs(button_point.Y - center_point.Y);
                    }
                    else
                    {
                        // Calculation by distance, if buttom point unavailable (fallback)
                        radius = (int)System.Math.Round(180.0 / head_joint.Position.Z);
                    }

                    return new Rectangle(
                        new Point(center_point.X - radius, center_point.Y - radius), 
                        new Size(2 * radius, 2 * radius));
                }
                catch (System.InvalidCastException)
                {
                    System.Console.WriteLine("[RuntimeInfo] Invalid cast!");
                }

                return new Rectangle(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Update"></param>
        public void UpdateTo(Skeleton Update)
        {
            Skeleton = Update;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ToWrap"></param>
        public KinectResource(Skeleton ToWrap)
        {
            Skeleton = ToWrap;
        }

        #region Foreign model bindings 

        /// <summary>
        /// Binder to an instance of the sensor model
        /// </summary>
        /// <param name="Sensor"></param>
        public void Bind(Sensor.Model Sensor)
        {
            m_sensor = Sensor;
        }

        #endregion
    } // End class

    /// <summary>
    /// Current State in the buisiness logic.
    /// </summary>
    public enum RecordState
    {
        Unidentified,
        Identified,
        Unknown,
        Tracked,
        Lost
    }

    /// <summary>
    /// Provided on a change of a record
    /// </summary>
    public class RecordChangedEventArgs : EventArgs
    {
    } // End class

    /// <summary>
    /// Class holding information for one skeleton currently on screen
    /// </summary>
    public class Record
    {
        /// <summary>
        /// Fired on each change of the object
        /// </summary>
        public event EventHandler<RecordChangedEventArgs> RecordChanged;
        /// <summary>
        /// Kinect Resource, that is for now only the Skeleton and the HeadRectangle
        /// </summary>
        public KinectResource KinectResource { get; private set; }

        /// <summary>
        /// Record that is stored within the permenant database
        /// </summary>
        public DatabaseRecord DatabaseRecord { get; private set; }

        /// <summary>
        /// State.
        /// </summary>
        private RecordState m_state;
        public RecordState State {
            get { return m_state; }
            set
            {
                m_state = value;

                if (RecordChanged != null)
                {
                    RecordChanged(this, new RecordChangedEventArgs());
                }
            }
        }

        /// <summary>
        /// Attach a database record to the runtime information. This is a one-time process.
        /// </summary>
        /// <param name="Record"></param>
        public void Attach(DatabaseRecord Record)
        {
            if (DatabaseRecord != null)
            {
                throw new RuntimeDatabaseException("DatabaseRecord already bound");
            }

            DatabaseRecord = Record;
        }

        /// <summary>
        /// Factory method to create a new RuntimeDatabase.Record by the skeleton to wrap
        /// </summary>
        /// <param name="Skeleton"></param>
        /// <returns></returns>
        public static Record Create(Skeleton Skeleton)
        {
            Record record = new Record()
            {
                KinectResource = new KinectResource(Skeleton),
                State = RecordState.Unidentified
            };

            return record;
        }

        #region Foreign model bindings

        /// <summary>
        /// Binder to an instance of the sensor model
        /// </summary>
        /// <param name="Sensor"></param>
        public void Bind(Sensor.Model Sensor)
        {
            KinectResource.Bind(Sensor);
        }

        #endregion
    } // End class
} // End namespace
