using System;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

namespace Blabbers.Game00
{
	public abstract class UI_TutorialWindowBase : MonoBehaviour
	{
		public Slider HoldSlider;
		public AnimationCurve SliderCurve;
		public float TapAnywhereDelay = 3f;
        public bool autoEnableHoldToSkip = true;
		[ReadOnly] public bool CanTapToDisableScreen;
		public float duration;
		public UnityEvent OnWindowOpened;
		public UnityEvent OnWindowClosed;
		private void OnEnable()
		{
			OnWindowOpened?.Invoke();
            ShowTapTextAfterDelay();
            var gameplayHUD = Singleton.Get<UI_GameplayHUD>();
            if (gameplayHUD) { gameplayHUD.HideFullHUD(); }
		}

        private void OnDisable()
        {
            var gameplayHUD = Singleton.Get<UI_GameplayHUD>();
            if (gameplayHUD) { gameplayHUD.ShowFullHUD(); }
        }

        public abstract void ShowScreen();
		public abstract void HideScreen();

		public void ShowTapTextAfterDelay()
		{
			if (!HoldSlider) return;
			duration = 0;
			CanTapToDisableScreen = false;
			HoldSlider.gameObject.SetActive(false);

            if (autoEnableHoldToSkip)
            {
                StartCoroutine(Routine());

                IEnumerator Routine()
                {
                    yield return new WaitForSecondsRealtime(TapAnywhereDelay);
                    EnableSkip();
                }
            }
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
					duration += Time.unscaledDeltaTime;
					if (HoldSlider.value >= 1)
					{
						Finish();
					}
				}
				else
				{
					duration -= Time.unscaledDeltaTime;
				}
				duration = Mathf.Clamp01(duration);
				HoldSlider.value = SliderCurve.Evaluate(duration);
			}

#if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.PageDown))
			{
				Finish();
			}
#endif
		}

		private void Finish()
		{
			HideScreen();
			OnWindowClosed?.Invoke();
			CanTapToDisableScreen = false;
		}
	}
}