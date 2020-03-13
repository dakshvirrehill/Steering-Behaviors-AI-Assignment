using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderSteeringBehaviour : SeekSteeringBehaviour
{
    public float mWanderDistance = 2.0f;
    public float mWanderRadius = 1.0f;
    public float mWanderJitter = 20.0f;

    Vector3 mWanderTarget;

    void Start()
    {
        float aTheta = Random.value * Mathf.PI * 2.0f;
        mWanderTarget = new Vector3(mWanderRadius * Mathf.Cos(aTheta), 0.0f, mWanderRadius * Mathf.Sin(aTheta));
    }


    public override Vector3 calculateForce()
    {
        float aJitterWRTTime = mWanderJitter * Time.deltaTime;
        mWanderTarget = mWanderTarget + new Vector3(Random.Range(-1.0f, 1.0f) * aJitterWRTTime, 0.0f, Random.Range(-1.0f, 1.0f) * aJitterWRTTime);
        mWanderTarget.Normalize();
        mWanderTarget = mWanderTarget * mWanderRadius;
        target = mWanderTarget + new Vector3(0, 0, mWanderDistance);
        target = transform.rotation * target + transform.position;
        return base.calculateForce();
    }

    void OnDrawGizmos()
    {
        Vector3 aCircleCenter = transform.rotation * new Vector3(0.0f, 0.0f, mWanderDistance) + transform.position;
        DebugExtension.DrawCircle(aCircleCenter,Vector3.up, Color.black, mWanderRadius);
        Debug.DrawLine(transform.position, target, Color.red);
        DebugExtension.DrawArrow(transform.position, transform.forward * mWanderDistance, Color.yellow);
    }

}
