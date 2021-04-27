using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Bot : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;
    public BotState state;

    public float wanderRadius = 10;
    public float wanderDistance = 10;
    public float wanderJitter = 5;
    private Vector3 wanderTarget = Vector3.zero;
    private float targetFieldOfView = 60;
    private float inRangeDistance = 20;
    private bool isOnCoolDown = false;


    private Vector3 ToTarget => target.position - transform.position;
    private bool TargetIsBehind => Vector3.Dot(transform.forward, ToTarget) < 0;
    float LookAheadDistance => ToTarget.magnitude / (agent.speed + target.GetComponent<Drive>().currentSpeed);
    Vector3 PredictedIntersect => target.position + LookAheadDistance * target.forward;
    private bool OutOfRange => ToTarget.sqrMagnitude > inRangeDistance * inRangeDistance;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isOnCoolDown) return;
        if (OutOfRange)
        {
            Debug.Log( "Out of range");
            Wander();
            return;
        }
        
        if (ClearLineOfSightToTarget() && WithinTargetFOV())
        {
            HideBehindObject();
            StartCooldown();
        }
        else
        {
            Pursue();
        }
    }

    void StartCooldown()
    {
        isOnCoolDown = true;
        Invoke(nameof(ResetCooldown), 5);
    }

    void ResetCooldown()
    {
        isOnCoolDown = false;
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
            Pursue();
        }

        //pursue
        //if target stopped moving, seek
        //if target and self are heading the same direction, turn around and seek.
    }

    void Evade()
    {
        //TODO: Test

        Flee(PredictedIntersect);
    }

    void Pursue()
    {
        //TODO: Test

        Seek(PredictedIntersect);
        state = BotState.Pursue;
    }

    void Seek(Vector3 targetPosition)
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
        state = BotState.Flee;
    }


    void Wander()
    {
        //TODO: Test

        wanderTarget = new Vector3(Random.Range(-1f, 1f) * wanderJitter, 0, Random.Range(-1f, 1f) * wanderJitter);
        wanderTarget = wanderTarget.normalized * wanderRadius;
        Vector3 wanderDestination = wanderTarget + Vector3.forward * wanderDistance;
        Vector3 globalDestination = transform.TransformVector(wanderDestination);
        Seek(globalDestination);
        state = BotState.Wander;
    }

    void Hide()
    {
        //TODO: Test

        const float distanceBehindObstacle = 20;

        Vector3 nearestObstaclePosition = GetNearestObstacle().transform.position;
        Vector3 fromTargetToObstacle = nearestObstaclePosition - target.transform.position;
        Vector3 hidingSpot = nearestObstaclePosition + fromTargetToObstacle.normalized * distanceBehindObstacle;
        Seek(hidingSpot);

        state = BotState.Hide;
    }


    void HideBehindObject()
    {
        const float hideDistanceBehindObstacle = 4;
        GameObject chosenObstacle = GetNearestObstacle();
        Vector3 nearestObstaclePosition = GetNearestObstacle().transform.position;
        Vector3 fromTargetToObstacle = nearestObstaclePosition - target.transform.position;
        Vector3 intersect = BehindObstacleIntersect(chosenObstacle);

        Seek(intersect + fromTargetToObstacle.normalized * hideDistanceBehindObstacle);
        state = BotState.Hide;
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

    private GameObject GetNearestObstacle()
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

    private bool ClearLineOfSightToTarget()
    {
        return Physics.Raycast(this.transform.position, ToTarget, out RaycastHit raycastHit) &&
               raycastHit.transform.CompareTag("Cop");
    }

    bool WithinTargetFOV()
    {
        Vector3 toAgent = -ToTarget;
        float viewAngle = Vector3.Angle(toAgent, target.forward);
        return (viewAngle < targetFieldOfView);
    }
}

public enum BotState
{
    Seek,
    Pursue,
    Flee,
    Hide,
    Wander,
}