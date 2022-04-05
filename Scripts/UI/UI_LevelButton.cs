using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Blabbers.Game00
{
	public class UI_LevelButton : MonoBehaviour
	{
		public int myLevel = 0;
        public bool autoLoadLevel = true;
        
		public Image targetImage;
		//public Sprite levelLocked, levelOpen;
		public List<GameObject> stars;

		public List<GameObject> showWhenOpen;
		public List<GameObject> hideWhenLocked;

		public TextMeshProUGUI textLevelNumberActive;
        public TextMeshProUGUI textLevelNumberLocked;
        
#if UNITY_EDITOR
		private void OnValidate()
		{
			SetNumberText();
		}
#endif

		void Start()
		{
			var button = this.gameObject.GetComponent<Button>();

            if (autoLoadLevel)
            {
                // adding click event to the button, it will load the scene "Level1" <- using "myLevel" as the number
                button.onClick.AddListener(() => { Singleton.Get<SceneLoader>().LoadGameLevel(myLevel); });    
            }

            button.interactable = true;

			if (ProgressController.GameProgress.reachedLevel < myLevel - 1)
			{
				button.interactable = false;
                foreach (var item in hideWhenLocked)
				{
					if (item)
						item.SetActive(false);
				}
			}
			// level open
			if (ProgressController.GameProgress.reachedLevel == myLevel - 1)
			{
				foreach (var item in showWhenOpen)
				{
					if (item)
						item.SetActive(true);
				}
			}

			// hide all stars
			for (int i = 0; i < stars.Count; i++)
			{
				stars[i].SetActive(false);
			}

			// shows level stars
			if (ProgressController.GameProgress.levels != null)
			{
				var length = ProgressController.GameProgress.levels[myLevel - 1].starAmount;
				for (int i = 0; i < stars.Count; i++)
				{
					stars[i].SetActive(i < length);
				}
			}
		}

		private void SetNumberText()
		{
			if (!textLevelNumberActive.text.Equals(myLevel.ToString()))
			{
                textLevelNumberActive.text = myLevel.ToString();
			}
            if (!textLevelNumberLocked.text.Equals(myLevel.ToString()))
            {
                textLevelNumberLocked.text = myLevel.ToString();
            }
		}
	}
}