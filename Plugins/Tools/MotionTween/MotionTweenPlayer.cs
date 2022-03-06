using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.Events;

namespace Blabbers.Game00
{
	public class MotionTweenPlayer : MonoBehaviour
	{
		public MotionTween tween;
		public bool isLoop;
		[ShowIf(nameof(isLoop))]
		public LoopType loopType;
		public bool playOnEnabled = true;
		[ShowIf(nameof(playOnEnabled))]
		public float delayOnEnable;
		public UnityEvent OnAnimationStart, OnAnimationFinished;

		public Vector3 StartAnchoredPosition { get; private set; }
		public Vector3 StartPosition { get; private set; }
		public Vector3 StartEulerAngles { get; private set; }
		public Vector3 StartScale { get; private set; }
		public RectTransform RectTransform { get; private set; }
		public CanvasScaler CanvasScaler { get; private set; }

		[Button()]
		public void PreviewAnimation()
		{
			PlayTween();
		}
		
		private void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
			CanvasScaler = GetComponentInParent<CanvasScaler>();

			StartAnchoredPosition = this.RectTransform.anchoredPosition;
			StartPosition = this.transform.position;
			StartEulerAngles = this.transform.eulerAngles;
			StartScale = this.transform.localScale;
		}

		private void OnEnable()
		{
			if (playOnEnabled)
			{
				StartCoroutine(Routine());
				IEnumerator Routine()
				{
					if (delayOnEnable > 0f)
					{
						yield return new WaitForSeconds(delayOnEnable);
					}
					PlayTween();
				}
			}
		}
		private void OnDisable()
		{
			//ResetTween();
		}

		public void PlayTween()
		{
			// Remove this if we don't depend on a local coroutine to start the animation anymore
			if (!this.gameObject.activeSelf)
				return;

			if (!isLoop || loopType == LoopType.Restart)
			{
				ResetTween();
			}
			this.tween.PlaySequence(this);
		}

		public void ResetTween()
		{
			this.StopAllCoroutines();
			this.transform.DOKill();

			this.RectTransform.anchoredPosition = StartAnchoredPosition;
			this.transform.eulerAngles = StartEulerAngles;
			this.transform.localScale = StartScale;
		}
	}
}