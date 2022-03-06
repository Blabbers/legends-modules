using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using NaughtyAttributes;

public class GameData : ScriptableObject
{
	public void OnEnable()
	{
		Application.runInBackground = false;

		SharedState.maxProgress = maxProgress;
	}

	[Header("Settings")]
	public string applicationID = "com.blabbers.gameName";
	// Progress should be a minimum of 8
	public int maxProgress = 8;
	public int totalLevels = 3;

    // I will be hiding this for now. This is only usefull if the LL staff asks us to actually use the namespaces and merge all the projects
	[HideInInspector]//[Header("Custom strings")]
	public string gameLevelTag = "blabbers00-";

	[Header("Saved Progress")]
	public GameProgress progress;
}

[System.Serializable]
public class GameProgress
{
	// Unfortunatelly, we cant use a constructor with LOL's API, so this is a loose function to initialize
	public void Initialize(int totalLevels)
	{
		isNewGame = true;
		levels = new Level[totalLevels];
		for (int i = 0; i < levels.Length; i++)
		{
			levels[i] = new Level();
		}
	}

	public bool isNewGame;

	// Level being played at the moment
	public int currentLevelId = 0;
	public Level CurrentLevel => levels[currentLevelId];
	public bool HasFinishedTheGame => reachedLevel >= levels.Length;

	// Max reached level from the player
	public int reachedLevel = 0;

	public int score = 0;
	public int currentProgress = 0;

	// Saved level infos
	public Level[] levels;

	public bool enableAutomaticTTS = true;
}

[System.Serializable]
public class Level
{
	// Info for each specific level instance
	public int score;
	public int starAmount;
}