using UnityEngine;
using UnityEngine.Events;

namespace Blabbers.Game00
{
	public class UI_GameplayHUD : MonoBehaviour, ISingleton
	{
		public bool shouldHideHUDAfterPopup;
		public UnityEvent OnGameplayHudShown;
		public GameObject touchInput;
		
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
	}
}