using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cognitec.FRsdk;
using System.Drawing;
using System.Runtime.Serialization;

namespace YoloTrack.MVC.Model.Storage
{

    public class IdentificationRecord
    {
        public FIR Value;

        public IdentificationRecord()
        {
        }

        public IdentificationRecord(IdentificationRecord orginal)
        {
            Value = orginal.Value;
        }

        // public Bitmap[] Sources;
        /*
        public SerializationBinder Binder
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public StreamingContext Context
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object Deserialize(System.IO.Stream serializationStream)
        {
            throw new NotImplementedException();
        }

        public void Serialize(System.IO.Stream serializationStream, object graph)
        {
            throw new NotImplementedException();
        }

        public ISurrogateSelector SurrogateSelector
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
         */
    }

    public class IdentificationRecordSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            IdentificationRecord rt = (IdentificationRecord)obj;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            rt.Value.serialize(ms);
            info.AddValue("Value", ms.ToArray(), typeof(byte[]));
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            IdentificationRecord rt = (IdentificationRecord)obj;
            FIRBuilder builder = Model.TrackingModel.Instance().FIRBuilder;
            System.IO.MemoryStream ms = new System.IO.MemoryStream((byte[])info.GetValue("Value", typeof(byte[])));
            rt.Value = builder.build(ms);

            return rt;
        }
    }

    public class Person
    {
        public Guid Id;
        public string Name;
        public DateTime Learned;
        public bool IsTarget;
        public IdentificationRecord IR;
        public Bitmap Picture;
        public bool IsPresent = false;
        private RuntimeInfo runtime_info;
        public int RecognizedCount;
        public int TrackedCount;

        public Person()
        {
        }

        public Person(Person original)
        {
            Id = new Guid(original.Id.ToString());
            Name = (string)original.Name.Clone();
            IsTarget = original.IsTarget;
            IR = new IdentificationRecord(original.IR);
            Picture = new Bitmap(original.Picture);
            IsPresent = original.IsPresent;
            runtime_info = new RuntimeInfo(runtime_info);
            RecognizedCount = original.RecognizedCount;
            TrackedCount = original.TrackedCount;
        }

        public override bool Equals(object obj)
        {
            return ((Person)obj).Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public RuntimeInfo RuntimeInfo
        {
            get {
                return runtime_info;
            }
            set
            {
                IsPresent = true;
                runtime_info = value;
                value.Person = this;
            }
        }
    }
}
