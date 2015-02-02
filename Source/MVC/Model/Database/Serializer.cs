using System;
using System.IO;
using System.Collections.Generic;

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
        /// Serializes a variable of type dictionary
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Serialize(MemoryStream ms, Dictionary<int, Record> d)
        {
            Serialize(ms, d.Count);
            foreach (KeyValuePair<int, Record> i in d)
            {
                Serialize(ms, i.Key);
                Serialize(ms, i.Value);
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
        public static void Serialize(MemoryStream ms, System.Drawing.Image d)
        {
            d.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        #endregion

        #region Unserializers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Unserialize(MemoryStream ms, ref ISerializable d)
        {
            d.Unserialize(ms);
        }

        /// <summary>
        /// Unserializes a variable of type bool
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Unserialize(MemoryStream ms, ref bool d)
        {
            byte[] buffer = new byte[sizeof(bool)];
            ms.Read(buffer, 0, sizeof(bool));
            d = BitConverter.ToBoolean(buffer, 0);
        }

        /// <summary>
        /// Unserializes a variable of type int
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Unserialize(MemoryStream ms, ref int d)
        {
            byte[] buffer = new byte[sizeof(int)];
            ms.Read(buffer, 0, sizeof(int));
            d = BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Unserializes a variable of type long
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Unserialize(MemoryStream ms, ref long d)
        {
            byte[] buffer = new byte[sizeof(long)];
            ms.Read(buffer, 0, sizeof(long));
            d = BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// Unserializes a variable of type double
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Unserialize(MemoryStream ms, ref double d)
        {
            byte[] buffer = new byte[sizeof(double)];
            ms.Read(buffer, 0, sizeof(double));
            d = BitConverter.ToDouble(buffer, 0);
        }

        /// <summary>
        /// Unserializes a variable of type float
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Unserialize(MemoryStream ms, ref float d)
        {
            byte[] buffer = new byte[sizeof(float)];
            ms.Read(buffer, 0, sizeof(float));
            d = BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Unserializes a variable of type char
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Unserialize(MemoryStream ms, ref char d)
        {
            byte[] buffer = new byte[sizeof(char)];
            ms.Read(buffer, 0, sizeof(char));
            d = BitConverter.ToChar(buffer, 0);
        }

        /// <summary>
        /// Unserializes a variable of type string
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Unserialize(MemoryStream ms, ref string d)
        {
            int length = 0;
            Unserialize(ms, ref length);
            d = "";
            for (int i = 0; i < length; i++)
            {
                Char ch = new Char();
                Unserialize(ms, ref ch);
                d += ch;
            }            
        }

        /// <summary>
        /// Unserializes a variable of type DateTime
        /// </summary>
        /// <param name="?"></param>
        public static void Unserialize(MemoryStream ms, ref DateTime d)
        {
            long imm = 0;
            Unserialize(ms, ref imm);
            d = new DateTime(imm);
        }

        /// <summary>
        /// Unserializes a variable of type image
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="d"></param>
        public static void Unserialize(MemoryStream ms, ref System.Drawing.Image d)
        {
            d = System.Drawing.Image.FromStream(ms);
        }

        #endregion
    } // End class
} // End namespace
