using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveSteeringBehaviour : SeekSteeringBehaviour
{
    [SerializeField] float mSlowDownDistance;
    [SerializeField] float mDeceleration;
    [SerializeField] float mStoppingDistance;

    public override Vector3 calculateForce()
    {
        checkMouseInput();

        Vector3 aDistanceVector = target - transform.parent.position;

        float aMag = aDistanceVector.magnitude;

        if(aMag > mSlowDownDistance)
        {
            return base.calculateForce();
        }
        else if(aMag < mStoppingDistance)
        {
            return -steeringAgent.velocity;
        }

        float aSpeed = aMag / mDeceleration;
        if(aSpeed > steeringAgent.maxSpeed)
        {
            aSpeed = steeringAgent.maxSpeed;
        }

        aSpeed /= mSlowDownDistance;

        Vector3 aDesiredVelocity = aDistanceVector.normalized * aSpeed;

        return aDesiredVelocity - steeringAgent.velocity;
    }


    private void OnDrawGizmos()
    {
        DebugExtension.DebugWireSphere(target);
        DebugExtension.DebugCircle(transform.parent.position,Color.green, mSlowDownDistance);
    }

}
