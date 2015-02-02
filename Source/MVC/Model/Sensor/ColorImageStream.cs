using System;
using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.Sensor
{
    public class ColorImageStream
    {
        /// <summary>
        /// Reference to owning model instance
        /// </summary>
        private Microsoft.Kinect.ColorImageStream m_to_wrap;

        /// <summary>
        /// The width of the color frame defined by the current FrameFormat
        /// </summary>
        public int Width { get { return m_to_wrap.FrameWidth; } }

        /// <summary>
        /// The height of the color frame defined by the current FrameFormat
        /// </summary>
        public int Height { get { return m_to_wrap.FrameHeight; } }

        /// <summary>
        /// A copy of the current color image frame captured by Kinect
        /// </summary>
        public ColorImageFrame ImageFrame { get; internal set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Model"></param>
        internal ColorImageStream(Microsoft.Kinect.ColorImageStream ToWrap)
        {
            m_to_wrap = ToWrap;
        }

        public ColorImageFormat FrameFormat { get { return m_to_wrap.Format; } }
    }
}
