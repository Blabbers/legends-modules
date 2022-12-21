using System.Collections;
using Blabbers.Game00;
using System.Collections.Generic;
using BeauRoutine;
using Blabbers;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Scripting.APIUpdating;


[System.Serializable]
public class Question
{
    public string questionDescriptionKey;
    public CorrectOption correctAnswer;
    public string[] answersKeys;
}

public class UI_PopupQuestion : UI_PopupWindow, ISingleton
{
    public bool enableQuestionTTS = true;

    private CorrectOption CorrectOption;
    public Transform PopupParent;
    public Transform ButtonsParent;
    public TextMeshProUGUI QuestionDescriptionText;
    public List<TextMeshProUGUI> AnswerTests;

    public MotionTweenPlayer starMotionPlayer;
    public MotionTween motionStarCorrect, motionStarWrong;
    public AudioSFX sfxExtraStar;

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

        answeredCorrectly = id == (int) CorrectOption; 
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

    public void ShowQuestion(Question question)
    {
        base.ShowPopup();
        AudioController.Instance.FadeGameplayVolume(0.1f);
        AudioController.Instance.FadeMusicVolume(0.02f);
        // Loads the question
        this.QuestionDescriptionText.LocalizeText(question.questionDescriptionKey);
        this.CorrectOption = question.correctAnswer;


        // Disable all
        foreach (var answerTexts in AnswerTests)
        {
            answerTexts.transform.parent.gameObject.SetActive(false);
        }
        // Enables the needed ones and loads them up
        for (int i = 0; i < question.answersKeys.Length; i++)
        {
            var answerKey = question.answersKeys[i];
            var textMesh = AnswerTests[i];
			//textMesh.LocalizeText(answerKey);
			textMesh.LocalizeText(answerKey,false);

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
         
        if(enableQuestionTTS) LocalizationExtensions.PlayTTS(question.questionDescriptionKey);
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
            base.HidePopup();
        }

        //PopupParent.DOScale(0.0f, 0.5f).OnComplete(() =>
        //{
        //    HidePopup();
        //});

    }
}

public enum CorrectOption
{
    OptionA, OptionB, OptionC, OptionD
}