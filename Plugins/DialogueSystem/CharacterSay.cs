using System.Collections;
using System.IO;
using BeauRoutine;
using Blabbers.Game00;
using NaughtyAttributes;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
	public bool HasKey => !string.IsNullOrEmpty(key);

	public void Execute(float delay = 0f)
	{
		if (!HasKey)
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

	// Language json file manipulation, we save and load it through here, but the main function will probably go to the LocalizationExtensions
	[Button()]
	public void SaveToLanguageJson()
	{
		if (!HasKey)
		{
			key = this.name;
		}
		var json = GetLanguageJson();
		var langCode = "en";
		var node = json[langCode];
		node.Add(key, text);
		Debug.Log($"<color=cyan>File language.json was updated</color> \n<color=white>[{key}]: {node[key]}</color>", this);
		const string languageJSONFilePath = "language.json";
		string langFilePath = Path.Combine(Application.streamingAssetsPath, languageJSONFilePath);
		if (File.Exists(langFilePath))
		{
			File.WriteAllText(langFilePath, json.ToString());
		}
	}
	[Button()]
	public void LoadFromLanguageJson()
	{
		if (!HasKey)
		{
			key = this.name;
		}
		var json = GetLanguageJson();
		var langCode = "en";
		var node = json[langCode];
		if(node[key] == null)
		{
			Debug.Log($"<color=red>File language.json was NOT loaded to this asset bacause key [{key}] is NULL.</color>", this);
		}
		else if (node[key] == text)
		{
			Debug.Log($"<color=yellow>File language.json was NOT loaded to this asset since it has THE SAME VALUE.</color>", this);
		}
		else
		{
			Debug.Log($"<color=yellow>File language.json was loaded to this asset</color> \n<color=white>[{key}]: {node[key]}</color>", this);
			this.text = node[key];
		}
	}

	public JSONNode GetLanguageJson()
	{
		const string languageJSONFilePath = "language.json";
		// Load Dev Language File from StreamingAssets
		string langFilePath = Path.Combine(Application.streamingAssetsPath, languageJSONFilePath);
		if (File.Exists(langFilePath))
		{
			string langDataAsJson = File.ReadAllText(langFilePath);
			JSONNode langDefs = JSON.Parse(langDataAsJson);
			return (langDefs);
		}
		return "";
	}
}