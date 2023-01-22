using UnityEngine;
using UnityEngine.UI;
using LoLSDK;
using TMPro;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;

namespace Blabbers.Game00
{
	public static class LocalizationExtensions
	{
		public static HashSet<string> AlreadyPlayedTTS = new HashSet<string>();
		public static bool HasPlayedTTS(string key) => AlreadyPlayedTTS.Contains(key);

		public static void LocalizeText(this Text textObj, string localizationKey, string appendLeft = null, string appendRight = null)
		{
			textObj.text = LocalizeText(localizationKey, appendLeft, appendRight);
		}
		public static void LocalizeText(this TextMeshProUGUI textObj, string localizationKey, string appendLeft = null, string appendRight = null)
		{
			textObj.text = LocalizeText(localizationKey, appendLeft, appendRight);
		}

		public static void LocalizeText(this TextMeshProUGUI textObj, string localizationKey, bool applyColorCode)
		{
			textObj.text = LocalizeText(localizationKey, null, null, applyColorCode);
		}

		public static string LocalizeText(string localizationKey, string appendLeft = null, string appendRight = null, bool applyColorCode = false)
		{
			var isDevBuild = false;
#if DEVELOPMENT_BUILD
			// If we are on a dev build, we will load the texts without the LoL platform.
			isDevBuild = true;
#endif

#if UNITY_EDITOR
			// This is just for Editor use. Our "LocalizedString" property goes through this path for example. (no need for color codes and other features)
			if (!Application.isPlaying)
			{
				var editorMainText = EditorLoadFromLanguageJson(localizationKey, null, false);
				return $"{appendLeft}{editorMainText}{appendRight}";
			}
#endif

			if (SharedState.languageDefs == null && !isDevBuild)
			{
				Debug.Log($"<TextNotFound> SharedState.languageDefs is not loaded yet. Localization Key: {localizationKey}");
			}

			// Regular call for LoL builds
			var mainText = SharedState.languageDefs != null ? SharedState.languageDefs[localizationKey].Value : $"<TNF> {localizationKey}";
			// Dev builds (for running on webgl for mobile devices)
			if (isDevBuild)
			{
				mainText = EditorLoadFromLanguageJson(localizationKey, null, false);
			}

			// Logs error if no text is found in any path
			if (string.IsNullOrEmpty(mainText))
			{
				Debug.Log($"<TextNotFound> Localization Key: {localizationKey}, is returning an empty value.");
				mainText = $"<TNF> {localizationKey}";
			}

			// Applies color codes to words
			if (GameData.Instance.textConfigs != null && !string.IsNullOrEmpty(mainText))
			{
				if (GameData.Instance.textConfigs.colorCodes !=null && GameData.Instance.textConfigs.colorCodes.Length > 0 && applyColorCode)
				{
					mainText = ApplyColorCodes(mainText);
				}
			}
		
			return $"{appendLeft}{mainText}{appendRight}";
		}

		private static string ApplyColorCodes(string mainText)
		{
			string key, term, plural;

			var isDevBuild = false;
#if DEVELOPMENT_BUILD
			// If we are on a dev build, we will load the texts without the LoL platform.
			isDevBuild = true;
#endif

			foreach (var color in GameData.Instance.textConfigs.colorCodes)
			{
				key = color.key;
				term = isDevBuild ? localLanguageJson[key].Value : SharedState.languageDefs[key].Value;
				plural = term + "S";				
				mainText = FindAndColorTerm(plural, mainText, color.color, out var success);
				if (!success) mainText = FindAndColorTerm(term, mainText, color.color, out success);

				if (color.extraKeys.Count > 0)
				{
					foreach (var extra in color.extraKeys)
					{
						key = extra;
						term = isDevBuild ? localLanguageJson[key].Value : SharedState.languageDefs[key].Value;
						plural = term + "S";
						mainText = FindAndColorTerm(plural, mainText, color.color, out success);
						if (!success) mainText = FindAndColorTerm(term, mainText, color.color, out success);
					}
				}
			}

			return mainText;
		}

		private static string FindAndColorTerm(string term, string mainText, Color color, out bool success)
		{
			success = true;

			string hex = Utils.ColorToHex(color);

			//First check
			if (mainText.Contains(term))
			{
				mainText = mainText.Replace(term, $"<color=#{hex}>{term}</color>");
				//Debug.Log($"Replacing: {term} -> <color=#{hex}>{term}</color> \n{mainText}");
				return mainText;
			}

			//Check all lowercase
			term = term.ToLower();

			if (mainText.Contains(term))
			{
				mainText = mainText.Replace(term, $"<color=#{hex}>{term}</color>");
				//Debug.Log($"(To lower) Replacing: {term} -> <color=#{hex}>{term}</color> \n{mainText}");
				return mainText;
			}


			//Check all uppercase
			term = term.ToUpper();

			if (mainText.Contains(term))
			{
				mainText = mainText.Replace(term, $"<color=#{hex}>{term}</color>");
				//Debug.Log($"(To upper) Replacing: {term} -> <color=#{hex}>{term}</color> \n{mainText}");
				return mainText;
			}


			//Check first letter capitalized

			term = term.ToLower();

			if (term.Length == 1) term = "" + char.ToUpper(term[0]);
			else term = char.ToUpper(term[0]) + term.Substring(1);


			if (mainText.Contains(term))
			{
				mainText = mainText.Replace(term, $"<color=#{hex}>{term}</color>");
				//Debug.Log($"(first letter cap) Replacing: {term} -> <color=#{hex}>{term}</color> \n{mainText}");
				return mainText;
			}


			success = false;
			return mainText;
		}

		public static void PlayTTS(string key)
		{
			try
			{
				Debug.Log("PlayTTS → EnableAutomaticTTS: " + ProgressController.GameProgress.enableAutomaticTTS);
				if (ProgressController.GameProgress.enableAutomaticTTS)
				{
					LOLSDK.Instance?.SpeakText(key);
					LocalizationExtensions.AlreadyPlayedTTS.Add(key);
				}
			}
			catch { }
		}
		private static JSONNode localLanguageJson;
		public static void ResetLanguageJson()
		{
			localLanguageJson = null;
			GetLanguageJson();
		}
		public static JSONNode GetLanguageJson()
		{
			//Debug.Log("Force? "+ forceLoadJsonFile + " / Null? " + (localLanguageJson == null));
			if (localLanguageJson != null) return localLanguageJson;

			const string languageJSONFilePath = "language.json";
			// Load Dev Language File from StreamingAssets
			string langFilePath = Path.Combine(Application.streamingAssetsPath, languageJSONFilePath);
			if (File.Exists(langFilePath))
			{
				string langDataAsJson = File.ReadAllText(langFilePath);
				JSONNode langDefs = JSON.Parse(langDataAsJson);
				localLanguageJson = langDefs;
				return (langDefs);
			}
			return "";

		}

		public static void EditorSaveToLanguageJson(string key, string value, Object unityObject = null, string langCode = "en")
		{
			var json = LocalizationExtensions.GetLanguageJson();
			var node = json[langCode];
			node.Add(key, value);

			const string languageJSONFilePath = "language.json";
			string langFilePath = Path.Combine(Application.streamingAssetsPath, languageJSONFilePath);
			if (File.Exists(langFilePath))
			{
				File.WriteAllText(langFilePath, json.ToString(1));
				Debug.Log($"<color=cyan>File language.json was updated</color>: <color=white>[{key}]: {node[key]}</color>", unityObject);
				localLanguageJson = json;
			}
		}
		public static string EditorLoadFromLanguageJson(string key, Object unityObject = null, bool displayMessages = true, string langCode = "en")
		{
			var json = LocalizationExtensions.GetLanguageJson();
			var node = json[langCode];
			if (string.IsNullOrEmpty(key))
			{
				if (displayMessages)
				{
					Debug.Log($"<color=yellow>Key is EMPTY. Set a key value. File language.json was NOT loaded to this asset bacause key is EMPTY.</color>", unityObject);
				}
				return "";
			}
			else if (node[key] == null)
			{
				if (displayMessages)
				{
					Debug.Log($"<color=red>File language.json was NOT loaded to this asset bacause key [{key}] is NULL.</color>", unityObject);
				}
				return "";
			}
			else
			{
				if (displayMessages)
				{
					Debug.Log($"<color=yellow>File language.json was loaded to this asset</color> \n<color=white>[{key}]: {node[key]}</color>", unityObject);
				}
				return node[key];
			}			
		}

	}
}