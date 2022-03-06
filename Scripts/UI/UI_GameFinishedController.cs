using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Blabbers.Game00
{
	public class UI_GameFinishedController : MonoBehaviour
	{
		public static UI_GameFinishedController Singleton;

		[Header("Object References")]
		public GameObject gameFinishedScreen;
		public Button buttonFinish;

		public static bool IsScreenActive
		{
			get
			{
				return Singleton.gameFinishedScreen.activeSelf;
			}
		}

		private void Awake()
		{
			Singleton = this;
		}

		private void OnEnable()
		{
			TryShowGameFinishedScreen();
		}

		/// <summary>
		/// Invokes the game over screen
		/// </summary>
		public void TryShowGameFinishedScreen()
		{
			if (ProgressController.GameProgress.HasFinishedTheGame)
			{
				Show();
			}
		}

		/// <summary>
		/// Invokes the game finished screen (when game is finally over)
		/// </summary>
		public void Show()
		{
			ProgressController.AddProgress();
			this.gameFinishedScreen.SetActive(true);
		}
	}
}