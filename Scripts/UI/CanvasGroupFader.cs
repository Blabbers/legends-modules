using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CanvasGroupFader : MonoBehaviour
{
	public float startValue = 0f;
	public CanvasGroup group;
	public UnityEvent OnFadeInFinish, OnFadeOutFinish;
	public bool unscaledTime = true;

	private void Awake()
	{
		if (!group) group = this.gameObject.GetComponent<CanvasGroup>();
	}

	private void OnEnable()
	{
		group.alpha = startValue;
	}

	public void FadeIn(float duration)
	{
		group.DOFade(1.0f, duration).SetUpdate(unscaledTime).OnComplete(()=> FadeInFinish());
	}
	public void FadeOut(float duration)
	{
		group.DOFade(0.0f, duration).SetUpdate(unscaledTime).OnComplete(() => FadeOutFinish());
	}


	void FadeInFinish()
	{
		OnFadeInFinish?.Invoke();
	}

	void FadeOutFinish()
	{
		OnFadeOutFinish?.Invoke();
	}

}
