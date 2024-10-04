using Blabbers.Game00;
using NaughtyAttributes;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Blabbers",
				 "Information",
				 "Shows the Level Info popup")]
	[AddComponentMenu("")]
	public class PopupLevelInfo : Command
	{
		private static UI_PopupLevelInfo _popupInstance;
		private static UI_PopupLevelInfo PopupInstance
		{
			get
			{
				if (!_popupInstance)
				{
					_popupInstance = Instantiate(Resources.Load<UI_PopupLevelInfo>("UI/--Popup--LevelInfo"));
					DontDestroyOnLoad(_popupInstance.gameObject);
				}				
				return _popupInstance;
			}
		}
		
		[LocalizedStringOptions(hasBigTextArea = true)]
		public LocalizedString informationText;
		[ShowAssetPreview]
		public Sprite image;
		public float imageScaleMultiplier = 1;
		public bool playTTS = true;

		#region Public members

		public override void OnEnter()
		{
			PopupInstance.Setup(informationText, image, imageScaleMultiplier, HandleOnClosedPopup);
			PopupInstance.ShowPopup();
			if (playTTS)
			{
				LocalizationExtensions.PlayTTS(informationText.Key);
			}
		}

		void HandleOnClosedPopup()
		{
			Continue();
		}

		public override Color GetButtonColor()
		{
			return new Color32(184, 210, 235, 255);
		}

		public override string GetSummary()
		{
			string namePrefix = "";			
			if (informationText.HasUnsavedChanges())
			{
				namePrefix = "<color=red><b>* UNSAVED CHANGES *</b></color> ";
			}
			return namePrefix + informationText.GetRawText();
		}

		public override void OnCommandAdded(Block parentBlock)
		{
			if (informationText != null)
			{
				informationText.OverrideLocKey(LocalizedString.GenerateLocKey());
			}
		}
		#endregion
	}
}