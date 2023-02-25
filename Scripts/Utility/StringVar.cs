using NaughtyAttributes;
using UnityEngine;

public class StringVar : ScriptableObject
{
	[field: SerializeField]
	[field: ResizableTextArea]
	public string Value { get; private set; }

	public void SetValue(string value)
	{
		Value = value;
	}
}