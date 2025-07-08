using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Blabbers.Game00
{
    public class UI_LevelButton : MonoBehaviour
    {
        public int myLevel = 0;
        public bool loadCutsceneInstead = false;

        [HideIf("loadCutsceneInstead")]
        public bool autoLoadLevel = true;

        public Sprite currentBtnSprite;
        public Image targetImage;
        public List<GameObject> stars;

        public List<GameObject> showWhenOpen;
        public List<GameObject> hideWhenLocked;

        public TextMeshProUGUI textLevelNumberActive;
        public TextMeshProUGUI textLevelNumberLocked;

        public MotionTweenPlayer motionPlayer;

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetNumberText();
        }
#endif

        void Start()
        {
            var button = this.gameObject.GetComponent<Button>();

            if (autoLoadLevel || loadCutsceneInstead)
            {
                // adding click event to the button, it will load the scene "Level1" <- using "myLevel" as the number
                button
                    .GetComponent<MotionTweenPlayer>()
                    .OnAnimationFinished.AddListener(() =>
                    {
                        LoadLevel(myLevel);
                    });
            }

            button.interactable = true;

            if (ProgressController.GameProgress.reachedLevel < myLevel - 1)
            {
                button.interactable = false;
                foreach (var item in hideWhenLocked)
                {
                    if (item)
                        item.SetActive(false);
                }
            }
            // level open
            if (ProgressController.GameProgress.reachedLevel == myLevel - 1)
            {
                foreach (var item in showWhenOpen)
                {
                    if (item)
                        item.SetActive(true);
                }

                targetImage.sprite = currentBtnSprite;
            }

            // hide all stars
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].SetActive(false);
            }

            // shows level stars
            if (ProgressController.GameProgress.levels != null)
            {
                var length = ProgressController.GameProgress.levels[myLevel - 1].starAmount;
                for (int i = 0; i < stars.Count; i++)
                {
                    stars[i].SetActive(i < length);
                }
            }
        }

        private void SetNumberText()
        {
            if (!textLevelNumberActive.text.Equals(myLevel.ToString()))
            {
                textLevelNumberActive.text = myLevel.ToString();
            }
            if (!textLevelNumberLocked.text.Equals(myLevel.ToString()))
            {
                textLevelNumberLocked.text = myLevel.ToString();
            }
        }

        void LoadLevel(int level)
        {
            if (loadCutsceneInstead)
            {
                Singleton.Get<SceneLoader>().LoadScene("cutscene-" + myLevel);
            }
            else
            {
                Singleton.Get<SceneLoader>().LoadGameLevel(myLevel);
            }
        }
    }
}
