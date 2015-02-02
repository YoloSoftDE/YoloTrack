using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace YoloTrack.MVC.Model.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class RecordAddedEventArgs : EventArgs
    {
        public Record Record;
    }

    /// <summary>
    /// 
    /// </summary>
    public class RecordRemovedEventArgs : EventArgs
    {
        public Record Record;
    }

    /// <summary>
    /// 
    /// </summary>
    public class Model : Dictionary<int, Record>
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
        /// Holding the last id, that was assigned by the database
        /// </summary>
        public int LastId { get; private set; }

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
                Dictionary<int, Record> copy = new Dictionary<int,Record>();

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

            if (RecordAdded != null)
            {
                RecordAdded(this, new RecordAddedEventArgs()
                {
                    Record = Value
                });
            }

            // Release lock
            m_container_modification_mutex.ReleaseMutex();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Record CreateRecord(IdentificationRecord IdentificationRecord)
        {
            LastId++;
            Record record = new Record(LastId, IdentificationRecord)
            {
                LearnedAt = DateTime.Now
            };

            Add(record.Id, record);
            return record;
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

            if (RecordRemoved != null)
            {
                RecordRemoved(this, new RecordRemovedEventArgs()
                {
                    Record = record
                });
            }

            // Release lock
            m_container_modification_mutex.ReleaseMutex();
        }

        /// <summary>
        /// Gets if a target has been set.
        /// This is a synchronized method.
        /// </summary>
        public bool HasTarget
        {
            get
            {
                // Lock the container for changes
                m_container_modification_mutex.WaitOne();

                foreach (KeyValuePair<int, Record> p in this)
                {
                    if (p.Value.IsTarget)
                    {
                        // Release lock
                        m_container_modification_mutex.ReleaseMutex();
                        return true;
                    }
                }

                // Release lock
                m_container_modification_mutex.ReleaseMutex();
                return false;
            }
        }

        /// <summary>
        /// Gets/sets the target
        /// This is a synchronized method.
        /// </summary>
        public Record Target
        {
            get
            {
                Record target = null;

                // Lock the container for changes
                m_container_modification_mutex.WaitOne();

                foreach (KeyValuePair<int, Record> p in this)
                {
                    if (p.Value.IsTarget)
                    {
                        target = p.Value;
                        break;
                    }
                }

                // Release lock
                m_container_modification_mutex.ReleaseMutex();
                return target;
            }
        }

        /// <summary>
        /// Saves the database to the given filename.
        /// </summary>
        /// <param name="FileName"></param>
        public void SaveTo(string FileName)
        {
            MemoryStream ms = new MemoryStream();
            _serialize_to(ms);
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite);
            ms.WriteTo(fs);
            ms.Close();
            fs.Close();
        }

        /// <summary>
        /// Factory for constructing a whole model containing the records that
        /// are present in the file with the given filename.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static Model LoadFrom(string FileName)
        {
            Model model = new Model();
            //Serializer.Unserialize(
            return model;
        }

        /// <summary>
        /// Serializes the instance to a memory stream
        /// </summary>
        /// <param name="ms"></param>
        private void _serialize_to(MemoryStream ms)
        {
            Serializer.Serialize(ms, this);
        }
    } // End class
} // End namespace
