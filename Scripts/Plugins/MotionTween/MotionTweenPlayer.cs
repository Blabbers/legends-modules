using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.Events;
using System;

namespace Blabbers.Game00
{
	public class MotionTweenPlayer : MonoBehaviour
	{
		[BoxGroup("Default Tween")]
		public MotionTween tween;
		[BoxGroup("Default Tween")]
		public bool isLoop;
		[BoxGroup("Default Tween")]
		[ShowIf(nameof(isLoop))]
		public LoopType loopType;
		[BoxGroup("Default Tween")]
		public bool playOnEnabled = true;
		[BoxGroup("Default Tween")]
		public bool playOnlyOnce = false;		
		private bool hasPlayed;
		[BoxGroup("Default Tween")]
		[ShowIf(nameof(playOnEnabled))]
		public float delayOnEnable;
		[BoxGroup("Default Tween")]
		public UnityEvent OnAnimationStart, OnAnimationFinished;

		[BoxGroup("Exit Tween")]
		public bool hasExitTween;
		[BoxGroup("Exit Tween")]
		[ShowIf(nameof(hasExitTween))]
		[Tooltip("Call from 'DisableWithExitTween()' method. Tween to play before disabling the object. This will NOT trigger the OnAnimationStart/Finshed events above.")]
		public MotionTween exitTween;
		[ShowIf(nameof(hasExitTween))]
		[BoxGroup("Exit Tween")]
		public UnityEvent OnHideAnimationStart, OnHideAnimationFinished;

		public Vector3 StartAnchoredPosition { get; private set; }
		public Vector3 StartPosition { get; private set; }
		public Vector3 StartEulerAngles { get; private set; }
		public Vector3 StartScale { get; private set; }
		public RectTransform RectTransform { get; private set; }
		public CanvasScaler CanvasScaler { get; private set; }

        private bool hasAwakened;
        
		[Button()]
		public void PreviewAnimation()
		{
			PlayTween();
		}
		
		private void Awake()
		{
            if(hasAwakened) return;
            hasAwakened = true;
            
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
				PlayTween(delayOnEnable);
			}
		}
		public void DisableWithExitTween()
		{
			DisableWithExitTween(null);
		}
		public void DisableWithExitTween(Action onFinished)
		{
			if (hasExitTween)
			{
				OnHideAnimationStart?.Invoke();
				exitTween.PlaySequence(this, playEvents: true, isLoop: false, null, HandleExitTweenFinished);

				void HandleExitTweenFinished()
				{
					onFinished?.Invoke();
					gameObject.SetActive(false);
					OnHideAnimationFinished?.Invoke();
				}
			}
		}

		private void OnDisable()
		{
			//ResetTween();
		}

        public void PlayTween()
        {
            PlayTween(0f);
        }
        
		public void PlayTween(float delay)
        {
			if (playOnlyOnce && hasPlayed)
				return;			
            Awake();
            if (!isLoop || loopType == LoopType.Restart)
			{
				ResetTween();
			}

            if (delay > 0f)
            {
				Routine.Start(Run());
				IEnumerator Run()
				{
					yield return Routine.WaitSeconds(delay);
					this.tween.PlaySequence(this, playEvents: true, isLoop, OnAnimationStart.Invoke, OnAnimationFinished.Invoke);
				}
			}
            else
            {
                this.tween.PlaySequence(this, playEvents: true, isLoop, OnAnimationStart.Invoke, OnAnimationFinished.Invoke);
            }
			hasPlayed = true;
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