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
		public readonly float TapAnywhereDelay = 3f;
		[ReadOnly] public bool CanTapToDisableScreen;
		public float duration;
		public UnityEvent OnWindowClosed;
		private void OnEnable()
		{
			ShowTapTextAfterDelay();
		}

		public abstract void ShowScreen(bool enable);

		public void ShowTapTextAfterDelay()
		{
			duration = 0;
			CanTapToDisableScreen = false;
			HoldSlider.gameObject.SetActive(false);

			StartCoroutine(Routine());
			IEnumerator Routine()
			{
				yield return new WaitForSeconds(TapAnywhereDelay);
				CanTapToDisableScreen = true;
				HoldSlider.gameObject.SetActive(true);
			}
		}

		private void Update()
		{
			if (CanTapToDisableScreen)
			{
				if (Input.anyKey)
				{
					duration += Time.deltaTime;
					if (HoldSlider.value >= 1)
					{
						ShowScreen(false);
						OnWindowClosed?.Invoke();
						CanTapToDisableScreen = false;
					}
				}
				else
				{
					duration -= Time.deltaTime;
				}
				duration = Mathf.Clamp01(duration);
				HoldSlider.value = SliderCurve.Evaluate(duration);
			}
		}
	}
}