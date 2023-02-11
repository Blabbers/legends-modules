using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectUtility
{

	public static void UpdateRectScreenPosition(ref Transform rectParent, Vector2 screenPos)
	{

		//Rect rect;
		RectTransform rectTransform;

		//rect = rectParent.GetComponent<Rect>();
		rectTransform = rectParent.GetComponent<RectTransform>();

		//rect.position = screenPos;
		rectTransform.position = screenPos;
	}


	public static Vector2 GetRectPosition(Transform rectParent)
	{
		return rectParent.position;
	}
}
