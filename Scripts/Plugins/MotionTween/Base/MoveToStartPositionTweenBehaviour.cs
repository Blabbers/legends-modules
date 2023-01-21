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

            var newPosition = tweenPlayer.StartAnchoredPosition;
            if (duration > 0)
            {
                tweenPlayer.RectTransform.DOAnchorPos(newPosition, duration).SetEase(curve).SetUpdate(true);
            }
            else
            {
                tweenPlayer.RectTransform.anchoredPosition = newPosition;
            }
		}
	}
}