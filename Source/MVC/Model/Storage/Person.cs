using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cognitec.FRsdk;
using System.Drawing;
using System.Runtime.Serialization;

namespace YoloTrack.MVC.Model.Storage
{
    [Serializable]
    public struct IdentificationRecord : ISerializable
    {
        public FIR Value;
        // public Bitmap[] Sources;

        public IdentificationRecord(SerializationInfo info, StreamingContext context)
        {
            FIRBuilder builder = Model.TrackingModel.Instance().FIRBuilder;
            System.IO.MemoryStream ms = new System.IO.MemoryStream((byte[])info.GetValue("Value", typeof(byte[])));
            Value = builder.build(ms);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Value.serialize(ms);
            info.AddValue("Value", ms.ToArray(), typeof(byte[]));
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

        public static Guid fail = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

        public RuntimeInfo RTInfo
        {
            get {
                return RTInfo;
            }
            set
            {
                IsPresent = true;
                RTInfo = value;
            }
        }
    }
}
