using Blabbers.Game00;
using Blabbers;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericLearningTrigger : MonoBehaviour
{
	[Foldout("Components")] public GenericTriggerData generic;
	
	[Foldout("Events")] public UnityEvent OnEnter;
    //[Foldout("Events")] public ActionEvent OnFocusFinish;
    [Foldout("Events")] public UnityEvent OnExitExplanation;
	//Action callback;

	[Button]
	protected virtual void GetTriggerData()
	{
        GetTriggerGeneric();
	}

	protected void GetTriggerGeneric()
	{
        Transform triggerParent;

		triggerParent = transform.Find("Trigger");
		if (!generic.enterTrigger.triggerTransform)
		{
			generic.enterTrigger.triggerTransform = triggerParent;
			generic.enterTrigger.collider = triggerParent.GetComponent<BoxCollider2D>();

		}

		//focusParent = transform.Find("Focus");
  //      if(!generic.focusPoint) generic.focusPoint = focusParent;

	}

	protected void GenericAwake()
    {
        OnExitExplanation.AddListener(() => Singleton.Get<GameplayController>().TogglePause(false));
    }

    public virtual void TriggerTutorial(Collider2D other)
    {
        if (!generic.enterTrigger.CheckTriggerEnabled()) return;
        //OnEnter?.Invoke();
        TriggerEnterGeneric();
		UI_TutorialController.AlreadyTriggeredInThisLevel.Add(gameObject.name);
	}

    //Change this to EndTutorial
    public virtual void ExitTrigger(Collider2D other)
    {
        TutorialEndGeneric();
    }



    #region Protected

    protected bool CheckTriggerEnabled()
	{
        //This activates the Trigger
        if (!generic.enterTrigger.CheckTriggerEnabled()) return false;
        return true;
    }

    protected virtual void TriggerEnterGeneric()
    {     
       
		if (generic.hasExplanation)
		{

			if (!UI_TutorialController.AlreadyTriggeredInThisLevel.Contains(gameObject.name))
			{

				Debug.Log("GenericLearningtrigger\nTriggerEnter() hasExplanation complete".Colored("orange"));

				generic.ToggleCollider(false);
				//callback = () => TutorialEnd();

				OnEnter?.Invoke();
				return;
			}

			//Debug.Log("GenericLearningtrigger\nTriggerEnter() hasExplanation".Colored("orange"));
			//UI_TutorialController.AlreadyTriggeredInThisLevel.DisplaySet();


			return;
		}


		
		Debug.Log("GenericLearningtrigger\nTriggerEnter() NO EXPLANATION".Colored("orange"));

	}

    public virtual void TutorialEnd()
    {
        Debug.Log("GenericLearningtrigger\nTutorialEnd()".Colored("orange"));
        TutorialEndGeneric();
    }

    protected void TutorialEndGeneric()
    {
        //Singleton.Get<CameraFollow>().ResetFocus(OnExitExplanation, focusDuration);
    }
    #endregion

    #region Gizmos


    public virtual void OnDrawGizmos()
    {
		DrawTrigger(generic.enterTrigger, Color.blue);
		//DrawFocusPoint(Color.blue);
	}

    //protected void DrawFocusPoint(Color color)
    //{
    //    if (!generic.focusPoint) return;

    //    Gizmos.color = Color.white;
    //    Gizmos.DrawSphere(generic.focusPoint.position, 0.5f);

    //    Gizmos.color = color;
    //    Gizmos.DrawWireSphere(generic.focusPoint.position, 1.0f);
    //}

    protected void DrawArea(BoxTriggerData enter, BoxTriggerData exit, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(enter.collider.transform.position, exit.collider.transform.position);
        Gizmos.DrawLine(enter.collider.transform.position.To2d() + (Vector3.up * enter.collider.size.y).To2d(), exit.collider.transform.position.To2d() + (Vector3.up * enter.collider.size.y).To2d());
    }

    protected void DrawTrigger(BoxTriggerData data, Color outerColor)
    {

        GizmosUtility.DrawRectangle(data.collider.transform.position, data.collider.size, data.gizmosColor, data.collider.offset);
        GizmosUtility.DrawWireRectangle(data.collider.transform.position, data.collider.size, outerColor, data.collider.offset);
    }

    protected void DrawTrigger(Vector3 pos, Vector2 size, Vector2 offset, Color inner, Color outer)
    {
        GizmosUtility.DrawRectangle(pos, size, inner, offset);
        GizmosUtility.DrawWireRectangle(pos, size, outer, offset);
    }
    #endregion
}

[Serializable]
public class GenericTriggerData
{
    [Header("Configs")]
    public bool hasExplanation;
	public bool onlyTriggerOnce = true;
	//public bool useAveragePoint = false;
	//public float focusDuration = 1.25f;
	//public ZoomConfigs focusConfigs;

	[Header("Components")]
    public BoxTriggerData enterTrigger;
    //public Transform focusPoint;


    public void ToggleCollider(bool active)
    {
        enterTrigger.collider.gameObject.SetActive(active);
    }

}


[Serializable]
public class BoxTriggerData
{
    [Header("Runtime")]
    public bool triggered = false;

    [Header("Configs")]
    public bool onlyTriggerOnce = true;
    public Color gizmosColor = Color.green;

    [Header("Components")]
    public Transform triggerTransform;
    public BoxCollider2D collider;


    public bool CheckTriggerEnabled()
    {
        if (onlyTriggerOnce && triggered) return false;

        Debug.Log("Trigger Tutorial");

        triggered = true;
        if (onlyTriggerOnce) collider.enabled = false;

        return true;
    }


    public void DrawGizmos()
    {
        Gizmos.color = gizmosColor;
        GizmosUtility.DrawWireRectangle(triggerTransform.position, collider.size, gizmosColor, collider.offset);
    }

}

