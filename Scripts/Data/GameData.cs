using Blabbers.Game00;
using NaughtyAttributes;
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
    
    [HorizontalLine(color: EColor.White)]
    // Progress should be a minimum of 8
    [BoxGroup("Settings")]
    public int maxProgress = 8;
    [BoxGroup("Settings")]
    public int totalLevels = 3;
    [BoxGroup("Settings")]
    [ValidateInput(nameof(HasChangedAppID), "ApplicationID must be have a custom name for the game.")]
    public string applicationID = "com.blabbers.gameName";
    //Added variables to control customization
    [BoxGroup("Settings")]
    public bool StartCustomizationFirst = false;

    // I will be hiding this for now. This is only usefull if the LL staff asks us to actually use the namespaces and merge all the projects
    [BoxGroup("Settings")][HideInInspector]
    public string gameLevelTag = "blabbers00-";

    [BoxGroup("Saved Progress")]
    [HorizontalLine(color: EColor.White)]
    public GameProgress progress;

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
}

[System.Serializable]
public class GameProgress
{
    // Unfortunatelly, we cant use a constructor with LOL's API, so this is a loose function to initialize
    public void Initialize(int totalLevels)
    {
        isNewGame = true;
        FirstTimeLevelSelect = true;

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
}

[System.Serializable]
public class Level
{
    // Info for each specific level instance
    public int score;
    public int starAmount;
}

[System.Serializable]
public class Customization
{
    // Info for each specific level instance
    public string name;
    public int id;
}