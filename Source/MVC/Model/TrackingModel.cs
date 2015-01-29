using System.Threading;
using Microsoft.Kinect;
using Cognitec.FRsdk;
using Identification = Cognitec.FRsdk.Identification;
using Enrollment = Cognitec.FRsdk.Enrollment;

namespace YoloTrack.MVC.Model
{
    public delegate void StateChangeHandler(StateMachine.State.States next_state);

    public enum Status
    {
        RUNNING, SENSOR_UNAVAILABLE, STOPPED, TRACKING, IDLE
    }

    public class TrackingModel
    {
        private static TrackingModel instance = null;

        public event Storage.RuntimeInfoChangeHandler OnRuntimeInfoChange;
        public event StateChangeHandler OnStateChange;

        private Storage.RuntimeDatabase m_runtime_database = new Storage.RuntimeDatabase();
        private Storage.MainDatabase m_main_database = new Storage.MainDatabase();

        private StateMachine.IState m_state;
        private bool m_stop_machine = false;
        private Thread m_th = null;

        private KinectSensor m_sensor = null;
        private byte[] m_buffer;
        private bool sync_frame = true;
        private Skeleton[] skeletons = new Skeleton[6];

        private Configuration m_conf = new Configuration("frsdk.cfg");
        private FIRBuilder m_fir_builder = null;
        private Population m_population = null;
        private Identification.Processor m_proc_ident = null;
		private Enrollment.Processor m_proc_enroll = null;
		private Score m_score;
        
        public static TrackingModel Instance()
        {
            //lock(this) {
                if (instance == null)
                    instance = new TrackingModel();

                return instance;
            //}
        }

        public TrackingModel()
        {
            m_population = new Population(m_conf);
            m_fir_builder = new FIRBuilder(m_conf);

            m_runtime_database.OnRuntimeInfoChange += new Storage.RuntimeInfoChangeHandler(m_runtime_database_OnRuntimeInfoChange);

            int MaxFIR = int.Parse(m_conf.getValue("FRSDK.LicenseSettings.MaxFIRInstances"));
            ConfigModel.Instance().conf.MaxDatabaseEntries = MaxFIR;
            ConfigModel.Instance().Save();            
        }

        void m_runtime_database_OnRuntimeInfoChange(Storage.RuntimeInfo info)
        {
            OnRuntimeInfoChange(info);
        }

        void StartSensor()
        {
            foreach (KinectSensor sensor in KinectSensor.KinectSensors)
                if (sensor.Status == KinectStatus.Connected)
                {
                    m_sensor = sensor;
                    break;
                }

            if (m_sensor == null)
                return;

            while (m_sensor.Status != KinectStatus.Connected)
                Thread.Sleep(200);

            m_sensor.Start();

            m_sensor.ColorStream.Enable(ColorImageFormat.RgbResolution1280x960Fps12);
            m_sensor.SkeletonStream.Enable();

            m_buffer = new byte[Kinect.ColorStream.FramePixelDataLength];
            m_sensor.ColorFrameReady += new System.EventHandler<ColorImageFrameReadyEventArgs>(m_sensor_ColorFrameReady);
            m_sensor.SkeletonFrameReady += m_sensor_SkeletonFrameReady;
        }

        void m_sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())     // Open the Skeleton frame
            {
                if (skeletonFrame != null && this.skeletonData != null)     // check that a frame is available
                {
                    skeletonFrame.CopySkeletonDataTo(this.skeletonData);    // get the skeletal information in this frame
                    UpdateHeads();
                }
            }
        }

        private void UpdateHeads()
        {
            CoordinateMapper mapper = new CoordinateMapper(Kinect);
            ColorImagePoint head;

            foreach (Skeleton skeleton in skeletonData)
            {
                head = mapper.MapSkeletonPointToColorPoint(skeleton.Joints[JointType.Head].Position, ColorImageFormat.RgbResolution1280x960Fps12);
                System.Drawing.Rectangle head_rect = new System.Drawing.Rectangle()
                {
                    X = head.X - 100,
                    Y = head.Y - 100,
                    Width = 200,
                    Height = 200
                };
                if (RuntimeDatabase.Has(skeleton.TrackingId))
                {
                    Storage.RuntimeInfo RTInfo = RuntimeDatabase.At(skeleton.TrackingId);
                    OnRuntimeInfoChange(RTInfo);
                }
            }
        }

        void m_sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame frame = e.OpenColorImageFrame())
            {
                if (frame == null)
                    return;

                if (sync_frame == true)
                    frame.CopyPixelDataTo(m_buffer);
            }
        }
            
        public Storage.MainDatabase MainDatabase
        {
            get { return m_main_database; }
        }

        public Storage.RuntimeDatabase RuntimeDatabase
        {
            get { return m_runtime_database; }
        }

        public KinectSensor Kinect
        {
            get { return m_sensor; }
        }

        //*****************************
        public byte[] rawImageData          // hinzugefügt
        {
            get { return m_buffer; }        
        }

        public bool sync_ColorFrame         // hinzugefügt
        {
            set { sync_frame = value; }     
            get { return sync_frame;  }
        }

        public Skeleton[] skeletonData
        {
            set { skeletons = value; }
            get { return skeletons;  }
        }
        //*****************************

        public bool Running()
        {
            return m_sensor != null;
        }

        public void Start()
        {
            StartSensor();
            if (!Running())
                return;

            m_stop_machine = false;
            m_th = new Thread(new ThreadStart(RunMachine));
            m_th.Start();
        }

        private void RunMachine()
        {
            m_state = new StateMachine.State.WaitForBodyState(new StateMachine.Arg.WaitForBodyArg());

            while (!m_stop_machine)
            {
                m_state = m_state.Transist();
                if (OnStateChange != null)
                {
                    OnStateChange(m_state.State);
                }
            }
        }

        public FIRBuilder FIRBuilder
        {
            get { return m_fir_builder; }
        }

        public Population Population
        {
            get { return m_population; }
        }

        private void InitCognitec ()
		{

			foreach (Storage.Person person in m_main_database.People)
				m_population.append (
                        //m_fir_builder.build(
                            person.IR.Value
                        /*)*/,
                        person.Name
				);

			/* Needed once, score is much useful */
			ScoreMappings sm = new ScoreMappings (m_conf);
			Score score = sm.requestFAR (0.001f);

			/* Identification Processor
             * Move to Runtime Storage */
			m_proc_ident = new Identification.Processor (m_conf, m_population);
			m_proc_enroll = new Enrollment.Processor (m_conf);

        }

        public Identification.Processor IdentificationProcessor
        {
            get { return m_proc_ident; }
        }
		
		public Enrollment.Processor EnrollmentProcessor
		{
			get { return m_proc_enroll; }
		}

        public Score FARScore
        {
            get { return m_score; }
        }
    }
}
