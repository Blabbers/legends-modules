using System;
using System.Collections;
using System.Collections.Generic;
using LoLSDK;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        void ISingleton.OnCreated() { }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (GameProgress == null)
                return;

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
            // Overrides serialized state data or continues with editor serialized values.
            if (loadedData != null)
            {
                gameData.SetProgressData(loadedData);
                gameData.Progress.isNewGame = false;
            }
            else
            {
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
            try
            {
                // Check for valid state data, from server or fallback local ( PlayerPrefs )
                LOLSDK.Instance.LoadState<GameProgress>(state =>
                {
                    if (state != null)
                    {
                        if (state.data.HasFinishedTheGame)
                        {
                            callback(null);
                            OnGameDataLoaded?.Invoke(null);
                        }
                        else
                        {
                            // If we have a partial state of a saved game, then we load it
                            callback(state.data);
                            OnGameDataLoaded?.Invoke(state.data);
                        }
                        // Broadcast saved progress back to the teacher app.
                        SubmitProgress();
                    }
                    else
                    {
                        callback(null);
                        OnGameDataLoaded(null);
                    }
                });
            }
            catch (Exception e)
            {
                Debug.Log("[Could not LOAD save state] " + e);
            }
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
            if (LOLSDK.Instance && LOLSDK.Instance.IsInitialized)
            {
                LOLSDK.Instance.SubmitProgress(
                    GameProgress.score,
                    GameProgress.currentProgress,
                    SharedState.maxProgress
                );
            }
            Debug.Log(
                "[Current Progress] " + GameProgress.currentProgress + "/" + SharedState.maxProgress
            );
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

            //Debug.Log("[Current Score] " + GameProgress.score);

            OnLevelScoreChanged?.Invoke(GameProgress.CurrentLevel.score);

            if (submitProgress)
            {
                SubmitProgress();
            }
        }
        #endregion
    }
}
