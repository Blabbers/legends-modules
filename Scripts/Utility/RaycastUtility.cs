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




}
