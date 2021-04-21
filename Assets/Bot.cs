using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;
    public BotState state;
    
    private Vector3 toTarget => target.position - transform.position;
    private bool targetIsBehind => Vector3.Dot(transform.forward, toTarget) < 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Behaviour();
    }

    void Seek(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
        state = BotState.Seek;
    }

    void Behaviour()
    {
        if (target.GetComponent<Drive>().currentSpeed < 0.1f)
        {
            Seek(target.position);
        }
        else if (targetIsBehind)
        {
            Seek(target.position);
        }
        else
        {
            Pursue();
        }
        //pursue
        //if target stopped moving, seek
        //if target and self are heading the same direction, turn around and seek.
    }

    void Flee(Vector3 scary)
    {
        Vector3 location = transform.position;
        Vector3 awayFrom = location - scary;
        Vector3 targetLocation = location + awayFrom;

        agent.SetDestination(targetLocation);
        state = BotState.Flee;
    }

    void Pursue()
    {
        Vector3 toTarget = target.position - transform.position;
        float lookAheadDistance = toTarget.magnitude / (agent.speed + target.GetComponent<Drive>().currentSpeed);
        Vector3 goal = target.position + lookAheadDistance * target.forward;
        Seek(goal);
        state = BotState.Pursue;
    }
}

public enum BotState
{
    Seek,
    Pursue,
    Flee
}

