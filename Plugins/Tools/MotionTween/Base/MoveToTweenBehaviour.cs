using UnityEngine;
using DG.Tweening;

namespace Blabbers.Game00
{
	public class MoveToTweenBehaviour : TweenBehaviour
	{
		public Vector2 targetPosition;
		public override void Play(MotionTweenPlayer tweenPlayer)
		{
			if (duration > 0)
			{
                tweenPlayer.RectTransform.DOAnchorPos(targetPosition, duration)
                    .SetEase(curve).SetUpdate(true);
			}
            else
            {
                tweenPlayer.RectTransform.anchoredPosition = targetPosition;
            }

        }
	}
}