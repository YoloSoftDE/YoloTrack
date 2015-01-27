
namespace YoloTrack.MVC.Model.StateMachine
{
    interface IState
    {
        IState Transist();

        State.States State
        {
            get;
        }
    }
}
