using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    struct IdentifyArg : BaseArg
    {
        public List<Bitmap> Faces { get; set; }
        public int SkeletonId { get; set; }
        public Storage.RuntimeInfo RTInfo
        {

        }
    }
}
