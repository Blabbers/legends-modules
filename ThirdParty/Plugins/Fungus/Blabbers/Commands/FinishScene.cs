using Blabbers.Game00;
using UnityEngine;

namespace Fungus
{
    [CommandInfo("Blabbers",
				 "Finish Scene",
                 "Finishes the current scene.")]
    [AddComponentMenu("")]
    public class FinishScene : Command
    {
        #region Public members

        public override void OnEnter()
        {
			var cutscene = Singleton.Get<CutsceneController>();
			if (cutscene)
			{
				cutscene.FinishCutscene();
				Continue();
				return;
			}
			var simulation = Singleton.Get<SimulationEnvironment>();
            if (simulation)
			{
                simulation.FinishSimulation();
				Continue();
				return;
			}
			var gameplay = Singleton.Get<GameplayController>();
			if (gameplay)
			{
				gameplay.Victory();
				Continue();
                return;
			}
			// TODO: There's a chance this is called from the customization controller too, we need to check what would be called here.
			Continue();
			return;
		}

		public override Color GetButtonColor()
        {
            return new Color32(216, 228, 170, 255);
        }

        #endregion
    }
}