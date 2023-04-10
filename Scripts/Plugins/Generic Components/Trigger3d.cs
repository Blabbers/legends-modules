using NaughtyAttributes;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace Blabbers
{
	public class Trigger3d : MonoBehaviour
	{

		[Foldout("Configs")] [ReorderableList]public TagFilter[] Filters;

		public ColliderEvent TriggerEnter;
		public ColliderEvent TriggerStay;
		public ColliderEvent TriggerExit;

		[Button]
		void SetUpTrigger()
		{
			Filters = new TagFilter[1];

			Filters[0] = new TagFilter();
			Filters[0].AllowedTags = new string[1] { "Player" };

			if(!CheckForValidCollider())
			{
				gameObject.AddComponent<BoxCollider>();
				gameObject.GetComponent<BoxCollider>().isTrigger = true;
			}
			
		}

		bool CheckForValidCollider()
		{
			bool hasCollider = false;

			if (gameObject.GetComponent<Collider>() == null)
			{
				hasCollider = false;
			}
			else
			{
				hasCollider = true;
				gameObject.GetComponent<Collider>().isTrigger = true;
			}

			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).GetComponent<Collider>() != null)
				{
					hasCollider = true;
					transform.GetChild(i).GetComponent<Collider>().isTrigger = true;

					break;
				}
			}

			return hasCollider;

		}



		private void OnTriggerEnter(Collider other)
		{

			if (!Filter(other))
			{
				return;
			}


			//Debug.Log($"Trigger OnTriggerEnter\n {other.transform.name} -> {this.transform.name}".Colored("yellow"));
			TriggerEnter.Invoke(other);
		}

		private void OnTriggerStay(Collider other)
		{
			if (!Filter(other))
			{
				return;
			}

			//Debug.Log($"OnTriggerStay2D \nTag: {other.tag}".Colored("red"));
			TriggerStay.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			if (!Filter(other))
			{
				return;
			}


			//Debug.Log($"OnTriggerExit2D \nTag: {other.tag}".Colored());
			TriggerExit.Invoke(other);
		}

		private bool Filter(Collider collider)
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

}