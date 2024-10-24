﻿using System.Collections;
using Blabbers.Game00;
using System.Collections.Generic;
using BeauRoutine;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
using Animancer.Editor;
[CustomPropertyDrawer(typeof(Question), true)]
public class QuestionDrawer : PropertyDrawer
{
	private LocalizedString descriptionLoc = null;

	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
	{
		// Assigning events on the property editor like if this was OnEnable
		if (descriptionLoc == null)
		{
			var questionObj = (Question)property.GetValue();
			descriptionLoc = (questionObj).questionDescription;
			descriptionLoc.OnSave -= HandleOnSave;
			descriptionLoc.OnSave += HandleOnSave;
			void HandleOnSave()
			{
				for (int i = 0; i < questionObj.answers.Count; i++)
				{
					var answerKey = $"{descriptionLoc.Key}_answer_{i}";
					LocalizationExtensions.EditorSaveToLanguageJson(answerKey, questionObj.answers[i]);
				}
			}
			descriptionLoc.OnLoad -= HandleOnLoad;
			descriptionLoc.OnLoad += HandleOnLoad;
			void HandleOnLoad(string value)
			{
				for (int i = 0; i < questionObj.answers.Count; i++)
				{
					var key = $"{descriptionLoc.Key}_answer_{i}";
					questionObj.answers[i] = LocalizationExtensions.EditorLoadFromLanguageJson(key);
				}
			}
		}

		var internalQuestionProp = property.FindPropertyRelative("questionDescription");
		var internalAnswersProp = property.FindPropertyRelative("answers");

		EditorGUI.PropertyField(rect, internalQuestionProp);
		rect.y += 40f;

		EditorGUI.PropertyField(rect, internalAnswersProp);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float extraHeight = 170f;

		return base.GetPropertyHeight(property, label) + extraHeight;
	}
}
#endif

[System.Serializable]
public class Question
{
	public LocalizedString questionDescription;	
	[Header("The FIRST answer in the list should be the CORRECT one.")]	
	public List<string> answers;
}

public class UI_PopupQuestion : UI_PopupWindow, ISingleton
{
	public bool enableQuestionTTS = true;
	public Transform selectedOptionFrame;
	public  Color selectedButtonColor;
	public Color disabledBtnColor;
	private Color originalBtnColor;
	public Transform PopupParent;
	public Transform ButtonsParent;
	public Button btnConfirm;
	public TextMeshProUGUI QuestionDescriptionText;
	public List<TextMeshProUGUI> AnswerTests;
	[SerializeField] List<string> answerKeys;

	public MotionTweenPlayer starMotionPlayer;
	public MotionTween motionStarCorrect, motionStarWrong;
	public AudioSFX sfxExtraStar;

	public bool shouldShowAnswerAnimationFeedback;
	private UnityAction<bool,int> OnAnswered;
	private Color originalTextColor;
	

	public bool QuestionWasAnsweredThisLevel { get; private set; }  = false;
	public bool ChoseCorrectly { get; private set; }
	public int SelectedAnswerId { get; private set; }

	public void OnCreated() {}

	private void Awake()
	{
		originalTextColor = AnswerTests[0].color;
		originalBtnColor = btnConfirm.GetComponent<Image>().color;
		EnableBtnConfirm(false);
		SceneManager.sceneUnloaded += HandleSceneLoaded;
	}

	private void OnDestroy()
	{
		SceneManager.sceneUnloaded -= HandleSceneLoaded;
	}

	private void HandleSceneLoaded(Scene arg0)
	{
		QuestionWasAnsweredThisLevel = false;
		ChoseCorrectly = false;
	}

	public void EnableBtnConfirm(bool value)
	{
		btnConfirm.interactable = value;
		btnConfirm.GetComponent<CanvasGroup>().alpha = value ? 1.0f : 0.2f;
		btnConfirm.GetComponent<Image>().color = value ? originalBtnColor : disabledBtnColor;
		if (value)
		{
			btnConfirm.GetComponent<MotionTweenPlayer>().PlayTween();
		}
	}
	
	public void HighlightSelectedButton(Transform buttonTransform)
	{
		ResetButtonAndTextColors();
		selectedOptionFrame.gameObject.SetActive(true);
		selectedOptionFrame.SetParent(buttonTransform);
		selectedOptionFrame.localPosition = Vector3.zero;
		buttonTransform.GetComponent<Image>().color = selectedButtonColor;
		buttonTransform.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
	}

	private void DisableSelectedButtonFrame()
	{
		selectedOptionFrame.SetParent(this.transform);
		selectedOptionFrame.gameObject.SetActive(false);
	}

	public void ClickOption(int id)
	{
		//Make TTS speak the option
		if (SelectedAnswerId != id)
		{
			LocalizationExtensions.PlayTTS(answerKeys[id]);
		}

		SelectedAnswerId = id;
		EnableBtnConfirm(true);

		//var clickedBtnTransform = AnswerTests[id].transform.parent;

		//if (shouldShowAnswerAnimationFeedback)
		//{
		//	// Position star at the correct place
		//	starMotionPlayer.gameObject.SetActive(true);
		//	starMotionPlayer.transform.position = clickedBtnTransform.position;
		//}
		//var clickedBtn = clickedBtnTransform.GetComponent<Button>();

		//var answeredCorrectly = id == 0;
		//SelectedAnswerId = id;

		//if (answeredCorrectly)
		//{
		//	if (shouldShowAnswerAnimationFeedback)
		//	{
		//		motionStarCorrect.PlaySequence(starMotionPlayer);
		//		sfxExtraStar.PlaySelectedIndex(1);
		//		clickedBtn.image.DOColor(Color.green, 0.5f);
		//	}
		//	CorrectOptionMethod();
		//}
		//else
		//{
		//	if (shouldShowAnswerAnimationFeedback)
		//	{
		//		motionStarWrong.PlaySequence(starMotionPlayer);
		//		sfxExtraStar.PlaySelectedIndex(0);
		//		clickedBtn.image.DOColor(Color.red, 0.5f);
		//	}
		//	WrongOptionMethod();
		//}
	}

	public void ConfirmOption()
	{
		int id = SelectedAnswerId;

		var clickedBtnTransform = AnswerTests[id].transform.parent;

		if (shouldShowAnswerAnimationFeedback)
		{
			// Position star at the correct place
			starMotionPlayer.gameObject.SetActive(true);
			starMotionPlayer.transform.position = clickedBtnTransform.position;
		}
		var clickedBtn = clickedBtnTransform.GetComponent<Button>();

		var answeredCorrectly = id == 0;
		SelectedAnswerId = id;

		if (answeredCorrectly)
		{
			if (shouldShowAnswerAnimationFeedback)
			{
				motionStarCorrect.PlaySequence(starMotionPlayer);
				sfxExtraStar.PlaySelectedIndex(1);
				clickedBtn.image.DOColor(Color.green, 0.5f);
			}
			CorrectOptionMethod();
		}
		else
		{
			if (shouldShowAnswerAnimationFeedback)
			{
				motionStarWrong.PlaySequence(starMotionPlayer);
				sfxExtraStar.PlaySelectedIndex(0);
				clickedBtn.image.DOColor(Color.red, 0.5f);
			}
			WrongOptionMethod();
		}
	}

	void ResetButtonAndTextColors()
	{
		foreach (var text in AnswerTests)
		{
			text.transform.parent.GetComponent<Image>().color = Color.white;
			text.color = originalTextColor;
		}
	}
	
	public void ShowQuestion(Question question, bool showAnswerAnimationFeedback, UnityAction<bool, int> onAnsweredCallback = null)
	{
		base.ShowPopup();
		ResetButtonAndTextColors();
		DisableSelectedButtonFrame();
		shouldShowAnswerAnimationFeedback = showAnswerAnimationFeedback;

		AudioController.Instance.FadeGameplayVolume(0.1f);
		AudioController.Instance.FadeMusicVolume(AudioController.MusicVolumeLow);
		// Loads the question
		this.QuestionDescriptionText.text = question.questionDescription;
		answerKeys = new List<string>();

		OnAnswered = onAnsweredCallback;

		// Disable all
		foreach (var answerTexts in AnswerTests)
		{
			answerTexts.transform.parent.gameObject.SetActive(false);
		}
		// Enables the needed ones and loads them up
		for (int i = 0; i < question.answers.Count; i++)
		{
			var answerKey = $"{question.questionDescription.Key}_answer_{i}";
			var textMesh = AnswerTests[i];

			answerKeys.Add(answerKey);
			textMesh.text = LocalizationExtensions.LocalizeText(answerKey, applyColorCode: false);

			var buttonObject = textMesh.transform.parent.gameObject;
			// Enable back the needed ones
			buttonObject.gameObject.SetActive(true);


			var button = buttonObject.GetComponent<Button>();
			button.interactable = true;


			#region old delay before confirm button was added
			//Routine.Start(Run());
			//IEnumerator Run()
			//{
			//	var button = buttonObject.GetComponent<Button>();
			//	button.interactable = false;
			//	yield return Routine.WaitRealSeconds(3f);
			//	button.interactable = true;
			//} 
			#endregion
		}
		ShuffleAnswers();

		if (enableQuestionTTS) LocalizationExtensions.PlayTTS(question.questionDescription);
	}

	public void ShuffleAnswers()
	{
		var indexes = new List<int>();
		var items = new List<Transform>();
		for (int i = 0; i < ButtonsParent.childCount; ++i)
		{
			indexes.Add(i);
			items.Add(ButtonsParent.GetChild(i));
		}
		var titleLabel = items[0];

		foreach (var item in items)
		{
			item.SetSiblingIndex(indexes[Random.Range(0, indexes.Count)]);
		}
		// Since the first item is a label, we move it back to the top.
		titleLabel.SetSiblingIndex(0);
		btnConfirm.transform.parent.SetSiblingIndex(ButtonsParent.childCount-1);
	}

	public void CorrectOptionMethod()
	{
		Debug.Log("CorrectOptionMethod()");

		QuestionWasAnsweredThisLevel = true;
		ChoseCorrectly = true;
		ClosePopup();
	}

	public void WrongOptionMethod()
	{
		Debug.Log("WrongOptionMethod()");

		QuestionWasAnsweredThisLevel = true;
		ChoseCorrectly = false;
		ClosePopup();
	}

	void ClosePopup()
	{
		AudioController.Instance.FadeResetGameplayVolume();
		AudioController.Instance.FadeResetMusicVolume();

		Routine.Start(Run());
		IEnumerator Run()
		{
			Debug.Log("UI_PopupQuestion.ClosePopup()");
			//yield return Routine.WaitSeconds(1.0f);

			for (int i = 0; i < AnswerTests.Count; i++)
			{
				var button = AnswerTests[i].transform.parent.gameObject.GetComponent<Button>();
				button.interactable = false;
			}

			EnableBtnConfirm(false);

			//Changed from WaitSeconds to WaitRealSeconds (it wasn't working with the game paused)
			yield return Routine.WaitRealSeconds(0.5f);
			OnAnswered?.Invoke(ChoseCorrectly, SelectedAnswerId);
			base.HidePopup();
		}
	}

}