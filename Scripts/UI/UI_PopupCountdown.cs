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
    [Header("Runtime")]
    [SerializeField] string goText;
    public bool isActive = false;
    IEnumerator countCoroutine;

	[Foldout("Configs")] public bool pauseTimeScale = false;
	[Foldout("Configs")] public float tickDuration;
	[Foldout("Configs")] public string goKey;

    [Header("Events")]
    public UnityEvent OnCountdownTick;
    public UnityEvent OnCountdownFinished;

	[Foldout("Components")] public TextMeshProUGUI countText;
	[Foldout("Components")] public Transform popupParent;


    void Awake()
	{
        Debug.Log("<UI_PopupCountdown> Awake()".Colored("white"));

        //Singleton.Get<GameplayController>().OnPause += HandleOnPause;
        //Singleton.Get<UI_PopupLevelInfo>().OnClose.AddListener(ExternalCountDown);

        //OnCountdownFinished.AddListener(Singleton.Get<UI_GameplayHUD>().ResetLives);
        //OnCountdownFinished.AddListener(Singleton.Get<GameStatusTracker>().ResetLives);

        goText = LocalizationExtensions.LocalizeText(goKey);

		if (Singleton.Get<GameplayController>().showLevelInfo)
		{
            gameObject.SetActive(false);
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
        Debug.Log("ExternalCountDown()".Colored("orange"));
        isActive = true;


		if (pauseTimeScale) StartCountdown_Pause();
        else StartCountdown();
    }


    void StartCountdown_Pause()
    {

		Debug.Log("<UI_PopupCountdown> StartCountdown_Pause()");

		Time.timeScale = 0;

		TogglePopup(true);

		countCoroutine = _Counting();
		StartCoroutine(countCoroutine);

		IEnumerator _Counting()
		{
		

			for (int i = 3; i >= 1; i--)
			{
				countText.text = "" + i;
				yield return new WaitForSecondsRealtime(tickDuration);
			}

			countText.text = goText + "!";
			yield return new WaitForSecondsRealtime(tickDuration);

			CountDownFinished();
		}

	}

    void StartCountdown()
    {
		Debug.Log("<UI_PopupCountdown> StartCountdown()");

		TogglePopup(true);

        countCoroutine = _Counting();
        StartCoroutine(countCoroutine);

        IEnumerator _Counting()
        {
           

            for (int i = 3; i >= 1; i--)
            {
                countText.text = "" + i;
                yield return new WaitForSeconds(tickDuration);
            }

            countText.text = goText + "!";
            yield return new WaitForSeconds(tickDuration);

            CountDownFinished();
        }

    }



    void CountDownFinished()
    {
        Debug.Log("<UI_PopupCountdown> CountDownFinished()");

        OnCountdownFinished?.Invoke();
        TogglePopup(false);

		if (pauseTimeScale) Time.timeScale = 1.0f;

		//Singleton.Get<GameplayController>().OnCountdownFinished?.Invoke();
	}


    void TogglePopup(bool active)
    {
        isActive = active;
        popupParent.gameObject.SetActive(active);
    }


}
