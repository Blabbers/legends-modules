using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UI_ButtonWaitToEnable : MonoBehaviour
{
	private float waitTime = 2f;

	[Header("Tween Settings")]
	public float punchScaleAmount = 0.05f;
	public float punchScaleDuration = 0.4f;
	public Ease punchScaleEase = Ease.Linear;

	Button button;

	public void SetWaitTime(float value)
	{
		waitTime = value;
	}

    void OnEnable()
    {
		button = GetComponent<Button>();

		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			button.interactable = false;
			yield return new WaitForSeconds(waitTime);
			this.transform.DOPunchScale(this.transform.localScale * punchScaleAmount, punchScaleDuration).SetEase(punchScaleEase);
			button.interactable = true;
		}
	}
}