using Blabbers;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDisplay : MonoBehaviour
{
	public GenericTriggerDisplay triggerDisplay;


	[Button]
	void GetTriggerData()
	{
		if (!triggerDisplay.transform) triggerDisplay.transform = this.transform;
		triggerDisplay.GetTriggerData();
	}

	private void OnDrawGizmos()
	{
		triggerDisplay.DrawGizmos();
	}
}




[Serializable]
public class GenericTriggerDisplay
{

	[Header("Configs")]
	public TriggerShape2d triggerShape;
	public Color gizmosColor = Color.green;
	public Color borderColor = Color.green;

	[Header("Components")]
	public Transform transform;
	public Transform parent;
	public Collider2D collider;
	[SerializeField] BoxCollider2D boxCollider;
	[SerializeField] CircleCollider2D circleCollider;


	public void GetTriggerData()
	{
		if (!transform) return;
		if (!collider) collider = transform.GetComponent<Collider2D>();
		if(!parent) parent = transform.parent;

		if (!parent) parent = transform;

		if (transform.GetComponent<BoxCollider2D>() !=null) boxCollider = transform.GetComponent<BoxCollider2D>();
		if (transform.GetComponent<CircleCollider2D>() != null) circleCollider = transform.GetComponent<CircleCollider2D>();
	}

	//public BoxCollider2D collider;


	public void DrawGizmos()
	{
		//Gizmos.color = borderColor;
		GizmosUtility.DrawWireRectangle(transform.position , new Vector2(boxCollider.size.x * parent.localScale.x, boxCollider.size.y *parent.localScale.y), borderColor, collider.offset);
		GizmosUtility.DrawRectangle(transform.position, new Vector2(boxCollider.size.x * parent.localScale.x, boxCollider.size.y * parent.localScale.y), gizmosColor, collider.offset);
	}

}

