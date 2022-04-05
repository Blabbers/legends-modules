using LoLSDK;
using UnityEngine;
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

        public void OnCreated()
        {
            Singleton.Get<ProgressController>().OnGameDataLoaded += HandleGameDataLoaded;
        }

        public void HandleGameDataLoaded(GameProgress progress)
        {
            // If there is progress to load, then we show the "continue / new game" buttons
            if (progress != null)
            {
                btnPlay.gameObject.SetActive(false);
                btnContinue.gameObject.SetActive(true);
                btnNewGame.gameObject.SetActive(true);
            }
        }
    }
}