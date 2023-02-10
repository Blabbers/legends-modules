using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RaycastUtility
{
	public static bool RaycastCheck2D(Vector2 origin, Vector2 dir, float distance, LayerMask mask, out RaycastHit2D hit)
	{
		hit = new RaycastHit2D();
		hit = Physics2D.Raycast(origin, dir, distance, mask);

		if (hit.collider != null)
		{
			return true;
		}

		return false;
	}


	public static bool RaycastCheck3D(Vector3 origin, Vector3 dir, float distance, LayerMask mask, out RaycastHit hit)
	{
		Ray ray = new Ray(origin, dir);
		hit = new RaycastHit();

		if (Physics.Raycast(ray, out hit, distance, mask))
		{
			return true;
		}

		return false;
	}

}
