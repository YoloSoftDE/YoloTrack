using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Text;

using Cognitec.FRsdk;
using Eyes = Cognitec.FRsdk.Eyes;
using Enrollment = Cognitec.FRsdk.Enrollment;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class LearnImpl : BaseImpl<Arg.LearnArg>
    {
        protected class EnrollmentException : System.Exception
        {
            public EnrollmentException(string Message)
                : base(Message) { }
        }

		protected class EnrollmentFeedback: Enrollment.Feedback
		{
			public FIR fir;
			public float sample_quality = 0;
			
            public void eyesNotFound()
            {
                throw new EnrollmentException("Eyes not found");
            }

            public void sampleQualityTooLow()
            {
                throw new EnrollmentException("Quality to low");
            }

            public void failure()
            {
                throw new EnrollmentException("Error: Out of magic dust");
            }

            public void sampleQuality(float f)
            {
                this.sample_quality = f;
            }
			
			public void start() { }
			
			public void processingImage( Cognitec.FRsdk.Image img) { }
			
			public void eyesFound( Eyes.Location eyes) { }
			
			
			public void success (FIR nfir)
			{
				Console.WriteLine("successful enrollment, \tFIR[id, size] = " +
					"[\"{0}\", \"{1}\"]", nfir.version (), nfir.size ());
				this.fir = nfir;
			}
			
			public void end() { }
			
			/* Probably not needed, keep it for the lulz */
			public byte[] getFIRbytes ()
			{
				MemoryStream ms = new MemoryStream ();
				fir.serialize (ms);
				return ms.ToArray ();
			}
		}
		
        public override void Run (Arg.LearnArg arg)
		{
			/* Prepare Samples for identification */
			

			
			/* Create FIR */
    
			/* Stupid class */
			EnrollmentFeedback feedback = new EnrollmentFeedback ();

            try
            {
                /* Process samples */
                Model.EnrollmentProcessor.process(arg.Samples.ToArray(), feedback);
                /* !Enrollment done */
                /* Create Person from Data */
                Storage.Person person = new Storage.Person();
                person.IR = new Storage.IdentificationRecord();
                person.Id = Guid.NewGuid();
                person.IsPresent = true;
                person.IsTarget = false;
                person.Learned = DateTime.Now;
                person.Picture = arg.Faces[0];
                
                person.IR.Value = feedback.fir;
                person.RuntimeInfo = Model.RuntimeDatabase[arg.SkeletonId];

                /* Add Person to Database ? */
                this.Model.MainDatabase.Add(person);
			
            }
            catch (EnrollmentException)
            {
                // Magic
                Console.WriteLine("[Learn] Learn failed");
            }
            finally
            {
                /* Set Result ? */
                Result = new Arg.WaitForBodyArg();
            }
			
			
        }
    }
}
