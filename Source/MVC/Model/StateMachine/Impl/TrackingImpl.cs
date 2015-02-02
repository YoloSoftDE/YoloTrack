using Microsoft.Kinect;
using System;
using System.Threading;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    /// <summary>
    /// Implementation of the state logic for 'Tracking'
    /// </summary>
    class TrackingImpl : BaseImpl<Arg.TrackingArg>
    {        
        /// <summary>
        /// State logic
        /// </summary>
        /// <param name="arg"></param>
        public override void Run(Arg.TrackingArg arg)
        {
            while (true)
            {
                // Update runtime database
                m_runtime_database.Refresh();

                // Lost skeleton?
                if (m_runtime_database.ContainsKey(arg.TrackingId) == false)
                {
                    Result = new Arg.WaitForBodyArg();
                    break;
                }

                // Changed target?
                if (m_runtime_database[arg.TrackingId].DatabaseRecord.IsTarget == false)
                {
                    Result = new Arg.SwitchTargetArg();
                    break;
                }

                // Slight delay to prevent high cpu utilization
                Thread.Sleep(100);
            }
        }
    } // End class
} // End namespace
