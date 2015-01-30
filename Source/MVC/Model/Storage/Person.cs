using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cognitec.FRsdk;
using System.Drawing;
using System.Runtime.Serialization;

namespace YoloTrack.MVC.Model.Storage
{

    public class IdentificationRecord : IFormatter
    {
        public FIR Value;
        // public Bitmap[] Sources;

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

        public override bool Equals(object obj)
        {
            return ((Person)obj).Id.Equals(Id);
        }

        public RuntimeInfo RTInfo
        {
            get {
                return RTInfo;
            }
            set
            {
                IsPresent = true;
                RTInfo = value;
                value.Person = this;
            }
        }
    }
}
