
namespace YoloTrack.MVC.Model.StateMachine
{
    abstract class BaseImpl<U>
    {
        private BaseArg result;

        public BaseArg Result
        {
            get 
            { 
                return (BaseArg)(result.Clone()); 
            }
            set 
            { 
                result = value; 
            }
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