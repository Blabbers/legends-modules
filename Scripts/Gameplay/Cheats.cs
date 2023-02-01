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

		base.Awake();
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

	void SelectLanguage(int id)
	{

		if(id == 0)
		{
			langCode = "en";
		}else if(id == 1)
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
		return (this.GetType().ToString().Colored("orange"));
	}

	

}
