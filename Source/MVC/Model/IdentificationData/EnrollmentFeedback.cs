using System;
using Cognitec.FRsdk;
using Eyes = Cognitec.FRsdk.Eyes;
using Enrollment = Cognitec.FRsdk.Enrollment;
using System.IO;

namespace YoloTrack.MVC.Model.IdentificationData
{
    /// <summary>
    /// Exception that is thrown on bad enrollment
    /// </summary>
    public class EnrollmentException : System.Exception
    {
        /// <summary>
        /// Constructor with message text.
        /// </summary>
        /// <param name="Message"></param>
        public EnrollmentException(string Message)
            : base(Message)
        {
        }
    }

    /// <summary>
    /// Feedback provieded on enrollment
    /// </summary>
    public class EnrollmentFeedback : Enrollment.Feedback
    {
        /// <summary>
        /// 
        /// </summary>
        public FIR IdentificationRecord { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public float SampleQuality { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void eyesNotFound()
        {
            throw new EnrollmentException("Eyes not found");
        }

        /// <summary>
        /// 
        /// </summary>
        public void sampleQualityTooLow()
        {
            throw new EnrollmentException("Quality to low");
        }

        /// <summary>
        /// 
        /// </summary>
        public void failure()
        {
            throw new EnrollmentException("Error: Out of magic dust");
        }

        /// <summary>
        /// 
        /// </summary>
        public void sampleQuality(float f)
        {
            this.SampleQuality = f;
        }

        /// <summary>
        /// 
        /// </summary>
        public void start()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void processingImage(Cognitec.FRsdk.Image img)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void eyesFound(Eyes.Location eyes)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void success(FIR nfir)
        {
            Console.WriteLine("successful enrollment, \tFIR[id, size] = " +
                "[\"{0}\", \"{1}\"]", nfir.version(), nfir.size());
            this.IdentificationRecord = nfir;
        }

        /// <summary>
        /// 
        /// </summary>
        public void end()
        {
        }

        /// <summary>
        /// Probably not needed, keep it for the lulz
        /// </summary>
        public byte[] getFIRbytes()
        {
            MemoryStream ms = new MemoryStream();
            IdentificationRecord.serialize(ms);
            return ms.ToArray();
        }
    } // End class
} // End namespace
