using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Blabbers.Game00
{
    public class UI_PopupLevelInfo : UI_PopupWindow, ISingleton
    {
        public TextMeshProUGUI informationText;
        public UI_ButtonPlayTTS buttonTTS;
		public Image image;

		[HideInInspector]
        public UnityEvent OnClose;

        private UnityAction externalOnClosed;

        public void Setup(LocalizedString localizedString, Sprite sprite, UnityAction onClosed)
        {
            informationText.text = localizedString;
            buttonTTS.overrideTTSkey = localizedString.Key;
            image.sprite = sprite;
			externalOnClosed = onClosed;
		}

        public override void ShowPopup()
        {
            //Debug.Log("<UI_PopupLevelInfo> ShowPopup()".Colored("white"));
            base.ShowPopup();
        }

        public override void HidePopup()
        {
			externalOnClosed?.Invoke();
			OnClose?.Invoke();
            base.HidePopup();
        }
    }
}