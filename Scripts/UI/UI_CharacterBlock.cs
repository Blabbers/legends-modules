using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Threading;

public class UI_CharacterBlock : MonoBehaviour
{
	[SerializeField] Transform helpButton;
	[SerializeField] Transform characterBox;
	Action entranceCallback;
	Action exitCallback;
	float blobDuration;
	float moveDuration;


	public void AnimateIn(Vector3 pos, float duration, Action callback)
	{
		blobDuration = duration * (0.7f);
		moveDuration = duration * 0.3f;
		entranceCallback = callback;


		MoveTo(pos, moveDuration, FinishAnimateIn);
	}

	public void AnimateOut(Vector3 pos, float duration, Action callback)
	{
		//exitCallback = callback;

		blobDuration = duration * (0.7f);
		moveDuration = duration * 0.3f;


		Sequence s = DOTween.Sequence();
		s.Append(characterBox.DOScale(Vector3.zero, blobDuration/2));
		s.Append(helpButton.DOScale(Vector3.one, blobDuration));
		s.AppendCallback(() => MoveTo(pos, moveDuration, callback));

		//MoveTo(pos, duration, FinishAnimateOut);
	}




	void FinishAnimateIn()
	{
		Sequence s = DOTween.Sequence();
		s.Append(helpButton.DOScale(Vector3.zero, blobDuration/2)) ;
		s.Append(characterBox.DOScale(Vector3.one, blobDuration));
		s.AppendCallback(() => entranceCallback?.Invoke());
	}

	void MoveTo(Vector3 pos, float duration, Action callback)
	{
		transform.DOMove(pos, duration).OnComplete(() => callback?.Invoke());
	}
}
