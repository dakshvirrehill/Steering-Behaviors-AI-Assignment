using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFollowSteeringBehaviour : ArriveSteeringBehaviour
{
    public Transform pathTarget;
    public float waypointSeekDist = 0.5f;
    public bool loop = false;

    public int currentWaypointIndex = 0;

    private NavMeshPath path;

    private void Start()
    {
        path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, pathTarget.position, NavMesh.AllAreas, path);
        if(path != null && path.corners.Length > 0)
        {
            target = path.corners[0];
        }
    }

    public override Vector3 calculateForce()
    {
        if(path == null || path.corners.Length == 0)
        {
            return base.calculateForce();
        }
        if(!loop && currentWaypointIndex == path.corners.Length - 1)
        {

        }
        else if((target - transform.position).magnitude <= waypointSeekDist)
        {
            if(currentWaypointIndex == path.corners.Length-1)
            {
                currentWaypointIndex = 0;
            }
            else
            {
                currentWaypointIndex++;
            }
            if (currentWaypointIndex < path.corners.Length)
            {
                target = path.corners[currentWaypointIndex];
            }
        }
        return base.calculateForce();
    }

    private void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = 1; i < path.corners.Length; i++)
            {
                Debug.DrawLine(path.corners[i - 1], path.corners[i], Color.blue);
            }
        }
    }
}
