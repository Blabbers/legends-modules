using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Blabbers.Game00
{
    public class UI_MainMenu : MonoBehaviour, ISingleton
    {
        [SerializeField]
        private Button btnPlay;
        [SerializeField]
        private Button btnContinue;
        [SerializeField]
        private Button btnNewGame;
        public UnityEvent OnStart;
        public void OnCreated()
        {
			//Debug.Log($"UI_MainMenu.OnCreated()");
			Singleton.Get<ProgressController>().OnGameDataLoaded += HandleGameDataLoaded;
        }
        public void OnDestroy()
        {

			//Debug.Log($"UI_MainMenu.OnDestroy()");
			Singleton.Get<ProgressController>().OnGameDataLoaded -= HandleGameDataLoaded;
        }

        public void Start()
		{
            OnStart?.Invoke();
        }

		public void HandleGameDataLoaded(GameProgress progress)
        {

            //Debug.Log($"UI_MainMenu.HandleGameDataLoaded() progress == null: {progress == null}");

            btnPlay.gameObject.SetActive(true);
            btnContinue.gameObject.SetActive(false);
            btnNewGame.gameObject.SetActive(false);

            // If there is progress to load, then we show the "continue / new game" buttons
            if (progress != null)
            {
                if (!progress.isNewGame)
                {
                    btnPlay.gameObject.SetActive(false);
                    btnContinue.gameObject.SetActive(true);
                    btnNewGame.gameObject.SetActive(true);
                }


            }

			//Debug.Log($"UI_MainMenu.HandleGameDataLoaded() progress == null: {progress == null}\n End Before Analytics");
			Analytics.OnGameStart();
        }
    }
}