using UnityEngine;
using DG.Tweening;

namespace Blabbers.Game00
{
	public class ScaleTweenBehaviour : TweenBehaviour
	{
		public Vector3 scaleTo = Vector3.one;

		public override void Play(MotionTweenPlayer tweenPlayer)
		{
			tweenPlayer.transform.DOScale(scaleTo, duration).SetEase(curve).SetUpdate(true);
		}
	}
}