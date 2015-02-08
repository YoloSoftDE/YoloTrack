using System;
using System.Collections.Generic;
using Cognitec.FRsdk;
using Identification = Cognitec.FRsdk.Identification;
using Enrollment = Cognitec.FRsdk.Enrollment;

namespace YoloTrack.MVC.Model.IdentificationData
{
    /// <summary>
    /// Capsulation class of required facevacs resources
    /// </summary>
    public class Model
    {
        /// <summary>
        /// Holder for the configuration of the library. Required to run.
        /// </summary>
        public Cognitec.FRsdk.Configuration Configuration { get; private set; }

        /// <summary>
        /// IdentificationRecordBuilder
        /// </summary>
        public FIRBuilder IdentificationRecordBuilder { get; private set; }

        /// <summary>
        /// Population
        /// </summary>
        public Population Population { get; private set; }

        /// <summary>
        /// IdentificationProcessor
        /// </summary>
        private Identification.Processor m_identification_processor;

        /// <summary>
        /// EnrollmentProcessor
        /// </summary>
        private Enrollment.Processor m_enrollment_processor;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Model(string ConfigurationFileName)
        {
            // Load configuration
            Configuration = new Cognitec.FRsdk.Configuration(ConfigurationFileName);

            // Create everything else by configuration
            Population = new Population(Configuration);
            IdentificationRecordBuilder = new FIRBuilder(Configuration);
            m_identification_processor = new Identification.Processor(Configuration, Population);
            m_enrollment_processor = new Enrollment.Processor(Configuration);
        }

        /// <summary>
        /// Try to find a list of matches for the given samples matching against the database.
        /// On error, return null.
        /// </summary>
        /// <param name="Samples"></param>
        /// <param name="MatchLimit"></param>
        /// <returns></returns>
        public IdentificationFeedback TryIdentify(List<Sample> Samples, uint MatchLimit)
        {
            try
            {
                IdentificationFeedback feedback = Identify(Samples, MatchLimit);
                return feedback;
            }
            catch (IdentificationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Try to find a list of matches for the given samples matching against the database.
        /// Throw on failure.
        /// </summary>
        /// <param name="Samples"></param>
        /// <param name="MatchLimit"></param>
        /// <returns></returns>
        public IdentificationFeedback Identify(List<Sample> Samples, uint MatchLimit)
        {
            IdentificationFeedback feedback = new IdentificationFeedback();

            Score Threshold = (new ScoreMappings(Configuration)).requestFAR(0.001f);

            m_identification_processor.process(
                Samples.ToArray(),
                Threshold,
                feedback,
                MatchLimit);

            return feedback;
        }

        /// <summary>
        /// Try to enroll a given set of samples and return null on failure.
        /// </summary>
        /// <param name="Samples"></param>
        /// <returns></returns>
        public EnrollmentFeedback TryEnroll(List<Sample> Samples)
        {
            try
            {
                EnrollmentFeedback feedback = Enroll(Samples);
                return feedback;
            }
            catch (IdentificationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Try to enroll a given set of samples. Throw on failure.
        /// </summary>
        /// <param name="Samples"></param>
        /// <returns></returns>
        public EnrollmentFeedback Enroll(List<Sample> Samples)
        {
            EnrollmentFeedback feedback = new EnrollmentFeedback();
            m_enrollment_processor.process(Samples.ToArray(), feedback);
            return feedback;
        }
        
        /// <summary>
        /// Merges two or more FIRs into one single FIR
        /// </summary>
        /// <param name="IdentificationRecordList"></param>
        /// <returns></returns>
        public FIR Merge(FIR[] IdentificationRecordList)
        {
            if (IdentificationRecordList.Length < 2)
            {
                throw new ArgumentException("You cannot merge less than two FIRs");
            }

            FIR result = m_enrollment_processor.merge(IdentificationRecordList[0], IdentificationRecordList[1]);
            for (int i = 2; i < IdentificationRecordList.Length; i++)
            {
                result = m_enrollment_processor.merge(result, IdentificationRecordList[i]);
            }
            return result;
        }
    } // End class
} // End namespace
