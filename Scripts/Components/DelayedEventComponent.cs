using BeauRoutine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEventComponent : MonoBehaviour
{

	public UnityEvent OnDelayFinished;
	public bool playOnEnabled = true;
	public float delay = 1.0f;

	private void OnEnable()
	{
		
	}


	public void Execute()
	{
		Routine.Start(Routine.Delay(() => OnDelayFinished?.Invoke(), delay));
	}




}
