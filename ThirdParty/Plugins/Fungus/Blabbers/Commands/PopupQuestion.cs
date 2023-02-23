using UnityEngine;

namespace Fungus
{
	[CommandInfo("Blabbers",
				 "Question",
				 "Shows a multiple choice question popup. The alternatives are always scrambled.")]
	[AddComponentMenu("")]
	public class PopupQuestion : Command
	{
		private static UI_PopupQuestion _popupInstance;
		private static UI_PopupQuestion PopupInstance
		{
			get
			{
				if (!_popupInstance)
				{
					_popupInstance = Instantiate(Resources.Load<UI_PopupQuestion>("UI/--Popup--SimpleQuestion"));
					DontDestroyOnLoad(_popupInstance.gameObject);
				}				
				return _popupInstance;
			}
		}
		
		[LocalizedStringOptions(hasBigTextArea = true)]		
		//public bool playTTS = true;
		public Question question;
		#region Public members

		public override void OnEnter()
		{
			PopupInstance.ShowQuestion(question, HandleOnClosedPopup);
			//PopupInstance.Setup(informationText, image, HandleOnClosedPopup);
			PopupInstance.ShowPopup();
			//if (playTTS)
			//{
			//	LocalizationExtensions.PlayTTS(informationText.Key);
			//}
		}

		void HandleOnClosedPopup(bool answeredCorrectly)
		{
			// TODO: A gente pode colocar algo aqui no sistema caso ele acerte, talvez trazer a pontuacao do GameplayManager pra cá? Ou sei la
			Continue();
		}

		public override Color GetButtonColor()
		{
			return new Color32(184, 210, 235, 255);
		}

		public override string GetSummary()
		{
			string namePrefix = "";			
			if (question.questionDescription.HasUnsavedChanges())
			{
				namePrefix = "<color=red><b>* UNSAVED CHANGES *</b></color> ";
			}
			return namePrefix + question.questionDescription.GetRawText();
		}
		#endregion
	}
}