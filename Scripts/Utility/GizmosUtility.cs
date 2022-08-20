using System.Collections;
using System.Collections.Generic;
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


    public static void DrawRectangle(Vector3 center, Vector2 dimensions, Color color)
    {
        Vector3 origin, edge;
        Gizmos.color = color;

        origin = new Vector3(center.x - dimensions.x /2 , center.y - dimensions.y / 2 , 0);
        edge = new Vector3(center.x + dimensions.x / 2, center.y + dimensions.y / 2, 0);

        Gizmos.DrawLine(origin, new Vector3(origin.x, origin.y + dimensions.y, origin.z));
        Gizmos.DrawLine(origin, new Vector3(origin.x + dimensions.x, origin.y, origin.z));

        Gizmos.DrawLine(edge, new Vector3(origin.x, origin.y + dimensions.y, origin.z));
        Gizmos.DrawLine(edge, new Vector3(origin.x + dimensions.x, origin.y, origin.z));

    }
}
