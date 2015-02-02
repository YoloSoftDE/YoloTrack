using System;
using Cognitec.FRsdk;
using System.IO;

namespace YoloTrack.MVC.Model.Database
{
    /// <summary>
    /// FIR Wrapper
    /// </summary>
    public class IdentificationRecord : ISerializable
    {
        /// <summary>
        /// 
        /// </summary>
        public FIR Value { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Orginal"></param>
        public IdentificationRecord(FIR Orginal)
        {
            Value = Orginal;
        }

        /// <summary>
        /// 
        /// </summary>
        public IdentificationRecord()
        {
            Value = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        public void Serialize(System.IO.MemoryStream ms)
        {
            MemoryStream tmp = new MemoryStream();
            Value.serialize(tmp);
            Serializer.Serialize(ms, tmp.Length);
            tmp.Seek(0, SeekOrigin.Begin);
            tmp.CopyTo(ms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        public void Unserialize(System.IO.MemoryStream ms)
        {
            // TODO: FIR Builder required
            throw new NotImplementedException();
        }
    }
}
