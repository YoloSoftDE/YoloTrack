using System.Collections.Generic;
using Cognitec.FRsdk;
using System.Drawing;

namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    class LearnArg : BaseArg
    {
        public List<Sample> Samples;
        public List<Bitmap> Faces;
        public int SkeletonId;
    }
}
