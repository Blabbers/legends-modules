using UnityEditor;
using UnityEngine;

public static class Utils
{
#if UNITY_EDITOR
	private static GUIStyle style;
#endif
	public static void DrawText(string text, Vector3 worldPosition, Color? color = null, int fontSize = 0, float yOffset = 0)
	{
#if UNITY_EDITOR
		var textContent = new GUIContent(text);

		if (style == null)
		{
			style = new GUIStyle();
		}

		if (color != null)
		{
			style.normal.textColor = (Color)color;
		}
		if (fontSize > 0)
		{
			style.fontSize = fontSize;
		}

		var textSize = style.CalcSize(textContent);
		var screenPoint = Camera.current.WorldToScreenPoint(worldPosition);
        
		// checks necessary to the text is not visible when the camera is pointed in the opposite direction relative to the object
		if (screenPoint.z > 0)
		{
			var p = new Vector3
			{
				x = screenPoint.x - textSize.x * 0.5f,
				y = screenPoint.y + textSize.y * 0.5f + yOffset,
				z = screenPoint.z
			};
            
			var finalWorldPosition = Camera.current.ScreenToWorldPoint(p);
			Handles.Label(finalWorldPosition, textContent, style);
		}
		
		style.normal.textColor = Color.white;
#endif
	}

	public static void DrawCross(Vector3 point, float size,  Color color, float duration = 0f)
	{
		Debug.DrawRay(point - Vector3.forward * size * 0.5f, Vector3.forward * size, color, duration);
		Debug.DrawRay(point - Vector3.right * size * 0.5f, Vector3.right * size, color, duration);
		Debug.DrawRay(point - Vector3.up * size * 0.5f, Vector3.up * size, color, duration);
	}
    
    public static string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }
    public static Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
}