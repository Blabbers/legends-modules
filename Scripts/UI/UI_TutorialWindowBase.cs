using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Blabbers.Game00
{
	public abstract class UI_TutorialWindowBase : MonoBehaviour
	{
		[Foldout("Components")]
		public Slider HoldSlider;
		[Foldout("Components")]
		public AnimationCurve SliderCurve;
		[Foldout("Components")]
		[SerializeField, ReadOnly] 
		private bool CanTapToDisableScreen;
		public UnityEvent OnWindowOpened;
		public UnityEvent OnWindowClosed;
		private float holdDuration;

		private void OnEnable()
		{
			// Calls "ShowScreen" to be sure this works out of the box.
			ShowScreen();
			HideHoldSlider();
			var gameplayHUD = Singleton.Get<UI_GameplayHUD>();
            if (gameplayHUD) { gameplayHUD.HideFullHUD(); }
			OnWindowOpened?.Invoke();
		}

        private void OnDisable()
        {
			// Calls "HideScreen" to be sure this works out of the box.
			HideScreen();
			var gameplayHUD = Singleton.Get<UI_GameplayHUD>();
            if (gameplayHUD) { gameplayHUD.ShowFullHUD(); }
			OnWindowClosed?.Invoke();
		}

        public abstract void ShowScreen();
		public abstract void HideScreen();

		public void HideHoldSlider()
		{
			if (!HoldSlider) return;
			holdDuration = 0;
			CanTapToDisableScreen = false;
			HoldSlider.gameObject.SetActive(false);
        }

        public void EnableSkip()
        {
            CanTapToDisableScreen = true;
            HoldSlider.gameObject.SetActive(true);
        }

		private void Update()
		{
			if(!HoldSlider) return;

			if (CanTapToDisableScreen)
			{
				if (Input.anyKey)
				{
					holdDuration += Time.unscaledDeltaTime;
					if (HoldSlider.value >= 1)
					{
						Finish();
					}
				}
				else
				{
					holdDuration -= Time.unscaledDeltaTime;
				}
				holdDuration = Mathf.Clamp01(holdDuration);
				HoldSlider.value = SliderCurve.Evaluate(holdDuration);
			}

#if UNITY_EDITOR || DEVELOPMENT_BUILD
			if (Input.GetKeyDown(KeyCode.PageDown))
			{
				Finish();
			}
#endif
		}

		private void Finish()
		{
			HideScreen();
			CanTapToDisableScreen = false;
		}
	}
}