using Blabbers.Game00;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Blabbers",
				 "Toggle CinematicBlackBars",
				 "Toggles Cinematic Black Bars")]
	[AddComponentMenu("")]
	public class ToggleCinematicBlackBars : Command
	{

		public bool show = true;
		#region Public members

		public override void OnEnter()
		{
			//TODO: Implement PAUSE call.

			if (show)
			{
				//Singleton.Get<UI_CameraFX>().ShowCinematicBlackBars();

				UI_CameraFX.Instance.ShowCinematicBlackBars();
			}
			else
			{
				Singleton.Get<UI_CameraFX>().HideCinematicBlackBars();
			}
			

			Continue();
		}


		public override Color GetButtonColor()
		{
			return new Color32(216, 228, 170, 255);
		}



		#endregion
	}
}