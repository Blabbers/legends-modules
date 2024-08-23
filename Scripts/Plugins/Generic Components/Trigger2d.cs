using NaughtyAttributes;
using System.IO;
using UnityEngine;

namespace Blabbers
{
	public class Trigger2d : MonoBehaviour
	{
		//[SerializeReference]
		//public Filter[] Filters;
		[Foldout("Configs")] [ReorderableList]public TagFilter[] Filters;

		[Foldout("Events")] public Collider2DEvent OnTriggerEnter;
		[Foldout("Events")] public Collider2DEvent OnTriggerStay;
		[Foldout("Events")] public Collider2DEvent OnTriggerExit;


		[Button]
		void SetUpTrigger()
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

			if (gameObject.GetComponent<Collider2D>() == null)
			{
				hasCollider = false;
			}
			else
			{
				hasCollider = true;
				gameObject.GetComponent<Collider2D>().isTrigger = true;
			}

			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).GetComponent<Collider2D>() != null)
				{
					hasCollider = true;
					transform.GetChild(i).GetComponent<Collider2D>().isTrigger = true;
					break;
				}
			}

			return hasCollider;

		}


		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!Filter(other))
			{
				return;
			}

			//Debug.Log($"Trigger OnTriggerEnter2D\n {other.transform.name} -> {this.transform.name}".Colored("yellow"));
			OnTriggerEnter.Invoke(other);
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (!Filter(other))
			{
				return;
			}

			//Debug.Log($"OnTriggerStay2D \nTag: {other.tag}".Colored("red"));
			OnTriggerStay.Invoke(other);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!Filter(other))
			{
				return;
			}


			//Debug.Log($"OnTriggerExit2D \nTag: {other.tag}".Colored());
			OnTriggerExit.Invoke(other);
		}

		private bool Filter(Collider2D collider)
		{
			if (Filters == null)
			{
				Debug.Log("Filter == null");
				return true;
			}

			var attachedRigidbody = collider.attachedRigidbody;

			var otherGameObject = attachedRigidbody != null ? attachedRigidbody.gameObject : collider.gameObject;

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



	public enum TriggerShape2d
	{
		Circle, Box
	}

}