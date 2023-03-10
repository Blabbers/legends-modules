using System.Collections;
using UnityEngine;

namespace Fungus
{
	public enum FadeType
	{
		In,
		Out,
	}

	[CommandInfo("Blabbers",
				 "Fade",
				 "Fades the screen in or out. You can choose to wait this command to finish to go to the next one")]
	[AddComponentMenu("")]
	public class FadeCommand : Command
	{
		public FadeType fadeType;
		public float duration = 1f;
		public AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
		public bool waitFadeBeforeContinuing = true;

		#region Public members
		public override void OnEnter()
		{
			switch (fadeType)
			{
				case FadeType.In:
					Fade.In(duration);
					break;
				case FadeType.Out:
					Fade.Out(duration);
					break;
			}
			if (waitFadeBeforeContinuing)
			{
				StartCoroutine(Routine());
				IEnumerator Routine()
				{
					yield return new WaitForSeconds(duration);
					Continue();
				}
			}
			else
			{
				Continue();
			}
		}

		public override string GetSummary()
		{
			return $"{fadeType}: {duration} seconds";
		}

		public override Color GetButtonColor()
		{
			return new Color32(184, 210, 235, 255);
		}

		#endregion
	}
}