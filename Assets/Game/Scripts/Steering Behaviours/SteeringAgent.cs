using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
	public bool mActive = false;
	public float mMass = 1.0f;
	public float mMaxSpeed = 1.0f;
	public float mMaxForce = 10.0f;

	public Vector3 mVelocity = Vector3.zero;

	private List<SteeringBehaviourBase> mSteeringBehaviours = new List<SteeringBehaviourBase>();

    private Animator mAnimator;

	public float mAngularDampeningTime = 5.0f;
	public float mDeadZone = 10.0f;

	void Start()
	{
		mSteeringBehaviours.AddRange(GetComponentsInChildren<SteeringBehaviourBase>());
		foreach(SteeringBehaviourBase aBehaviour in mSteeringBehaviours)
		{
			aBehaviour.mSteeringAgent = this;
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

    void Update()
	{
		if(!mActive)
		{
			return;
		}
		Vector3 aSteeringForce = calculateSteeringForce();
		aSteeringForce.y = 0.0f;

		Vector3 aAcceleration = aSteeringForce * (1.0f / mMass);
		mVelocity = mVelocity + (aAcceleration * Time.deltaTime);
		mVelocity = Vector3.ClampMagnitude(mVelocity, mMaxSpeed);

        float aSpeed = mVelocity.magnitude;

        mAnimator.SetFloat("Speed", aSpeed);

		if (aSpeed > 0.0f)
		{
			float aAngle = Vector3.Angle(transform.forward, mVelocity);
			if (Mathf.Abs(aAngle) <= mDeadZone)
			{
				transform.LookAt(transform.position + mVelocity);
			}
			else
			{
				transform.rotation = Quaternion.Slerp(transform.rotation,
													  Quaternion.LookRotation(mVelocity),
													  Time.deltaTime * mAngularDampeningTime);
			}
		}
	}

	private Vector3 calculateSteeringForce()
	{
		Vector3 aTotalForce = Vector3.zero;

		foreach(SteeringBehaviourBase aBehaviour in mSteeringBehaviours)
		{
			if (aBehaviour.enabled)
			{
				aTotalForce += (aBehaviour.CalculateForce() * aBehaviour.mWeight);
				aTotalForce = Vector3.ClampMagnitude(aTotalForce, mMaxForce);
			}
		}

		return aTotalForce;
	}
}
