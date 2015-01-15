using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using System.IO;
using System.Drawing;

namespace Kinect1_skeletonTracking
{
    class SkeletonTracking
    {
        byte[] rawImgData;
        KinectSensor kinect = null;
        Skeleton[] skeletonData;
        CoordinateMapper cm;

        int pixelcutout = 100;

        public void StartKinect()
        {
            kinect = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected); // Get first Kinect Sensor
            
            kinect.SkeletonStream.Enable(); // Enable skeletal tracking
            kinect.ColorStream.Enable(ColorImageFormat.RgbResolution1280x960Fps12);

            skeletonData = new Skeleton[kinect.SkeletonStream.FrameSkeletonArrayLength]; // Allocate ST data
            rawImgData = new byte[kinect.ColorStream.FramePixelDataLength];

            kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady); // Get Ready for Skeleton Ready Events
            kinect.ColorFrameReady += kinect_ColorFrameReady;
            
            kinect.Start(); // Start Kinect sensor
        }

        private void kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    colorFrame.CopyPixelDataTo(rawImgData);                       // copy Frame-data to byte-array
                    System.IO.File.WriteAllBytes("img.txt", rawImgData);
                    //Console.WriteLine("Aufname fertig!");
                    return;
                }
            }
        }

        private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())     // Open the Skeleton frame
            {
                if (skeletonFrame != null && this.skeletonData != null)     // check that a frame is available
                {
                    skeletonFrame.CopySkeletonDataTo(this.skeletonData);    // get the skeletal information in this frame
                    accessTrackedSkeletons();
                }
            }
        }

        private void accessTrackedSkeletons()
        {
            foreach (Skeleton skeleton in this.skeletonData)
            {
                //skeleton.TrackingState = SkeletonTrackingState.Tracked;
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    /* joint position in meters
                     * x-position: < 0, when standing at the left (user view), > 0, when standing at the right (user view)
                     * y-position: < 0, when standing below the sensor, > 0, when standing above the sensor
                     * z-position: only positive values
                     */
                    Console.WriteLine("X-Position: " + skeleton.Joints[JointType.Head].Position.X); 
                    Console.WriteLine("Y-Position: " + skeleton.Joints[JointType.Head].Position.Y); 
                    Console.WriteLine("Z-Position: " + skeleton.Joints[JointType.Head].Position.Z); 
                    //DrawTrackedSkeletonJoints(skeleton.Joints);

                    // convert joint-Position to Colorpoint
                    cm = new CoordinateMapper(kinect);
                    ColorImagePoint headImagePoint = cm.MapSkeletonPointToColorPoint(skeleton.Joints[JointType.Head].Position, ColorImageFormat.RgbResolution1280x960Fps12);
                    Console.WriteLine("X-Position Image: " + headImagePoint.X);
                    Console.WriteLine("Y-Position Image: " + headImagePoint.Y);
                    
                    // write Image-Data to ppm-file
                    write_ppm(rawImgData, "img.ppm", "1280", "960");
                    write_ppm(cutoutImage(rawImgData, headImagePoint.X, headImagePoint.Y), "cutout.ppm", "200", "200");
                    Console.WriteLine("PPM fertig");
                    Console.Read();
                }
                else if (skeleton.TrackingState == SkeletonTrackingState.PositionOnly)
                {
                    //DrawSkeletonPosition(skeleton.Position);
                }
            }
        }

        private void write_ppm(byte[] rawImgData, string file, string width, string height)
        {
            //Use a streamwriter to write the text part of the encoding
            var writer = new StreamWriter(file);
            writer.Write("P6" + " ");
            writer.Write(width + " " + height + " ");
            writer.Write("255" + " ");
            writer.Close();
            //Switch to a binary writer to write the data
            var writerB = new BinaryWriter(new FileStream(file, FileMode.Append));
            for (int x = 0; x < rawImgData.Length; x = x + 4)
            {
                writerB.Write(rawImgData[x+2]);
                writerB.Write(rawImgData[x+1]);
                writerB.Write(rawImgData[x]);
            }
            writerB.Flush();
            writer.Close();
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
                for (int cx = x1*4; cx < (x + pixelcutout) * 4; cx++)
                {
                    sourceIndex = (4 * 1280 * cy) + cx;
                    cutout[destIndex] = rawImgData[sourceIndex];
                    destIndex++;
                }
            }
            return cutout;
        }

        private byte[] cutoutImage2(byte[] rawImgData, int x, int y)
        {
            byte[] destArray = new byte[100 * 100 * 4];

            for (int pos_x = 0; pos_x < 100; pos_x++)
            {
                for (int pos_y = 0; pos_y < 100; pos_y++)
                {
                    int imgX = x + pos_x- 50;
                    int imgY = y + pos_y - 50;

                    int pixelCount = (imgY * 1280) + imgX;

                    for (int b = 0; b < 4; b++)
                    {
                        destArray[((pos_y * 100) + pos_x) * 4 + b] = rawImgData[b + pixelCount * 4];
                    }

                }

                Console.WriteLine("Pos {0}", pos_x);
            }

            return destArray;

        }
    }
}
