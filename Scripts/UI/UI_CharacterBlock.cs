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
	Action exitCallback;


	public void AnimateIn(Vector3 pos, float duration)
	{
		MoveTo(pos, duration, FinishAnimateIn);
	}

	public void AnimateOut(Vector3 pos, float duration, Action callback)
	{
		//exitCallback = callback;

		Sequence s = DOTween.Sequence();
		s.Append(characterBox.DOScale(Vector3.zero, 0.25f));
		s.Append(helpButton.DOScale(Vector3.one, 0.5f));
		s.AppendCallback(() => MoveTo(pos, duration, callback));

		//MoveTo(pos, duration, FinishAnimateOut);
	}




	void FinishAnimateIn()
	{
		Sequence s = DOTween.Sequence();
		s.Append(helpButton.DOScale(Vector3.zero, 0.25f));
		s.Append(characterBox.DOScale(Vector3.one, 0.5f));
	}

	//void FinishAnimateOut()
	//{
	//	//Sequence s = DOTween.Sequence();
	//	//s.Append(characterBox.DOScale(Vector3.zero, 0.25f));
	//	//s.Append(helpButton.DOScale(Vector3.one, 0.5f));
	//	//s.AppendCallback(()=> exitCallback?.Invoke());
	//}



	void MoveTo(Vector3 pos, float duration, Action callback)
	{
		transform.DOMove(pos, duration).OnComplete(() => callback?.Invoke());
	}
}
