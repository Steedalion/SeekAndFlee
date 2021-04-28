namespace DefaultNamespace
{
    public class PursueState:BotState
    {
        protected override void StateUpdate()
        {
            if (TargetSeesMe)
            {
                ProceedToNextStage(new HideState(this));
            }
            
            Seek(PredictedIntersect);
        }

        public PursueState(BotState previousState) : base(previousState)
        {
            stateName = BotStates.Pursue;
        }
    }
}