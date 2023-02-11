using System;
using UnityEngine;


public static class ColorExtension
{

    public static Color LerpTowards(this Color current, Color target, float maxDistanceDelta)
    {
        float num = target.r - current.r;
        float num2 = target.g - current.g;
        float num3 = target.b - current.b;
        float num4 = target.a - current.a;
        float num5 = num * num + num2 * num2 + num3 * num3 + num4 * num4;

        if (num5 == 0f || (maxDistanceDelta >= 0f && num5 <= maxDistanceDelta * maxDistanceDelta))
        {
            return target;
        }

        float num6 = (float)Math.Sqrt(num5);
        return new Color(current.r + num / num6 * maxDistanceDelta, current.g + num2 / num6 * maxDistanceDelta, current.b + num3 / num6 * maxDistanceDelta, current.a + num4 / num6 * maxDistanceDelta);
    }

    public static void LerpThisTowards(ref this Color current, Color target, float maxDistanceDelta)
    {
        float num = target.r - current.r;
        float num2 = target.g - current.g;
        float num3 = target.b - current.b;
        float num4 = target.a - current.a;
        float num5 = num * num + num2 * num2 + num3 * num3 + num4 * num4;

        if (num5 == 0f || (maxDistanceDelta >= 0f && num5 <= maxDistanceDelta * maxDistanceDelta))
        {
            current = target;
        }

        float num6 = (float)Math.Sqrt(num5);
        current = new Color(current.r + num / num6 * maxDistanceDelta, current.g + num2 / num6 * maxDistanceDelta, current.b + num3 / num6 * maxDistanceDelta, current.a + num4 / num6 * maxDistanceDelta);
    }

}
