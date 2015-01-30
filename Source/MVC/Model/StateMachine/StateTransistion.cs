namespace YoloTrack.MVC.Model.StateMachine
{
    class Exception : System.Exception 
    {
        public Exception(string Message)
            : base(Message) { }
    }

    abstract class StateTransistion
    {
        protected abstract StateTransistion Transist();
        public StateTransistion Next()
        {
            StateTransistion next_state = Transist();
            if (next_state == null)
                throw new Exception("Problem dude.");

            return next_state;
        }
    }
}
