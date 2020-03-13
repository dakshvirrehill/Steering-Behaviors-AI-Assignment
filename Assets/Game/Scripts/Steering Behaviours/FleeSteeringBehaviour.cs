using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeSteeringBehaviour : SteeringBehaviourBase
{
    [SerializeField] float mFleeDistance;
    [SerializeField] Transform mEnemyTarget;


    public override Vector3 calculateForce()
    {
        if(mEnemyTarget == null)
        {
            checkMouseInput();
        }
        else
        {
            target = mEnemyTarget.position;
        }
        Vector3 aDistVector = transform.parent.position - target;

        if(aDistVector.sqrMagnitude >= mFleeDistance * mFleeDistance)
        {
            return Vector3.zero;
        }

        Vector3 desiredVelocity = aDistVector.normalized;
        desiredVelocity = desiredVelocity * steeringAgent.maxSpeed;
        return desiredVelocity - steeringAgent.velocity;
    }

    private void OnDrawGizmos()
    {
        DebugExtension.DebugCircle(transform.parent.position, mFleeDistance);
        DebugExtension.DebugWireSphere(target, Color.red);
    }


}
