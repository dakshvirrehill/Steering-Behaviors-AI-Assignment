using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventInt : UnityEvent<int> { }

public class AnimationListener : MonoBehaviour, IAnimationCompleted
{

    #region Animator Move
    private UnityEvent mOnAnimatorMoveEvent = new UnityEvent();


    public void AddOnAnimatorMoveListener(UnityAction pCallback)
    {
        mOnAnimatorMoveEvent.AddListener(pCallback);
    }

    public void RemoveAnimatorMoveListener(UnityAction pCallback)
    {
        mOnAnimatorMoveEvent.RemoveListener(pCallback);
    }

    private void OnAnimatorMove()
    {
        mOnAnimatorMoveEvent.Invoke();
    }
    #endregion

    Dictionary<int, UnityEventInt> mAnimationCompletedCallbacks = new Dictionary<int, UnityEventInt>();


    public void AnimationCompleted(int shortHashName)
    {
        UnityEventInt aEventCallback;
        if(mAnimationCompletedCallbacks.TryGetValue(shortHashName, out aEventCallback))
        {
            aEventCallback.Invoke(shortHashName);
        }
    }

    public void AddAnimationCompletedListener(int pShortHashName, UnityAction<int> pCallback)
    {
        if(!mAnimationCompletedCallbacks.ContainsKey(pShortHashName))
        {
            mAnimationCompletedCallbacks.Add(pShortHashName, new UnityEventInt());
        }
        mAnimationCompletedCallbacks[pShortHashName].AddListener(pCallback);
    }

    public void RemoveAnimationCompletedListener(int pShortHashName, UnityAction<int> pCallback)
    {
        if(mAnimationCompletedCallbacks.ContainsKey(pShortHashName))
        {
            mAnimationCompletedCallbacks[pShortHashName].RemoveListener(pCallback);
        }
    }



}
