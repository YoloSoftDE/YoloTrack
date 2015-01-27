
namespace YoloTrack.MVC.Model.StateMachine
{
    abstract class BaseImpl<T, U>
    {
        private T m_result;

        public T Result
        {
            get { return m_result; }
            protected set { m_result = value; }
        }

        protected BaseImpl()
        {
        }

        public abstract void Run(U arg);

        public TrackingModel Model
        {
            get { return TrackingModel.Instance(); }
        }
    }
}
;