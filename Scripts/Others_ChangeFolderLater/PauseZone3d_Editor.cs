

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseZone3d_Editor : MonoBehaviour
{

	public Collider trigger;


	private void Awake()
	{
#if UNITY_EDITOR
		FindValidCollider();
#endif
	}


	[Button]
	void SetupPauseZone()
	{
		FindValidCollider();
		gameObject.tag = "EditorOnly";
	}

	bool FindValidCollider()
	{
		bool hasCollider = false;

		if (gameObject.GetComponent<Collider>() != null)
		{
			hasCollider = true;
			trigger = gameObject.GetComponent<Collider>();
		}


		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).GetComponent<Collider>() != null)
			{
				hasCollider = true;
				trigger = transform.GetChild(i).GetComponent<Collider>();

				break;
			}
		}

		return hasCollider;

	}


	public void TriggerEnter(Collider col)
	{
#if UNITY_EDITOR
		trigger.gameObject.SetActive(false);
		Debug.Break();
		Debug.ClearDeveloperConsole();
#endif
	}




}


