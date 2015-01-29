using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    struct TrackingArg : BaseArg
    {
        public int SkeletonId { get; set; }
        public Storage.RuntimeInfo RTInfo
        {
            get
            {

            };
        }
    }
}
