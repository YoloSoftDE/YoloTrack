using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    class TrackingArg : BaseArg
    {
        public int TrackingId;

        public override object Clone()
        {
            return new TrackingArg()
            {
                TrackingId = TrackingId
            };
        }
    }
}
