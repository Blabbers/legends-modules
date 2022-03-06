using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Blabbers.Game00
{
	public class UI_DidYouKnowPopupManager : MonoBehaviour
	{
		public GameData gameData;
		public List<GameObject> didYouKnowPopups;

		void Start()
		{
			ShowDidYouKnowPopup();
		}

		void ShowDidYouKnowPopup()
		{
			int level = ProgressController.GameProgress.currentLevelId;
			if (level > 0)
			{
				if (level <= gameData.totalLevels && didYouKnowPopups.Count >= level)
				{
					if (didYouKnowPopups[level - 1] != null)
					{
						didYouKnowPopups[level - 1].SetActive(true);
					}
				}
			}
		}
	}
}