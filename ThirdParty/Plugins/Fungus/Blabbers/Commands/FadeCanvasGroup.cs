using UnityEngine;
using DG.Tweening;

namespace Fungus
{
    [CommandInfo("Blabbers",
				 "Fade Canvas",
				 "Fades a UI Canvas Group.")]
    [AddComponentMenu("")]
    public class FadeCanvasGroup : Command
    {
		public CanvasGroup canvasGroup;
		public float fadeTo = 1f;
		public float duration = 0.3f;
		public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
		public bool waitUntilFinished = true;		

		#region Public members
		public override void OnEnter()
        {
			var targetValue = fadeTo;			
			if (canvasGroup)
			{
				if (duration > 0)
				{
					Debug.Log("<><> START FADE " + duration, canvasGroup);
					canvasGroup.DOFade(fadeTo, duration).SetEase(curve).SetUpdate(true).OnComplete(HandleOnComplete);
					void HandleOnComplete()
					{
						Debug.Log("<><> ONCOMPLETE ", canvasGroup);
						if (waitUntilFinished)
						{
							Continue();
						}
					}
					return;
				}
				else
				{
					canvasGroup.alpha = targetValue;
					Continue();
					return;
				}				
			}
			if (!waitUntilFinished)
			{
				Continue();
			}
		}
		public override string GetSummary()
		{
			return canvasGroup.name + " = " + fadeTo.ToString() + " alpha";
		}

		public override Color GetButtonColor()
		{
			return new Color32(180, 250, 250, 255);
		}
		#endregion
	}
}