using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using System.Drawing;
using System.Runtime.InteropServices;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    /// <summary>
    /// Implementation of the state logic for 'WaitTakePicture'
    /// </summary>
    class WaitTakePictureImpl : BaseImpl<Arg.WaitTakePictureArg>
    {
        /// <summary>
        /// Execution step
        /// </summary>
        /// <param name="arg"></param>
        public override void Run(Arg.WaitTakePictureArg arg)
        {
            int picture_count = 0;
            List<Bitmap> faces = new List<Bitmap>();
            DateTime start_time = DateTime.Now;

            while (picture_count < 5)
            {
                m_runtime_database.Refresh();

                // Timeout? (too long to keep doing this shit)
                if ((DateTime.Now - start_time).TotalSeconds >= 3)
                {
                    Result = new Arg.WaitForBodyArg();
                    Console.WriteLine("WaitTakePicture timed out.");
                    return;
                }
                
                // Lost skeleton?
                if (!m_runtime_database.ContainsKey(arg.TrackingId))
                {
                    Result = new Arg.WaitForBodyArg();
                    return;
                }

                RuntimeDatabase.Record record = m_runtime_database[arg.TrackingId];
                Skeleton skeleton = record.KinectResource.Skeleton;

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

                    head_point = m_sensor.CoordinateMapper.MapSkeletonPointToColorPoint(skeleton.Joints[JointType.Head].Position);
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine("[WaitTakePicture] InvalidCastException");
                    continue;
                }

                ColorImageFrame frame = m_sensor.ColorStream.ImageFrame;
                if (frame == null)
                    continue;

                //byte[] buffer = new byte[frame.PixelDataLength];
                byte[] head_picture_data = GetHeadPicture(frame.GetRawPixelData(), record.KinectResource.HeadRectangle);

                // save head-cutout as Bitmap-Object
                faces.Add(WriteBitmap(head_picture_data, record.KinectResource.HeadRectangle));
                picture_count++;
            }
            
            Result = new Arg.IdentifyArg()
            {
                Faces = faces,
                TrackingId = arg.TrackingId
            };

            return;
        }

        /// <summary>
        /// Crops the image to only contain an image of the head
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        ///        
        unsafe private byte[] GetHeadPicture(byte[] source, Rectangle rectangle)
        {
            int length = rectangle.Width * rectangle.Height * 4;
            byte[] dest = new byte[length]; 
            fixed (byte* dest_ptr = &dest[0]) 
            {
                for (int y = 0; y < rectangle.Height; y++)
                {
                    Marshal.Copy(source, m_sensor.ColorStream.Width * 4 * (y + rectangle.Y) + rectangle.X * 4,
                                 new IntPtr(dest_ptr + y * rectangle.Width * 4), rectangle.Width * 4);
                }
            }

            return dest;
        }

        /// <summary>
        /// Write bitmap object out of a given rgb array
        /// </summary>
        /// <param name="rbg_array"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        private Bitmap WriteBitmap(byte[] rbg_array, Rectangle rect)
        {
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);

            System.Drawing.Imaging.BitmapData bmp_data = bmp.LockBits(
                new Rectangle(0, 0, rect.Width, rect.Height), 
                System.Drawing.Imaging.ImageLockMode.ReadWrite, 
                System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            IntPtr ptr = bmp_data.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(rbg_array, 0, ptr, rbg_array.Length);
            bmp.UnlockBits(bmp_data);
            
            return bmp;
        }
    } // End class
} // End namespace
