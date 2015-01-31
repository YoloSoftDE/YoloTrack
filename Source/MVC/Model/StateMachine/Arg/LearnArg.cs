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

        public override object Clone()
        {
            return new LearnArg()
            {
                Samples = new List<Sample>(Samples),
                Faces = new List<Bitmap>(Faces),
                SkeletonId = SkeletonId
            };
        }
    }
}
