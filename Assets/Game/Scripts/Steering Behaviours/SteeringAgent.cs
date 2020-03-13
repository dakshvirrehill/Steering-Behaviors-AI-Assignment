using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
	public enum SummingMethod
	{
		WeightedAverage,
		Prioritized,
		Dithered
	};
	public SummingMethod summingMethod = SummingMethod.WeightedAverage;

	public float mass = 1.0f;
	public float maxSpeed = 1.0f;
	public float maxForce = 10.0f;

	public Vector3 velocity = Vector3.zero;

	private List<SteeringBehaviourBase> steeringBehaviours = new List<SteeringBehaviourBase>();

    private Animator mAnimator;

	public float angularDampeningTime = 5.0f;
	public float deadZone = 10.0f;

	private void Start()
	{
		steeringBehaviours.AddRange(GetComponentsInChildren<SteeringBehaviourBase>());
		foreach(SteeringBehaviourBase behaviour in steeringBehaviours)
		{
			behaviour.steeringAgent = this;
		}

        mAnimator = GetComponent<Animator>();

	}

    void OnAnimatorMove()
    {
        if(Time.deltaTime > 0.0f)
        {
            Vector3 aVelocity = mAnimator.deltaPosition / Time.deltaTime;
            transform.position += transform.forward * aVelocity.magnitude * Time.deltaTime;
        }
    }

    private void Update()
	{
		Vector3 steeringForce = calculateSteeringForce();
		steeringForce.y = 0.0f;

		Vector3 acceleration = steeringForce * (1.0f / mass);
		velocity = velocity + (acceleration * Time.deltaTime);
		velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        float aSpeed = velocity.magnitude;

        mAnimator.SetFloat("Speed", aSpeed);

		if (aSpeed > 0.0f)
		{
			float angle = Vector3.Angle(transform.forward, velocity);
			if (Mathf.Abs(angle) <= deadZone)
			{
				transform.LookAt(transform.position + velocity);
			}
			else
			{
				transform.rotation = Quaternion.Slerp(transform.rotation,
													  Quaternion.LookRotation(velocity),
													  Time.deltaTime * angularDampeningTime);
			}
		}
	}

	private Vector3 calculateSteeringForce()
	{
		Vector3 totalForce = Vector3.zero;

		foreach(SteeringBehaviourBase behaviour in steeringBehaviours)
		{
			if (behaviour.enabled)
			{
				switch(summingMethod)
				{
					case SummingMethod.WeightedAverage:
						totalForce = totalForce + (behaviour.calculateForce() * behaviour.weight);
						totalForce = Vector3.ClampMagnitude(totalForce, maxForce);
						break;

					case SummingMethod.Prioritized:
						break;
				}

			}
		}

		return totalForce;
	}
}
