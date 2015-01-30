using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Microsoft.Kinect;
using System.Drawing;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class WaitTakePictureImpl : BaseImpl<Arg.WaitTakePictureArg>
    {
        int pictureCount = 0;
        Skeleton[] wtp_skeletonData;
        int pixelcutout;

        public override void Run(Arg.WaitTakePictureArg arg)
        {
            KinectSensor kinect_sensor = Model.Kinect;
            ColorImagePoint headPoint;
            byte[] rawHeadData;
            List<Bitmap> faces = new List<Bitmap>();

            Bitmap[] headPictures = new Bitmap[5];
            wtp_skeletonData = Model.skeletonData;

            Arg.IdentifyArg res = new Arg.IdentifyArg();

            pictureCount = 0;
            pixelcutout = 100;

            while (pictureCount < 5)
            {
                // synchronisation with ColorFrameReady-Event
                foreach (Skeleton skeleton in wtp_skeletonData)
                {
                    if (skeleton.TrackingId == 0)
                        continue;

                    Console.WriteLine("[WaitTakePicture] [{2}/5] Test Skeleton ({0} == {1})", skeleton.TrackingId, arg.SkeletonId, pictureCount);

                    // block ColorFrameReady-Event
                    Model.sync_ColorFrame = false;

                    if (arg.SkeletonId != 0)
                    {
                        // search for tracked person
                        if (skeleton.TrackingId == arg.SkeletonId)
                        {
                            try
                            {
                                // found tracked person
                                if (skeleton.Joints[JointType.Head].TrackingState != JointTrackingState.Tracked)
                                {
                                    Console.WriteLine("[WaitTakePicture] Head joint not tracked, skipping.");
                                    continue;
                                }

                                headPoint = kinect_sensor.MapSkeletonPointToColor(skeleton.Joints[JointType.Head].Position,
                                                                            ColorImageFormat.RgbResolution1280x960Fps12);
                            } catch (InvalidCastException) {
                                Console.WriteLine("[WaitTakePicture] InvalidCastException");
                                continue; 
                            }

                            Console.WriteLine("[WaitTakePicture] (guessed) Head Point is at {0}|{1}", headPoint.X, headPoint.Y);
                            /*
                            if (headPoint.X <= 0 || headPoint.Y <= 0)
                                continue;
                             */

                            // get head-cutout
                            rawHeadData = cutoutImage(Model.rawImageData, headPoint.X, headPoint.Y);

                            // save head-cutout as Bitmap-Object
                            faces.Add(write_Bitmap(rawHeadData));
                            pictureCount++;

                            res = new Arg.IdentifyArg()
                            {
                                SkeletonId = skeleton.TrackingId
                            };
                        }
                    }
                }
                Thread.Sleep(500);

                // get new frame
                Model.sync_ColorFrame = true;

                // refresh skeleton-Data
                wtp_skeletonData = Model.skeletonData;
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
            if (x < pixelcutout)
                x1 = 0;
            else
                x1 = x - pixelcutout;   // compute x of the top-left corner of the cutout image

            if (y < pixelcutout)
                y1 = 0;
            else
                y1 = y - pixelcutout;   // compute y of the top-left corner of the cutout image

            int x2, y2;
            x2 = x + pixelcutout;
            if (x2 > 1280)
                x2 = 1280;

            y2 = y + pixelcutout;
            if (y2 > 960)
                y2 = 960;

            byte[] cutout = new byte[(pixelcutout * pixelcutout * 4) * 4];
            destIndex = 0;
            for (int cy = y1; cy < y2; cy++)
            {
                for (int cx = x1 * 4; cx < x2 * 4; cx++)
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
            Bitmap bmp = new Bitmap(200, 200);
            System.Drawing.Imaging.BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, 200, 200), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            IntPtr ptr = bmp_data.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(rbg_array, 0, ptr, rbg_array.Length);
            bmp.UnlockBits(bmp_data);
            //bmp.Save("penis123.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            return bmp;
        }
    }
}
