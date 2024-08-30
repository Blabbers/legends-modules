using Blabbers;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionChecker2d : MonoBehaviour
{
	[Foldout("Configs")][ReorderableList] public TagFilter[] Filters;
	[Foldout("Events")] public Collision2DEvent OnCollisionEnter;
	[Foldout("Events")] public Collision2DEvent OnCollisionStay;
	[Foldout("Events")] public Collision2DEvent OnCollisionExit;

	[Button]
	void SetUpCollider()
	{
		Filters = new TagFilter[1];

		Filters[0] = new TagFilter();
		Filters[0].AllowedTags = new string[1] { "Player" };


		if (!CheckForValidCollider())
		{
			gameObject.AddComponent<BoxCollider2D>();
		}
	}

	bool CheckForValidCollider()
	{
		bool hasCollider = false;

		if (gameObject.GetComponent<UnityEngine.Collider2D>() == null)
		{
			hasCollider = false;
		}
		else
		{
			hasCollider = true;
			gameObject.GetComponent<UnityEngine.Collider2D>().isTrigger = true;
		}

		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).GetComponent<UnityEngine.Collider2D>() != null)
			{
				hasCollider = true;
				transform.GetChild(i).GetComponent<UnityEngine.Collider2D>().isTrigger = true;
				break;
			}
		}

		return hasCollider;

	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!Filter(other)) return;

		//Debug.Log($"OnTriggerStay2D \nTag: {other.tag}".Colored("red"));
		OnCollisionEnter.Invoke(other);
	}


	private void OnCollisionStay2D(Collision2D other)
	{
		if (!Filter(other)) return;
		OnCollisionStay.Invoke(other);
	}


	private void OnCollisionExit2D(Collision2D other)
	{
		if (!Filter(other)) return;
		OnCollisionExit.Invoke(other);
	}



	private bool Filter(Collision2D collision)
	{
		if (Filters == null)
		{
			Debug.Log("Filter == null");
			return true;
		}

		//These are inverted for some reason, and I hate it
		//Collision.rigidbody == the rigidbody on the OTHER object that collided with this one
		//Collision.otherRigidbody == the rigidbody on THIS object
		//--
		//Collision.collider == the collider on the OTHER object that collided with this one
		//Collision.otherCollider == the collider on THIS object

		var attachedRigidbody = collision.rigidbody;
		var otherGameObject = attachedRigidbody != null ? attachedRigidbody.gameObject : collision.gameObject;

		foreach (var filter in Filters)
		{
			if (!filter.Check(otherGameObject))
			{
				return false;
			}
		}

		return true;
	}
}
