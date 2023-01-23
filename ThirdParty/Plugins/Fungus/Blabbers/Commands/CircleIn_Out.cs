using Blabbers.Game00;
using Sigtrap.Relays;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Blabbers",
				 "Show Circle In/Out",
				 "Toggles Cinematic Black Bars")]
	[AddComponentMenu("")]
	public class ToggleCircleIn : Command
	{

		public bool isIn = true;
		public Transform target;
		public float delay = 0.0f;


		#region Public members

		public override void OnEnter()
		{

			if (isIn)
			{

				UI_CameraFX.Instance.CircleIn(target.position, AnimationFinished, delay);
			}
			else
			{

				UI_CameraFX.Instance.CircleOut(target.position, AnimationFinished, delay);
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



		#endregion
	}
}