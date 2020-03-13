using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MoveToGoalStateBehaviour : StateMachineBehaviour
{
    public float mAngularDampeningTime = 5.0f;
    public float mDeadZone = 10.0f;

    private NavMeshAgent mAgent;
    private Animator mAnimator;
    private Transform mAgentTransform;
    private AnimationListener mListener;

    private UnityAction mOnAnimatorMoveCallback;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(mAgent == null)
        {
            mAgentTransform = fsm.transform.parent;
            mAgent = mAgentTransform.GetComponent<NavMeshAgent>();
            mAnimator = mAgentTransform.GetComponent<Animator>();
            mListener = mAgentTransform.GetComponent<AnimationListener>();
            mOnAnimatorMoveCallback = new UnityAction(OnAnimatorMove);
        }
        mListener.AddOnAnimatorMoveListener(mOnAnimatorMoveCallback);
    }

    void OnAnimatorMove()
    {
        mAgent.velocity = mAnimator.deltaPosition / Time.deltaTime;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(mAgent.isOnOffMeshLink)
        {
            //fsm.SetBool("Climb", true);
            //mAnimator.SetTrigger("Climb");
            fsm.SetBool("JumpWithStyle", true);
            mAnimator.SetBool("Jump", true);
        }


        if (mAgent.desiredVelocity.magnitude >= 0.1f)
        {
            float aSpeed = Vector3.Project(mAgent.desiredVelocity, mAgentTransform.forward).magnitude * mAgent.speed;
            mAnimator.SetFloat("Speed", aSpeed);

            float aAngle = Vector3.Angle(mAgentTransform.forward, mAgent.desiredVelocity);

            if(Mathf.Abs(aAngle) <= mDeadZone)
            {
                mAgentTransform.LookAt(mAgentTransform.position + mAgent.desiredVelocity);
            }
            else
            {
                mAgentTransform.rotation = Quaternion.Lerp(mAgentTransform.rotation, Quaternion.LookRotation(mAgent.desiredVelocity), Time.deltaTime * mAngularDampeningTime);
            }

        }
        else
        {
            fsm.SetBool("MoveToGoal", false);
            mAnimator.SetFloat("Speed", 0.0f);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mListener.RemoveAnimatorMoveListener(mOnAnimatorMoveCallback);
    }

}

