using BennyKok.RuntimeDebug.Actions;
using BennyKok.RuntimeDebug.Attributes;
using BennyKok.RuntimeDebug.Components;
using BennyKok.RuntimeDebug.Systems;
using Blabbers.Game00;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Cheats : RuntimeDebugBehaviour {


	public string langCode;
	public float timeScale;

	#region Cheats Screen
	[DebugAction]
	public void ReloadScene()
	{
		Debug.Log($"{ScriptName()}" + $".ReloadScene() ".Colored("white"));
		InstantReloadScene();
	}

	[DebugAction]
	public void AdvanceLevel()
	{

		Debug.Log($"{ScriptName()}" + $".AdvanceLevel() ".Colored("white"));
		RuntimeDebugSystem.Instance.runtimeDebugUI.TogglePanel();
		Singleton.Get<ProgressController>().FinishLevel();
	}

	[DebugAction]
	public void EndScene()
	{
	
		RuntimeDebugSystem.Instance.runtimeDebugUI.TogglePanel();

		if (Singleton.Get<GameplayController>() != null)
		{
			Debug.Log($"{ScriptName()}" + $".LevelVictory() ".Colored("white"));

			Singleton.Get<GameplayController>().Victory();
			return;
		}

		if (Singleton.Get<SimulationEnvironment>() != null)
		{
			Debug.Log($"{ScriptName()}" + $".SkipSimulation() ".Colored("white"));

			Singleton.Get<SimulationEnvironment>().FinishSimulation();
			return;
		}

		if (Singleton.Get<CutsceneController>() != null)
		{
			Debug.Log($"{ScriptName()}" + $".SkipCutscene() ".Colored("white"));
			Singleton.Get<CutsceneController>().LoadNextScene();
			return;
		}

		Debug.Log($"{ScriptName()}" + $"This scene cannot be ended".Colored("red"));
	}

	[DebugAction]
	public void ResetProgress()
	{
		Debug.Log($"{ScriptName()}" + $".ResetProgress() ".Colored("white"));

		GameData.Instance.ClearAllPlayerPrefs();
		InstantReloadScene();


	}
	#endregion



	private void OnEnable()
	{
	}

	protected virtual void Awake()
	{
		if (RuntimeDebugSystem.IsSystemEnabled)
		{
			LanguageCheat();
			TimeScaleCheat();

			base.Awake();
		}
	}
	

	void LanguageCheat()
	{
		var allLanguageNames = new string[2];
		allLanguageNames[0] = "en";
		allLanguageNames[1] = "es";

		var chooseLanguageAction = DebugActionBuilder.Flag()
			.WithName("Choose Language")
			.WithGroup("Language")
			.WithFlag("choose-language", allLanguageNames, false)
			//.WithShortcutKey("f8")
			.WithFlagListener((flag) =>
			{
				SelectLanguage(flag);
			}, false);

		RuntimeDebugSystem.RegisterActions(chooseLanguageAction);


		var applyLanguageAction = DebugActionBuilder.Button()
			.WithName("Apply Language")
			.WithGroup("Language")
			.WithAction(() => LoadLanguage(langCode));

		RuntimeDebugSystem.RegisterActions(applyLanguageAction);
	}


	void TimeScaleCheat()
	{

		RuntimeDebugSystem.RegisterActions(
			DebugActionBuilder.Button()
			.WithGroup("TimeScale")
			.WithName("Increase TimeScale")
			.WithAction(() => IncreaseTimeScale())
		);


		RuntimeDebugSystem.RegisterActions(
			DebugActionBuilder.Button()
			.WithGroup("TimeScale")
			.WithName("Decrease TimeScale")
			.WithAction(() => DecreaseTimeScale())
		);

		RuntimeDebugSystem.RegisterActions(
			DebugActionBuilder.Button()
			.WithGroup("TimeScale")
			.WithName("Reset TimeScale")
			.WithAction(() => ResetTimeScale())
		);
	}

	#region Shortcuts
	private void Update()
	{
#if DEVELOPMENT_BUILD || UNITY_CLOUD_BUILD || UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Home))
		{
			ReloadScene();
		}
			
#endif
	}


	#endregion

	#region Language
	void SelectLanguage(int id)
	{

		if (id == 0)
		{
			langCode = "en";
		}
		else if (id == 1)
		{
			langCode = "es";
		}
	}

	void LoadLanguage(string code)
	{
		Debug.Log($"{this}.LoadLanguage: {code}\n-");
		GameData.Instance.currentSelectedLangCode = code;


		InstantReloadScene();
	}


	#endregion

	void IncreaseTimeScale()
	{
		timeScale += 0.5f;
		Time.timeScale = timeScale;
	}

	void DecreaseTimeScale()
	{
		timeScale -= 0.5f;
		Time.timeScale = timeScale;
	}

	void ResetTimeScale()
	{
		timeScale = 1;
		Time.timeScale = timeScale;
	}

	void InstantReloadScene()
	{

		//RuntimeDebugSystem.Instance.Hide();
		RuntimeDebugSystem.Instance.runtimeDebugUI.TogglePanel();

		var scene = SceneManager.GetActiveScene();

		Debug.Log(ScriptName() + $".InstantReloadScene() ".Colored("white") +
			$"\nScene: [{scene.name}]");

		Singleton.Get<SceneLoader>().LoadScene(scene.name);
	}

	string ScriptName()
	{
		string header = "CHEAT USED\n".Colored("orange");
		string title = header + (this.GetType().ToString().Colored("orange"));
	

		return title;
	}

	

}
