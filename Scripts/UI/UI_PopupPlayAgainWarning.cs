using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using UnityEngine;
using UnityEngine.UI;

namespace Blabbers.Game00
{
    public class UI_PopupPlayAgainWarning : UI_PopupWindow, ISingleton
    {
        //Create variable to detect the first time the player gets less than 3 stars
        public static bool HasClearedWithTwoStars = false;
        private static bool HasShownPlayAgainPopup = false;
        
        public GameObject target;
        public List<Transform> buttons;
        public Button continue_btn;

        public override void ShowPopup()
        {
            // Only shows this if it has too
            if (!HasClearedWithTwoStars || HasShownPlayAgainPopup) return;
            HasShownPlayAgainPopup = true;

            if(continue_btn=null) continue_btn.interactable = false;

			//Aparentemente a animação do motion tween resetava a posição do target sem essa linha
			target.SetActive(false);

            base.ShowPopup();

            // TODO: Quando abrir o popup, liga esse cara e seta a posicao dele em cima do level que a pessoa acabou de perder estrelas
            int levelId = ProgressController.GameProgress.currentLevelId - 1;

            StartCoroutine(_Delay(Singleton.Get<UI_LevelSelect>().TotalButtonDropTime + 0.3f));
            IEnumerator _Delay(float delay)
            {
                yield return new WaitForSeconds(delay);
                target.transform.position = buttons[levelId].position;
                target.SetActive(true);
				if (continue_btn = null) continue_btn.interactable = true;
            }
        }

        public override void HidePopup()
        {
            base.HidePopup();
        }

        private bool clicked = false;
        private void Update()
        {
            if (Input.anyKeyDown && !clicked)
            {
                clicked = true;
                Routine.Start(Run());
                IEnumerator Run()
                {
                    // Starts tween to fade out this screen
                    GetComponent<MotionTweenPlayer>().PlayTween();
                    yield return Routine.WaitSeconds(1f);
                    // Then force-hides the screen
                    HidePopup();
                }
            }
        }
    }
}