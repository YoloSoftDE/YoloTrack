using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace YoloTrack.MVC.Model.Database
{
    /// <summary>
    /// Provides free serialization methods for those (mostly base) types that cannot
    /// be derived or should not be derived from.
    /// </summary>
    class Serializer
    {
        #region Serializers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Serialize(MemoryStream ms, ISerializable d)
        {
            d.Serialize(ms);
        }

        /// <summary>
        /// Serializes a variable of type bool
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Serialize(MemoryStream ms, bool d)
        {
            ms.Write(BitConverter.GetBytes(d), 0, sizeof(bool));
        }

        /// <summary>
        /// Serializes a variable of type int
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Serialize(MemoryStream ms, int d)
        {
            ms.Write(BitConverter.GetBytes(d), 0, sizeof(int));
        }

        /// <summary>
        /// Serializes a variable of type long
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Serialize(MemoryStream ms, long d)
        {
            ms.Write(BitConverter.GetBytes(d), 0, sizeof(long));
        }

        /// <summary>
        /// Serializes a variable of type double
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Serialize(MemoryStream ms, double d)
        {
            ms.Write(BitConverter.GetBytes(d), 0, sizeof(double));
        }

        /// <summary>
        /// Serializes a variable of type float
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Serialize(MemoryStream ms, float d)
        {
            ms.Write(BitConverter.GetBytes(d), 0, sizeof(float));
        }

        /// <summary>
        /// Serializes a variable of type char
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Serialize(MemoryStream ms, char d)
        {
            ms.Write(BitConverter.GetBytes(d), 0, sizeof(char));
        }

        /// <summary>
        /// Serializes a variable of type string
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Serialize(MemoryStream ms, string d)
        {
            Serialize(ms, d.Length);
            foreach (char ch in d.ToCharArray())
            {
                Serialize(ms, ch);
            }
        }

        /// <summary>
        /// Serializes a variable of type datetime
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Serialize(MemoryStream ms, DateTime d)
        {
            Serialize(ms, d.ToBinary());
        }

        /// <summary>
        /// Serializes a variable of type image
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Serialize(MemoryStream ms, System.Drawing.Bitmap Bitmap)
        {
            Serialize(ms, Bitmap.Height);
            Serialize(ms, Bitmap.Width);

            var data_ptr = Bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, Bitmap.Width, Bitmap.Height), 
                System.Drawing.Imaging.ImageLockMode.ReadWrite, 
                System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            int length = data_ptr.Stride * data_ptr.Height;
            Serialize(ms, length);

            byte[] bytes = new byte[length];

            Marshal.Copy(data_ptr.Scan0, bytes, 0, length);
            Bitmap.UnlockBits(data_ptr);

            for (int i = 0; i < length; i++)
            {
                ms.WriteByte(bytes[i]);
            }
        }

        #endregion

        #region Unserializers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static bool UnserializeBool(MemoryStream ms)
        {
            byte[] buffer = new byte[sizeof(bool)];
            ms.Read(buffer, 0, sizeof(bool));
            return BitConverter.ToBoolean(buffer, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int UnserializeInt(MemoryStream ms)
        {
            byte[] buffer = new byte[sizeof(int)];
            ms.Read(buffer, 0, sizeof(int));
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Unserializes a variable of type long
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static long UnserializeLong(MemoryStream ms)
        {
            byte[] buffer = new byte[sizeof(long)];
            ms.Read(buffer, 0, sizeof(long));
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// Unserializes a variable of type double
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static double UnserializeDouble(MemoryStream ms)
        {
            byte[] buffer = new byte[sizeof(double)];
            ms.Read(buffer, 0, sizeof(double));
            return BitConverter.ToDouble(buffer, 0);
        }

        /// <summary>
        /// Unserializes a variable of type float
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static float UnserializeFloat(MemoryStream ms)
        {
            byte[] buffer = new byte[sizeof(float)];
            ms.Read(buffer, 0, sizeof(float));
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Unserializes a variable of type char
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static char UnserializeChar(MemoryStream ms)
        {
            byte[] buffer = new byte[sizeof(char)];
            ms.Read(buffer, 0, sizeof(char));
            return BitConverter.ToChar(buffer, 0);
        }

        /// <summary>
        /// Unserializes a variable of type string
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static string UnserializeString(MemoryStream ms)
        {
            int length = UnserializeInt(ms);
            string str = "";

            for (int i = 0; i < length; i++)
            {
                str += UnserializeChar(ms);
            }

            return str;
        }

        /// <summary>
        /// Unserializes a variable of type DateTime
        /// </summary>
        /// <param name="?"></param>
        public static DateTime UnserializeDateTime(MemoryStream ms)
        {
            long ts = UnserializeLong(ms);
            return DateTime.FromBinary(ts);
        }

        /// <summary>
        /// Unserializes a variable of type image
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static System.Drawing.Bitmap UnserializeImage(MemoryStream ms)
        {
            int height = UnserializeInt(ms);
            int width = UnserializeInt(ms);
            int length = UnserializeInt(ms);
            
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);

            var data_ptr = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++) 
            {
                bytes[i] = (byte)ms.ReadByte();
            }

            Marshal.Copy(bytes, 0, data_ptr.Scan0, length);

            bitmap.UnlockBits(data_ptr);
            return bitmap;
        }
        #endregion
    } // End class
} // End namespace
