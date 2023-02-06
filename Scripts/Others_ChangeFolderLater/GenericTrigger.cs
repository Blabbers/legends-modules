using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Blabbers;
using NaughtyAttributes;
using System;

public class GenericTrigger : MonoBehaviour
{

    //[Header("Configs")]


    [Header("Main")]
    [SerializeField] bool triggered = false;
	[SerializeField] bool showGizmos = false;
	public UnityEvent OnTriggerEnter;
	public UnityEvent OnTriggerExit;


	[Foldout("Configs")] public int id = 0;
	[Foldout("Configs")] public TriggerShape2d shape;
	[Foldout("Configs")] public bool onlyTriggerOnce = true;
	[Foldout("Configs")] public Action<int> HiddenEvent;
	[Foldout("Configs")] public Action<int> HiddenExitEvent;
	[Foldout("Configs")] public Color color1 = Color.cyan;

	//public CircleCollider2D collider;
	[Foldout("Components")] public Collider2D collider;
    CircleCollider2D circle;
    BoxCollider2D box;

	#region Editor

#if UNITY_EDITOR

	private void Awake()
	{
		if (Application.isEditor)
		{
			GetCollider();
		}

	}

	[Button]
	void AdjustOffset()
	{
		GetCollider();

		if (shape == TriggerShape2d.Box)
		{
			box.offset = new Vector2(box.offset.x, box.size.y / 2);
		}

	}

	void GetCollider()
	{
		if (shape == TriggerShape2d.Circle)
		{
			circle = collider.gameObject.GetComponent<CircleCollider2D>();
		}
		else
		{
			box = collider.gameObject.GetComponent<BoxCollider2D>();
		}
	}

#endif

	#endregion

	public void TriggerTutorial(Collider2D other)
    {
        if (onlyTriggerOnce && triggered) return;
        Debug.Log("Trigger Tutorial".Colored("orange"));
        triggered = true;
        if (onlyTriggerOnce) collider.enabled = false;

        OnTriggerEnter.Invoke();
        HiddenEvent.Invoke(id);

	}

	public void TriggerExit(Collider2D other)
	{
		if (onlyTriggerOnce && triggered) return;
		triggered = true;
		if (onlyTriggerOnce) collider.enabled = false;

		OnTriggerEnter.Invoke();
		HiddenEvent.Invoke(id);

	}

	private void OnDrawGizmos()
    {
        GetComponents();

		if (showGizmos)
		{
			Gizmos.color = Color.blue;
			if (shape == TriggerShape2d.Circle)
			{
				Gizmos.DrawWireSphere(transform.position, circle.radius);
			}
			else
			{
				GizmosUtility.DrawWireRectangle(transform.position, box.size, color1, box.offset);
				GizmosUtility.DrawRectangle(transform.position, box.size, color1, box.offset, 0.15f);
			}
		}
    }


    void GetComponents()
    {
        if (shape == TriggerShape2d.Circle)
        {
            if (circle != null) return;
            circle = collider.gameObject.GetComponent<CircleCollider2D>();
        }
        else
        {
            if (box != null) return;
            box = collider.gameObject.GetComponent<BoxCollider2D>();
        }
    }
}

