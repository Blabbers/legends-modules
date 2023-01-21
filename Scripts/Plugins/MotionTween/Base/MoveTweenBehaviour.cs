using UnityEngine;
using DG.Tweening;

namespace Blabbers.Game00
{
	public class MoveTweenBehaviour : TweenBehaviour
	{
		public Vector2 addPosition;

		public override void Play(MotionTweenPlayer tweenPlayer)
        {
            var newPosition = tweenPlayer.RectTransform.anchoredPosition + addPosition;
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