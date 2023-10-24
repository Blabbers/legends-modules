using UnityEngine;
using UnityEngine.UI;

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

	public static Vector3 WorldToCameraPosition(Vector3 target, Camera cam, float offsetX = 0, float offsetY = 0)
	{
		return WorldToCameraPosition(target, cam, new Vector2(offsetX, offsetY));
	}

	public static Vector3 WorldToScreenSpaceCameraPosition(Vector3 target, Camera cam, float offsetX = 0, float offsetY = 0)
	{
		//return WorldToCameraPosition(target, cam, new Vector2(offsetX, offsetY));
		return cam.WorldToViewportPoint(target);
	} 


	public static Vector3 WorldToCameraPosition_Clamped(Transform target, Camera cam, float offsetX = 0, float offsetY = 0, float paddingX = 0, float paddingY = 0)
	{
		Vector3 pos;
		Vector3 clampedPos;
		Vector3 finalPos;

		pos = cam.WorldToViewportPoint(target.position + new Vector3(offsetX, offsetY, 0));
		//pos += new Vector3(offsetX, offsetY,0);
		clampedPos = new Vector3(Mathf.Clamp(pos.x, paddingX, 1.0f - paddingX), Mathf.Clamp(pos.y, paddingY, 1.0f - paddingY), pos.z);
		finalPos = new Vector3(clampedPos.x * Screen.width, clampedPos.y * Screen.height, 0);

		//Debug.Log($"WorldToCameraPosition_Clamped() \nviewport pos: {pos} | finalPos {finalPos}");

		return finalPos;
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

	static Vector3 WorldToCameraPosition(Vector3 target, Camera cam, Vector2 offSet)
	{
		return cam.WorldToScreenPoint(target).To2d() + offSet;
	}

	#endregion


	#region Camera to World

	public static Vector3 CameraPosToWorldPos(Vector3 mousePos, Camera cam, float distance)
	{
		Vector3 worldPosition = Vector3.zero;
		Plane plane = new Plane(Vector3.up, 0);

		Ray ray = cam.ScreenPointToRay(mousePos);
		if (plane.Raycast(ray, out distance))
		{
			worldPosition = ray.GetPoint(distance);
		}

		return worldPosition;
	}

	public static Vector2 CameraPosToWorldPos2d(Vector2 mousePos, Camera cam)
	{
		Vector2 worldPosition = Vector2.zero;

		Ray ray = cam.ScreenPointToRay(mousePos);
		return ray.origin;
	}


	public static Vector2 CanvasPosToWorldPos2d(Transform element, Transform parentCanvas, Camera cam)
	{
		Vector3 absoluteAnchored;
		Vector3 percentPosition;
		float halfWidth, halfHeight;
		float width, height;

		absoluteAnchored = RectTransformUtility.CalculateRelativeRectTransformBounds(parentCanvas.transform, element.transform).center;

		CanvasScaler scaler = parentCanvas.GetComponent<CanvasScaler>();

		width = scaler.referenceResolution.x;
		height = scaler.referenceResolution.y;
		halfWidth = width / 2;
		halfHeight = height / 2;

		Vector2 convertedPixels = new Vector2(
			MathUtility.Value_from_another_Scope(absoluteAnchored.x, -halfWidth, halfWidth, 0, width),
			MathUtility.Value_from_another_Scope(absoluteAnchored.y, -halfHeight, halfHeight, 0, height)
		);

		//convertedAnchored = convertedPixels;

		Vector2 percentagePosition = new Vector2(
			convertedPixels.x / width,
			convertedPixels.y / height
		);

		percentPosition = percentagePosition;


		Vector2 mousePos = new Vector2(
			Mathf.Lerp(0, Screen.width, percentPosition.x),
			Mathf.Lerp(0, Screen.height, percentPosition.y)
		);

		return CameraPosToWorldPos2d(mousePos, cam);
	}


	#endregion
}
