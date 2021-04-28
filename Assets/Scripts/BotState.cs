using UnityEngine;
using UnityEngine.AI;
using UnityToolbox.Tools;

namespace DefaultNamespace
{
    public class BotState : FsmState<BotState, BotStates>
    {
        protected readonly Transform target;
        readonly NavMeshAgent agent;
        protected readonly Transform transform;
        protected const float TargetFieldOfView = 60;
        private const float InRangeDistance = 20;
        protected Vector3 ToTarget => target.position - transform.position;
        protected bool TargetIsBehind => Vector3.Dot(transform.forward, ToTarget) < 0;
        float LookAheadDistance => ToTarget.magnitude / (agent.speed + target.GetComponent<Drive>().currentSpeed);
        protected Vector3 PredictedIntersect => target.position + LookAheadDistance * target.forward;
        protected bool OutOfRange => ToTarget.sqrMagnitude > InRangeDistance * InRangeDistance;
        protected bool TargetSeesMe => ClearLineOfSightToTarget() && WithinTargetFOV();

        protected bool ClearLineOfSightToTarget()
        {
            return Physics.Raycast(this.transform.position, ToTarget, out RaycastHit raycastHit) &&
                   raycastHit.transform.CompareTag("Cop");
        }

        protected bool WithinTargetFOV()
        {
            Vector3 toAgent = -ToTarget;
            float viewAngle = Vector3.Angle(toAgent, target.forward);
            return (viewAngle < TargetFieldOfView);
        }


        protected void Seek(Vector3 targetPosition)
        {
            //TODO: Test

            agent.SetDestination(targetPosition);
        }

        void Flee(Vector3 scary)
        {
            //TODO: Test

            Vector3 location = transform.position;
            Vector3 awayFrom = location - scary;
            Vector3 targetLocation = location + awayFrom;

            agent.SetDestination(targetLocation);
        }

        public BotState(Transform target, NavMeshAgent agent, Transform transform)
        {
            this.target = target;
            this.agent = agent;
            this.transform = transform;
        }

        public BotState(BotState previousState)
        {
            this.target = previousState.target;
            this.agent = previousState.agent;
            this.transform = previousState.transform;
        }
    }
}