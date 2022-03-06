using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

namespace Blabbers.Game00
{
	public abstract class UI_TutorialWindowBase : MonoBehaviour
	{
		public readonly float TapAnywhereDelay = 3f;
		[ReadOnly] public bool CanTapToDisableScreen;
		public float duration;
		public UnityEvent OnWindowClosed;
		private void OnEnable()
		{
		}

		public abstract void ShowScreen(bool enable);

	}
}