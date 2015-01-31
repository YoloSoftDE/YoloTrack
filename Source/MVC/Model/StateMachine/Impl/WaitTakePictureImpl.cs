using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using System.Drawing;
using System.Threading;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class WaitTakePictureImpl : BaseImpl<Arg.WaitTakePictureArg>
    {
        int pixelcutout = 100;

        public override void Run(Arg.WaitTakePictureArg arg)
        {
            int picture_count = 0;
            List<Bitmap> faces = new List<Bitmap>();
            CoordinateMapper mapper = new CoordinateMapper(Model.Kinect);

            while (picture_count < 5)
            {
                Model.RuntimeDatabase.Refresh();
                
                // Lost skeleton?
                if (!Model.RuntimeDatabase.ContainsKey(arg.SkeletonId))
                {
                    Result = new Arg.WaitForBodyArg();
                    return;
                }

                Storage.RuntimeInfo info = Model.RuntimeDatabase[arg.SkeletonId];
                Skeleton skeleton = info.Skeleton;

                // Lost skeleton? TODO: check if still needed
                if (skeleton.TrackingState == SkeletonTrackingState.NotTracked)
                {
                    Result = new Arg.WaitForBodyArg();
                    return;
                }

                ColorImagePoint head_point;
                try
                {
                    // found tracked person
                    if (skeleton.Joints[JointType.Head].TrackingState != JointTrackingState.Tracked)
                        continue;

                    head_point = mapper.MapSkeletonPointToColorPoint(skeleton.Joints[JointType.Head].Position, ColorImageFormat.RgbResolution1280x960Fps12);
                }
                catch (InvalidCastException)
                {
                    //Console.WriteLine("[WaitTakePicture] InvalidCastException");
                    return;
                }

                ColorImageFrame frame = Model.Kinect.ColorStream.OpenNextFrame(1000);
                if (frame == null)
                    continue;

                //byte[] buffer = new byte[frame.PixelDataLength];
                byte[] rawHeadData = GetHeadPicture(frame.GetRawPixelData(), info.HeadRect);

                // save head-cutout as Bitmap-Object
                faces.Add(write_Bitmap(rawHeadData));
                picture_count++;
            }
            
            Result = new Arg.IdentifyArg()
            {
                Faces = faces,
                SkeletonId = arg.SkeletonId
            };

            return;
        }

        private byte[] GetHeadPicture(byte[] p, Rectangle rectangle)
        {
            int x1, y1, sourceIndex, destIndex;
            if (rectangle.X < pixelcutout)
                x1 = 0;
            else
                x1 = rectangle.X - pixelcutout;

            if (rectangle.Y < pixelcutout)
                y1 = 0;
            else
                y1 = rectangle.Y - pixelcutout;

            int x2, y2;
            x2 = rectangle.X + pixelcutout;
            if (x2 > 1280)
                x2 = 1280;

            y2 = rectangle.Y + pixelcutout;
            if (y2 > 960)
                y2 = 960;

            byte[] cutout = new byte[(pixelcutout * pixelcutout * 4) * 4];
            destIndex = 0;
            for (int cy = y1; cy < y2; cy++)
            {
                for (int cx = x1 * 4; cx < x2 * 4; cx++)
                {
                    sourceIndex = (4 * 1280 * cy) + cx;
                    cutout[destIndex] = p[sourceIndex];
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
