using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PathFollowSteeringBehaviour : ArriveSteeringBehaviour
{
    public float mWaypointSeekDist = 0.5f;
    public bool mLoop = false;

    int mCurrentWaypointIndex = 0;

    [HideInInspector] public NavMeshPath mPath = null;
    public override void CalculateNewPath(Vector3 pPathTarget)
    {
        mPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, pPathTarget, NavMesh.AllAreas, mPath);
        if(mPath != null && mPath.corners.Length > 0)
        {
            mTarget = mPath.corners[0];
        }
    }

    public override Vector3 CalculateForce()
    {
        if (!(mPath != null && mPath.corners.Length > 0))
        {
            return base.CalculateForce();
        }
        if(!mLoop && mCurrentWaypointIndex == mPath.corners.Length - 1)
        {
            mPath = null;
        }
        else if((mTarget - transform.position).magnitude <= mWaypointSeekDist)
        {
            mCurrentWaypointIndex = (mCurrentWaypointIndex + 1) % mPath.corners.Length;
            if (mCurrentWaypointIndex < mPath.corners.Length)
            {
                mTarget = mPath.corners[mCurrentWaypointIndex];
            }
        }
        return base.CalculateForce();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if(mPath != null)
        {
            for(int i = 1; i < mPath.corners.Length; i++)
            {
                Debug.DrawLine(mPath.corners[i - 1], mPath.corners[i], Color.yellow);
            }
        }
    }
}
