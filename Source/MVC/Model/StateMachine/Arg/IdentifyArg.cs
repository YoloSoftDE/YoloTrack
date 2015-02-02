using System.Collections.Generic;
using System.Drawing;

namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    /// <summary>
    /// Arguments provided to the Identify-state
    /// </summary>
    class IdentifyArg : BaseArg
    {
        /// <summary>
        /// List of images containing the face of the person to be identified
        /// </summary>
        public List<Bitmap> Faces;

        /// <summary>
        /// The assigned id from within the runtime database
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
            return new IdentifyArg()
            {
                Faces = new List<Bitmap>(Faces),
                TrackingId = TrackingId
            };
        }
    } // End class
} // End namespace
