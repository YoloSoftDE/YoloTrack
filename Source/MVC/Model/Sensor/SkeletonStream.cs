using System;
using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.Sensor
{
    public class SkeletonStream
    {
        /// <summary>
        /// Reference to owning model instance
        /// </summary>
        private Microsoft.Kinect.SkeletonStream m_to_wrap;

        /// <summary>
        /// Getter for the current skeleton frame
        /// </summary>
        public Skeleton[] Skeletons { get; internal set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Model"></param>
        internal SkeletonStream(Microsoft.Kinect.SkeletonStream ToWrap)
        {
            m_to_wrap = ToWrap;
            m_to_wrap.AppChoosesSkeletons = true;
            Skeletons = new Skeleton[6];
        }

        /// <summary>
        /// Starts tracking of skeleton with given id.
        /// </summary>
        /// <param name="TrackingId"></param>
        public void Track(int TrackingId)
        {
            m_to_wrap.ChooseSkeletons(TrackingId);
        }

        /// <summary>
        /// Releases all tracked skeletons. (Don't track)
        /// </summary>
        public void Track()
        {
            m_to_wrap.ChooseSkeletons();
        }
    } // End class
} // End namespace
