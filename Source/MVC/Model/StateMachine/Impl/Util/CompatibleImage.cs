using System;
using System.Drawing;

namespace YoloTrack.MVC.Model.StateMachine.Impl.Util
{
    /// <summary>
    /// Ultitily class for providing an native interface from a standard bitmap object to the 
    /// libraries image class (adapter).
    /// </summary>
    class CompatibleImage : Cognitec.FRsdk.Image
    {
        /// <summary>
        /// Holds the original bitmap
        /// </summary>
        private System.Drawing.Bitmap color;

        /// <summary>
        /// Holds the pointer to the data
        /// </summary>
        private IntPtr data_ptr_color;

        /// <summary>
        /// Factory method for creating a Campatible image from standard bitmap
        /// </summary>
        /// <param name="orginal"></param>
        /// <returns></returns>
        public static CompatibleImage FromBitmap(Bitmap orginal)
        {
            CompatibleImage ci = new CompatibleImage()
            {
                color = new Bitmap(orginal)
            };

            ci.data_ptr_color = ci.color.LockBits(new Rectangle(0, 0, ci.color.Width, ci.color.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb).Scan0;

            return ci;
        }

        /// <summary>
        /// Getter for the raw color stream information
        /// </summary>
        /// <returns></returns>
        public IntPtr colorRepresentation()
        {
            return data_ptr_color;
        }

        /// <summary>
        /// Unused
        /// </summary>
        /// <returns></returns>
        public IntPtr grayScaleRepresentation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Picture is always colorized
        /// </summary>
        /// <returns></returns>
        public bool isColor()
        {
            return true;
        }

        /// <summary>
        /// Unused
        /// </summary>
        /// <returns></returns>
        public string name()
        {
            return "";
        }

        /// <summary>
        /// The width of the bitmap
        /// </summary>
        /// <returns></returns>
        public uint width()
        {
            return (uint)color.Width;
        }

        /// <summary>
        /// The height of the bitmap
        /// </summary>
        /// <returns></returns>
        public uint height()
        {
            return (uint)color.Height;
        }
    } // End class
} // End namespace
