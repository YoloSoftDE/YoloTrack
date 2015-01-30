using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class TrackingImpl : BaseImpl<Arg.TrackingArg>
    {
        private KinectSensor sensor;
        private Skeleton[] skeletonData;

        public override void Run(Arg.TrackingArg arg)
        {
            sensor = Model.Kinect;
            skeletonData = Model.skeletonData;
            bool skeleton_found;

            do
            {
                skeleton_found = false;

                // find TrackingID in skeleton Collection
                foreach (Skeleton skeleton in skeletonData)
                {
                    if (skeleton.TrackingId == arg.SkeletonId)
                        skeleton_found = true;
                }

                if (!Model.RuntimeDatabase[arg.SkeletonId].Person.IsTarget)
                {
                    Result = new Arg.SwitchTargetArg();
                    break;
                }

                // refresh skeleton-Data
                skeletonData = Model.skeletonData;
            } while (skeleton_found == true);

            // skeleton is not tracked anymore
            // exit function
            return;
        }
    }
}
