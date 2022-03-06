using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Blabbers.Game00
{
    public class UI_PopupsManager : MonoBehaviour
    {
        public static UI_PopupsManager Singleton;
        public List<GameObject> thingsToHideIfPlayerIsStuck;

        private void Awake()
        {
            Singleton = this;
        }

        private void OnEnable()
        {
            if (SceneLoader.isStuckOnThisLevel && thingsToHideIfPlayerIsStuck != null && thingsToHideIfPlayerIsStuck.Count > 0)
            {
                foreach (var item in thingsToHideIfPlayerIsStuck)
                {
                    item.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Invokes the game over screen
        /// </summary>
        public void TryShowGameFinishedScreen()
        {
            int level = ProgressController.GameProgress.reachedLevel;
            Debug.Log("Cur Level: " + level + " / " + ProgressController.GameProgress.levels.Length);
            if (level >= ProgressController.GameProgress.levels.Length)
            {
                UI_GameFinishedController.Singleton.Show();
            }
        }
    }
}