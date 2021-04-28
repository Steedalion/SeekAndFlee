using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class HideState : BotState
    {
        public HideState(BotState previousState) : base(previousState)
        {
            stateName = BotStates.Hide;
        }

        protected override void StateUpdate()
        {
            if (!TargetSeesMe)
            {
                ProceedToNextStage(new WanderState(this));
            }
            Hide();
        }

        void Hide()
        {
            //TODO: Test

            const float distanceBehindObstacle = 20;

            Vector3 nearestObstaclePosition = GetNearestObstacle().transform.position;
            Vector3 fromTargetToObstacle = nearestObstaclePosition - target.transform.position;
            Vector3 hidingSpot = nearestObstaclePosition + fromTargetToObstacle.normalized * distanceBehindObstacle;
            Seek(hidingSpot);
        }

        GameObject GetNearestObstacle()
        {
            //TODO: Duplicated in smartHide

            float bestDistance = Single.PositiveInfinity;
            int bestIndex = 0;
            for (int i = 0; i < World.HidingSpots.Length; i++)
            {
                Vector3 hidePos = World.HidingSpots[i].transform.position;
                if (!(Vector3.Distance(transform.position, hidePos) < bestDistance)) continue;
                bestDistance = Vector3.Distance(transform.position, hidePos);
                bestIndex = i;
            }

            return World.HidingSpots[bestIndex];
        }
    }
}