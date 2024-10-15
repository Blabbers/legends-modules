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

        public void Setup(LocalizedString localizedString, Sprite sprite, float imageScaleMultiplier, UnityAction onClosed)
        {
			informationText.text = localizedString;
            buttonTTS.ExternalSetup(localizedString.Key);
			image.sprite = sprite;
            if(sprite !=null) image.transform.localScale = new Vector3(imageScaleMultiplier, imageScaleMultiplier, 1);
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