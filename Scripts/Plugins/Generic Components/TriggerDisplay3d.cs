using Blabbers;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TriggerDisplay3d : MonoBehaviour
{

	[SerializeField] bool drawTrigger = true;
	public GenericTriggerDisplay3d triggerDisplay;

	//[Button]
	//void SetupTrigger()
	//{
	//	if (transform.GetComponent<BoxCollider>() == null) gameObject.AddComponent<BoxCollider>();
	//	gameObject.layer = LayerMask.NameToLayer("Objects");
	//}


	[Button]
	void GetTriggerData()
	{
		if (!triggerDisplay.transform)
		{
			if (FindValidCollider())
			{
				triggerDisplay.GetTriggerData();
			}
		}
		else
		{
			if (triggerDisplay.transform.gameObject.GetComponent<Collider2D>() == null)
			{
				if (FindValidCollider())
				{
					triggerDisplay.GetTriggerData();
				}
			}
		}

	}


	bool FindValidCollider()
	{
		bool hasCollider = false;

		if (gameObject.GetComponent<Collider>() != null)
		{
			hasCollider = true;
			triggerDisplay.transform = transform;
		}


		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).GetComponent<Collider>() != null)
			{
				hasCollider = true;
				triggerDisplay.transform = transform.GetChild(i);

				break;
			}
		}

		return hasCollider;

	}



	private void OnDrawGizmos()
	{
		if (triggerDisplay.collider.isTrigger)
		{
			if (!GizmosConfigs.Instance.triggerGizmos) return;
		}
		else
		{
			if (!GizmosConfigs.Instance.colliderGizmos) return;
		}
	
		if (drawTrigger) triggerDisplay.DrawGizmos();
	}
}




[Serializable]
public class GenericTriggerDisplay3d
{

	[Header("Configs")]
	public TriggerShape2d triggerShape;
	public Color gizmosColor = Color.green;
	public Color borderColor = Color.green;

	[Header("Components")]
	public Transform transform;
	public Transform parent;
	public Collider collider;
	[SerializeField] BoxCollider boxCollider;
	[SerializeField] SphereCollider sphereCollider;

	[Header("Runtime")]
	public Vector3 parentScale;
	public Vector3 colliderSize;
	public Vector3 finalScale;


	public void GetTriggerData()
	{
		if (!transform) return;
		if (!collider) collider = transform.GetComponent<Collider>();
		if(!parent) parent = transform.parent;

		if (!parent) parent = transform;

		if (transform.GetComponent<BoxCollider>() !=null) boxCollider = transform.GetComponent<BoxCollider>();
		if (transform.GetComponent<SphereCollider>() != null) sphereCollider = transform.GetComponent<SphereCollider>();
	}

	//public BoxCollider2D collider;


	public void DrawGizmos()
	{
		if (boxCollider != null)
		{
			//var scale = new Vector3(boxCollider.size.x * parent.localScale.x, boxCollider.size.y * parent.localScale.y, boxCollider.size.z * parent.localScale.z);

			//colliderSize = boxCollider.size;
			//parentScale = parent.localScale;



			Gizmos.color = gizmosColor;
			//Gizmos.DrawCube(transform.position, scale);

			Gizmos.color = borderColor;
			GizmosUtility.DrawWireCube(transform, boxCollider, borderColor);

			GizmosUtility.DrawFilledCube(transform, boxCollider, gizmosColor, borderColor);


			//Gizmos.DrawWireCube(transform.position, scale);
		}

		if(sphereCollider != null)
		{
			Gizmos.color = borderColor;
			Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);


			Gizmos.color = gizmosColor;
			Gizmos.DrawSphere(transform.position, sphereCollider.radius);
		}




	}

}

