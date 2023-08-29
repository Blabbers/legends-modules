using BennyKok.RuntimeDebug.Utils;
using Blabbers.Game00;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GameData : ScriptableObject
{
    #region Instance
    private static GameData _instance = null;
    public static GameData Instance
    {
        get
        {
            if (!_instance) _instance = Resources.Load<GameData>("GameData");
            return _instance;
        }
    }
	#endregion

	public void OnEnable()
    {
        Application.runInBackground = false;
        SharedState.maxProgress = maxProgress;
    }

    private bool HasChangedAppID(string value)
    {
        return !value.Equals("com.blabbers.gameName");
    }

    // Progress should be a minimum of 8
    [Title("Platform Settings", 0)]
    [Comment("Mandatory settings for LL's build.", order = 1)]
    public int maxProgress = 8;    
    public int totalLevels = 3;    
    public string applicationID = "com.blabbers.gameName";
    
    [Title("Text Configs", 0)]
	public string currentSelectedLangCode = "en";
	[ReorderableList]
    [Tooltip("Adjust the KeyCodes that are important to be highlighted in this project.", order = 1)]
	public LocalizationKeyCode[] textConfigs;

	[Tooltip("Everytime a level finishes it automatically goes to the level select scene. If you populate this list, you can override this behaviour and go to a simulation screen instead for example.")]
    [ReorderableList]
    public SceneToLoad[] levelSelectOverrideScenes;

	[Tooltip("Scenes that when entered or exited will trigger a loading screen first")]
	[ReorderableList]
	public SceneReference[] scenesWithLoadingScreen;

	[Tooltip("Hints/Learning that will be displayed on the loading screen before a scene")]
	[ReorderableList]
	public LoadingHint[] loadingHints;


	[System.Serializable]
    public struct SceneToLoad
	{
        public SceneReference previousScene;
        public SceneReference targetScene;
	}

	[System.Serializable]
	public struct LoadingHint
	{
		public string hintKey;
		public SceneReference nextScene;
      
	}


	private GameProgress progress;    
    public GameProgress Progress => this.progress;
    public void SetProgressData(GameProgress newProgressData)
	{
        progress = newProgressData;
    }

    //Fields for debugging only.
    [ShowNativeProperty]
    public bool IsStuckOnThisLevel => SceneLoader.isStuckOnThisLevel;
    [ShowNativeProperty]
    public bool HardReset => UI_RetryButton.HardReset;

    [Button("Test → Finish Current Level")]
    public void TestFinishCurrentLevel()
    {
        Singleton.Get<ProgressController>().FinishLevel();
    }

    [Button("Test → Submit Save")]
    public void TestSubmitSave()
    {
        Singleton.Get<ProgressController>().Save();
    }

    [Button("Reset Save → Clear All Player Prefs")]
    public void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Save was reset. All PlayerPrefs are cleared.");
    }
}

[System.Serializable]
public class GameProgress
{
    // Unfortunatelly, we cant use a constructor with LOL's API, so this is a loose function to initialize
    public void Initialize(int totalLevels)
    {
        isNewGame = true;
        FirstTimeLevelSelect = true;
        choices = new Choice[0];

        levels = new Level[totalLevels];
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i] = new Level();
        }
    }

    public bool isNewGame;

    //Moved variable from GameData to Progress // Used to control if Customization will be open
    public bool FirstTimeLevelSelect = true;

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

    public Customization[] customizations;
	public Choice[] choices;

	public bool AllLevelsWith3Stars()
    {
        foreach (var level in levels)
        {
            if (level.starAmount < 3)
                return false;
        }

        return true;
    }

    public void AddChoice(Choice choice)
    {
        var tempList = new List<Choice>(choices);

        var found = tempList.Find((x) => choice.key == x.key);

        if (found != null)
        {
            found.selectedId = choice.selectedId;
        }
        else
        {
            tempList.Add(choice);
        }

        choices = tempList.ToArray();

        Singleton.Get<ProgressController>().Save();
    }

    public Choice GetChoice(string key)
    {
        foreach(var choice in choices)
        {
            if(choice.key == key)
            {
                return choice;
            }
        }

        return null;
    }

}

[System.Serializable]
public class Level
{
    // Info for each specific level instance
    public int score;
    public int starAmount;

    //New timer added for Quantitative Analogy
    public int timer;
}

[System.Serializable]
public class Customization
{
    // Info for each specific level instance
    public string name;
    public int id;
}

[System.Serializable]
public class Choice
{
	// Info for each specific level instance
	public string key;
	public int selectedId;
}