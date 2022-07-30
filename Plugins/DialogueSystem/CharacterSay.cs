using System.Collections;
using System.IO;
using BeauRoutine;
using Blabbers.Game00;
using NaughtyAttributes;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Blabbers.Game00.LoadSDKText;

[CreateAssetMenu]
public class CharacterSay : ScriptableObject
{
	public Sprite character;
	public string key;	
	[TextArea]
	public string text;
	public bool allowContinue = true;
	public bool allowSkip = true;
	public bool playTTS = true;
	public UnityEvent OnStart;
	public UnityEvent OnIsOver;

	public void Execute(float delay = 0f)
	{
		if (!HasKey)
			return;

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

	#region SaveLoadFromEditor
	public bool HasKey => !string.IsNullOrEmpty(key);
	[Button()]
	public void SaveToLanguageJson()
	{
		if (!HasKey)
		{
			key = this.name;
		}
		LocalizationExtensions.EditorSaveToLanguageJson(key, text, this);
	}
	[Button()]
	public void LoadFromLanguageJson()
	{
		if (!HasKey)
		{
			key = this.name;
		}

		this.text = LocalizationExtensions.EditorLoadFromLanguageJson(key, this);
	}
	#endregion
}