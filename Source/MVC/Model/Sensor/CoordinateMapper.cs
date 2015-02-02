using System;
using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.Sensor
{
    public class CoordinateMapper
    {
        private ColorImageStream m_color_stream;

        /// <summary>
        /// 
        /// </summary>
        private Microsoft.Kinect.CoordinateMapper m_mapper;

        /// <summary>
        /// 
        /// </summary>
        private Microsoft.Kinect.KinectSensor m_sensor;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CoordinateMapper(Model Model)
        {
            m_color_stream = Model.ColorStream;
            m_sensor = Model.Sensor;
            m_mapper = new Microsoft.Kinect.CoordinateMapper(Model.Sensor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public ColorImagePoint MapSkeletonPointToColorPoint(SkeletonPoint point)
        {
            //return m_mapper.MapSkeletonPointToColorPoint(point, m_color_stream.FrameFormat);
            return m_sensor.MapSkeletonPointToColor(point, m_color_stream.FrameFormat);
        }
    } // End class
} // End namespace
