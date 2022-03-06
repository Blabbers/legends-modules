using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_TweenScaleOnEnable : MonoBehaviour
{
	[Header("Tween Settings")]
	public float delay = 0.3f;
	public float tweenDuration = 0.8f;
	public Ease easeType = Ease.OutBack;

	void OnEnable()
    {
		this.transform.localScale = Vector3.zero;// * 0.1f;
		this.transform.DOScale(1, tweenDuration).SetEase(easeType).SetDelay(delay);
	}
}
