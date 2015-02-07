using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Kinect;
using YoloTrack.MVC.Controller;

namespace YoloTrack.MVC.Model.RuntimeDatabase
{
    /// <summary>
    /// Event args provided when a new record becomes available
    /// </summary>
    public class RecordAddedEventArgs : EventArgs
    {
        public Record Record;
    } // End class

    /// <summary>
    /// Event args provided on removal of a record
    /// </summary>
    public class RecordRemovedEventArgs : EventArgs
    {
        public Record Record;
    } // End class

    /// <summary>
    /// 
    /// </summary>
    public class RuntimeDatabaseException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public RuntimeDatabaseException(string Message)
            : base(Message)
        {
        }
    } // End class

    /// <summary>
    /// Storage of all runtime information. this is moreless considered the stateful part of the
    /// state machines internal process beside the state itself.
    /// All information that is required directly during runtime is stored here, except of those
    /// information that belongs into the "main" database.
    /// </summary>
    public class Model : Dictionary<int, Record>, 
        IBindable<Sensor.Model>
    {
        /// <summary>
        /// Fired on addition of a record
        /// </summary>
        public event EventHandler<RecordAddedEventArgs> RecordAdded;

        /// <summary>
        /// Fired on removal of a record
        /// </summary>
        public event EventHandler<RecordRemovedEventArgs> RecordRemoved;

        /// <summary>
        /// Mutex that takes care that modifications to the entire container
        /// will be done synchronized. For now, this gives us the possibility to 
        /// obtain a clean copy of the list in our Clone() method.
        /// </summary>
        private Mutex m_container_modification_mutex = new Mutex();

        /// <summary>
        /// Sensor model instance.
        /// </summary>
        private Sensor.Model m_sensor;

        /// <summary>
        /// Create a flat copy only. No contaied data will be copied.
        /// This is mostly for foreign-thread accesses like from within a view.
        /// This is a synchronized method.
        /// </summary>
        /// <returns></returns>
        public object ContainerCopy
        {
            get
            {
                Dictionary<int, Record> copy = new Dictionary<int, Record>();

                // Lock the container for changes
                // Lock this only as long as really needed. Otherwise this will result
                // in a very large performance reduction. Pay attention what you do here!
                m_container_modification_mutex.WaitOne();
                foreach (KeyValuePair<int, Record> p in this)
                {
                    copy.Add(p.Key, p.Value);
                }
                // Release lock
                m_container_modification_mutex.ReleaseMutex();

                return copy;
            }
        }

        /// <summary>
        /// Synchronized Add implementation.
        /// This is a synchronized method.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public new void Add(int Key, Record Value)
        {
            // Lock the container for changes
            m_container_modification_mutex.WaitOne();

            base.Add(Key, Value);

            // Release lock
            m_container_modification_mutex.ReleaseMutex();

            if (RecordAdded != null)
            {
                RecordAdded(this, new RecordAddedEventArgs()
                {
                    Record = Value
                });
            }
        }

        /// <summary>
        /// Synchronized Clear implementation.
        /// This is a synchronized method.
        /// </summary>
        public new void Clear()
        {
            // Lock the container for changes
            m_container_modification_mutex.WaitOne();

            base.Clear();

            // Release lock
            m_container_modification_mutex.ReleaseMutex();
        }

        /// <summary>
        /// Synchronized Remove implementation.
        /// This is a synchronized method.
        /// </summary>
        /// <param name="Key"></param>
        public new void Remove(int Key)
        {
            // Precheck if key exists
            if (!ContainsKey(Key))
                return;

            // Lock the container for changes
            m_container_modification_mutex.WaitOne();

            Record record = this[Key];
            base.Remove(Key);

            // Release lock
            m_container_modification_mutex.ReleaseMutex();

            if (RecordRemoved != null)
            {
                RecordRemoved(this, new RecordRemovedEventArgs()
                {
                    Record = record
                });
            }
        }

        /// <summary>
        /// Refreshs the runtime database to a synchronized state with the kinect sensor.
        /// This is a synchronized method.
        /// </summary>
        public void Refresh()
        {
            SkeletonFrame frame = m_sensor.SkeletonStream.SkeletonFrame;
            if (frame == null)
                return;            
            
            Skeleton[] skeletons = new Skeleton[frame.SkeletonArrayLength];
            frame.CopySkeletonDataTo(skeletons);

            // Lock the container for changes
            //m_container_modification_mutex.WaitOne();

            // Add newly appered Persons (?) to RuntimeDB
            // Update existing skeletons
            Skeleton skeleton;
            for (int i = 0; i < skeletons.Length; i++)
            {
                skeleton = skeletons[i];

                if (skeleton.TrackingId == 0)
                    continue;

                if (!ContainsKey(skeleton.TrackingId))
                {
                    Record record = Record.Create(skeleton);
                    record.Bind(m_sensor);
                    Add(skeleton.TrackingId, record);
                }
                else
                {
                    this[skeleton.TrackingId].KinectResource.UpdateTo(skeleton);
                }
            }

            // Remove obsolete RuntimeInfos from RuntimeDB
            List<int> ids = new List<int>();

            // Lock the container for changes
            m_container_modification_mutex.WaitOne();
            foreach (KeyValuePair<int, Record> p in this)
                ids.Add(p.Key);
            // Unlock
            m_container_modification_mutex.ReleaseMutex();

            foreach (int id in ids)
            {
                bool present = false;
                foreach (Skeleton skel in skeletons)
                {
                    if (skel.TrackingId == id)
                    {
                        present = true;
                        break;
                    }
                }
                if (!present)
                    Remove(id);
            }

            // Release lock
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Record SelectLeastIdentifyAttempts()
        {
            Record least = null;
            foreach (KeyValuePair<int, RuntimeDatabase.Record> entry in this)
            {
                if (entry.Value.State == RuntimeDatabase.RecordState.Unidentified ||
                    entry.Value.State == RuntimeDatabase.RecordState.Unknown)
                {
                    if (least == null || least.IdentifyAttempts > entry.Value.IdentifyAttempts)
                    {
                        least = entry.Value;
                    }
                }
            }

            if (least != null)
            {
                least.IdentifyAttempts++;
            }
            return least;
        }

        #region Foreign model bindings

        /// <summary>
        /// Binder for sensor model instance.
        /// </summary>
        /// <param name="Sensor"></param>
        public void Bind(Sensor.Model Sensor)
        {
            m_sensor = Sensor;
        }

        #endregion
    } // End class
} // End namespace
