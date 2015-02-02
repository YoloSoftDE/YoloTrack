using System.Collections.Generic;
using Cognitec.FRsdk;
using System.Drawing;

namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    /// <summary>
    /// Arguments provided to the Learn-State
    /// </summary>
    class LearnArg : BaseArg
    {
        /// <summary>
        /// The list of samples to learn
        /// </summary>
        public List<Sample> Samples;

        /// <summary>
        /// List of the original bitmaps, the samples were created from
        /// </summary>
        public List<Bitmap> Faces;

        /// <summary>
        /// Id-reference to the record that should be learned
        /// </summary>
        public int TrackingId;

        /// <summary>
        /// Clones the whole object to ensure that we start with a fresh copy 
        /// into the next state and release everything of the previous
        /// one.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new LearnArg()
            {
                Samples = new List<Sample>(Samples),
                Faces = new List<Bitmap>(Faces),
                TrackingId = TrackingId
            };
        }
    } // End class
} // End namespace
