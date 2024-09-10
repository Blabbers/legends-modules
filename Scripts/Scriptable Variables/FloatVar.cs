using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Values/Float", order = 1)]
public class FloatVar : ScriptableObject
{
	public float value;
	public Action<float> OnValueUpdate;

	public void SetValue(float value)
	{
		this.value = value;
		OnValueUpdate?.Invoke(value);
	}

}
