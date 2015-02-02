using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    class WaitTakePictureArg : BaseArg
    {
        public int TrackingId;

        public override object Clone()
        {
            return new WaitTakePictureArg()
            {
                TrackingId = TrackingId
            };
        }
    }
}
