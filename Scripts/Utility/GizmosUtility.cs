using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public static class GizmosUtility
{
    public static void DrawRay(Vector3 pos, Vector3 dir, Color color, float distance = 5, float radius = 0.2f)
    {
        Vector3 target;
        Gizmos.color = color;

        target = pos + dir.normalized * distance;
        Gizmos.DrawLine(pos, target);
        Gizmos.DrawSphere(target, radius);
    }

    public static void DrawRayWithOrigin(Vector3 pos, Vector3 dir, Color color, float distance = 5, float radius = 0.2f)
    {
        Vector3 target;
        Gizmos.color = color;

        target = pos + dir.normalized * distance;
        Gizmos.DrawLine(pos, target);
        Gizmos.DrawWireSphere(pos, radius * 0.75f);
        Gizmos.DrawSphere(target, radius);
    }


    public static void DrawForce(Vector3 pos, Vector3 dir, Color color, float distance = 5, float radius = 0.2f)
    {
        Vector3 target;
        Gizmos.color = color;

        target = pos + dir.normalized * distance;
        Gizmos.DrawLine(pos, target);

        radius = radius * 0.75f;
        Gizmos.DrawCube(target, new Vector3(radius, radius, radius));
        //Gizmos.DrawWireSphere(target, radius);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float radius = 0.2f)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(end, radius);
    }



	public static void DrawRectangle(Vector3 center, Vector2 dimensions, Color color, Vector2 offset, float alpha = 1.0f)
	{
		Vector3 adjustedCenter;
		Gizmos.color = new Color(color.r, color.g, color.b, color.a * alpha);

		adjustedCenter = new Vector3(center.x + offset.x, center.y + offset.y, center.z);
		Gizmos.DrawCube(adjustedCenter, dimensions);
	}

	//public static void DrawRectangle(Vector3 center, Vector2 dimensions, Color color, Vector2 offset)
	//{
	//    Vector3 adjustedCenter;
	//    Gizmos.color = color;

	//    adjustedCenter = new Vector3(center.x + offset.x, center.y + offset.y, center.z);
	//    Gizmos.DrawCube(adjustedCenter, dimensions);
	//    //DrawRectangle(adjustedCenter, dimensions, color);

	//}

	public static void DrawWireRectangle(Vector3 center, Vector2 dimensions, Color color, Vector2 offset)
    {
        Vector3 adjustedCenter;
        adjustedCenter = new Vector3(center.x + offset.x, center.y + offset.y, center.z);

        DrawWireRectangle(adjustedCenter, dimensions, color);

    }

    static void DrawWireRectangle(Vector3 center, Vector2 dimensions, Color color)
    {
        Vector3 origin, edge;
        Gizmos.color = color;

        origin = new Vector3(center.x - dimensions.x / 2, center.y - dimensions.y / 2, 0);
        edge = new Vector3(center.x + dimensions.x / 2, center.y + dimensions.y / 2, 0);

        Gizmos.DrawLine(origin, new Vector3(origin.x, origin.y + dimensions.y, origin.z));
        Gizmos.DrawLine(origin, new Vector3(origin.x + dimensions.x, origin.y, origin.z));

        Gizmos.DrawLine(edge, new Vector3(origin.x, origin.y + dimensions.y, origin.z));
        Gizmos.DrawLine(edge, new Vector3(origin.x + dimensions.x, origin.y, origin.z));
    }


    public enum RaycastRayType
    {
        Check, Force
    }

}
