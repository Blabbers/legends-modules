using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using Blabbers.Game00;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelSelect : MonoBehaviour
{
    public float buttonDropInterval = 0.1f;
    public float pathFadeInDuration = 1f;
    public MotionTween startTween;
    public Transform buttonsParent;
    public Image pathImage; 
    void OnEnable()
    {
        // Path fade in
        var pathImageColor = pathImage.color;
        pathImageColor.a = 0f;
        pathImage.color = pathImageColor;
        pathImage.DOFade(1f, pathFadeInDuration);
        
        // Start buttons animation
        var buttonMotions = buttonsParent.GetComponentsInChildren<MotionTweenPlayer>();
        var buttonAmount = buttonMotions.Length;
        Routine.Start(Run());
        IEnumerator Run()
        {
            for (int i = 0; i < buttonAmount; i++)
            {
                if(buttonMotions[i] != null)
                {
                    buttonMotions[i].gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < buttonAmount; i++)
            {
                yield return Routine.WaitSeconds(buttonDropInterval);
                var buttonMotion = buttonMotions[i];
                if(buttonMotion = null)
                {
                    buttonMotion.gameObject.SetActive(true);
                    startTween.PlaySequence(buttonMotion, false);
                }
            }
        }
    }
    
    void Update()
    {
        
    }
}
