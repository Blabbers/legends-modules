using UnityEngine;
using UnityEngine.UI;
using LoLSDK;
using TMPro;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using NaughtyAttributes;

namespace Blabbers.Game00
{
	[DefaultExecutionOrder(+1000)]
	public class LoadSDKText : MonoBehaviour
	{
		//Key in which the text is placed on the JSON file
		public string key;
		public bool playTTSOnEnable = false;
		[Header("Level Options"), Tooltip("Se esse texto tiver separado por algum underscore '_' e tiver um level na frente, marque essa caixa pra ele trocar sozinho o que tiver na frente do simbolo pelo level atual, automaticamente.")]
		public bool replaceUnderscoreWithCurrentLevel;

		[HideInInspector]
		public Text myText;

		[HideInInspector]
		public TextMeshProUGUI myTextM;

		[HideInInspector]
		public TextMeshPro myTextP;

		private string placeholderText;
		public bool HasKey => !string.IsNullOrEmpty(key);

		private void Awake()
		{
			LoadPossibleTextComponents();
		}

		private void LoadPossibleTextComponents()
		{
			if (!myTextM)
			{
				myTextM = GetComponent<TextMeshProUGUI>();
				if (myTextM) placeholderText = $"<TNF> {myTextM.text}";
			}
			if (!myText)
			{
				myText = GetComponent<Text>();
				if (myText) placeholderText = $"<TNF> {myText.text}";
			}

			if (!myTextP)
			{
				myTextP = GetComponent<TextMeshPro>();
				if (myTextP) placeholderText = $"<TNF> {myTextP.text}";
			}
		}

		//If the key is right, updates it on enable
		void OnEnable()
		{
			if (!string.IsNullOrEmpty(key))
			{
				if (replaceUnderscoreWithCurrentLevel)
				{
					var index = key.IndexOf('_');
					var text = key.Substring(0, index);
					key = text + "_" + (ProgressController.GameProgress.currentLevelId + 1);
				}

				UpdateText();
				if (playTTSOnEnable)
				{
					// Only plays each TTS info ONCE per session.
					//if (!LocalizationExtensions.HasPlayedTTS(key))
					{
						LocalizationExtensions.PlayTTS(key);
					}
				}
			}
			else
			{
				Debug.LogWarning("Key variable is empty at:" + this.gameObject.name, this.gameObject);
			}
		}

		//Set key from outside (through UI events)
		public void SetKey(string key)
		{
			this.key = key;
			UpdateText();
		}

		//Updates the UI text that is shown
		public void UpdateText()
		{
			//Adds an asterisk if the text coult not be loaded from the language file
			if (myTextM)
			{
				myTextM.text = LocalizationExtensions.LocalizeText(key);
			}
			if (myText)
			{
				myText.text = LocalizationExtensions.LocalizeText(key);
			}
			if (myTextP)
			{
				myTextP.text = LocalizationExtensions.LocalizeText(key);
			}
		}

		public void UpdateText_Concat(string add)
		{
			//Adds an asterisk if the text coult not be loaded from the language file
			if (myTextM)
			{
				myTextM.text = LocalizationExtensions.LocalizeText(key) + " " + add;
			}
			if (myText)
			{
				myText.text = LocalizationExtensions.LocalizeText(key) + " " + add;
			}
			if (myTextP)
			{
				myTextP.text = LocalizationExtensions.LocalizeText(key) + " " + add;
			}
		}

		public void PlayThisSpeechText()
		{
			LocalizationExtensions.PlayTTS(key);
		}

		[Button()]
		public void SaveToLanguageJson()
		{
			if (!HasKey)
			{
				Debug.Log("<color=red>Please insert a key to this text or it won't be saved. [Click to highlight]", this);
				return;
			}

			var textValue = "";
			LoadPossibleTextComponents();
			if (myText) textValue = myText.text;
			if (myTextM) textValue = myTextM.text;
			if (myTextP) textValue = myTextP.text;			
			LocalizationExtensions.EditorSaveToLanguageJson(key, textValue, this);
		}

		[Button()]
		public void LoadFromLanguageJson()
		{
			if (!HasKey)
			{
				Debug.Log("<color=red>There's no key to load this text from, insert a key to this text. [Click to highlight]", this);
				return;
			}
			var text = LocalizationExtensions.EditorLoadFromLanguageJson(key, this);
			if (!string.IsNullOrEmpty(text))
			{
				LoadPossibleTextComponents();
				if (myText) myText.text = text;
				if (myTextM) myTextM.text = text;
				if (myTextP) myTextP.text = text;
			}
		}
	}

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
		public static string LocalizeText(string localizationKey, string appendLeft = null, string appendRight = null)
		{
			if (!Application.isPlaying)
			{
				// TODO: Read fro json file and get the text from here
			}

			if (SharedState.languageDefs == null)
				Debug.Log($"<TextNotFound> SharedState.languageDefs is not loaded yet. Localization Key: {localizationKey}");

			var mainText = SharedState.languageDefs != null ? SharedState.languageDefs[localizationKey].Value : $"<TNF> {localizationKey}";
			if (string.IsNullOrEmpty(mainText))
			{
				Debug.Log($"<TextNotFound> Localization Key: {localizationKey}, is returning an empty value.");
				mainText = $"<TNF> {localizationKey}";
			}

			mainText = ApplyColorCodes(mainText);

			return $"{appendLeft}{mainText}{appendRight}";
		}

		private static string ApplyColorCodes(string mainText)
		{
			string key, term, plural;
			foreach (var color in GameData.Instance.textConfigs.colorCodes)
			{
				key = color.key;
				term = SharedState.languageDefs[key].Value;
				plural = term + "S";				
				mainText = FindAndColorTerm(plural, mainText, color.color, out var success);
				if (!success) mainText = FindAndColorTerm(term, mainText, color.color, out success);

				if (color.extraKeys.Count > 0)
				{
					foreach (var extra in color.extraKeys)
					{
						key = extra;
						term = SharedState.languageDefs[key].Value;
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

		public static JSONNode GetLanguageJson()
		{
			const string languageJSONFilePath = "language.json";
			// Load Dev Language File from StreamingAssets
			string langFilePath = Path.Combine(Application.streamingAssetsPath, languageJSONFilePath);
			if (File.Exists(langFilePath))
			{
				string langDataAsJson = File.ReadAllText(langFilePath);
				JSONNode langDefs = JSON.Parse(langDataAsJson);
				return (langDefs);
			}
			return "";

		}

		public static void EditorSaveToLanguageJson(string key, string value, Object unityObject = null)
		{
			var json = LocalizationExtensions.GetLanguageJson();
			var langCode = "en";
			var node = json[langCode];
			node.Add(key, value);

			const string languageJSONFilePath = "language.json";
			string langFilePath = Path.Combine(Application.streamingAssetsPath, languageJSONFilePath);
			if (File.Exists(langFilePath))
			{
				File.WriteAllText(langFilePath, json.ToString(1));
				Debug.Log($"<color=cyan>File language.json was updated</color>: <color=white>[{key}]: {node[key]}</color>", unityObject);
			}
		}
		public static string EditorLoadFromLanguageJson(string key, Object unityObject = null)
		{
			var json = LocalizationExtensions.GetLanguageJson();
			var langCode = "en";
			var node = json[langCode];
			if (node[key] == null)
			{
				Debug.Log($"<color=red>File language.json was NOT loaded to this asset bacause key [{key}] is NULL.</color>", unityObject);
				return "";
			}
			else
			{
				Debug.Log($"<color=yellow>File language.json was loaded to this asset</color> \n<color=white>[{key}]: {node[key]}</color>", unityObject);
				return node[key];
			}			
		}
	}
}