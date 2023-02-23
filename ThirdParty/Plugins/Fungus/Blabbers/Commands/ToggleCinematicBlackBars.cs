using Blabbers.Game00;
using NaughtyAttributes;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Blabbers",
				 "Cinematic Bars",
				 "Toggles Cinematic Black Bars")]
	[AddComponentMenu("")]
	public class ToggleCinematicBlackBars : Command
	{
		public bool show = true;
		[ShowIf(nameof(show))]
		public bool displayHintText = false;
		[ShowIf(nameof(displayHintText))]
		public LocalizedString hintText;
		#region Public members

		public override void OnEnter()
		{
			if (show)
			{		

				UI_CameraFX.Instance.ShowCinematicBlackBars(AnimationFinished, displayHintText ? hintText : "");
			}
			else
			{
				UI_CameraFX.Instance.HideCinematicBlackBars(AnimationFinished);
			}
		}

		public override Color GetButtonColor()
		{
			return new Color32(216, 228, 170, 255);
		}

		void AnimationFinished()
		{
			Continue();
		}

		public override string GetSummary()
		{
			string namePrefix = "";
			if (hintText.HasUnsavedChanges())
			{
				namePrefix = "<color=red><b>* UNSAVED CHANGES *</b></color> ";
			}
			return (show ? 
				"<color=green>(ON)</color> " + (displayHintText ? namePrefix + hintText.GetRawText() : "")
				: "<color=red>(OFF)</color>");
		}
		#endregion
	}
}