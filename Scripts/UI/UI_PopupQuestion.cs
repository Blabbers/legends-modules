using System.Collections;
using Blabbers.Game00;
using System.Collections.Generic;
using BeauRoutine;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

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
	public Transform PopupParent;
	public Transform ButtonsParent;
	public TextMeshProUGUI QuestionDescriptionText;
	public List<TextMeshProUGUI> AnswerTests;

	public MotionTweenPlayer starMotionPlayer;
	public MotionTween motionStarCorrect, motionStarWrong;
	public AudioSFX sfxExtraStar;

	private UnityAction<bool> OnAnswered;

	[ReadOnly]
	public bool answeredCorrectly = false;

	public bool ChoseCorrectly { get; private set; }

	public void OnCreated() { }

	public void ClickOption(int id)
	{
		var clickedBtnTransform = AnswerTests[id].transform.parent;
		// Position star at the correct place
		starMotionPlayer.gameObject.SetActive(true);
		starMotionPlayer.transform.position = clickedBtnTransform.position;
		var clickedBtn = clickedBtnTransform.GetComponent<Button>();

		answeredCorrectly = id == 0;
		if (answeredCorrectly)
		{
			motionStarCorrect.PlaySequence(starMotionPlayer);
			sfxExtraStar.PlaySelectedIndex(1);
			clickedBtn.image.DOColor(Color.green, 0.5f);
			CorrectOptionMethod();
		}
		else
		{
			motionStarWrong.PlaySequence(starMotionPlayer);
			sfxExtraStar.PlaySelectedIndex(0);
			clickedBtn.image.DOColor(Color.red, 0.5f);
			WrongOptionMethod();
		}
	}

	public void ShowQuestion(Question question, UnityAction<bool> onAnsweredCallback = null)
	{
		base.ShowPopup();
		AudioController.Instance.FadeGameplayVolume(0.1f);
		AudioController.Instance.FadeMusicVolume(0.02f);
		// Loads the question
		this.QuestionDescriptionText.text = question.questionDescription;		

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
			
			textMesh.text = LocalizationExtensions.LocalizeText(answerKey, applyColorCode: false);

			var buttonObject = textMesh.transform.parent.gameObject;
			// Enable back the needed ones
			buttonObject.gameObject.SetActive(true);

			Routine.Start(Run());
			IEnumerator Run()
			{
				var button = buttonObject.GetComponent<Button>();
				button.interactable = false;
				yield return Routine.WaitRealSeconds(3f);
				button.interactable = true;
			}
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

		foreach (var item in items)
		{
			item.SetSiblingIndex(indexes[Random.Range(0, indexes.Count)]);
		}
	}

	public void CorrectOptionMethod()
	{
		ChoseCorrectly = true;
		ClosePopup();
	}

	public void WrongOptionMethod()
	{
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
			yield return Routine.WaitSeconds(1.0f);
			OnAnswered?.Invoke(ChoseCorrectly);
			base.HidePopup();
		}
	}
}