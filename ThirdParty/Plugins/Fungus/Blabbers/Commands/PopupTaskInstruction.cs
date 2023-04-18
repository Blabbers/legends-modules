using Blabbers.Game00;
using NaughtyAttributes;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Blabbers",
				 "Task",
				 "Shows the Task Instruction popup")]
	[AddComponentMenu("")]
	public class PopupTaskInstruction : Command
	{
		private static UI_TaskInstruction _popupInstance;
		private static UI_TaskInstruction PopupInstance
		{
			get
			{
				if (!_popupInstance)
				{
					_popupInstance = Instantiate(Resources.Load<UI_TaskInstruction>("UI/--Popup--TaskInstruction"));
					DontDestroyOnLoad(_popupInstance.gameObject);
				}				
				return _popupInstance;
			}
		}
		
		public bool show = true;

		[ShowIf(nameof(show))]
		public LocalizedString instructionText;
		[ShowIf(nameof(show))]
		public bool playTTS = false;

		#region Public members

		public override void OnEnter()
		{
			if (show)
			{
				PopupInstance.gameObject.SetActive(true);
				PopupInstance.Setup(instructionText);
				if (playTTS)
				{
					LocalizationExtensions.PlayTTS(instructionText.Key);
				}
			}
			else
			{
				PopupInstance.Hide();
			}
			Continue();
		}

		public override Color GetButtonColor()
		{
			return new Color32(216, 228, 170, 255);
		}

		public override string GetSummary()
		{
			string namePrefix = "";			
			if (instructionText.HasUnsavedChanges())
			{
				namePrefix = "<color=red><b>* UNSAVED CHANGES *</b></color> ";
			}
			return (show ? "<color=green>(ON)</color> " + namePrefix + instructionText.GetRawText() : "<color=red>(OFF)</color>");
		}

		public override void OnCommandAdded(Block parentBlock)
		{
			if (instructionText != null)
			{
				instructionText.OverrideLocKey(LocalizedString.GenerateLocKey());
			}
		}
		#endregion
	}
}