
namespace YoloTrack.MVC.Model.StateMachine
{
    abstract class BaseImpl<U>
    {
        public BaseArg Result;

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