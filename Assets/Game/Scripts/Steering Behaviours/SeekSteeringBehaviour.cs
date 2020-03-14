using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekSteeringBehaviour : SteeringBehaviourBase
{
	public override Vector3 CalculateForce()
	{
		Vector3 aDesiredVelocity = (mTarget - transform.parent.position).normalized;
		aDesiredVelocity = aDesiredVelocity * mSteeringAgent.mMaxSpeed;
		return aDesiredVelocity - mSteeringAgent.mVelocity;
	}

	protected virtual void OnDrawGizmos()
	{
		DebugExtension.DebugWireSphere(mTarget, Color.red);
	}
}
