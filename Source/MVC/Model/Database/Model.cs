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
        /// 
        /// </summary>
        private IdentificationData.Model m_identification_api;

        /// <summary>
        /// Holding the last id, that was assigned by the database
        /// </summary>
        public int LastId { get; private set; }

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
        /// Create a flat copy only. No contaied data will be copied.
        /// This is mostly for foreign-thread accesses like from within a view.
        /// This is a synchronized method.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Record> ContainerCopy
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
        public static Model LoadFrom(string FileName, IdentificationData.Model IdentificationAPI)
        {
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();

            byte[] bytes = new byte[fs.Length];
            int bytes_to_read = (int)fs.Length;
            int bytes_read = 0;
            while (bytes_to_read > 0)
            {
                int n = fs.Read(bytes, bytes_read, bytes_to_read);
                if (n == 0)
                    break;

                ms.Write(bytes, bytes_read, n);
                bytes_read += n;
                bytes_to_read -= n;
            }
            fs.Close();
            ms.Seek(0, SeekOrigin.Begin);

            Model model = new Model();
            model.Bind(IdentificationAPI);
            model._unserialize_from(ms);

            ms.Close();

            return model;
        }

        /// <summary>
        /// Tries to load from the given filename. If it does not exist
        /// an empty instance will be created.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Model LoadFromOrEmpty(string FileName, IdentificationData.Model IdentificationAPI)
        {
            if (System.IO.File.Exists(FileName))
            {
                return LoadFrom(FileName, IdentificationAPI);
            }

            Model model = new Model();
            model.Bind(IdentificationAPI);
            return model;
        }

        /// <summary>
        /// Serializes the instance to a memory stream
        /// </summary>
        /// <param name="ms"></param>
        private void _serialize_to(MemoryStream ms)
        {
            Serializer.Serialize(ms, Count);
            foreach (KeyValuePair<int, Record> i in this)
            {
                Serializer.Serialize(ms, i.Key);
                i.Value.IdentificationRecord.Serialize(ms);
                Serializer.Serialize(ms, i.Value);
            }
        }

        /// <summary>
        /// Unserializes the instance from a memory stream
        /// </summary>
        /// <param name="ms"></param>
        private void _unserialize_from(MemoryStream ms)
        {
            int count = Serializer.UnserializeInt(ms);
            for (int i = 0; i < count; i++)
            {
                int key = Serializer.UnserializeInt(ms);
                
                long fir_length = Serializer.UnserializeLong(ms);
                MemoryStream fir_stream = new MemoryStream();
                for (long j = 0; j < fir_length; j++)
                {
                    fir_stream.WriteByte((byte)ms.ReadByte());
                }
                fir_stream.Seek(0, SeekOrigin.Begin);
                IdentificationRecord ir = new IdentificationRecord(m_identification_api.IdentificationRecordBuilder.build(fir_stream));
                fir_stream.Close();
                Record value = new Record(key, ir);
                value.Unserialize(ms);

                if (LastId < key)
                    LastId = key;
                Add(key, value);
                m_identification_api.Population.append(ir.Value, key.ToString());
            }
        }

        /// <summary>
        /// Binder for the identification API
        /// </summary>
        /// <param name="IdentificationAPI"></param>
        public void Bind(IdentificationData.Model IdentificationAPI)
        {
            m_identification_api = IdentificationAPI;
        }
    } // End class
} // End namespace
