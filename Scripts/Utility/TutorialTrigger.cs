using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Blabbers;
using NaughtyAttributes;

public class TutorialTrigger : MonoBehaviour
{

    [Header("Configs")]
	public int id = 0;
	public TriggerShape2d shape;
    public bool onlyTriggerOnce = true;
    public UnityEvent Event;

    [Header("Runtime")]
    [SerializeField] bool triggered = false;

    [Header("Components")]
    //public CircleCollider2D collider;
    public Collider2D collider;
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

		//Debug.Log("Trigger Tutorial".Colored("orange"));

		triggered = true;
        if (onlyTriggerOnce) collider.enabled = false;

        Event.Invoke();
    }


    private void OnDrawGizmos()
    {
        GetComponents();

        Gizmos.color = Color.blue;
        if (shape == TriggerShape2d.Circle)
        {
            Gizmos.DrawWireSphere(transform.position, circle.radius);
        }
        else
        {
            GizmosUtility.DrawWireRectangle(transform.position, box.size, Color.cyan, box.offset);
			GizmosUtility.DrawRectangle(transform.position, box.size, Color.cyan, box.offset , 0.5f);
		}
        //Gizmos.DrawWireSphere(transform.position, collider.);
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

