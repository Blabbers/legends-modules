using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using Blabbers.Game00;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu]
public class CharacterSay : ScriptableObject
{
	public Sprite character;
	public string key;
	[ReadOnly] public string text;
	public bool allowContinue = true;
	public bool allowSkip = true;
	public bool playTTS = true;
    public UnityEvent OnStart;
	public UnityEvent OnIsOver;

	public void Execute(float delay = 0f)
	{
		if (string.IsNullOrEmpty(key))
			return;

		text = LocalizationExtensions.LocalizeText(key);

		Routine.Start(MyRoutine());

		IEnumerator MyRoutine()
		{
			if (delay > 0f)
			{
				yield return Routine.WaitSeconds(delay);
			}

			if (playTTS && ProgressController.GameProgress.enableAutomaticTTS)
			{
				LocalizationExtensions.PlayTTS(key);
			}
            OnStart?.Invoke();
			Singleton.Get<UI_PopupDialogue>().Execute(this, allowContinue, allowSkip);
		}
	}

	public void Stop()
	{
		Singleton.Get<UI_PopupDialogue>().HidePopup();
	}

	public void PauseTimeline(bool value)
	{
		var cutsceneController = Singleton.Get<InitialCutsceneController>().Instance;
		if (cutsceneController)
		{
			cutsceneController.PauseTimeline(value);
		}
	}
    
    // Inspector button for testing during runtime
    [Button()]
    public void TestExecution()
    {
        Execute(0f);
    }
}