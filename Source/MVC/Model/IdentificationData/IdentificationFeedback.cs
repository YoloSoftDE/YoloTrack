using System;
using Cognitec.FRsdk;
using Identification = Cognitec.FRsdk.Identification;
using Eyes = Cognitec.FRsdk.Eyes;

namespace YoloTrack.MVC.Model.IdentificationData
{
    /// <summary>
    /// Thrown on error during identification process
    /// </summary>
    public class IdentificationException : System.Exception
    {
        /// <summary>
        /// Constructor containing message text.
        /// </summary>
        /// <param name="message"></param>
        public IdentificationException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Feedback callbacks
    /// </summary>
    public class IdentificationFeedback : Identification.Feedback
    {
        /// <summary>
        /// Holder of the list of probably matching results
        /// </summary>
        public Match[] Match { get; private set; }

        /// <summary>
        /// Currently unused: holder of the sample quality value
        /// </summary>
        public float SampleQuality { get; private set; }

        /// <summary>
        /// Callback on processing failure
        /// </summary>
        public void eyesNotFound()
        {
            throw new IdentificationException("Eyes could not be found on image");
        }

        /// <summary>
        /// Callback on processing failure
        /// </summary>
        public void sampleQualityTooLow()
        {
            throw new IdentificationException("The quality of the samples is too low");
        }

        /// <summary>
        /// Callback on processing failure
        /// </summary>
        public void failure()
        {
            throw new IdentificationException("General failure");
        }

        /// <summary>
        /// Setter for the sample quality
        /// </summary>
        /// <param name="f"></param>
        public void sampleQuality(float f)
        {
            this.SampleQuality = f;
        }

        /// <summary>
        /// Setter for the result
        /// </summary>
        /// <param name="matches"></param>
        public void matches(Match[] matches)
        {
            this.Match = matches;
        }

        /// <summary>
        /// ...
        /// </summary>
        public void end()
        {
        }

        /// <summary>
        /// Yay
        /// </summary>
        /// <param name="eyeLoc"></param>
        public void eyesFound(Eyes.Location eyeLoc)
        {
        }

        /// <summary>
        /// Callback during process
        /// </summary>
        /// <param name="img"></param>
        public void processingImage(Cognitec.FRsdk.Image img)
        {
        }

        /// <summary>
        /// ...
        /// </summary>
        public void start()
        {
        }
    } // End class
} // End namespace
