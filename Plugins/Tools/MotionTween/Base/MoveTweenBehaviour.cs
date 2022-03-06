using UnityEngine;
using DG.Tweening;

namespace Blabbers.Game00
{
	public class MoveTweenBehaviour : TweenBehaviour
	{
		public Vector2 addPosition;

		public override void Play(MotionTweenPlayer tweenPlayer)
		{
			tweenPlayer.RectTransform.DOAnchorPos(tweenPlayer.RectTransform.anchoredPosition + addPosition, duration).SetEase(curve);
		}
	}
}