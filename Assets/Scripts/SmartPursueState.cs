namespace DefaultNamespace
{
    public class SmartPursueState : BotState
    {
        protected override void StateUpdate()
        {
            if (TargetSeesMe)
            {
                ProceedToNextStage(new SmarthideState(this));
            }

            SmartPursue();
        }

        void SmartPursue()
        {
            //TODO: Test

            if (target.GetComponent<Drive>().currentSpeed < 0.1f)
            {
                Seek(target.position);
            }
            else if (TargetIsBehind)
            {
                Seek(target.position);
            }
            else
            {
                Seek(PredictedIntersect);
            }
        }

        public SmartPursueState(BotState previousState) : base(previousState)
        {
            stateName = BotStates.Pursue;
        }
    }
}