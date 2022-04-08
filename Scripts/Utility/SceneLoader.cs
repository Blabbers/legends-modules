using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blabbers.Game00
{
    public class SceneLoader : MonoBehaviour, ISingleton
    {
        public void OnCreated() { }

        // If the player has already loaded this level more than once whithout leaving it
        public static bool isStuckOnThisLevel;
        public GameData gameData;

        public void Awake()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.Equals($"{gameData.gameLevelTag}level-select"))
            {
                isStuckOnThisLevel = false;
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneUnloaded(Scene scene)
        {
            if (scene.name.Contains("level") && !scene.name.Equals($"{gameData.gameLevelTag}level-select"))
            {
                isStuckOnThisLevel = true;
            }
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadGameLevel(int level)
        {
            ProgressController.GameProgress.currentLevelId = level - 1;
            SceneManager.LoadScene($"{gameData.gameLevelTag}level-" + level);
        }

        public void LoadLevelSelectScene()
        {
            if (gameData.FirstTimeLevelSelect)
            {
                gameData.FirstTimeLevelSelect = false;
                LoadSceneByName("customization");
            }
            else
            {
                SceneManager.LoadScene($"{gameData.gameLevelTag}level-select");
            }

        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene($"{gameData.gameLevelTag}main-menu");
        }

        public void LoadSceneByName(string fullSceneName)
        {
            SceneManager.LoadScene(fullSceneName);
        }

        //Added
        public void LoadNewGame()
        {
            if (gameData.StartCustomizationFirst)
            {
                LoadSceneByName("customization");
            }
            else
            {
                LoadSceneByName("cutscene-start");
            }

        }

    }
}