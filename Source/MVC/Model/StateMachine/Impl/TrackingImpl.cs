using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class TrackingImpl : BaseImpl<Arg.WaitForBodyArg, Arg.TrackingArg>
    {
        private KinectSensor sensor;
        private Skeleton[] skeletonData;

        public override void Run(Arg.TrackingArg arg)
        {
            sensor = Model.Kinect;
            skeletonData = new Skeleton[sensor.SkeletonStream.FrameSkeletonArrayLength];
            bool skeleton_found;

            // register event
            sensor.SkeletonFrameReady += sensor_SkeletonFrameReady;

            do
            {
                skeleton_found = false;

                // find TrackingID in skeleton Collection
                foreach (Skeleton skeleton in skeletonData)
                {
                    if (skeleton.TrackingId == arg.SkeletonId)
                        skeleton_found = true;
                }

                if (!arg.RTInfo.Person.IsTarget)
                {
                    // ...
                }
            } while (skeleton_found == true);

            // skeleton is not tracked anymore
            // exit function
            return;
        }

        void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())     // Open the Skeleton frame
            {
                if (skeletonFrame != null && this.skeletonData != null)     // check that a frame is available
                {
                    skeletonFrame.CopySkeletonDataTo(this.skeletonData);    // get the skeletal information in this frame
                }
            }
        }
    }
}
