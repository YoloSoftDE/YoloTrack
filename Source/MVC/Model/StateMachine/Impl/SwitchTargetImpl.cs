using System;
using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    /// <summary>
    /// State implementatin for 'SwitchTarget'
    /// </summary>
    class SwitchTargetImpl : BaseImpl<Arg.SwitchTargetArg>
    {
        /// <summary>
        /// State logic
        /// </summary>
        /// <param name="arg"></param>
        public override void Run(Arg.SwitchTargetArg arg)
        {
            // Check whether there is an other person that should be tracked
            if (!m_database.HasTarget)
            {
                // No, so return to the wait for body state
                Result = new Arg.WaitForBodyArg();
                return;
            }

            // Check if new target is currently avialable
            RuntimeDatabase.Record record = m_database.Target.RuntimeRecord;
            if (record == null)
            {
                // No, so return to the wait for body state
                Result = new Arg.WaitForBodyArg();
                return;
            }

            // New target is available, so switch to...
            m_sensor.SkeletonStream.Track(record.KinectResource.Skeleton.TrackingId);
            // ...and resume in tracking state
            Result = new Arg.TrackingArg()
            {
                TrackingId = record.KinectResource.Skeleton.TrackingId
            };
            return;
        }
    } // End class
} // End namespace
