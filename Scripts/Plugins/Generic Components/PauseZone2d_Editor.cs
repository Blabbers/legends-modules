

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseZone2d_Editor : MonoBehaviour
{

	public Collider2D trigger;


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

		if (gameObject.GetComponent<Collider2D>() != null)
		{
			hasCollider = true;
			trigger = gameObject.GetComponent<Collider2D>();
		}


		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).GetComponent<Collider2D>() != null)
			{
				hasCollider = true;
				trigger = transform.GetChild(i).GetComponent<Collider2D>();

				break;
			}
		}

		return hasCollider;

	}


	public void TriggerEnter(Collider2D col)
	{
#if UNITY_EDITOR
		trigger.gameObject.SetActive(false);
		Debug.Break();
		Debug.ClearDeveloperConsole();
#endif
	}




}


