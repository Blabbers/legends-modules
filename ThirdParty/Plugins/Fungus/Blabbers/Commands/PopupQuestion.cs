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
					SetPopupInstance(Resources.Load<UI_PopupQuestion>("UI/--Popup--SimpleQuestion"));					
				}
				return _popupInstance;
			}
		}
		private static void SetPopupInstance(UI_PopupQuestion prefab)
		{
			_popupInstance = Instantiate(prefab);
			DontDestroyOnLoad(_popupInstance.gameObject);
		}

		[Tooltip("Variable to store the value in.")]
		//[LocalizedStringOptions(hasBigTextArea = true)]		
		//public bool playTTS = true;		
		public Question question;

		[VariableProperty(typeof(BooleanVariable))]
		[SerializeField] private Variable setAnswerBoolTo;
		[SerializeField] private UI_PopupQuestion overrideQuestionPopup;
		[SerializeField] private bool showAnswerFeedback = true;

		#region Public members

		public override void OnEnter()
		{
			if (overrideQuestionPopup)
			{
				SetPopupInstance(overrideQuestionPopup);
			}
			PopupInstance.ShowQuestion(question, showAnswerFeedback, HandleOnClosedPopup);
			PopupInstance.ShowPopup();
			//if (playTTS)
			//{
			//	LocalizationExtensions.PlayTTS(informationText.Key);
			//}
		}

		void HandleOnClosedPopup(bool answeredCorrectly)
		{
			setAnswerBoolTo?.Apply(SetOperator.Assign, answeredCorrectly);
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

		public override void OnCommandAdded(Block parentBlock)
		{
			if (question != null)
			{
				question.questionDescription.OverrideLocKey(LocalizedString.GenerateLocKey());
			}
		}
		#endregion
	}
}