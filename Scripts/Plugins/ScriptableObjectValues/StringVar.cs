using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Values/StringVar", order = 1)]
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