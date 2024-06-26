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

public class UI_PopupCountdown : UI_PopupWindow, ISingleton
{

	[Foldout("Runtime")] public bool isActive = false;
    IEnumerator countCoroutine;

	[BoxGroup("Configs")] public bool enableCountdown = false;
	[BoxGroup("Configs")] public bool showGoText = false;
	[BoxGroup("Configs")] public bool hideHUD = true;
	[BoxGroup("Configs")] public bool pauseTimeScale = false;
	[BoxGroup("Configs")] public float tickDuration;
	[BoxGroup("Configs")] public UnityEvent OnCountdownTick;
	[BoxGroup("Configs")] public UnityEvent OnCountdownFinished;

	[Foldout("Components")] public CanvasGroup group;
	[Foldout("Components")] public string goKey;
	[Foldout("Components")] public TextMeshProUGUI countText;
	[Foldout("Components")] public Transform popupParent;
	[Foldout("Components")][SerializeField] string goText;


	void Awake()
	{
        Debug.Log("<UI_PopupCountdown> Awake()".Colored("white"));

        if (enableCountdown)
        {
			Singleton.Get<GameplayController>().OnPause += HandleOnPause;
			//Singleton.Get<UI_PopupLevelInfo>().OnClose.AddListener(ExternalCountDown);

			//OnCountdownFinished.AddListener(Singleton.Get<UI_GameplayHUD>().ResetLives);
			//OnCountdownFinished.AddListener(Singleton.Get<GameStatusTracker>().ResetLives);

			goText = LocalizationExtensions.LocalizeText(goKey);
			gameObject.SetActive(false);



			if (Singleton.Get<GameplayController>().showLevelInfo)
			{
				gameObject.SetActive(false);
			}
		} 
    }


    private bool isPaused;
    private void HandleOnPause(bool pause)
    {
        isPaused = pause;

        if (isPaused)
        {
            return;
        }

        StartCountdown();
    }


    public void ExternalCountDown()
    {
        //Debug.Log("ExternalCountDown()".Colored("orange"));
        isActive = true;


		if (pauseTimeScale) StartCountdown_Pause();
        else StartCountdown();
    }


    void StartCountdown_Pause()
    {

		Debug.Log("<UI_PopupCountdown> StartCountdown_Pause()");

		Time.timeScale = 0;

        StartCountdownGeneric();

		countCoroutine = _Counting();
		StartCoroutine(countCoroutine);

		IEnumerator _Counting()
		{
		

			for (int i = 3; i >= 1; i--)
			{
				countText.text = "" + i;
				yield return new WaitForSecondsRealtime(tickDuration);
			}

            if (showGoText)
            {
				countText.text = goText + "!";
				yield return new WaitForSecondsRealtime(tickDuration);
			}

			CountDownFinished();
		}

	}

    void StartCountdown()
    {
		Debug.Log("<UI_PopupCountdown> StartCountdown()");

        StartCountdownGeneric();

		countCoroutine = _Counting();
        StartCoroutine(countCoroutine);

        IEnumerator _Counting()
        {
          
            for (int i = 3; i >= 1; i--)
            {
                countText.text = "" + i;
                yield return new WaitForSecondsRealtime(tickDuration);
            }

			if (showGoText)
			{
				countText.text = goText + "!";
				yield return new WaitForSecondsRealtime(tickDuration);
			}

			CountDownFinished();
        }

    }


    void StartCountdownGeneric()
    {
		TogglePopup(true);
		group.alpha = 1.0f;

		if (hideHUD)
        {
            Singleton.Get<UI_GameplayHUD>().HideFullHUD();
        }
    }


    void CountDownFinished()
    {
        //Debug.Log("<UI_PopupCountdown> CountDownFinished()");

        OnCountdownFinished?.Invoke();
        TogglePopup(false);

		if (hideHUD) Singleton.Get<UI_GameplayHUD>().ShowFullHUD();

		if (pauseTimeScale) Time.timeScale = 1.0f;

		//Singleton.Get<GameplayController>().OnCountdownFinished?.Invoke();
	}


    void TogglePopup(bool active)
    {
        isActive = active;
        popupParent.gameObject.SetActive(active);
    }


}
