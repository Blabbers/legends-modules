using UnityEngine;
using DG.Tweening;

namespace Blabbers.Game00
{
	public class ScaleTweenBehaviour : TweenBehaviour
	{
		public Vector3 scaleTo = Vector3.one;

		public override void Play(MotionTweenPlayer tweenPlayer)
		{
            var targetValue = scaleTo;
            if (duration > 0)
            {
                tweenPlayer.transform.DOScale(targetValue, duration).SetEase(curve).SetUpdate(true);    
            }
            else
            {
                tweenPlayer.transform.localScale = targetValue;
            }
		}
	}
}