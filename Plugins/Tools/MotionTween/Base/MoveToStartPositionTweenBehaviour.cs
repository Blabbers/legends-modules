using UnityEngine;
using DG.Tweening;

namespace Blabbers.Game00
{
	public class MoveToStartPositionTweenBehaviour : TweenBehaviour
	{
		public override void Play(MotionTweenPlayer tweenPlayer)
		{
			//var outsidePosition = HideOutsideTweenBehaviour.GetOutsidePosition(tweenPlayer, swipeFrom);
			//tweenPlayer.rectTransform.position = outsidePosition;

			tweenPlayer.RectTransform.DOAnchorPos(tweenPlayer.StartAnchoredPosition, duration)
				.SetEase(curve).SetUpdate(true);
		}
	}
}