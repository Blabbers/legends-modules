using Fungus;
using Raycast.Utility;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using UnityEditor;
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



	#region 3d Shapes

	public static void DrawWireCube(Transform current, BoxCollider collider, Color color)
	{






		#region MyRegion

		//Transform parent;
		//Vector3 parentScale;

		//parentScale = Vector3.one;


		//if (current.parent != null)
		//{
		//	parent = current.parent;
		//	parentScale = parent.lossyScale;

		//}

		//var scaleX = collider.size.x * parent.localScale.x * current.localScale.x;
		//var scaleY = collider.size.y * parent.localScale.y * current.localScale.y;
		//var scaleZ = collider.size.z * parent.localScale.z * current.localScale.z;

		//var scaleX = collider.size.x * parentScale.x * current.localScale.x;
		//var scaleY = collider.size.y * parentScale.y * current.localScale.y;
		//var scaleZ = collider.size.z * parentScale.z * current.localScale.z; 

		//var rotation = current.rotation.eulerAngles;
		//      var center = current.position + collider.center;

		//var scale = collider.bounds.size;

		#endregion

		var scaleX = collider.size.x * current.lossyScale.x;
		var scaleY = collider.size.y * current.lossyScale.y;
		var scaleZ = collider.size.z * current.lossyScale.z;

		var scale = new Vector3(scaleX, scaleY, scaleZ);


		var rotation = current.rotation.eulerAngles;
		var center = collider.bounds.center;

		DrawWireCube(scale, center, rotation,color);
	}

	public static void DrawWireCube(Vector3 scale, Vector3 center, Vector3 rotation , Color color)
    {
        var up = Vector3.up.Rotated(rotation);
		var right = Vector3.right.Rotated(rotation);
		var forward = Vector3.forward.Rotated(rotation);


		Vector3 baseCenter, upperCenter;
		baseCenter = center - up * scale.y / 2;
		upperCenter = center + up * scale.y / 2;


		List<Vector3> baseVertices = GenerateRectangleVertices(scale, baseCenter, right, forward);
        DrawRectangle(baseVertices);
        
		List<Vector3> upperVertices = GenerateRectangleVertices(scale, upperCenter, right, forward);
		DrawRectangle(upperVertices);

		DrawCubeEdges(baseVertices, upperVertices);
	}


	public static void DrawFilledCube(Transform current, BoxCollider collider, Color fillColor, Color wireColor)
	{
        #region MyRegion
        //Transform parent;
        //      Transform gParent;

        //      Vector3 parentScale, gParentScale;

        //      parentScale = gParentScale = Vector3.one;

        //if (current.parent != null)
        //{
        //	parent = current.parent;
        //          parentScale = parent.localScale;

        //	if (parent.parent != null)
        //	{
        //		gParent = parent.parent;
        //		gParentScale = parent.localScale;
        //	}

        //}


        ////var scaleX = collider.size.x * parent.localScale.x * current.localScale.x;
        ////var scaleY = collider.size.y * parent.localScale.y * current.localScale.y;
        ////var scaleZ = collider.size.z * parent.localScale.z * current.localScale.z;

        //var scaleX = collider.size.x * parentScale.x * gParentScale.x * current.localScale.x;
        //var scaleY = collider.size.y * parentScale.y * gParentScale.y * current.localScale.y;
        //var scaleZ = collider.size.z * parentScale.z * gParentScale.z * current.localScale.z;

        #endregion


  //      var scale = collider.bounds.size;
		//var rotation = current.rotation.eulerAngles;
  //      var center = collider.bounds.center;

		#region MyRegion
		//var scale = new Vector3(scaleX, scaleY, scaleZ);

		//var rotation = current.rotation.eulerAngles;
		//var center = current.position + collider.center; 
		#endregion


		var scaleX = collider.size.x * current.lossyScale.x;
		var scaleY = collider.size.y * current.lossyScale.y;
		var scaleZ = collider.size.z * current.lossyScale.z;

		var scale = new Vector3(scaleX, scaleY, scaleZ);


		var rotation = current.rotation.eulerAngles;
		var center = collider.bounds.center;


		DrawFilledCube(scale, center, rotation, fillColor, wireColor);
	}






	public static void DrawFilledCube(Vector3 scale, Vector3 center, Vector3 rotation, Color fillColor, Color wireColor)
	{
		var up = Vector3.up.Rotated(rotation);
		var right = Vector3.right.Rotated(rotation);
		var forward = Vector3.forward.Rotated(rotation);


		Vector3 baseCenter, upperCenter;
		baseCenter = center - up * scale.y / 2;
		upperCenter = center + up * scale.y / 2;


		List<Vector3> baseVertices = GenerateRectangleVertices(scale, baseCenter, right, forward);
		//DrawRectangle(baseVertices);

		List<Vector3> upperVertices = GenerateRectangleVertices(scale, upperCenter, right, forward);
        //DrawRectangle(upperVertices);

#if UNITY_EDITOR
        Handles.DrawSolidRectangleWithOutline(baseVertices.ToArray(), fillColor, wireColor);
		Handles.DrawSolidRectangleWithOutline(upperVertices.ToArray(), fillColor, wireColor);
#endif

        DrawSideRectangles(baseVertices, upperVertices, fillColor, wireColor);
		//DrawCubeEdges(baseVertices, upperVertices);
	}
	static void DrawSideRectangles(List<Vector3> basePoints, List<Vector3> upperPoints, Color fillColor, Color wireColor)
	{
        List<Vector3> rect1, rect2, rect3, rect4;

        rect1 = new List<Vector3>();
        rect1.Add(basePoints[0]);
		rect1.Add(basePoints[1]);
		rect1.Add(upperPoints[1]);
		rect1.Add(upperPoints[0]);



		rect2 = new List<Vector3>();
		rect2.Add(basePoints[1]);
		rect2.Add(basePoints[2]);
		rect2.Add(upperPoints[2]);
		rect2.Add(upperPoints[1]);



		rect3 = new List<Vector3>();
		rect3.Add(basePoints[2]);
		rect3.Add(basePoints[3]);
		rect3.Add(upperPoints[3]);
		rect3.Add(upperPoints[2]);



		rect4 = new List<Vector3>();
		rect4.Add(basePoints[3]);
		rect4.Add(basePoints[0]);
		rect4.Add(upperPoints[0]);
		rect4.Add(upperPoints[3]);


#if UNITY_EDITOR
		Handles.DrawSolidRectangleWithOutline(rect1.ToArray(), fillColor, wireColor);
		Handles.DrawSolidRectangleWithOutline(rect2.ToArray(), fillColor, wireColor);
		Handles.DrawSolidRectangleWithOutline(rect3.ToArray(), fillColor, wireColor);
		Handles.DrawSolidRectangleWithOutline(rect4.ToArray(), fillColor, wireColor);
#endif


	}



	static void DrawRectangle(List<Vector3> points)
	{
        for (int i = 0; i < points.Count-1; i++)
        {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }

		Gizmos.DrawLine(points[points.Count-1], points[0]);
	}

	static List<Vector3> GenerateRectangleVertices(Vector3 scale, Vector3 center, Vector3 right, Vector3 forward)
    {
        List<Vector3> vertices = new List<Vector3>();
        Vector3 current;

		current = center + (forward * scale.z / 2) + (-right * scale.x / 2);
		vertices.Add(current);

		current = center + (forward * scale.z / 2) + (right * scale.x / 2);
		vertices.Add(current);


        current = center + (-forward * scale.z / 2) + (right * scale.x / 2);
		vertices.Add(current);

		current = center + (-forward * scale.z / 2) + (-right * scale.x / 2);
		vertices.Add(current);

		return vertices;

    }

    static void DrawCubeEdges(List<Vector3> basePoints, List<Vector3> upperPoints)
    {
        for (int i = 0; i < basePoints.Count; i++)
        {
            Gizmos.DrawLine(basePoints[i], upperPoints[i]);
        }
    }



	#endregion

	#region 2d Shapes
	public static void DrawRectangle(Vector3 center, Vector2 dimensions, Color color, Vector2 offset, float alpha = 1.0f)
    {
        Vector3 adjustedCenter;
        Gizmos.color = new Color(color.r, color.g, color.b, color.a * alpha);

        adjustedCenter = new Vector3(center.x + offset.x, center.y + offset.y, center.z);
        Gizmos.DrawCube(adjustedCenter, dimensions);
    }

	public static void DrawSphere(Vector3 center, float radius, Color color, Vector3 offset, float alpha = 1.0f)
	{
		Vector3 adjustedCenter;
		Gizmos.color = new Color(color.r, color.g, color.b, color.a * alpha);

		adjustedCenter = new Vector3(center.x + offset.x, center.y + offset.y, center.z);
		
		Gizmos.DrawSphere(adjustedCenter, radius);
	}


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
    #endregion



    public static void DrawRaycastData3D(RaycastData3d data, float pointRadius = 0.2f ,bool hasOrigin = false)
    {
        if (!hasOrigin)
        {
            DrawRay(data.GetOrigin(), data.direction, data.color, data.Range, pointRadius);
            return;
        }


        DrawRayWithOrigin(data.GetOrigin(), data.direction, data.color, data.Range, pointRadius);
    }

    public enum RaycastRayType
    {
        Check, Force
    }

}
