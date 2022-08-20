using TMPro;
using UnityEngine.Events;
using UnityEngine;

namespace Blabbers.Game00
{
    public class UI_PopupTryAgain : UI_PopupWindow, ISingleton
    {
        public TextMeshProUGUI field;
        [HideInInspector]
        public UnityEvent CustomDefeatEvent; // The listener for this event is being added by the PlayerController

        public void TryAgain()
        {
            Debug.Log("<UI_PopupTryAgain> TryAgain()".Colored());
            Singleton.Get<SceneLoader>().ReloadCurrentScene();
        }

        public void CustomRetry()
        {
            Debug.Log("<UI_PopupTryAgain> CustomRetry()".Colored("red"));
            CustomDefeatEvent?.Invoke();
        }

        public void SetDefeatText(string key)
        {

            if (key != "")
            {
                field.LocalizeText(key);
            }

        }

    }

}