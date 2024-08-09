using Blabbers.Game00;
using NaughtyAttributes;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UI_PopupCountdown : UI_PopupWindow, ISingleton
{
	#region Variables
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

	#endregion

	void Awake()
	{
		Singleton.Get<GameplayController>().OnPause += HandleOnPause;

		if (showGoText) goText = LocalizationExtensions.LocalizeText(goKey);
		gameObject.SetActive(false);

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
		if (Singleton.Get<GameplayController>().gameOver) return;

		StartCountdown();
	}


	public void ExternalCountDown(Action callback = null)
	{
		if (!enableCountdown) return;

		isActive = true;
		StartCountdown(pauseTimeScale, callback);
	}

	void StartCountdown(bool pauseTimescale = false, Action callback = null)
	{
		if (!enableCountdown) return;

		if (pauseTimescale) Time.timeScale = 0;
		StartCountdownGeneric();

		countCoroutine = _Counting();
		StartCoroutine(countCoroutine);

		IEnumerator _Counting()
		{

			for (int i = 3; i >= 1; i--)
			{
				countText.text = "" + i;
				OnCountdownTick?.Invoke();
				yield return new WaitForSecondsRealtime(tickDuration);
			}

			if (showGoText)
			{
				countText.text = goText + "!";
				yield return new WaitForSecondsRealtime(tickDuration);
			}

			CountDownFinished(callback);
		}

	}

	void StartCountdownGeneric()
	{
		if (!enableCountdown) return;

		TogglePopup(true);
		group.alpha = 1.0f;

		if (hideHUD)
		{
			Singleton.Get<UI_GameplayHUD>().HideFullHUD();
		}
	}


	void CountDownFinished(Action callback = null)
	{
		OnCountdownFinished?.Invoke();
		TogglePopup(false);

		if (hideHUD) Singleton.Get<UI_GameplayHUD>().ShowFullHUD();
		if (pauseTimeScale) Time.timeScale = 1.0f;

		callback?.Invoke();
	}


	void TogglePopup(bool active)
	{
		isActive = active;
		popupParent.gameObject.SetActive(active);
	}

	public void SetEnableCountdown(bool value)
	{
		enableCountdown = value;
	}
}
