using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Converter2d
{

    public static float Distance(Vector3 origin, Vector3 target)
    {
        origin = new Vector3(origin.x, origin.y, 0);
        target = new Vector3(target.x, target.y, 0);

        return Vector3.Distance(origin, target);
    }


    public static Vector3 Generate2dDirection(Vector3 from, Vector3 to)
    {
        from = new Vector3(from.x, from.y, 0);
        to = new Vector3(to.x, to.y, 0);

        return (to - from).normalized;
    }


}
