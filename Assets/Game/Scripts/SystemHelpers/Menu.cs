using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Menu : MonoBehaviour
{
	public MenuClassifier mMenuClassifier;
	public enum StartMenuState
	{
		Ingnore,
		Active,
		Disabled
	}
	public StartMenuState mStartMenuState = StartMenuState.Active;
	public bool mResetPosition = true;
    public float mDelay = 0.1f;
    public float mFadeInTime = 0.2f;
    public float mFadeOutTime = 0.2f;
    protected CanvasGroup mGroup;
	public virtual void Awake()
	{
		MenuManager.Instance.AddMenu(this);
		if (mResetPosition == true)
		{
			var rect = GetComponent<RectTransform>();
			rect.localPosition = Vector3.zero;
		}
	}

	public virtual void Start()
	{
        mGroup = GetComponent<CanvasGroup>();
        switch (mStartMenuState)
		{
			case StartMenuState.Active:
				gameObject.SetActive(true);
				break;

			case StartMenuState.Disabled:
				gameObject.SetActive(false);
				break;
		}
	}

	public virtual void ShowMenu(string pOptions)
	{
        if(LeanTween.isTweening(gameObject))
        {
            LeanTween.cancel(gameObject);
        }
        gameObject.SetActive(true);
        if(mGroup == null)
        {
            mGroup = GetComponent<CanvasGroup>();
        }
        LeanTween.alphaCanvas(mGroup, 1.0f, mFadeInTime).setDelay(mDelay);
	}

	public virtual void HideMenu(string pOptions)
	{
        if (LeanTween.isTweening(gameObject))
        {
            LeanTween.cancel(gameObject);
        }
        if (mGroup == null)
        {
            mGroup = GetComponent<CanvasGroup>();
        }
        LeanTween.alphaCanvas(mGroup, 0.0f, mFadeInTime).setDelay(mDelay).setOnComplete(() => gameObject.SetActive(false));
    }

}
