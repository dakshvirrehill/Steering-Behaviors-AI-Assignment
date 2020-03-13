using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekSteeringBehaviour : SteeringBehaviourBase
{
	public override Vector3 calculateForce()
	{
		checkMouseInput();

		Vector3 desiredVelocity = (target - transform.parent.position).normalized;
		desiredVelocity = desiredVelocity * steeringAgent.maxSpeed;
		return desiredVelocity - steeringAgent.velocity;
	}

	private void OnDrawGizmos()
	{
		DebugExtension.DebugWireSphere(target);
	}
}
