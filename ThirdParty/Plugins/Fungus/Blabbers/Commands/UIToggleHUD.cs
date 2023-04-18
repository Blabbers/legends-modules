using Blabbers.Game00;
using UnityEngine;

namespace Fungus
{
    [CommandInfo("Blabbers",
                 "Toggle HUD",
				 "Toggles the game HUD")]
    [AddComponentMenu("")]
    public class UIToggleHUD : Command
    {
        public bool enable = true;
		public bool instantly = false;
		#region Public members

		public override void OnEnter()
        {
            Singleton.Get<UI_GameplayHUD>().ToggleDisplay(enable, instantly);
            Continue();
        }

		public override Color GetButtonColor()
        {
            return new Color32(216, 228, 170, 255);
        }

        #endregion
    }
}