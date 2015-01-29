using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Microsoft.Kinect;
using System.Drawing;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class WaitTakePictureImpl : BaseImpl<Arg.IdentifyArg, Arg.WaitTakePictureArg>
    {
        int pictureCount = 0;
        KinectSensor kinect_sensor;
        Skeleton[] wtp_skeletonData;
        int pixelcutout;

        public override void Run(Arg.WaitTakePictureArg arg)
        {
            //Thread.Sleep(1000);
            ColorImagePoint headPoint;
            byte[] rawHeadData;
            List<Bitmap> faces = new List<Bitmap>();

            CoordinateMapper cm = new CoordinateMapper(kinect_sensor);
            Bitmap[] headPictures = new Bitmap[5];
            kinect_sensor = Model.Kinect;
            wtp_skeletonData = new Skeleton[kinect_sensor.SkeletonStream.FrameSkeletonArrayLength];
            Arg.IdentifyArg res = new Arg.IdentifyArg();
            
            pictureCount = 0;
            pixelcutout = 100;

            while (pictureCount < 5)
            {
                // synchronisation with ColorFrameReady-Event
                foreach (Skeleton skeleton in wtp_skeletonData)
                {
                    // block ColorFrameReady-Event
                    Model.sync_ColorFrame = false;

                    // search for tracked person
                    if (skeleton.TrackingId == arg.SkeletonId)
                    {
                        // found tracked person
                        headPoint = cm.MapSkeletonPointToColorPoint(skeleton.Joints[JointType.Head].Position,
                                                                    ColorImageFormat.RgbResolution1280x960Fps12);

                        // get head-cutout
                        rawHeadData = cutoutImage(Model.rawImageData, headPoint.X, headPoint.Y);

                        // save head-cutout as Bitmap-Object
                        faces.Add(write_Bitmap(rawHeadData));
                        pictureCount++;

                        res.SkeletonId = skeleton.TrackingId;
                    }
                }
                Thread.Sleep(500);

                // get new frame
                Model.sync_ColorFrame = true;
            }

            // return Faces
            res.Faces = faces;
            Result = res;
            return;
        }

        private byte[] cutoutImage(byte[] rawImgData, int x, int y)
        {
            // x, y --> centre of cutout area in entire image
            int x1, y1, sourceIndex, destIndex;
            x1 = x - pixelcutout;   // compute x of the top-left corner of the cutout image
            y1 = y - pixelcutout;   // compute y of the top-left corner of the cutout image


            byte[] cutout = new byte[(pixelcutout * pixelcutout * 4) * 4];
            destIndex = 0;
            for (int cy = y1; cy < y + pixelcutout; cy++)
            {
                for (int cx = x1 * 4; cx < (x + pixelcutout) * 4; cx++)
                {
                    sourceIndex = (4 * 1280 * cy) + cx;
                    cutout[destIndex] = rawImgData[sourceIndex];
                    destIndex++;
                }
            }
            return cutout;
        }

        private Bitmap write_Bitmap(byte[] rbg_array)
        {
            var imgConverter = new ImageConverter();
            var image = (Image)imgConverter.ConvertFrom(rbg_array);
            return new Bitmap(image);
        }
    }
}
