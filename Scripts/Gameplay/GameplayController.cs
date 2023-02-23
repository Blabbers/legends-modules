using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using BeauRoutine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Blabbers.Game00
{
    public class GameplayController : MonoBehaviour, ISingleton
    {
        [ReadOnly]
        public bool gameOver;
        [BoxGroup("Level Config")] public bool showLevelInfo;
        [BoxGroup("Level Config")] public CharacterSay initialDialogue;
        [BoxGroup("Level Config")] public GameObject screenBeforeVictory;
        [BoxGroup("Level Config")] public UnityEvent OnLevelStart;
        [BoxGroup("Level Config")] public UnityEvent OnDefeat;
        [BoxGroup("Level Config")] public UnityEvent OnVictory;
        public Action<bool> OnPause;

        public Question finalLevelQuestion;

        public bool ShowLevelQuestion => finalLevelQuestion != null && finalLevelQuestion.answers != null && finalLevelQuestion.answers.Count > 0;

        void ISingleton.OnCreated()
        {
            UI_PopupPlayAgainWarning.HasClearedWithTwoStars = false;
            
            if (showLevelInfo)
            {
                Singleton.Get<UI_PopupLevelInfo>().ShowPopup();
            }

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

        private void Start()
        {
            Analytics.OnLevelStart();
        }

        void Update()
        {
            if (gameOver) return;

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

        public void TogglePause(bool active)
        {
            Singleton.Get<GameplayController>().OnPause?.Invoke(active);
        }

        public void Defeat()
        {
            if (gameOver) return;
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
            if (gameOver) return;
            OnVictory?.Invoke();
			Singleton.Get<UI_ObjetiveArrow>().ToggleArrow(false);
			gameOver = true;
            ProgressController.GameProgress.isNewGame = false;
            //Singleton.Get<Player>().ToggleLockedInput(true);
            FinalVictoryRoutine();
        }

        public void FinalVictoryRoutine()
        {
            //Debug.Log("<GameplayController> FinalVictoryRoutine()");

            Routine.Start(this, Run());
            //StartCoroutine(Run());

			//Debug.Log("<GameplayController> FinalVictoryRoutine() 2");
			IEnumerator Run()
            {
				//Debug.Log("<GameplayController> FinalVictoryRoutine() _Run()");

				var currentLevel = ProgressController.GameProgress.currentLevelId + 1;
                // Extra score based on gameplay rules
                //var extraScore = 1000 + (currentLevel * 50);
                //var scorePenalty = Mathf.Clamp(Time.timeSinceLevelLoad, 0, 120);
                //extraScore -= (int)scorePenalty;
                //ProgressController.AddProgressiveScore(extraScore);

                Singleton.Get<AudioController>().FadeMusicVolume(0.075f, 1f);

                // Show learning screen if any
                if (screenBeforeVictory != null)
                {
                    screenBeforeVictory.SetActive(true);
                    yield return Routine.WaitCondition(() => !screenBeforeVictory.activeSelf);
                }

                // Show question popup if necessary
                if (ShowLevelQuestion)
                {
                    var popupQuestion = Singleton.Get<UI_PopupQuestion>();
                    popupQuestion.ShowQuestion(finalLevelQuestion);
                    yield return Routine.WaitCondition(() => !popupQuestion.gameObject.activeSelf);
                }

                // Star system based on gameplay rules
                var stars = new VictoryStar[3];
                // Default star
                stars[0] = new VictoryStar(true, "star_finishTheLevel");

				#region Question
				// Extra star based on question
				if (ShowLevelQuestion)
				{
				    var correct = Singleton.Get<UI_PopupQuestion>().ChoseCorrectly;
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
                if (currentLevel > 2)
                {
                    stars[2] = new VictoryStar(!SceneLoader.isStuckOnThisLevel, "star_dontRepeatLevel");
                }
                else
                {
                    stars[2] = new VictoryStar(true, "star_dontRepeatLevel");
                }

				//Debug.Log($"FinalVictoryRoutine() \nisStuckOnThisLevel: {SceneLoader.isStuckOnThisLevel}");
				//Debug.Log($"FinalVictoryRoutine()\nstars[0]: {stars[0].earnedStar} | stars[1]: {stars[1].earnedStar} | stars[2]: {stars[2].earnedStar}");

				//Debug.Log("<GameplayController> FinalVictoryRoutine() _Run() end");


				yield return new WaitForSeconds(1f);

                // Finally shows the victory popup screen
                Singleton.Get<UI_PopupVictoryScreen>()
                    .ShowLevelVictoryScreen(stars, ProgressController.GameProgress.CurrentLevel.score);
                yield return new WaitForSeconds(3f);
                Singleton.Get<AudioController>().FadeResetMusicVolume(1f);


            }

			Debug.Log("<GameplayController> FinalVictoryRoutine() End");
		}
	}
}