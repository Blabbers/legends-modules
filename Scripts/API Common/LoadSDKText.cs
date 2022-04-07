using UnityEngine;
using UnityEngine.UI;
using LoLSDK;
using TMPro;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Blabbers.Game00
{
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

		private string placeholderText;

		private void Awake()
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
		}
		
		public void PlayThisSpeechText()
		{
			LocalizationExtensions.PlayTTS(key);
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
            
			if(SharedState.languageDefs == null)
				Debug.Log($"<TextNotFound> SharedState.languageDefs is not loaded yet. Localization Key: {localizationKey}");
			
			var mainText = SharedState.languageDefs != null ? SharedState.languageDefs[localizationKey].Value : $"<TNF> {localizationKey}";
			if (string.IsNullOrEmpty(mainText))
			{
				Debug.Log($"<TextNotFound> Localization Key: {localizationKey}, is returning an empty value.");
				mainText = $"<TNF> {localizationKey}";
			}
			return $"{appendLeft}{mainText}{appendRight}";
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
	}
}