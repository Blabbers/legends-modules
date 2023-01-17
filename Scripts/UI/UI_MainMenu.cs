using LoLSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Blabbers.Game00
{
    public class UI_MainMenu : MonoBehaviour, ISingleton
    {
        public static bool hasFinishedTheGameOnce = false;
        [SerializeField]
        private Button btnPlay;
        [SerializeField]
        private Button btnContinue;
        [SerializeField]
        private Button btnNewGame;
        public UnityEvent OnStart;
        public void OnCreated()
        {
            Singleton.Get<ProgressController>().OnGameDataLoaded += HandleGameDataLoaded;
        }

        public void Start()
		{
            OnStart?.Invoke();


            if (hasFinishedTheGameOnce)
			{
				EnablePlayButton();
			}                
        }

		public void HandleGameDataLoaded(GameProgress progress)
		{
			//Debug.Log("isNewGame:" + progress.isNewGame);
			EnablePlayButton();

			// If there is progress to load, then we show the "continue / new game" buttons
			if (progress != null)
			{
				if (!progress.isNewGame)
				{
					EnableNewGameAndContinueButtons();
				}


			}
			Analytics.OnGameStart();
		}

		private void EnableNewGameAndContinueButtons()
		{
			btnPlay.gameObject.SetActive(false);
			btnContinue.gameObject.SetActive(true);
			btnNewGame.gameObject.SetActive(true);
		}

		private void EnablePlayButton()
		{
			btnPlay.gameObject.SetActive(true);
			btnContinue.gameObject.SetActive(false);
			btnNewGame.gameObject.SetActive(false);
		}
	}
}