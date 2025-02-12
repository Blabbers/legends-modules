using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using Blabbers.Game00;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_LevelSelect : MonoBehaviour, ISingleton
{
    public float buttonDropInterval = 0.1f;
    public float pathFadeInDuration = 1f;
    public MotionTween startTween;
    public Transform buttonsParent;
    public Image pathImage;

    public UI_PopupPlayAgainWarning playAgainPopup;
    public float TotalButtonDropTime => buttonDropInterval *  ButtonMotions.Length;

    private MotionTweenPlayer[] buttonMotions;
    public MotionTweenPlayer[] ButtonMotions
    {
        get
        {
            if (buttonMotions == null || buttonMotions.Length == 0)
            {
                buttonMotions = buttonsParent.GetComponentsInChildren<MotionTweenPlayer>();
            }

            return buttonMotions;
        }
    } 

    void OnEnable()
    {
        //Path fade in
        if (pathImage != null)
        {
            var pathImageColor = pathImage.color;
            pathImageColor.a = 0f;
            pathImage.color = pathImageColor;
            pathImage.DOFade(1f, pathFadeInDuration);
        }

        // Start buttons animation
        var buttonAmount = ButtonMotions.Length;
        Routine.Start(Run());
        IEnumerator Run()
        {
            playAgainPopup.ShowPopup();
            for (int i = 0; i < buttonAmount; i++)
            {
                if (ButtonMotions[i] != null)
                {
                    ButtonMotions[i].gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < buttonAmount; i++)
            {
                yield return Routine.WaitSeconds(buttonDropInterval);
                var buttonMotion = ButtonMotions[i];
                if (buttonMotion != null)
                {
                    buttonMotion.gameObject.SetActive(true);
                    startTween.PlaySequence(buttonMotion, false);
                }
            }

            yield return Routine.WaitSeconds(buttonDropInterval + 0.25f);
            
        }
    }

    public void OnCreated()
    {
    }
}
