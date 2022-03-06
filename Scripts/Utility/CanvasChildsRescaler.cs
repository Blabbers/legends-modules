using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace Blabbers.Game00
{
	public class CanvasChildsRescaler : MonoBehaviour
	{
		[InfoBox("Warning: This is intended for editor use fix. Once its done, all the childs will be rescaled and repositioned by the given value", EInfoBoxType.Warning)]
		public float scaleFactor = 1f;

		[Button]
		void ResizeAndReposition()
		{
			var rectTransforms = GetComponentsInChildren<RectTransform>(true);
			foreach (var rect in rectTransforms)
			{
				var pos = rect.anchoredPosition3D;
				pos.x *= scaleFactor;
				pos.y *= scaleFactor;
				pos.z *= scaleFactor;
				rect.anchoredPosition3D = pos;

				var size = rect.sizeDelta;
				size.x *= scaleFactor;
				size.y *= scaleFactor;
				rect.sizeDelta = size;
			}

			var texts = GetComponentsInChildren<Text>(true);
			foreach (var text in texts)
			{
				text.fontSize = (int)(text.fontSize * scaleFactor);
			}

			var textsMP = GetComponentsInChildren<TextMeshProUGUI>(true);
			foreach (var text in textsMP)
			{
				text.fontSize = (int)(text.fontSize * scaleFactor);
			}
		}



		[System.Serializable]
		public class ReplaceFonts
		{
			public Font oldVersion;
			public TMP_FontAsset tmpVersion;
		}

		public List<ReplaceFonts> fontsToReplace;
		public TMP_FontAsset defaultFontAsset;

		[Button]
		void ChangeTextsToTMP()
		{
			var texts = GetComponentsInChildren<Text>(true);

			for (int i = 0; i < texts.Length; i++)
			{
				var textObj = texts[i];
				var textGameObject = textObj.gameObject;
				var oldAlignment = textObj.alignment;
				var oldFontSize = textObj.fontSize;
				var oldColor = textObj.color;
				var oldString = textObj.text;
				var oldFont = textObj.font;
				var oldStyle = textObj.fontStyle;

				DestroyImmediate(textObj);
				var tmp = textGameObject.AddComponent<TextMeshProUGUI>();

				tmp.color = oldColor;
				tmp.text = oldString;

				tmp.fontSize = oldFontSize;
				tmp.enableAutoSizing = true;
				tmp.fontSizeMin = Mathf.Min(18, oldFontSize);
				tmp.fontSizeMax = oldFontSize;

				switch (oldAlignment)
				{
					case TextAnchor.UpperLeft:
						tmp.alignment = TextAlignmentOptions.TopLeft;
						break;
					case TextAnchor.UpperCenter:
						tmp.alignment = TextAlignmentOptions.Top;
						break;
					case TextAnchor.UpperRight:
						tmp.alignment = TextAlignmentOptions.TopRight;
						break;
					case TextAnchor.MiddleLeft:
						tmp.alignment = TextAlignmentOptions.Left;
						break;
					case TextAnchor.MiddleCenter:
						tmp.alignment = TextAlignmentOptions.Center;
						break;
					case TextAnchor.MiddleRight:
						tmp.alignment = TextAlignmentOptions.Right;
						break;
					case TextAnchor.LowerLeft:
						tmp.alignment = TextAlignmentOptions.BottomLeft;
						break;
					case TextAnchor.LowerCenter:
						tmp.alignment = TextAlignmentOptions.Bottom;
						break;
					case TextAnchor.LowerRight:
						tmp.alignment = TextAlignmentOptions.BottomRight;
						break;
				}

				switch (oldStyle)
				{
					case FontStyle.Normal:
						tmp.fontStyle = FontStyles.Normal;
						break;
					case FontStyle.Bold:
						tmp.fontStyle = FontStyles.Bold;
						break;
					case FontStyle.Italic:
						tmp.fontStyle = FontStyles.Italic;
						break;
					case FontStyle.BoldAndItalic:
						tmp.fontStyle = FontStyles.Bold;
						break;					
				}

				var newFont = fontsToReplace.Find((x) => x.oldVersion == oldFont);
				if (newFont != null)
				{
					tmp.font = newFont.tmpVersion;
				}
				else
				{
					tmp.font = defaultFontAsset;
				}

			}

			MakeEveryTextAutoSize();
		}

		[Button]
		public void MakeEveryTextAutoSize()
		{
			var tmps = GetComponentsInChildren<TextMeshProUGUI>(true);

			for (int i = 0; i < tmps.Length; i++)
			{
				var tmp = tmps[i];

				if (!tmp.enableAutoSizing)
				{
					var oldFontSize = tmp.fontSize;
					tmp.enableAutoSizing = true;
					tmp.fontSizeMin = Mathf.Min(18, oldFontSize);
					tmp.fontSizeMax = oldFontSize;
				}
			}
		}
	}
}