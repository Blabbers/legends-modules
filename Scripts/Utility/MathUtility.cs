using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility 
{
	public static float Value_from_another_Scope(float value, float oldMin, float oldMax, float newMin, float newMax)
	{
		float returnValue = 0;

		returnValue = ((value - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;

		return returnValue;
	}
}
