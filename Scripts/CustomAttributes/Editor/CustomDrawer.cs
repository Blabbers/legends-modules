using UnityEditor;
using UnityEngine;
using System;

[CustomPropertyDrawer(typeof(TimeSpanBoxAttribute))]
public class TimeSpanBoxDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var attr = attribute as TimeSpanBoxAttribute;
		var value = TimeSpan.FromSeconds(property.floatValue);
		var text = FormatText(attr.Format, value);

		var width = position.width * 0.65f;
		position = new Rect(position.x, position.y, width, position.height);
		EditorGUI.PropertyField(position, property, label, true);
		position = new Rect(position.x + width, position.y, width, position.height);
		EditorGUI.HelpBox(position, text, MessageType.None);
	}

	private string FormatText(string format, TimeSpan value)
	{
		format = format
			 .Replace(":", @"\:")
			 .Replace(".", @"\")
			 .Replace(@"0\:", "0:");

		return string.Format(format, value);
	}
}

[CustomPropertyDrawer(typeof(PercentBoxAttribute))]
public class PercentBoxDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var attr = attribute as PercentBoxAttribute;
		var value = property.floatValue;
		var text = FormatText(value);

		var width = position.width * 0.65f;
		position = new Rect(position.x, position.y, width, position.height);
		EditorGUI.PropertyField(position, property, label, true);
		position = new Rect(position.x + width, position.y, width, position.height);
		EditorGUI.HelpBox(position, text, MessageType.None);
	}

	private string FormatText(float value)
	{
		return (value * 100f) + "%";
	}
}