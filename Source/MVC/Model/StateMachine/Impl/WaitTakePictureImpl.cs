using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using System.Drawing;
using System.Threading;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class WaitTakePictureImpl : BaseImpl<Arg.WaitTakePictureArg>
    {
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

                    head_point = mapper.MapSkeletonPointToColorPoint(skeleton.Joints[JointType.Head].Position, Model.ColorStreamFormat);
                }
                catch (InvalidCastException)
                {
                    //Console.WriteLine("[WaitTakePicture] InvalidCastException");
                    return;
                }

                ColorImageFrame frame = Model.OpenNextColorFrame();
                if (frame == null)
                    continue;

                //byte[] buffer = new byte[frame.PixelDataLength];
                byte[] rawHeadData = GetHeadPicture(frame.GetRawPixelData(), info.HeadRect);

                // save head-cutout as Bitmap-Object
                faces.Add(write_Bitmap(rawHeadData, info.HeadRect));
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
            if (rectangle.X < 0)
                x1 = 0;
            else
                x1 = rectangle.X;

            if (rectangle.Y < 0)
                y1 = 0;
            else
                y1 = rectangle.Y;

            int x2, y2;
            x2 = rectangle.X + rectangle.Width;
            if (x2 >= Model.ColorStreamSize.Width)
                x2 = Model.ColorStreamSize.Width-1;

            y2 = rectangle.Y + rectangle.Height;
            if (y2 >= Model.ColorStreamSize.Height)
                y2 = Model.ColorStreamSize.Height-1;

            byte[] cutout = new byte[rectangle.Width * rectangle.Height * 4];
            destIndex = 0;
            for (int cy = y1; cy < y2; cy++)
            {
                for (int cx = x1 * 4; cx < x2 * 4; cx++)
                {
                    sourceIndex = (4 * Model.ColorStreamSize.Width * cy) + cx;
                    cutout[destIndex] = p[sourceIndex];
                    destIndex++;
                }
            }
            return cutout;
        }

        private Bitmap write_Bitmap(byte[] rbg_array, Rectangle rect)
        {
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            System.Drawing.Imaging.BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, rect.Width, rect.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            IntPtr ptr = bmp_data.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(rbg_array, 0, ptr, rbg_array.Length);
            bmp.UnlockBits(bmp_data);
            //bmp.Save("penis123.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            return bmp;
        }
    }
}
