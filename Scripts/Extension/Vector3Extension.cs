using UnityEngine;

public static class Vector3Extension
{

    public static Vector3 Rotate(this Vector3 v, Vector3 rotation)
	{
        v = Quaternion.Euler(rotation) * v;
		return v;
    }

    public static Vector3 Rotated(this Vector3 v, Vector3 rotation)
	{
		Vector3 newV;
		newV =Quaternion.Euler(rotation) * v;

		return newV;
    }


    //public static Vector2 LerpDirectionTowards(this Vector2 v, Vector2 target, float percent)
    //{
    //    float angle = Vector2.Angle(v, target);
    //    float rotAngle = angle * percent;

    //    return v.Rotated(-rotAngle);
    //}

    //public static Vector2 To2d(this Vector3 v)
    //{
    //    return new Vector2(v.x, v.y);
    //}
}