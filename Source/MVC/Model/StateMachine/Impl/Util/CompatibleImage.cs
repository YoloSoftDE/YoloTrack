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
        private System.Drawing.Bitmap color;
        private IntPtr data_ptr_color;

        public static CompatibleImage FromBitmap(Bitmap orginal)
        {
            CompatibleImage ci = new CompatibleImage()
            {
                color = new Bitmap(orginal)
            };

            ci.data_ptr_color = ci.color.LockBits(new Rectangle(0, 0, ci.color.Width, ci.color.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb).Scan0;

            return ci;
        }

        public IntPtr colorRepresentation()
        {
            return data_ptr_color;
        }

        public IntPtr grayScaleRepresentation()
        {
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
