namespace Blabbers.Game00
{
	public class UI_PopupTryAgain : UI_PopupWindow, ISingleton
	{
        public void TryAgain()
        {
            Singleton.Get<SceneLoader>().ReloadCurrentScene();
        } 
	}
}