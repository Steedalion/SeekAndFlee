using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Flee(target.position);
    }

    void Seek(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }

    void Flee(Vector3 scary)
    {
        Vector3 location = transform.position;
        Vector3 awayFrom = location - scary;
        Vector3 targetLocation = location + awayFrom;

        agent.SetDestination(targetLocation);
    }
}
