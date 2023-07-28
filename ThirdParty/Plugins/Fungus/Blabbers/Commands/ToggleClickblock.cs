using Blabbers.Game00;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Blabbers",
				 "Clickblock",
				 "Toggles Clickblock.")]
	[AddComponentMenu("")]
	public class ToggleClickblock : Command
	{
		public bool active = true;
		#region Public members

		public override void OnEnter()
		{
			Debug.Log($"ToggleClickBlock.OnEnter(): {active}");
			UI_Clickblock.Instance.ToggleClickBlock(active);
			Continue();
		}

		public override Color GetButtonColor()
		{
			return new Color32(232, 130, 130, 255);
		}

		public override string GetSummary()
		{
			string namePrefix = $"ToggleClickBlock: {active}" ;

			if (active) {
				namePrefix = $"Enable Clickblock";
			}
			else
			{
				namePrefix = $"Disable Clickblock";
			}

			return namePrefix;
		}


		#endregion
	}
}