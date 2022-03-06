using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using UnityEngine;
using UnityEngine.UI;

namespace Blabbers.Game00
{
	public class UI_PopupVictoryScreen : UI_PopupWindow, ISingleton
	{
		// Objects
		[SerializeField]
		private List<GameObject> stars;

		/// <summary>
		/// Invokes the victory screen with given star amount (from 1 to 3)
		/// </summary>
		public void ShowLevelVictoryScreen(int starAmount, int score)
		{
			base.ShowPopup();

			//OpenedPopupList.Add(this.gameObject);
			//this.gameObject.SetActive(true);

			// Functionality
			if (ProgressController.GameProgress.levels != null)
			{
				ProgressController.GameProgress.levels[ProgressController.GameProgress.currentLevelId].starAmount = Mathf.Max(starAmount, ProgressController.GameProgress.levels[ProgressController.GameProgress.currentLevelId].starAmount);
				ProgressController.GameProgress.levels[ProgressController.GameProgress.currentLevelId].score = Mathf.Max(score, ProgressController.GameProgress.levels[ProgressController.GameProgress.currentLevelId].score);
			}

			foreach (var item in stars)
			{
				item.SetActive(false);
			}

			int i = 1;
			foreach (var item in stars)
			{
				if (i > starAmount)
					break;

				item.SetActive(true);
				i++;
			}
		}

		public override void HidePopup()
		{
			base.HidePopup();

			//this.gameObject.SetActive(false);
			//OpenedPopupList.Remove(this.gameObject);
		}
	}
}