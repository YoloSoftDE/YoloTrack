using System;
using Cognitec.FRsdk;

namespace YoloTrack.MVC.Model.Database
{
    /// <summary>
    /// FIR Wrapper
    /// </summary>
    public class IdentificationRecord
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
    }
}
