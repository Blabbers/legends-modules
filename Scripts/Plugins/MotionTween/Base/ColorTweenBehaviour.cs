using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace Blabbers.Game00
{
	public class ColorTweenBehaviour : TweenBehaviour
	{
		public Color colorTo = Color.white;

		public override void Play(MotionTweenPlayer tweenPlayer)
		{
            var targetValue = colorTo;
            var textMeshProUGUI = tweenPlayer.GetComponent<TextMeshProUGUI>();
            if (textMeshProUGUI)
            {
                if (duration > 0)
                {
                    textMeshProUGUI.DOColor(colorTo, duration).SetEase(curve).SetUpdate(true);    
                }
                else
                {
                    textMeshProUGUI.color = targetValue;
                }
                return;
            }
            var image = tweenPlayer.GetComponent<Image>();
            if (image)
            {
                if (duration > 0)
                {
                    image.DOColor(colorTo, duration).SetEase(curve).SetUpdate(true);    
                }
                else
                {
                    image.color = targetValue;
                }
                return;
            }
            var text = tweenPlayer.GetComponent<Text>();
            if (text)
            {
                if (duration > 0)
                {
                    text.DOColor(colorTo, duration).SetEase(curve).SetUpdate(true);    
                }
                else
                {
                    text.color = targetValue;
                }
                return;
            }
            var textMeshPro = tweenPlayer.GetComponent<TextMeshPro>();
            if (textMeshPro)
            {
                if (duration > 0)
                {
                    textMeshPro.DOColor(colorTo, duration).SetEase(curve).SetUpdate(true);    
                }
                else
                {
                    textMeshPro.color = targetValue;
                }
                return;
            }
            Debug.Log($"<color=red>Trying to tween [{this.name}] the object's [{tweenPlayer.name}] color, but it has not image or text.</color>", tweenPlayer);
		}
	}
}