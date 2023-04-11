using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;
using System;
using Sigtrap.Relays;
using static UnityEngine.AudioSettings;

namespace Blabbers.Game00
{
    public class ProgressController : MonoBehaviour, ISingleton
    {
        private GameData gameData => GameData.Instance;
		public static GameProgress GameProgress => GameData.Instance.Progress;
		public static bool enableAutomaticTTS = true;



		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void Init()
        {
            enableAutomaticTTS = false;
		}


		private void Awake()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;            
        }

        void ISingleton.OnCreated()
        {
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
			//Debug.Log($"ProgressController.HandleSceneLoaded() | scene is null? {scene == null}" +
			//	$"\nGameProgress == null? {GameProgress == null}");

			//Erro on findinding scene.name??
		//	Debug.Log($"{this.name}.HandleSceneLoaded({scene.name})" +
		//$"\nGameProgress == null? {GameProgress == null}");

			if (GameProgress == null) return;

			// If on a level, set its score to zero?
			if (GameProgress.currentLevelId < GameProgress.levels.Length)
                GameProgress.CurrentLevel.score = 0;
        }

        private void Start()
        {
            StateInitialize(OnLoad);
			// Used for player feedback. Not required by SDK.
			LOLSDK.Instance.SaveResultReceived += OnSaveResult;
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
#endif
            LOLSDK.Instance.SaveResultReceived -= OnSaveResult;
        }

        #region SaveLoadFunctions
        public void Save()
        {
            Debug.Log(" Saving current progress... " + gameData.Progress.currentProgress);
            try
            {
                LOLSDK.Instance.SaveState(gameData.Progress);
            }
            catch (Exception e)
            {
                Debug.Log("[Could NOT SAVE state data] " + e);
            }
        }

        void OnSaveResult(bool success)
        {
            if (!success)
            {
                Debug.LogWarning("Saving not successful");
                return;
            }

            // ...Auto Saving Complete
            StartCoroutine(Routine());
            IEnumerator Routine()
            {
                yield return new WaitForSeconds(2f);
                Debug.Log("Saving successfull?");
            }
        }

        /// <summary>
        /// This is the setting of your initial state when the scene loads.
        /// The state can be set from your default editor settings or from the
        /// users saved data after a valid save is called.
        /// </summary>
        public void OnLoad(GameProgress loadedData)
        {
            //Debug.Log($"{this.name}.OnLoad() | loadedData == null? {loadedData == null}");

			// Overrides serialized state data or continues with editor serialized values.
			if (loadedData != null)
            {
				//Debug.Log($"OnLoad()" + $"\n{this}.OnLoad() | loadedData != null");

				gameData.SetProgressData(loadedData);
                gameData.Progress.isNewGame = false;
            }
            else
            {
				//Debug.Log($"OnLoad()" + $"\n{this}.OnLoad() | loadedData == null");
				CreateNewGameData();
            }
        }

        public void CreateNewGameData()
        {
            //Debug.Log($"{this}.CreateNewGameData()");

            // If there is no data to load, we start the game from scratch
            // (this is just because this variable is saved in an SO that is persistent in the editor)
            gameData.SetProgressData(new GameProgress());
            gameData.Progress.Initialize(gameData.totalLevels);
            gameData.Progress.enableAutomaticTTS = enableAutomaticTTS;

		}

        /// <summary>
        /// Helper to handle your required NEW GAME and CONTINUE buttons.
        /// Stops double clicking of buttons and shows the continue button only when needed.
        /// Also handles broadcasting out the serialized progress back to the teacher app.
        /// <para>NOTE: This is just a helper method, you can implement this flow yourself but it must send Progress when the state loads.</para>
        /// </summary>
        public Action<GameProgress> OnGameDataLoaded;
        public void StateInitialize(Action<GameProgress> callback)
        {

            //Debug.Log($"{this}.StateInitialize()");

			try
            {
				//Debug.Log($"{this}.StateInitialize() try");

				// Check for valid state data, from server or fallback local ( PlayerPrefs )
				LOLSDK.Instance.LoadState<GameProgress>(state =>
                {
                    if (state != null)
                    {
                        if (state.data.HasFinishedTheGame)
                        {
                            //Normally follows this path
							//Debug.Log($"{this}.StateInitialize() HasFinishedTheGame");

							// If the game was already finished before, then we want this to defintelly be a new game
							// TODO: Maybe in a state like this one, we will later want to make sure to
							// add some ACHIEVEMENT or something to remember the player that they have already BEATTEN THE GAME once!!!
							// Like "second run" or "champion" or something like that
							callback(null);

							//Debug.Log($"{this}.StateInitialize() after callback(null)");

                            //Error was here
                            OnGameDataLoaded?.Invoke(null);

                            //Doesn't reach this
							//Debug.Log($"{this}.StateInitialize() HasFinishedTheGame End");

							// If legends of learning wants, we may add the possibility to leave the old scores and stars there yet, but at this point we just reset "level" progress
						}
                        else
                        {
							//Debug.Log($"{this}.StateInitialize() !HasFinishedTheGame");

							// If we have a partial state of a saved game, then we load it
							callback(state.data);
							OnGameDataLoaded?.Invoke(state.data);
						}
						// Broadcast saved progress back to the teacher app.



						//Debug.Log($"{this.name}.StateInitialize()" + $"\nGameProgress == null? {GameProgress == null}");
						LOLSDK.Instance.SubmitProgress(GameProgress.score, GameProgress.currentProgress, SharedState.maxProgress);

                    }
                    else
                    {

						//Debug.Log($"{this}.StateInitialize() state == null? {state == null}");

						callback(null);
                        OnGameDataLoaded(null);
                    }
                });
            }
            catch (Exception e)
            {
                Debug.Log("[Could not LOAD save state] " + e);
            }


			//Debug.Log($"{this}.StateInitialize() End");
		}
        #endregion

        #region GameManagerFunctions 
        /// <summary>
        /// Finishes the level going to the level select screen, enabling the next level.
        /// This is called on the "Continue" button UI event, inside every level.
        /// </summary>
        public void FinishLevel()
        {
            GameProgress.currentLevelId++;
            if (GameProgress.currentLevelId > GameProgress.reachedLevel)
            {
                GameProgress.reachedLevel++;
            }

            AddProgress();

            Save();
            Singleton.Get<SceneLoader>().LoadLevelSelectScene();
        }

        /// <summary>
        /// Finishes the game and sends the message to the LOL platform.
        /// </summary>
        private bool isGameFinished = false;
        public void FinishGame()
        {
            if (LoLSDK.LOLSDK.Instance.IsInitialized)
            {
                LoLSDK.LOLSDK.Instance.CompleteGame();
            }
            if (!Singleton.Get<ProgressController>().isGameFinished)
            {
                Analytics.OnGameFinished();
                Singleton.Get<ProgressController>().isGameFinished = true;
            }
        }
        /// <summary>
        /// Submit progress informing the LOL platform that the student advanced.
        /// </summary>
        public static void SubmitProgress()
        {
            if (LoLSDK.LOLSDK.Instance && LoLSDK.LOLSDK.Instance.IsInitialized)
            {
                LoLSDK.LOLSDK.Instance.SubmitProgress(GameProgress.score, GameProgress.currentProgress, SharedState.maxProgress);
            }
            Debug.Log("[Current Progress] " + GameProgress.currentProgress + "/" + SharedState.maxProgress);
        }
        /// <summary>
        /// Adds 1 progress to the game SharedState.
        /// </summary>
        /// <param name="submit">If true, will also submit the values to the LOL platform after adding a progress.</param>
        public static void AddProgress(bool submit = true)
        {
            int value = GameProgress.currentProgress + 1;
            if (value < SharedState.maxProgress)
                GameProgress.currentProgress = value;
            else
                GameProgress.currentProgress = SharedState.maxProgress;

            if (submit)
            {
                SubmitProgress();
            }
        }

        public static readonly Relay<int> OnLevelScoreChanged = new Relay<int>();
        /// <summary>
        /// Adds X score to the game SharedState and to current level score.
        /// </summary>
        /// <param name="submitProgress">If true, will also submit the values to the LOL platform after adding a progress.</param>
        public static void AddProgressiveScore(int amount, bool submitProgress = false)
        {
            GameProgress.score += amount;
            GameProgress.CurrentLevel.score += amount;

            OnLevelScoreChanged?.Invoke(GameProgress.CurrentLevel.score);

            if (submitProgress)
            {
                SubmitProgress();
            }

            //Debug.Log("Total Score: " + GameProgress.score + " | Level Score: " + GameProgress.CurrentLevel.score);
        }
        #endregion
    }
}