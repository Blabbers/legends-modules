using UnityEngine;
using DG.Tweening;

namespace Blabbers.Game00
{
	public class HideOutsideTweenBehaviour : TweenBehaviour
	{
		public enum SwipeFrom { Top, Bottom, Right, Left};
		public SwipeFrom hideOutsidePosition;

		public override void Play(MotionTweenPlayer tweenPlayer)
		{
			var outsidePosition = GetOutsidePosition(tweenPlayer, hideOutsidePosition);

			if(duration > 0)
			{
				tweenPlayer.RectTransform.DOMove(outsidePosition, duration)
				.SetEase(curve);
			}
			else
			{
				tweenPlayer.RectTransform.position = outsidePosition;
			}
			
		}

		public static Vector3 GetOutsidePosition(MotionTweenPlayer tweenPlayer, SwipeFrom hideOutsidePosition)
		{
			var canvasRect = (tweenPlayer.CanvasScaler.transform as RectTransform);
			var scale = canvasRect.localScale.x;

			var startPosition = tweenPlayer.transform.position;
			
			var screenSide = startPosition;
			var size = tweenPlayer.RectTransform.sizeDelta * 0.5f * scale;
			
			var width = Screen.width; //tweenPlayer.CanvasScaler.referenceResolution.x;
			var height = Screen.height; //tweenPlayer.CanvasScaler.referenceResolution.y;

			switch (hideOutsidePosition)
			{
				case SwipeFrom.Top:
					screenSide.y = height + size.y;					
					break;
				case SwipeFrom.Bottom:
					screenSide.y = 0f - size.y;
					break;
				case SwipeFrom.Right:
					screenSide.x = width + size.x;
					break;
				case SwipeFrom.Left:
					screenSide.x = 0f - size.x;					
					break;
			}

			var dir = (screenSide - startPosition);
			var outsidePosition = startPosition + dir;
			return outsidePosition;
		}
	}
}