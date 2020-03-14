using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderSteeringBehaviour : SeekSteeringBehaviour
{
    public float mWanderDistance = 2.0f;
    public float mWanderRadius = 1.0f;
    public float mWanderJitter = 20.0f;

    Vector3 mWanderTarget;
    bool mWander = false;

    public void SetWanderTarget()
    {
        mWander = true;
        float aTheta = Random.value * Mathf.PI * 2.0f;
        mWanderTarget = new Vector3(mWanderRadius * Mathf.Cos(aTheta), 0.0f, mWanderRadius * Mathf.Sin(aTheta));
    }

    public override void CalculateNewPath(Vector3 pPathTarget)
    {
        mWander = false;
        base.CalculateNewPath(pPathTarget);
    }


    public override Vector3 CalculateForce()
    {
        if(mWander)
        {
            float aJitterWRTTime = mWanderJitter * Time.deltaTime;
            mWanderTarget = mWanderTarget + new Vector3(Random.Range(-1.0f, 1.0f) * aJitterWRTTime, 0.0f, Random.Range(-1.0f, 1.0f) * aJitterWRTTime);
            mWanderTarget.Normalize();
            mWanderTarget = mWanderTarget * mWanderRadius;
            mTarget = mWanderTarget + new Vector3(0, 0, mWanderDistance);
            mTarget = transform.rotation * mTarget + transform.position;
        }
        return base.CalculateForce();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Vector3 aCircleCenter = transform.rotation * new Vector3(0.0f, 0.0f, mWanderDistance) + transform.position;
        DebugExtension.DrawCircle(aCircleCenter,Vector3.up, Color.green, mWanderRadius);
        Debug.DrawLine(transform.position, mTarget, Color.yellow);
        DebugExtension.DrawArrow(transform.position, transform.forward * mWanderDistance, Color.grey);
    }

}
