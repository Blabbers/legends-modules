using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ObjectStatusTracker : MonoBehaviour
{
	public Transform obj;
	public Rigidbody2D rigid2D;

	[Foldout("Rigidbody")] public Vector2 velocity;
	[Foldout("Rigidbody")] public float velocityMagnitude;

	[Foldout("Transform")] public Vector3 position;
	[Foldout("Transform")] public Vector3 right;
	[Foldout("Transform")] public Vector3 rotation;

#if UNITY_EDITOR
	// Update is called once per frame
	void Update()
	{
		if (obj == null) return;
		position = obj.position;
		rotation = obj.rotation.eulerAngles;
		right = obj.right;

		GetRigidbodyData();

	}

	void GetRigidbodyData()
	{
		if (rigid2D == null) return;

		velocity = rigid2D.velocity;
		velocityMagnitude = rigid2D.velocity.magnitude;
	}

#endif
}
