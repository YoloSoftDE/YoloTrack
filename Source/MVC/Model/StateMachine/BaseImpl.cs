
namespace YoloTrack.MVC.Model.StateMachine
{
    abstract class BaseImpl<T, U>
    {
        public T Result;

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