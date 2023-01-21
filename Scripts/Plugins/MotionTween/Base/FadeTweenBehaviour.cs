using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace Blabbers.Game00
{
	public class FadeTweenBehaviour : TweenBehaviour
	{
		public float fadeTo = 1f;

		public override void Play(MotionTweenPlayer tweenPlayer)
		{
            var targetValue = fadeTo;
            var canvasGroup = tweenPlayer.GetComponent<CanvasGroup>();
            if (canvasGroup)
            {
                if (duration > 0)
                {
                    canvasGroup.DOFade(fadeTo, duration).SetEase(curve).SetUpdate(true);
                }
                else
                {
                    canvasGroup.alpha = targetValue;
                }
                return;
            }
            var textMeshProUGUI = tweenPlayer.GetComponent<TextMeshProUGUI>();
            if (textMeshProUGUI)
            {
                if (duration > 0)
                {
                    textMeshProUGUI.DOFade(fadeTo, duration).SetEase(curve).SetUpdate(true);    
                }
                else
                {
                    var color = textMeshProUGUI.color;
                    color.a = targetValue;
                    textMeshProUGUI.color = color;
                    
                }
                return;
            }
            var image = tweenPlayer.GetComponent<Image>();
            if (image)
            {
                if (duration > 0)
                {
                    image.DOFade(fadeTo, duration).SetEase(curve).SetUpdate(true);    
                }
                else
                {
                    var color = image.color;
                    color.a = targetValue;
                    image.color = color;
                }
                return;
            }            
            var text = tweenPlayer.GetComponent<Text>();
            if (text)
            {
                if (duration > 0)
                {
                    text.DOFade(fadeTo, duration).SetEase(curve).SetUpdate(true);    
                }
                else
                {
                    var color = text.color;
                    color.a = targetValue;
                    text.color = color;
                }
                return;
            }
            var textMeshPro = tweenPlayer.GetComponent<TextMeshPro>();
            if (textMeshPro)
            {
                if (duration > 0)
                {
                    textMeshPro.DOFade(fadeTo, duration).SetEase(curve).SetUpdate(true);    
                }
                else
                {
                    var color = textMeshPro.color;
                    color.a = targetValue;
                    textMeshPro.color = color;
                }
                return;
            }
            Debug.Log($"<color=red>Trying to tween the object's [{tweenPlayer.name}] alpha, but it has not Image, Text or CanvasGroup.</color>", tweenPlayer);
		}
	}
}