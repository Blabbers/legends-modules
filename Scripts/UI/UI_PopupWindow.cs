using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Blabbers.Game00
{
    public class UI_PopupWindow : MonoBehaviour, ISingleton
    {
        [NaughtyAttributes.ShowNativeProperty()]
        public static bool IsAnyPopupOpen => OpenedPopupList.Count > 0;
        public static List<GameObject> OpenedPopupList = new List<GameObject>();

        public Action OnPopupHidden;
        //static Action OnAnyPopupOpen, OnAnyPopupClose;

        public Button buttonConfirmation;

#if UNITY_EDITOR
        //// for debugging only
        //public List<GameObject> OpenedPssss = new List<GameObject>();
        //private void Update()
        //{
        //	OpenedPssss = OpenedPopupList;
        //}
#endif

        public Transform popupBackground;

        void ISingleton.OnCreated()
        {
            if (buttonConfirmation)
            {
                buttonConfirmation.onClick.AddListenerOnce(HandleHideWindow);
            }
        }

        public virtual void ShowPopup()
        {
            var gameplayHUD = Singleton.Get<UI_GameplayHUD>();
            if (gameplayHUD) { gameplayHUD.HideFullHUD(); }

            OpenedPopupList.Add(this.gameObject);

            this.gameObject.SetActive(true);

            //OnAnyPopupOpen?.Invoke();
        }

        public virtual void HidePopup()
        {
            popupBackground.DOKill();
            HandleHideWindow();

            //OnAnyPopupClose?.Invoke();

        }
        private void HandleHideWindow()
        {
            this.gameObject.SetActive(false);
            OpenedPopupList.RemoveAll((x) => x == this.gameObject);

            OnPopupHidden?.Invoke();

            //var gameplayHUD = Singleton.Get<UI_GameplayHUD>();
            //if (gameplayHUD) { gameplayHUD.ShowFullHUD(); }
        }

        private void OnDestroy()
        {
            OpenedPopupList.Remove(this.gameObject);
        }
    }
}
