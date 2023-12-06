using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEventComponent : MonoBehaviour
{

	public UnityEvent OnDelayFinished;
	public bool playOnEnabled = true;
	[SerializeField] bool ignoreTimescale = true;

	[MinValue(0)]
	public float delay = 1.0f;

	private void OnEnable()
	{
		if (playOnEnabled) Execute();
	}

	public void Execute()
	{
		//Routine.Start(Routine.Delay(() => OnDelayFinished?.Invoke(), delay));

		StartCoroutine(_Delay());
		IEnumerator _Delay()
		{

			if (ignoreTimescale)
			{
				yield return new WaitForSecondsRealtime(delay);
			}
			else
			{
				yield return new WaitForSeconds(delay);
			}
			
			OnDelayFinished?.Invoke();
		}
	}

}
