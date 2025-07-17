using System;
using System.Collections;
using BeauRoutine;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Blabbers.Game00
{
    public class GameplayController : MonoBehaviour, ISingleton
    {
        [ReadOnly]
        public bool gameOver;

        [BoxGroup("Level Config")]
        public bool showLevelInfo;

        [BoxGroup("Level Config")]
        public bool hasAnimation = false;

        [BoxGroup("Level Config")]
        public bool alwaysForce3Stars = false;

        [BoxGroup("Level Config")]
        public CharacterSay initialDialogue;

        [BoxGroup("Level Config")]
        public UnityEvent OnLevelStart;

        [BoxGroup("Level Config")]
        public UnityEvent OnDefeat;

        [BoxGroup("Level Config")]
        public UnityEvent OnVictory;

        [field: ReadOnly, SerializeField]
        public bool IsPaused { get; private set; }
        public Action<bool> OnPause;

        //public Question finalLevelQuestion;

        //public bool ShowLevelQuestion => finalLevelQuestion != null && finalLevelQuestion.answers != null && finalLevelQuestion.answers.Count > 0;

        void ISingleton.OnCreated()
        {
            Debug.Log("GameplayController.OnCreated()".Colored("red"));

            UI_PopupPlayAgainWarning.HasClearedWithTwoStars = false;

            if (showLevelInfo)
            {
                Singleton.Get<UI_PopupLevelInfo>().ShowPopup();
            }

            StartCoroutine(_WaitForAnimation());
            IEnumerator _WaitForAnimation()
            {
                yield return new WaitUntil(() => !hasAnimation);

                OnLevelStart?.Invoke();

                if (!SceneLoader.isStuckOnThisLevel)
                {
                    if (initialDialogue != null)
                    {
                        //Debug.Log("<GameplayController>initialDialogue");
                        initialDialogue.Execute(0.5f);
                    }
                }
            }
        }

        private void Start()
        {
            Analytics.OnLevelStart();
        }

        void Update()
        {
            if (gameOver)
                return;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            // Cheats
            if (Input.GetKeyDown(KeyCode.End))
            {
                // Finish level
                Victory();
            }
            // Cheats
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                // Finish level
                Defeat();
            }
#endif
        }

        void ScoreExample()
        {
            // Pontuar clicando com o mouse, para testes
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // ADICIONA score para o level em "Level.CurrentLevel.score"
                ProgressController.AddProgressiveScore(Random.Range(5, 10));
            }

            // Pontuar clicando com o mouse, para testes
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                // REMOVE score para o level em "Level.CurrentLevel.score"
                ProgressController.AddProgressiveScore(-Random.Range(5, 10));
            }

            if (ProgressController.GameProgress.CurrentLevel.score > 100)
            {
                Victory();
            }
        }

        public void TogglePause(bool value)
        {
            var gameplayInstance = Singleton.Get<GameplayController>();
            gameplayInstance.IsPaused = value;
            gameplayInstance.OnPause?.Invoke(value);
        }

        public void TogglePause(bool value, bool countdownState)
        {
            TogglePause(value);

            Routine.Start(Run());
            IEnumerator Run()
            {
                yield return null;
                Singleton.Get<UI_PopupCountdown>()?.SetEnableCountdown(countdownState);
            }
        }

        public void Defeat()
        {
            if (gameOver)
                return;
            gameOver = true;

            var key = "";
            var delay = 2.0f;

            Analytics.OnDefeat("default");

            Routine.Start(Run());
            IEnumerator Run()
            {
                yield return Routine.WaitSeconds(delay);
                OnDefeat?.Invoke();
                SceneLoader.isStuckOnThisLevel = true;
                Singleton.Get<UI_PopupTryAgain>().ShowPopup();
                Singleton.Get<UI_PopupTryAgain>().SetDefeatText(key);
            }
        }

        public void Victory()
        {
            Debug.Log("<GameplayController> Victory()");
            if (gameOver)
                return;
            OnVictory?.Invoke();
            Singleton.Get<UI_ObjetiveArrow>().ToggleArrow(false);
            gameOver = true;
            ProgressController.GameProgress.isNewGame = false;
            //Singleton.Get<Player>().ToggleLockedInput(true);
            FinalVictoryRoutine();
        }

        public Action OnBeforeVictoryScreenShown;

        public void FinalVictoryRoutine()
        {
            Routine.Start(this, Run());
            IEnumerator Run()
            {
                var currentLevel = ProgressController.GameProgress.currentLevelId + 1;
                // Extra score based on gameplay rules
                //var extraScore = 1000 + (currentLevel * 50);
                //var scorePenalty = Mathf.Clamp(Time.timeSinceLevelLoad, 0, 120);
                //extraScore -= (int)scorePenalty;
                //ProgressController.AddProgressiveScore(extraScore);

                Singleton
                    .Get<AudioController>()
                    .FadeMusicVolume(AudioController.MusicVolumeLow, 1f);

                // If a flowchart registered a block here we will wait until it is removed
                OnBeforeVictoryScreenShown?.Invoke();
                yield return Routine.WaitCondition(() => OnBeforeVictoryScreenShown == null);

                // Star system based on gameplay rules
                var stars = new VictoryStar[3];
                // Default star
                stars[0] = new VictoryStar(true, "star_finishTheLevel");

                #region Question
                var popupQuestion = Singleton.Get<UI_PopupQuestion>();
                var questionWasShown = popupQuestion && popupQuestion.QuestionWasAnsweredThisLevel;
                // Extra star based on question
                if (!alwaysForce3Stars &&  questionWasShown)
                {
                    var correct = popupQuestion.ChoseCorrectly;
                    stars[1] = new VictoryStar(correct, "star_correctAnswer");
                }
                else
                {
                    stars[1] = new VictoryStar(true, "star_correctAnswer");
                }
                #endregion

                //var correct = Singleton.Get<GameStatusController>().CheckIfAnyFireLeft();
                //stars[1] = new VictoryStar(correct, "star_putOutAllFires");

                // Star for finishing the level at first try. Ignore this for the first 2 levels
                if (!alwaysForce3Stars && currentLevel > 2)
                {
                    stars[2] = new VictoryStar(
                        !SceneLoader.isStuckOnThisLevel,
                        "star_dontRepeatLevel"
                    );
                }
                else
                {
                    stars[2] = new VictoryStar(true, "star_dontRepeatLevel");
                }

                yield return new WaitForSeconds(1f);

                // Finally shows the victory popup screen
                Singleton
                    .Get<UI_PopupVictoryScreen>()
                    .ShowLevelVictoryScreen(
                        stars,
                        ProgressController.GameProgress.CurrentLevel.score
                    );
                yield return new WaitForSeconds(3f);
                Singleton.Get<AudioController>().FadeResetMusicVolume(1f);
            }
        }
    }
}
