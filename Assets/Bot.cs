using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;
    public BotState state;

    private Vector3 ToTarget => target.position - transform.position;
    private bool TargetIsBehind => Vector3.Dot(transform.forward, ToTarget) < 0;
    float lookAheadDistance => ToTarget.magnitude / (agent.speed + target.GetComponent<Drive>().currentSpeed);
    Vector3 predictedIntersect => target.position + lookAheadDistance * target.forward;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Evade();
    }

    void SmartPursue()
    {
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
            Pursue();
        }

        //pursue
        //if target stopped moving, seek
        //if target and self are heading the same direction, turn around and seek.
    }

    void Evade()
    {
        Flee(predictedIntersect);
    }

    void Pursue()
    {
        Seek(predictedIntersect);
        state = BotState.Pursue;
    }

    void Seek(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
        state = BotState.Seek;
    }

    void Flee(Vector3 scary)
    {
        Vector3 location = transform.position;
        Vector3 awayFrom = location - scary;
        Vector3 targetLocation = location + awayFrom;

        agent.SetDestination(targetLocation);
        state = BotState.Flee;
    }
}

public enum BotState
{
    Seek,
    Pursue,
    Flee
}