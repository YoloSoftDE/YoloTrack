using System;
using System.Drawing;
using Cognitec.FRsdk;

namespace YoloTrack.MVC.Model.Database
{
    /// <summary>
    /// Event args provided on change of a record
    /// </summary>
    public class RecordChangedEventArgs : EventArgs
    {
    } // End class

    /// <summary>
    /// Record stored in the Database (permenant information)
    /// </summary>
    public class Record
    {
        /// <summary>
        /// Fired on each change of the object
        /// </summary>
        public event EventHandler<RecordChangedEventArgs> RecordChanged;

        /// <summary>
        /// Database unique id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IdentificationRecord IdentificationRecord { get; private set; }

        /// <summary>
        /// MetaInfo, FirstName
        /// </summary>
        private string m_first_name;
        public string FirstName {
            get { return m_first_name; }
            set
            {
                m_first_name = value;
                if (RecordChanged != null)
                {
                    RecordChanged(this, new RecordChangedEventArgs());
                }
            }
        }

        /// <summary>
        /// MetaInfo, LastName
        /// </summary>
        private string m_last_name;
        public string LastName
        {
            get { return m_last_name; }
            set
            {
                m_last_name = value;
                if (RecordChanged != null)
                {
                    RecordChanged(this, new RecordChangedEventArgs());
                }
            }
        }

        /// <summary>
        /// MetaInfo, Image
        /// </summary>
        private System.Drawing.Image m_image;
        public System.Drawing.Image Image
        {
            get { return m_image; }
            set
            {
                m_image = value;
                if (RecordChanged != null)
                {
                    RecordChanged(this, new RecordChangedEventArgs());
                }
            }
        }

        /// <summary>
        /// Counter of how many times the record was identified
        /// </summary>
        public int TimesRecognized { get; private set; }

        /// <summary>
        /// Counter of how many times the record was tracked
        /// </summary>
        public int TimesTracked { get; private set; }

        /// <summary>
        /// Increments the times recognized counter
        /// </summary>
        public void IncrementTimesRecognized()
        {
            TimesRecognized++;
            if (RecordChanged != null)
            {
                RecordChanged(this, new RecordChangedEventArgs());
            }
        }

        /// <summary>
        /// Increments the Times Tracked counter
        /// </summary>
        public void IncrementTimesTracked()
        {
            TimesTracked++;
            if (RecordChanged != null)
            {
                RecordChanged(this, new RecordChangedEventArgs());
            }
        }

        /// <summary>
        /// Gets if the record is currently set as the target.
        /// </summary>
        private bool m_is_target;
        public bool IsTarget
        {
            get { return m_is_target; }
            set
            {
                m_is_target = value;
                if (RecordChanged != null)
                {
                    RecordChanged(this, new RecordChangedEventArgs());
                }
            }
        }

        /// <summary>
        /// Learned timestamp
        /// </summary>
        private DateTime m_learned_at;
        public DateTime LearnedAt
        {
            get { return m_learned_at; }
            set
            {
                m_learned_at = value;
                if (RecordChanged != null)
                {
                    RecordChanged(this, new RecordChangedEventArgs());
                }
            }
        }

        /// <summary>
        /// Gives the associated Runtime record
        /// </summary>
        private RuntimeDatabase.Record m_runtime_record;
        public RuntimeDatabase.Record RuntimeRecord
        {
            get { return m_runtime_record; }
            set
            {
                m_runtime_record = value;
                if (RecordChanged != null)
                {
                    RecordChanged(this, new RecordChangedEventArgs());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Record(int Id, IdentificationRecord IdentificationInformation)
        {
            this.IdentificationRecord = IdentificationInformation;
            this.Id = Id;
        }
    } // End class
} // End namespace
