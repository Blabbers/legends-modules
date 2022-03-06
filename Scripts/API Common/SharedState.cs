using UnityEngine;
using SimpleJSON;
using LoLSDK;

// SharedState class handles all the "game save" info as static values
public static class SharedState
{
	public static JSONNode startGameData;
	public static JSONNode languageDefs;

	// This is defined inside Loader.Awake() through the "GameConfig" ScriptableObject.
	public static int maxProgress;
}