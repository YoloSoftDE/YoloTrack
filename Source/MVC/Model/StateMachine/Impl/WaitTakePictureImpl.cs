using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using System.Drawing;
using System.Threading;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class WaitTakePictureImpl : BaseImpl<Arg.WaitTakePictureArg>
    {
        int pictureCount = 0;
        int pixelcutout = 100;
        int SkeletonId;
        Skeleton skeleton;
        List<Bitmap> faces = new List<Bitmap>();
        AutoResetEvent ae = new AutoResetEvent(false);

        public override void Run(Arg.WaitTakePictureArg arg)
        {
            EventHandler<ColorImageFrameReadyEventArgs> OnNextColorFrame = delegate(object sender, ColorImageFrameReadyEventArgs e)
            {
                ColorImagePoint headPoint;
                byte[] rawHeadData;

                try
                {
                    // found tracked person
                    if (skeleton.Joints[JointType.Head].TrackingState != JointTrackingState.Tracked)
                    {
                        Console.WriteLine("[WaitTakePicture] Head joint not tracked, skipping.");
                        return;
                    }

                    headPoint = Model.Kinect.MapSkeletonPointToColor(skeleton.Joints[JointType.Head].Position,
                                                                ColorImageFormat.RgbResolution1280x960Fps12);
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine("[WaitTakePicture] InvalidCastException");
                    return;
                }

                Console.WriteLine("[WaitTakePicture] (guessed) Head Point is at {0}|{1}", headPoint.X, headPoint.Y);

                // get head-cutout
                ColorImageFrame frame = e.OpenColorImageFrame();
                //byte[] buffer = new byte[frame.PixelDataLength];
                rawHeadData = cutoutImage(frame.GetRawPixelData(), headPoint.X, headPoint.Y);

                // save head-cutout as Bitmap-Object
                faces.Add(write_Bitmap(rawHeadData));
                pictureCount++;
            };

            EventHandler<SkeletonFrameReadyEventArgs> OnNextSkeletonFrame = delegate(object sender, SkeletonFrameReadyEventArgs e)
            {
                SkeletonFrame frame = e.OpenSkeletonFrame();
                Skeleton[] skeleton_list = new Skeleton[frame.SkeletonArrayLength];
                frame.CopySkeletonDataTo(skeleton_list);
                for (int i = 0; i < skeleton_list.Length; i++)
                {
                    Skeleton compare = skeleton_list[i];
                    if (compare.TrackingId == SkeletonId)
                    {
                        skeleton = compare;
                        ae.Set();
                        return;
                    }
                }
                // FIXME: Possible NULL-Reference
                skeleton.TrackingState = SkeletonTrackingState.NotTracked;
            };

            pictureCount = 0;
            SkeletonId = arg.SkeletonId;
            Model.Kinect.SkeletonFrameReady += OnNextSkeletonFrame;

            ae.WaitOne();
            Model.Kinect.ColorFrameReady += OnNextColorFrame;

            while (pictureCount < 5)
            {
                // muss noch schöner gehn
                if (skeleton == null)
                    continue;

                // synchronisation with ColorFrameReady-Event
                //Skeleton skeleton = FindSkeleton(arg.SkeletonId);
                if (skeleton.TrackingState == SkeletonTrackingState.NotTracked)
                {
                    Model.Kinect.ColorFrameReady -= OnNextColorFrame;
                    Model.Kinect.SkeletonFrameReady -= OnNextSkeletonFrame;
                    Result = new Arg.WaitForBodyArg();
                    return;
                }

                System.Threading.Thread.Sleep(500);
            }
            Model.Kinect.ColorFrameReady -= OnNextColorFrame;
            Model.Kinect.SkeletonFrameReady -= OnNextSkeletonFrame;
            
            Result = new Arg.IdentifyArg()
            {
                Faces = faces,
                SkeletonId = arg.SkeletonId
            };
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
