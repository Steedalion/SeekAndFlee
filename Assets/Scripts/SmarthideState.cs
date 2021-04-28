using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class SmarthideState : BotState
    {
        private bool isOnCoolDown = false;

        public SmarthideState(BotState previousState) : base(previousState)
        {
            stateName = BotStates.Hide;
        }

        protected override void Enter()
        {
            base.Enter();
            StartCooldown();
        }

        protected override void StateUpdate()
        {
            if (!TargetSeesMe)
            {
                ProceedToNextStage(new WanderState(this));
            }

            if (!isOnCoolDown)
            {
                HideBehindObject();
            }
        }


        void StartCooldown()
        {
            isOnCoolDown = true;
            //TODO: Cannot delay cooldown.

            // Invoke(nameof(ResetCooldown), 5);
            ResetCooldown();
        }

        void ResetCooldown()
        {
            isOnCoolDown = false;
        }


        void HideBehindObject()
        {
            const float hideDistanceBehindObstacle = 4;
            GameObject chosenObstacle = GetNearestObstacle();
            Vector3 nearestObstaclePosition = GetNearestObstacle().transform.position;
            Vector3 fromTargetToObstacle = nearestObstaclePosition - target.transform.position;
            Vector3 intersect = BehindObstacleIntersect(chosenObstacle);
            Seek(intersect + fromTargetToObstacle.normalized * hideDistanceBehindObstacle);
        }

        private Vector3 BehindObstacleIntersect(GameObject chosenObstacle)
        {
            const float distanceBehindObstacle = 20;
            const float backRayDistance = 100;

            Vector3 position = chosenObstacle.transform.position;
            Vector3 targetToObstacle = position - target.transform.position;
            Collider obstacleCollider = chosenObstacle.GetComponent<Collider>();

            Vector3 positionBehindObstacle =
                position + targetToObstacle.normalized * distanceBehindObstacle;
            Ray backfire = new Ray(positionBehindObstacle, -targetToObstacle);
            obstacleCollider.Raycast(backfire, out RaycastHit info, backRayDistance);
            Vector3 intersectFromBack = info.point;
            return intersectFromBack;
        }

        GameObject GetNearestObstacle()
        {
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