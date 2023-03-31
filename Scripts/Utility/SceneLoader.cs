using BeauRoutine;
using System.Collections;
using System.Linq;
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
			if (scene.name.Equals("level-select"))
			{
				isStuckOnThisLevel = false;
			}
		}

		void OnSceneUnloaded(Scene scene)
		{
			if (!UI_RetryButton.HardReset && scene.name.Contains("level") && !scene.name.Equals("level-select"))
			{
				isStuckOnThisLevel = true;
			}
		}

		public void LoadScene(string nextSceneName)
		{
			

			var previousScene = SceneManager.GetActiveScene().name;
			var scenes = GameData.Instance.scenesWithLoadingScreen.ToList();

			bool foundScene = scenes.Find(x => ConvertScenePathToName(x.ScenePath) == previousScene || ConvertScenePathToName(x.ScenePath) == nextSceneName) != null;

			//foundScene = false;

			if (foundScene)
			{

				//Debug.Log($"LoadScene: {nextSceneName} with Loading".Colored("cyan"));


				Routine.Start(_LoadRoutine());

				//StartCoroutine(_LoadRoutine());
				IEnumerator _LoadRoutine()
				{

					SceneManager.LoadScene("loading", LoadSceneMode.Additive);
					yield return new WaitForSeconds(1);
					Singleton.Get<UI_LoadingScreen>().SetNextSceneName(nextSceneName);


					SceneManager.UnloadSceneAsync(previousScene);
					yield return new WaitForSeconds(2);
					SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);


					var loaded = false;
					SceneManager.sceneLoaded += LoadedNextScene;
					yield return new WaitUntil(()=> loaded);
					SceneManager.sceneLoaded -= LoadedNextScene;

					Singleton.Get<UI_LoadingScreen>().motionTween.DisableWithExitTween(() => { SceneManager.UnloadSceneAsync("loading"); });
					

					void LoadedNextScene(Scene scene, LoadSceneMode mode)
					{
						if (scene.name.Equals(nextSceneName)) loaded = true;
					}
				}


			}
			else
			{

				//Debug.Log($"LoadScene: {nextSceneName} without Loading".Colored("orange"));
				SceneManager.LoadScene(nextSceneName);
			}

		}

		public void ReloadCurrentScene()
		{
			//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			LoadScene(SceneManager.GetActiveScene().name);
		}

		public void LoadGameLevel(int level)
		{
			ProgressController.GameProgress.currentLevelId = level - 1;
			//SceneManager.LoadScene($"level-{level}");
			LoadScene($"level-{level}");
		}

		public void LoadLevelSelectScene()
		{
			if (gameData.levelSelectOverrideScenes.Length == 0)
			{
				//SceneManager.LoadScene("level-select");
				LoadScene("level-select");
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
					if (previousScene == SceneManager.GetActiveScene().name)
					{
						targetScene = ConvertScenePathToName(scene.targetScene.ScenePath);
						//Debug.Log($"activeScene: {SceneManager.GetActiveScene().name}\n" +
						//$"previousScene: [{previousScene}] | targetScene: [{targetScene}]");
						LoadSceneByName(targetScene);
						return;
					}
				}

				//SceneManager.LoadScene("level-select");
				LoadScene("level-select");
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
			//SceneManager.LoadScene("main-menu");
			LoadScene("main-menu");
		}

		public void LoadSceneByName(string fullSceneName)
		{
			//SceneManager.LoadScene(fullSceneName);
			LoadScene(fullSceneName);
		}

		//Added
		public void LoadNewGame()
		{
			LoadSceneByName("cutscene-start");
		}

	}
}