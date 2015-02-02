using System;
using Cognitec.FRsdk;

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
        /// <param name="ms"></param>
        public void Serialize(System.IO.MemoryStream ms)
        {
            Value.serialize(ms);
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
