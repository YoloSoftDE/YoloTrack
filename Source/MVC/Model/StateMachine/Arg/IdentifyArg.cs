using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    class IdentifyArg : BaseArg
    {
        public List<Bitmap> Faces;
        public int TrackingId;

        public override object Clone()
        {
            return new IdentifyArg()
            {
                Faces = new List<Bitmap>(Faces),
                TrackingId = TrackingId
            };
        }
    }
}
