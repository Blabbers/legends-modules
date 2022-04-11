using TMPro;

namespace Blabbers.Game00
{
    public class UI_PopupTryAgain : UI_PopupWindow, ISingleton
    {
        public TextMeshProUGUI field;

        public void TryAgain()
        {
            Singleton.Get<SceneLoader>().ReloadCurrentScene();
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