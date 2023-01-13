using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;
using SimpleJSON;
using System.IO;
using Blabbers.Game00;
using System;

[DefaultExecutionOrder(-900)]
public class Loader : MonoBehaviour, ISingleton
{
	//Added variable to track device type
	public bool isMobile = false;

	// Relative to Assets /StreamingAssets/
	private const string languageJSONFilePath = "language.json";
	//private const string questionsJSONFilePath = "questions.json";
	private const string startGameJSONFilePath = "startGame.json";
	private GameData gameData => GameData.Instance;
	public bool loadMainMenuAfterInitialize = true;

	public void OnCreated()
	{
	}

	void Awake()
	{
		isMobile = CheckDeviceType();

		Debug.Log($"Post CheckDeviceType() \nDeviceType: {SystemInfo.deviceType} | operatingSystem: {SystemInfo.operatingSystem.ToString()}" +
		$"| isMobile: {isMobile}");


		if (LOLSDK.Instance != null && LOLSDK.Instance.IsInitialized)
		{
			return;
		}

		// Create the WebGL (or mock) object
#if UNITY_EDITOR
		ILOLSDK webGL = new LoLSDK.MockWebGL();
#elif UNITY_WEBGL
			ILOLSDK webGL = new LoLSDK.WebGL();
#endif

		// Initialize the object, passing in the WebGL
		LOLSDK.Init(webGL, gameData.applicationID);

		// Register event handlers
		LOLSDK.Instance.StartGameReceived += new StartGameReceivedHandler(this.HandleStartGame);
		LOLSDK.Instance.GameStateChanged += new GameStateChangedHandler(this.HandleGameStateChange);
		LOLSDK.Instance.LanguageDefsReceived += new LanguageDefsReceivedHandler(this.HandleLanguageDefs);

		// Mock the platform-to-game messages when in the Unity editor.
#if UNITY_EDITOR
		LoadMockData();
#endif

		// Then, tell the platform the game is ready.
		LOLSDK.Instance.GameIsReady();

		// Sets application to run in background (for pausing through the platform)
		//Application.runInBackground = false;
		//SharedState.maxProgress = gameData.maxProgress;
		//SharedState.levels = new Level[gameData.totalLevels];
		//for (int i = 0; i < SharedState.levels.Length; i++)
		//{
		//	SharedState.levels[i] = new Level();
		//}
	}

	// Start the game here
	void HandleStartGame(string json)
	{
		if (loadMainMenuAfterInitialize)
		{
			SceneManager.LoadScene(gameData.gameLevelTag + "main-menu", LoadSceneMode.Single);
		}
		SharedState.startGameData = JSON.Parse(json);
	}

	// Handle pause / resume
	void HandleGameStateChange(GameState gameState)
	{
		// Either GameState.Paused or GameState.Resumed
		Debug.Log("HandleGameStateChange");
	}

	// Use language to populate UI
	void HandleLanguageDefs(string json)
	{
		JSONNode langDefs = JSON.Parse(json);

		// Example of accessing language strings
		// Debug.Log(langDefs);
		// Debug.Log(langDefs["welcome"]);

		SharedState.languageDefs = langDefs;
	}
	private void LoadMockData()
	{
#if UNITY_EDITOR
		// Load Dev Language File from StreamingAssets

		string startDataFilePath = Path.Combine(Application.streamingAssetsPath, startGameJSONFilePath);
		string langCode = "en";

		Debug.Log(File.Exists(startDataFilePath));

		if (File.Exists(startDataFilePath))
		{
			string startDataAsJSON = File.ReadAllText(startDataFilePath);
			JSONNode startGamePayload = JSON.Parse(startDataAsJSON);
			// Capture the language code from the start payload. Use this to switch fontss
			langCode = startGamePayload["languageCode"];
			HandleStartGame(startDataAsJSON);
		}

		// Load Dev Language File from StreamingAssets
		string langFilePath = Path.Combine(Application.streamingAssetsPath, languageJSONFilePath);
		if (File.Exists(langFilePath))
		{
			string langDataAsJson = File.ReadAllText(langFilePath);
			// The dev payload in language.json includes all languages.
			// Parse this file as JSON, encode, and stringify to mock
			// the platform payload, which includes only a single language.
			JSONNode langDefs = JSON.Parse(langDataAsJson);
			// use the languageCode from startGame.json captured above
			HandleLanguageDefs(langDefs[langCode].ToString());
		}
#endif
	}

	//Method to check which platform the game is running in
	bool CheckDeviceType()
	{

		Debug.Log($"CheckDeviceType()");

		if (Contains(SystemInfo.operatingSystem.ToString(), "Android"))
		{
			return true;
		}

		if (Contains(SystemInfo.operatingSystem.ToString(), "MacOS"))
		{
			return true;
		}

		if (Contains(SystemInfo.operatingSystem.ToString(), "iPhone"))
		{
			return true;
		}

		if (Contains(SystemInfo.operatingSystem.ToString(), "iPad"))
		{
			return true;
		}

		if (SystemInfo.deviceType == DeviceType.Handheld)
		{
			return true;
		}

		return false;
	}

	public static bool Contains(string text, string searchString)
	{
		return text.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0;
	}
}
