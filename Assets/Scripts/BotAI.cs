using System;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class BotAI : MonoBehaviour
    {
        private NavMeshAgent agent;
        public Transform target;
        BotState state;
        public BotStates stateName;
        private bool isOnCooldown = false;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            state = new WanderState(target, agent, transform);
        }

        private void Update()
        {
            if (isOnCooldown)
            {
                return;
            }

            state = state.Process();
            stateName = state.stateName;
            if (stateName == BotStates.Wander)
            {
                StartCooldown();
            }
        }

        void StartCooldown()
        {
            isOnCooldown = true;
            Invoke(nameof(ResetCooldown), 5);
        }

        void ResetCooldown()
        {
            isOnCooldown = false;
        }
    }
}