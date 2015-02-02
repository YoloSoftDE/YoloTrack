using System.IO;
namespace YoloTrack.MVC.Model.Database
{
    /// <summary>
    /// Serialization interface for various objects that should be writable to disk (database)
    /// </summary>
    interface ISerializable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        void Serialize(MemoryStream ms);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        void Unserialize(MemoryStream ms);
    } // End class
} // End namespace
