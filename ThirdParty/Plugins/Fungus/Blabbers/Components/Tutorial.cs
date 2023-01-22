using Fungus;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
	[SerializeField]
	private LocalizedString text;
	[SerializeField]
	private Writer writer;
	[SerializeField]
	private UnityEvent OnWritingCompleted;

	private void OnEnable()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			// Needs to wait a frame, for execution order purposes.
			yield return null;
			StartCoroutine(writer.Write(text, onComplete: HandleCompleted));
		}
	}

	void HandleCompleted()
	{
		OnWritingCompleted?.Invoke();
		Debug.Log("Writer completed.");
	}
}
