using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Microsoft.Kinect;
using System.Drawing;

namespace YoloTrack.MVC.Model.StateMachine.Impl.Util
{
    class CompatibleImage : Cognitec.FRsdk.Image
    {
        private System.Drawing.Bitmap color, grayscale;
        private IntPtr data_ptr_color, data_ptr_grayscale;

        public static CompatibleImage FromBitmap(Bitmap orginal)
        {
            CompatibleImage ci = new CompatibleImage()
            {
                color = new Bitmap(orginal),
                //grayscale = new Bitmap(orginal.Width, orginal.Height)
            };

            /*
            int x, y;
            // Loop through the images pixels to reset color.
            for (x = 0; x < orginal.Width; x++)
            {
                for (y = 0; y < orginal.Height; y++)
                {
                    Color pixelColor = orginal.GetPixel(x, y);
                    Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    ci.grayscale.SetPixel(x, y, newColor); // Now greyscale
                }
            } 
            
            System.Drawing.Imaging.BitmapData data_grayscale = ci.grayscale.LockBits(new Rectangle(0, 0, ci.grayscale.Width, ci.grayscale.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            ci.data_ptr_grayscale = data_grayscale.Scan0;
             */

            System.Drawing.Imaging.BitmapData data_color = ci.color.LockBits(new Rectangle(0, 0, ci.color.Width, ci.color.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            ci.data_ptr_color = data_color.Scan0;

            return ci;
        }

        public IntPtr colorRepresentation()
        {
            return data_ptr_color;
        }

        public IntPtr grayScaleRepresentation()
        {
            //return data_ptr_grayscale;
            throw new NotImplementedException();
        }

        public uint height()
        {
            return (uint)color.Height;
        }

        public bool isColor()
        {
            return true;
        }

        public string name()
        {
            return "";
        }

        public uint width()
        {
            return (uint)color.Width;
        }
    }
}
