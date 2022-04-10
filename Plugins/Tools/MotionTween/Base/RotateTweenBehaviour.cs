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
            var targetValue = tweenPlayer.transform.eulerAngles + eulerAngles;
            if (duration > 0)
            {
                tweenPlayer.transform.DORotate(targetValue, duration).SetEase(curve).SetUpdate(true);    
            }
            else
            {
                tweenPlayer.transform.eulerAngles = targetValue;
            }
        }
	}
}