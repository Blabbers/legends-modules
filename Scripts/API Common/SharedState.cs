using UnityEngine;
using SimpleJSON;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
public static class SharedStateEditor
{
	static SharedStateEditor()
	{
		EditorApplication.playModeStateChanged += ModeChanged;
	}

	static void ModeChanged(PlayModeStateChange playModeState)
	{
		if (playModeState == PlayModeStateChange.EnteredEditMode)
		{
			Debug.Log("Entered Edit mode.");
			SharedState.Init();
		}
	}
}
#endif

// SharedState class handles all the "game save" info as static values
public static class SharedState
{
	public static JSONNode startGameData;
	public static JSONNode languageDefs;

	// This is defined inside Loader.Awake() through the "GameConfig" ScriptableObject.
	public static int maxProgress;
	
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	public static void Init()
	{
		languageDefs = null;
		startGameData = null;
		//maxProgress = 0;
		maxProgress = GameData.Instance.maxProgress;
	}
}