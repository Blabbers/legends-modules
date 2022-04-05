using UnityEngine;
using DG.Tweening;

namespace Blabbers.Game00
{
	public class RotateTweenBehaviour : TweenBehaviour
	{
		public Vector3 eulerAngles;
		//public RotateMode rotateMode;

		public override void Play(MotionTweenPlayer tweenPlayer)
		{
			tweenPlayer.transform.DORotate(tweenPlayer.transform.eulerAngles + eulerAngles, duration).SetEase(curve).SetUpdate(true);
		}
	}
}