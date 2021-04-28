using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class WanderState : BotState
    {
        public float wanderRadius = 10;
        public float wanderDistance = 10;
        public float wanderJitter = 5;
        private Vector3 wanderTarget = Vector3.zero;

        public WanderState(Transform target, NavMeshAgent agent, Transform transform) : base(target, agent, transform)
        {
            stateName = BotStates.Wander;
        }

        public WanderState(HideState hideState) : base(hideState)
        {
            stateName = BotStates.Wander;
        }

        protected override void StateUpdate()
        {
            if (!OutOfRange) ProceedToNextStage(new PursueState(this));
                Vector3 wanderTarget = new Vector3(Random.Range(-1f, 1f) * wanderJitter, 0,
                    Random.Range(-1f, 1f) * wanderJitter);
                wanderTarget = wanderTarget.normalized * wanderRadius;
                Vector3 wanderDestination = wanderTarget + Vector3.forward * wanderDistance;
                Vector3 globalDestination = transform.TransformVector(wanderDestination);
                Seek(globalDestination);
        }
    }
}