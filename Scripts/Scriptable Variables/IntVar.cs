using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Values/Integer", order = 1)]
public class IntVar : ScriptableObject
{
	public int value;
	public Action<int> OnValueUpdate;

	public void SetValue(int value)
	{
		this.value = value;
		OnValueUpdate?.Invoke(value);
	}

}
