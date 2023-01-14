using UnityEngine;

namespace Fungus
{
    [CommandInfo("Blabbers",
                 "Pause Game",
                 "Pauses the game.")]
    [AddComponentMenu("")]
    public class PauseGame : Command
    {
        public bool pause = true;
        #region Public members

        public override void OnEnter()
        {
            //TODO: Implement PAUSE call.
            Continue();
        }

		public override Color GetButtonColor()
        {
            return new Color32(216, 228, 170, 255);
        }

        #endregion
    }
}