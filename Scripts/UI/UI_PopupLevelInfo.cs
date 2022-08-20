using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Blabbers.Game00
{
    public class UI_PopupLevelInfo : UI_PopupWindow, ISingleton
    {
        [HideInInspector]
        public UnityEvent OnClose;

        public override void ShowPopup()
        {
            Debug.Log("<UI_PopupLevelInfo> ShowPopup()".Colored("white"));
            base.ShowPopup();
        }

        public override void HidePopup()
        {
            OnClose?.Invoke();
            base.HidePopup();
        }
    }
}