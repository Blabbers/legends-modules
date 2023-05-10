using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Blabbers.Game00
{
	public class UI_GameplayHUD : MonoBehaviour, ISingleton
	{
		public CanvasGroup canvasGroup;
		public bool shouldHideHUDAfterPopup;
		public UnityEvent OnGameplayHudShown;
		public GameObject touchInput;


		//Lives
		public Action<int> OnLivesUpdate;
		public Action<int> OnMaxLivesSet;
		[SerializeField] int currentLives = 3;
		[SerializeField] int maxLives = 3;

		public void OnCreated()
		{
		}
		
		void Start()
		{
			if (touchInput)
			{
				touchInput.SetActive(false);
			}
		}

		void ToggleTouchInput()
		{
			//Debug.Log("ToggleTouchInput()");
			if (touchInput)
			{
				if (Game.IsMobile)
				{
					touchInput?.SetActive(true);
				}
				else
				{
					touchInput?.SetActive(false);
				}
			}
		}

		public void ToggleDisplay(bool active, bool instantly = false)
		{
			var value = active ? 1.0f : 0f;
			if (instantly)
			{
				canvasGroup.alpha = value;
			}
			else
			{
				canvasGroup.DOFade(value, 0.5f);
			}
		}

		public void HideFullHUD()
		{
			this.gameObject.SetActive(false);
		}

		public void ShowFullHUD()
		{
			//if(UI_PopupWindow.IsAnyPopupOpen) return;

			this.gameObject.SetActive(true);
			OnGameplayHudShown?.Invoke();
		}

		#region Lives
		public void SetLivesValue(int lives)
		{
			currentLives = lives;
			OnLivesUpdate?.Invoke(currentLives);
		}

		public void UpdateMaxLives(int lives)
		{
			maxLives = lives;
			OnMaxLivesSet?.Invoke(maxLives);
		}
		#endregion

	}
}