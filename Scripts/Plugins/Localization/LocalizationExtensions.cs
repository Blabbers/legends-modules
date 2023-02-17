using UnityEngine;
using UnityEngine.UI;
using LoLSDK;
using TMPro;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System;
using Object = UnityEngine.Object;

namespace Blabbers.Game00
{
	public static class LocalizationExtensions
	{
		public static HashSet<string> AlreadyPlayedTTS = new HashSet<string>();
		public static bool HasPlayedTTS(string key) => AlreadyPlayedTTS.Contains(key);
		private static JSONNode localLanguageJson = null;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void Init()
		{
			AlreadyPlayedTTS = new HashSet<string>();
			localLanguageJson = null;
		}

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
			var mainText = "";
#if DEVELOPMENT_BUILD || UNITY_EDITOR || UNITY_CLOUD_BUILD
			// If we are on a dev build, we will load the texts without the LoL platform.			
			// This is just for Editor use and CloudBuilds. Our "LocalizedString" property goes through this path for example.
			// (no need for color codes and other features)
			mainText = EditorLoadFromLanguageJson(localizationKey, null, false, GameData.Instance.currentSelectedLangCode);
#endif

			if (string.IsNullOrEmpty(mainText))
			{
				// If this text was loaded by the LoL platform.
				if(SharedState.languageDefs != null)
				{
					mainText = SharedState.languageDefs[localizationKey]?.Value;
				}
			}

			if (SharedState.languageDefs != null)
			{
				// Applies color codes to words
				if (GameData.Instance.textConfigs != null && !string.IsNullOrEmpty(mainText))
				{
					if (GameData.Instance.textConfigs != null && GameData.Instance.textConfigs.Length > 0 && applyColorCode)
					{
						mainText = ApplyColorCodes(mainText);
					}
				}
			}

			// Shows error if no text is found in any path
			if (string.IsNullOrEmpty(mainText))
			{
				mainText = $"<TNF> {localizationKey}";
			}
		
			return $"{appendLeft}{mainText}{appendRight}";
		}

		private static string ApplyColorCodes(string mainText)
		{
			string key, term;

			var isDevBuild = false;
#if DEVELOPMENT_BUILD || UNITY_CLOUD_BUILD
			// If we are on a dev build, we will load the texts without the LoL platform.
			isDevBuild = true;
#endif

			if (!Application.isPlaying) return mainText;

			foreach (var color in GameData.Instance.textConfigs)
			{
				key = color.localization.Key;
				term = isDevBuild ? localLanguageJson[key].Value : SharedState.languageDefs[key].Value;
				mainText = ApplyTagsToTerm(term, color.tags, mainText);
			}

			return mainText;
		}

		private static string ApplyTagsToTerm(string term, List<string> tags, string mainText)
		{
			bool found = false;
			string foundTerm ="";
			string editedText = "";
			string currentTerm;

			foundTerm = StringUtility.FindTermInString(term, mainText, out found);

			if (!found) return mainText;

			currentTerm = foundTerm;

			foreach (var item in tags)
			{
				currentTerm = GenerateTaggedTerm(currentTerm, item);
			}


			editedText = StringUtility.ReplaceTermInString(term, currentTerm, mainText);

			return editedText;
		}

		public static string GenerateTaggedTerm(string term, string tag)
		{
			string tagName = "";
			string openTag, closeTag;
			string[] split;

			split = tag.Split('=');

			tagName = split[0];

			openTag = $"{{{tagName}={split[1]}}}";
			closeTag = $"{{/{tagName}}}";

			return $"{openTag}{term}{closeTag}";
		}

		private static string FindKeyTerm(string term, string mainText, out bool success)
		{
			string termFormat = "";
			success = true;

			//First check
			termFormat = term;
			if (mainText.Contains(termFormat)) return termFormat;


			//Check all lowercase
			termFormat = term.ToLower();
			if (mainText.Contains(term)) return termFormat;


			//Check all uppercase
			termFormat = term.ToUpper();
			if (mainText.Contains(term)) return termFormat;

			//Check first letter capitalized
			termFormat = term.ToLower();

			if (termFormat.Length == 1) term = "" + char.ToUpper(term[0]);
			else termFormat = char.ToUpper(term[0]) + term.Substring(1);

			if (mainText.Contains(term)) return termFormat;


			success = false;
			return termFormat;
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
		
		public static void ResetLanguageJson()
		{
			localLanguageJson = null;
			GetLanguageJson();
		}
		public static JSONNode GetLanguageJson()
		{
			//Debug.Log("Force? "+ forceLoadJsonFile + " / Null? " + (localLanguageJson == null));						
			if (localLanguageJson != null)
			{
				Debug.Log("LanguageJson [0] = " + localLanguageJson.Count + "...");
				return localLanguageJson;
			}

			Debug.Log("LanguageJson [1]");

			const string languageJSONFilePath = "language.json";
			// Load Dev Language File from StreamingAssets
			string langFilePath = Path.Combine(Application.streamingAssetsPath, languageJSONFilePath);
			if (File.Exists(langFilePath))
			{
				string langDataAsJson = File.ReadAllText(langFilePath);
				JSONNode langDefs = JSON.Parse(langDataAsJson);
				localLanguageJson = langDefs;
				Debug.Log("LanguageJson [2]");
				return (langDefs);
			}
			Debug.Log("LanguageJson [3]");
			return "";

		}

		public static IEnumerator LoadLanguageFileForDevBuild(Action onFinish)
		{
			const string languageJSONFilePath = "language.json";
			// Load Dev Language File from StreamingAssets
			var langFilePath = Path.Combine(Application.streamingAssetsPath, languageJSONFilePath);
			var uwr = UnityWebRequest.Get(langFilePath);
			yield return uwr.SendWebRequest();
			var langDataAsJson = uwr.downloadHandler.text;
			localLanguageJson = JSON.Parse(langDataAsJson);
			onFinish?.Invoke();
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

			if (string.IsNullOrEmpty(langCode))
			{
				langCode = "en";
			}


			Debug.Log($"EditorLoadFromLanguageJson() key [{key}] " +
				$"\njson is null? {(json == null)} ".Colored("orange") +
				$"| langCode: [{langCode}]".Colored(""));

			var node = json[langCode]; // This is null after opening the project


			Debug.Log($"EditorLoadFromLanguageJson() key [{key}] " +
				$"\nnode is null? {(node == null)} ".Colored("orange"));

			if(node == null)
			{
				if (displayMessages)
				{
					Debug.Log($"<color=red>File language.json was NOT loaded bacause it is NULL.</color>", unityObject);
				}
				return "";

			}

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