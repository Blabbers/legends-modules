using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraToWorldUtility
{
	#region World to UI
	public static void SetUIToRelativeWorldPosition(ref Transform currentUI ,Transform target, Camera cam, float offsetX = 0, float offsetY = 0)
	{
		currentUI.position = WorldToCameraPosition(target, cam, offsetX,offsetY);
	}




	public static Vector3 WorldToCameraPosition(Transform target, Camera cam, float offsetX = 0, float offsetY = 0)
	{
		return WorldToCameraPosition(target, cam, new Vector2(offsetX,offsetY));
	}

	public static Vector3 WorldToCameraPosition_Clamped(Transform target, Camera cam, float offsetX = 0, float offsetY = 0)
	{
		Vector3 pos;
		Vector3 clampedPos;
		Vector3 finalPos;

		pos = cam.WorldToViewportPoint(target.position);
		clampedPos = new Vector3(Mathf.Clamp(pos.x, offsetX, 1.0f - offsetX), Mathf.Clamp(pos.y, offsetY, 1.0f - offsetY),pos.z);
		finalPos = new Vector3(clampedPos.x * Screen.width, clampedPos.y * Screen.height,0);

		//Debug.Log($"WorldToCameraPosition_Clamped() \nviewport pos: {pos} | finalPos {finalPos}");

		return finalPos;
	}

	public static Vector3 WorldToCameraPosition(Transform target, Camera cam, Vector2 offSet)
	{
		return cam.WorldToScreenPoint(target.position).To2d() + offSet;
	}

	#endregion


}
