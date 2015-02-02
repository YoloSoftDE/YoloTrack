using System;
using System.Collections.Generic;
using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.Sensor
{
    public class ColorImageFrameEventArgs : EventArgs
    {
        public ColorImageFrame Frame;
    }

    public class SkeletonFrameEventArgs : EventArgs
    {
        public SkeletonFrame Frame;
    }

    /// <summary>
    /// Wrapper class for the kinect sensor
    /// </summary>
    public class Model
    {
        /// <summary>
        /// Fired when a new color frame becomes available.
        /// </summary>
        public event EventHandler<ColorImageFrameEventArgs> ColorFrameAvailable;

        /// <summary>
        /// Fired when a new skeleton frame becomes available.
        /// </summary>
        public event EventHandler<SkeletonFrameEventArgs> SkeletonFrameAvailable;

        /// <summary>
        /// Capsulation of the color stream properties.
        /// </summary>
        public ColorImageStream ColorStream { get; private set; }

        /// <summary>
        /// Capsulation of the skeleton stream properties.
        /// </summary>
        public SkeletonStream SkeletonStream { get; private set; }

        /// <summary>
        /// Capsulation of the coordinate mapper.
        /// </summary>
        public CoordinateMapper CoordinateMapper { get; private set; }

        /// <summary>
        /// The wrapped KinectSensor instance.
        /// </summary>
        public KinectSensor Sensor { get; private set; }

        /// <summary>
        /// Wrap constructor.
        /// </summary>
        /// <param name="Sensor"></param>
        public Model(KinectSensor Sensor)
        {
            this.Sensor = Sensor;
            SkeletonStream = new SkeletonStream(Sensor.SkeletonStream);
            ColorStream = new ColorImageStream(Sensor.ColorStream);
            CoordinateMapper = new CoordinateMapper(this);
        }

        /// <summary>
        /// Grabs the first available kinect sensor and wraps it into a model object.
        /// </summary>
        /// <returns></returns>
        public static Model GrabAny()
        {
            KinectSensor[] sensors = _get_available_sensors();
            if (sensors.Length < 1)
            {
                throw new IndexOutOfRangeException();
            }

            return new Model(sensors[0]);
        }

        /// <summary>
        /// Grabs the sensor with the given number and wraps it into a model object.
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public static Model Grab(int Number)
        {
            KinectSensor[] sensors = _get_available_sensors();
            if (Number >= sensors.Length)
            {
                throw new IndexOutOfRangeException();
            }

            return new Model(sensors[Number]);
        }

        /// <summary>
        /// Obtains a list of available kinect sensors
        /// </summary>
        /// <returns></returns>
        private static KinectSensor[] _get_available_sensors()
        {
            List<KinectSensor> list = new List<KinectSensor>();
            foreach (KinectSensor sensor in KinectSensor.KinectSensors)
            {
                if (sensor.Status == KinectStatus.Connected)
                {
                    list.Add(sensor);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Gets the current Kinect sensor status
        /// </summary>
        public KinectStatus Status
        {
            get
            {
                return Sensor.Status;
            }
        }

        /// <summary>
        /// Initialize the events, etc.
        /// </summary>
        public void Initialize()
        {
            // Subscribe to the skeleton frames for firther processing
            Sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(_process_skeleton_frame);

            // Subscribe to the color image frames for firther processing
            Sensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(_process_color_frame);

            Sensor.SkeletonStream.Enable();
            Sensor.ColorStream.Enable(ColorImageFormat.RgbResolution1280x960Fps12);
            Sensor.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _process_skeleton_frame(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame frame = e.OpenSkeletonFrame();
            if (frame == null)
            {
                return;
            }

            // Update the frame and fire the event
            SkeletonStream.SkeletonFrame = frame;

            if (SkeletonFrameAvailable != null)
            {
                SkeletonFrameAvailable(this, new SkeletonFrameEventArgs()
                {
                    Frame = frame
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _process_color_frame(object sender, ColorImageFrameReadyEventArgs e)
        {
            ColorImageFrame frame = e.OpenColorImageFrame();
            if (frame == null)
            {
                return;
            }

            // Update the frame and fire the event
            ColorStream.ImageFrame = frame;

            if (ColorFrameAvailable != null)
            {
                ColorFrameAvailable(this, new ColorImageFrameEventArgs()
                {
                    Frame = frame
                });
            }
        }
    }
}
