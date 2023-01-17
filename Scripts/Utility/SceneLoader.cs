using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blabbers.Game00
{
    public class SceneLoader : MonoBehaviour, ISingleton
    {
        public void OnCreated() { }

        // If the player has already loaded this level more than once whithout leaving it
        public static bool isStuckOnThisLevel;
        private GameData gameData => GameData.Instance;

        public void Awake()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            UI_RetryButton.HardReset = false;
            if (scene.name.Equals($"{gameData.gameLevelTag}level-select"))
            {
                isStuckOnThisLevel = false;
            }
        }

        void OnSceneUnloaded(Scene scene)
        {
            if (!UI_RetryButton.HardReset && scene.name.Contains("level") && !scene.name.Equals($"{gameData.gameLevelTag}level-select"))
            {
                isStuckOnThisLevel = true;
            }
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
            if(gameData.levelSelectOverrideScenes.Length == 0)
			{
                if (gameData.progress.FirstTimeLevelSelect && !gameData.StartCustomizationFirst)
                {
                    gameData.progress.FirstTimeLevelSelect = false;
                    LoadSceneByName("customization");
                }
                else
                {
                    if (gameData.AlwaysShowStatsScreen)
                    {
            		    LoadSceneByName("customization");
                    }
                    else
                    {
            		    SceneManager.LoadScene($"{gameData.gameLevelTag}level-select");
            	    }               
                }
			}
			else
			{

                //Gambeta pra build beta:
                //var currentScene = SceneManager.GetActiveScene().name;
                string previousScene, targetScene;
                string[] split;

                foreach (var scene in gameData.levelSelectOverrideScenes)
                {

                    previousScene = ConvertScenePathToName(scene.previousScene.ScenePath);


					//Debug.Log($"currentScene: {SceneManager.GetActiveScene().name}" +
					//	$"\n[{previousScene}]");

                    if(previousScene == SceneManager.GetActiveScene().name)
                    {

						targetScene = ConvertScenePathToName(scene.targetScene.ScenePath);

						//Debug.Log($"activeScene: {SceneManager.GetActiveScene().name}\n" +
	     //               $"previousScene: [{previousScene}] | targetScene: [{targetScene}]");

						LoadSceneByName(targetScene);
                        return;
					}

                }

				SceneManager.LoadScene($"{gameData.gameLevelTag}level-select");



				//switch (currentScene)
				//            {
				//                case "level-1":
				//                    LoadSceneByName("simulation-1");
				//                    break;
				//                case "level-2":
				//                    LoadSceneByName("simulation-2");
				//                    break;
				//                case "level-3":
				//                    LoadSceneByName("customization");
				//                    break;
				//                case "level-4":
				//                    LoadSceneByName("customization");
				//                    break;
				//                case "level-5":
				//                    LoadSceneByName("simulation-3");
				//                    break;
				//                case "level-6":
				//                    LoadSceneByName("cutscene-final");
				//                    break;
				//                default:
				//                    SceneManager.LoadScene($"{gameData.gameLevelTag}level-select");
				//                    break;
				//            }


			}
        }


        private string ConvertScenePathToName(string path)
        {
            string sceneName;
            string[] split;
			string[] split2;


			split = path.Split('/');
            sceneName = split[split.Length - 1];

			split2 = sceneName.Split('.');


            return split2[0];
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
            //if (gameData.StartCustomizationFirst)
            //{
            //    LoadSceneByName("customization");
            //}
            //else
            {
                LoadSceneByName("cutscene-start");
            }

        }

    }
}