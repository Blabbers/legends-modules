using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine.UI;

namespace Blabbers.Game00
{
	public class UI_TutorialHand : MonoBehaviour, ISingleton
	{
		private Camera camera;

		void ISingleton.OnCreated()
		{
			camera = Camera.main;
		}

		private Vector3 targetPosition;

		private bool isWorldSpace;

		public void SetHandPosition(Vector3 targetWorldPosition)
		{
			this.targetPosition = targetWorldPosition;
			isWorldSpace = true;
			UpdateToTargetPosition();
		}

		public void SetHandPosition(Vector2 targetScreenPosition)
		{
			this.targetPosition = targetScreenPosition;
			isWorldSpace = false;
			UpdateToTargetPosition();
		}

		public void ShowScreen(bool enable)
		{
			if (enable)
			{
				Singleton.Get<UI_TutorialHand>().gameObject.SetActive(true);
			}
			else
			{
				Singleton.Get<UI_TutorialHand>().gameObject.SetActive(false);
			}
		}

		void UpdateToTargetPosition()
		{
			if (isWorldSpace)
			{
				this.transform.position = camera.WorldToScreenPoint(targetPosition);
			}
			else
			{
				this.transform.position = targetPosition;
			}
		}
		
		void Update()
		{
			UpdateToTargetPosition();
		}

		public void SetHandToClosestCollectable(float delay)
		{
			Routine.Start(NextStep());
			IEnumerator NextStep()
			{
				yield return Routine.WaitSeconds(delay);
				Singleton.Get<UI_TutorialHand>().ShowScreen(true);
				//Singleton.Get<UI_TutorialHand>().SetHandPosition(Singleton.Get<GameplayController>().FindClosestCollectable().transform.position);
			}
		}
	}
}