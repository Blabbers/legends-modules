using NaughtyAttributes;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerComponent : MonoBehaviour
{
	[Header("Settings")]
	[Tooltip("If the game pauses, this timer will still run, using unscaled time instead.")]
	public bool ignorePause = false;
	[Tooltip("Will only finish when something calls the FinishTimer() function from the outside.")]
	public bool unlimitedTimer = false;
	[HideIf(nameof(unlimitedTimer))]
	public float maxDuration = 0f;

	[Header("Optional")]
	public TextMeshProUGUI textToUpdate;
	private bool HasText => !unlimitedTimer && textToUpdate != null;
	[ShowIf(nameof(HasText))]
	public bool displayAsCountdown = false;

	[SerializeField]
	private float currentTimePassed;
	private Coroutine coroutine;

	public UnityEvent OnTimerFinished;

	[Button]
	public void StartTimer()
	{
		currentTimePassed = 0f;
		coroutine = StartCoroutine(TimerRoutine());
	}

	[Button]
	public void FinishTimer()
	{
		if(coroutine != null)
		{
			StopCoroutine(coroutine);
			coroutine = null;
			OnTimerFinished?.Invoke();
		}
	}

	IEnumerator TimerRoutine()
	{
		currentTimePassed = 0f;
		while (unlimitedTimer || currentTimePassed < maxDuration)
		{
			currentTimePassed += ignorePause ? Time.unscaledDeltaTime : Time.deltaTime;
			if (textToUpdate != null)
			{
				var displayTime = displayAsCountdown && !unlimitedTimer ? 
									Mathf.Max(maxDuration - currentTimePassed, 0f) : 
									Mathf.Min(currentTimePassed, maxDuration);

				var formatted = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(displayTime), Mathf.FloorToInt((displayTime - Mathf.Floor(displayTime)) * 100));
				textToUpdate.text = formatted;
			}
			yield return null;
		}
		FinishTimer();
	}
}
