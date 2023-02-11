using NaughtyAttributes;
using System.IO;
using UnityEngine;

namespace Blabbers
{
	public class Trigger : MonoBehaviour
	{
		//[SerializeReference]
		//public Filter[] Filters;
		[Foldout("Configs")] public TagFilter[] Filters;

		[Foldout("Events")] public Collider2DEvent TriggerEnter;
		[Foldout("Events")] public Collider2DEvent TriggerStay;
		[Foldout("Events")] public Collider2DEvent TriggerExit;


		[Button]
		void SetUpTrigger()
		{
			Filters = new TagFilter[1];

			Filters[0] = new TagFilter();
			Filters[0].AllowedTags = new string[1] { "Player" };
		}


		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!Filter(other))
			{
				return;
			}


			//Debug.Log($"Trigger OnTriggerEnter2D\n {other.transform.name} -> {this.transform.name}".Colored("yellow"));


			//string t1, t2;
			//t1 = other.transform.name;
			//t2 = this.transform.name;

			//if (other.transform.parent != null)
			//{
			//	t1 = other.transform.parent.name;
			//}

			//if (this.transform.parent != null)
			//{
			//	t2 = this.transform.parent.name;
			//}

			//Debug.Log($"Trigger OnTriggerEnter2D\n {t1} -> {t2}".Colored("yellow"));
			TriggerEnter.Invoke(other);
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (!Filter(other))
			{
				return;
			}

			//Debug.Log($"OnTriggerStay2D \nTag: {other.tag}".Colored("red"));
			TriggerStay.Invoke(other);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!Filter(other))
			{
				return;
			}


			//Debug.Log($"OnTriggerExit2D \nTag: {other.tag}".Colored());
			TriggerExit.Invoke(other);
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