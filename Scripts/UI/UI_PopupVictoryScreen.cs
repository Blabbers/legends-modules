using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blabbers.Game00
{
    public class VictoryStar
    {
        public bool earnedStar;
        public string reason;

        public VictoryStar(bool earned, string reason)
        {
            this.reason = reason;
            this.earnedStar = earned;
        }
    }
    public class UI_PopupVictoryScreen : UI_PopupWindow, ISingleton
    {
        // Objects
        [SerializeField]
        private List<MotionTweenPlayer> starsTweens;
        [SerializeField]
        private List<MotionTweenPlayer> textsTweens;

        [SerializeField]
        private Button continueButton;

        private float continueDelay = 3.0f;

        /// <summary>
        /// Invokes the victory screen with given star amount (from 1 to 3)
        /// </summary>
        public void ShowLevelVictoryScreen(VictoryStar[] starList, int score)
        {
            continueButton.interactable = false;
            base.ShowPopup();

            //OpenedPopupList.Add(this.gameObject);
            //this.gameObject.SetActive(true);



            foreach (var item in starsTweens)
            {
                item.gameObject.SetActive(false);
            }

            var starsRemaining = 3;
            // First, separate the stars that were not earned and places the reason for losing them
            
            Debug.Log("starsRemaining " + starsRemaining);
            for (int i = starList.Length - 1; i >= 0; i--)
            {
                var star = starList[i];
                if (!star.earnedStar)
                {
                    starsRemaining--;
                    //starsTweens[starsLost].gameObject.SetActive(false);
                    var text = textsTweens[starsRemaining].GetComponent<TextMeshProUGUI>();
                    text.LocalizeText(star.reason);
                    Debug.Log("failedStar " + text.name);
                }
            }
            
            for (int i = 0; i < starsTweens.Count; i++)
            {
                if (i < starsRemaining)
                {
                    var star = starsTweens[i];
                    star.PlayTween(star.delayOnEnable);
                    Debug.Log("WIN " + star.name);
                }
                else
                {
                    // se nao, toca o texto de motivo que perdeu
                    var textTween = textsTweens[i];
                    textTween.PlayTween(textTween.delayOnEnable);
                    Debug.Log("LOSE " + textTween.name);
                }
            }
            
            // Functionality
            if (ProgressController.GameProgress.levels != null)
            {
                ProgressController.GameProgress.levels[ProgressController.GameProgress.currentLevelId].starAmount = Mathf.Max(starsRemaining, ProgressController.GameProgress.levels[ProgressController.GameProgress.currentLevelId].starAmount);
                ProgressController.GameProgress.levels[ProgressController.GameProgress.currentLevelId].score = Mathf.Max(score, ProgressController.GameProgress.levels[ProgressController.GameProgress.currentLevelId].score);
            }
            
            //var i = 1;
            //foreach (var item in stars)
            //{
            //    if (i > starAmount)
            //        break;
            //
            //    item.SetActive(true);
            //    i++;
            //}

            EnableContinueButton();
        }


        void EnableContinueButton()
        {
            StartCoroutine(_Delay());
            IEnumerator _Delay()
            {
                yield return new WaitForSeconds(continueDelay);
                continueButton.interactable = true;
            }
        }


        public override void HidePopup()
        {
            base.HidePopup();

            //this.gameObject.SetActive(false);
            //OpenedPopupList.Remove(this.gameObject);
        }
    }
}