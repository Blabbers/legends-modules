using UnityEngine;

public static class Vector2Extension
{

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static Vector2 Rotated(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
        Vector2 newV;


        float tx = v.x;
        float ty = v.y;
        newV.x = (cos * tx) - (sin * ty);
        newV.y = (sin * tx) + (cos * ty);

        return newV;
    }


    public static Vector2 LerpDirectionTowards(this Vector2 v, Vector2 target, float percent)
    {
        float angle = Vector2.Angle(v, target);
        float rotAngle = angle * percent;

        return v.Rotated(-rotAngle);
    }

    public static Vector2 To2d(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }
}