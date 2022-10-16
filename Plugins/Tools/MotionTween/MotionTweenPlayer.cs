using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
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
		public bool playOnlyOnce = false;
		private bool hasPlayed;
		[ShowIf(nameof(playOnEnabled))]
		public float delayOnEnable;
		public UnityEvent OnAnimationStart, OnAnimationFinished;

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
				//StartCoroutine(Run());
				Routine.Start(Run());
				IEnumerator Run()
				{
					yield return Routine.WaitSeconds(delay);
					this.tween.PlaySequence(this);
				}
			}
            else
            {
                this.tween.PlaySequence(this);
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