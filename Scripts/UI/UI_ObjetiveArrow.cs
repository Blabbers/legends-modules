using Blabbers.Game00;
using DG.Tweening;
using NaughtyAttributes;
//using System.Collections;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_ObjetiveArrow : MonoBehaviour, ISingleton
{
	[Foldout("Runtime")][SerializeField] private Camera cam;
	[Foldout("Runtime")][SerializeField] private Transform origin;
	[Foldout("Runtime")][SerializeField] private Transform target;
	[Foldout("Runtime")][SerializeField] bool isActive;
	private Vector2 dir;
	[SerializeField] float distance;

	[Foldout("Configs")]
	[SerializeField] Vector2 borders = Vector2.zero;
	[Foldout("Configs")][SerializeField] Vector2 fadeRange;

	[Foldout("Components")][SerializeField] private CanvasGroup pointerGroup;
	[Foldout("Components")]	[SerializeField] private Transform pointer;


	void Start()
	{		
		if (!cam) cam = Camera.main;
	}

	// Update is called once per frame
	void Update()
    {
		if (!origin || !target) return;

		PositionPointer();
		PointTowardsTarget();
		UpdatePointerAlpha();
	}


	#region External
	public void SetTarget(Transform newTarget)
	{
		target = newTarget;
	}
	public void SetOrigin(Transform newOrigin)
	{
		origin = newOrigin;
	}
	public void SetCamera(Camera cam)
	{
		this.cam = cam;
	}

	public void ToggleArrow(bool active)
	{
		Debug.Log($"<UI_ObjetiveArrow> ToggleArrow {active}");
		isActive = active;

		if (active) {
			pointerGroup.DOFade(1.0f, 0.5f);		
		} 
		else
		{
			pointerGroup.DOFade(0.0f, 0.5f);
		}

	}

	#endregion

	void PointTowardsTarget()
	{	
		dir = target.position - origin.position;
		pointer.AllignRightWith(dir);
	}

	void PositionPointer()
	{
		Vector2 targetPos;

		targetPos = CameraToWorldUtility.WorldToCameraPosition_Clamped(target, cam, borders.x, borders.y);
		pointer.position = targetPos;

		distance = Vector2.Distance(target.position, origin.position);
	}

	void UpdatePointerAlpha()
	{
		if (!isActive) return;
		pointerGroup.alpha = Mathf.InverseLerp(fadeRange.x, fadeRange.y, distance);
	}


	public void OnCreated()
	{

	}

	void OnDrawGizmos()
	{
		if (!target) return;

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(target.position, fadeRange.x);

		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(target.position, fadeRange.y);
	}
}
