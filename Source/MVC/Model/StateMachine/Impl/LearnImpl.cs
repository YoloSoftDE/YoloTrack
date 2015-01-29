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
    class LearnImpl : BaseImpl<Arg.WaitForBodyArg, Arg.LearnArg>
    {
		class EnrollmentFeedback: Enrollment.Feedback
		{
			public FIR fir;
			public float sample_quality = 0;
			
            public void eyesNotFound()
            {
                throw new System.Exception("Eyes not found");
            }

            public void sampleQualityTooLow()
            {
                throw new System.Exception("Quality to low");
            }

            public void failure()
            {
                throw new System.Exception("Error: Out of magic dust");
            }

            public void sampleQuality(float f)
            {
                this.sample_quality = f;
            }
			
			public void start() { }
			
			public void processingImage( Image img) { }
			
			public void eyesFound( Eyes.Location eyes) { }
			
			
			public void success (FIR nfir)
			{
				Console.WriteLine
				("successful enrollment, \tFIR[ filename, id, size] = " +
					"[\"{0}\", \"{1}\", {2}]", firFilename, fir.version (), fir.size ());
				
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
			List<Sample> samples = new List<Sample> ();

			foreach (Bitmap fratze in arg.Faces) {
				MemoryStream ms = new MemoryStream ();

				fratze.Save (ms, System.Drawing.Imaging.ImageFormat.Bmp);

				samples.Add (new Sample (Bmp.load (ms)));
			}
			
			/* Create FIR */
    
			/* Stupid class */
			EnrollmentFeedback feedback = new EnrollmentFeedback ();
			
			/* Process samples */
			Model.EnrollmentProcessor.process (samples, feedback);
			/* !Enrollment done */
			
			
			/* Create Person from Data */
			Storage.Person person = new Storage.Person ();
			person.IR.Value = feedback.fir;
			
			
			/* Add Person to Database ? */
			this.Model.MainDatabase.Add (person);
			
			
			/* Set Result ? */
			Result = new YoloTrack.MVC.Model.StateMachine.Arg.WaitForBodyArg ();
        }
    }
}
