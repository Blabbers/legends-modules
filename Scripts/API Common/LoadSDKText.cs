using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

namespace Blabbers.Game00
{
	[DefaultExecutionOrder(+1000)]
	public class LoadSDKText : MonoBehaviour
	{
		//Key in which the text is placed on the JSON file
		public string key;
		public bool playTTSOnEnable = false;
		public bool applyColorCodes = true;

		[Tooltip("Requires the Component AnimatedSDKText")]
		public bool isAnimated = false;

		[HideInInspector]
		public Text myText;

		[HideInInspector]
		public TextMeshProUGUI myTextM;

		[HideInInspector]
		public TextMeshPro myTextP;

		private string placeholderText;
		string targetText;
		public bool HasKey => !string.IsNullOrEmpty(key);
		public bool hasLoadedKey =false;


		private void Awake()
		{
			LoadPossibleTextComponents();
		}

		private void LoadPossibleTextComponents()
		{
			if (!myTextM)
			{
				myTextM = GetComponent<TextMeshProUGUI>();
				if (myTextM)
				{
					myTextM.text = "";
					placeholderText = $"<TNF> {myTextM.text}";
				}
			}
			if (!myText)
			{
				myText = GetComponent<Text>();
				if (myText)
				{
					myText.text = "";
					placeholderText = $"<TNF> {myText.text}";
				}
			}

			if (!myTextP)
			{
				myTextP = GetComponent<TextMeshPro>();
				if (myTextP)
				{
					myTextP.text = "";
					placeholderText = $"<TNF> {myTextP.text}";
				}
			}
		}

		//If the key is right, updates it on enable
		void OnEnable()
		{
			if (!string.IsNullOrEmpty(key))
			{
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
			if(isAnimated)
			{
				targetText = LocalizationExtensions.LocalizeText(key, null, null, applyColorCodes);
				hasLoadedKey = true;
				return;
			}
				
			//Adds an asterisk if the text coult not be loaded from the language file
			if (myTextM)
			{
				//myTextM.text = LocalizationExtensions.LocalizeText(key);
				myTextM.text = LocalizationExtensions.LocalizeText(key,null,null, applyColorCodes);
				hasLoadedKey = true;
			}
			if (myText)
			{
				//myText.text = LocalizationExtensions.LocalizeText(key);
				myText.text = LocalizationExtensions.LocalizeText(key, null, null, applyColorCodes);
				hasLoadedKey = true;
			}
			if (myTextP)
			{
				//myTextP.text = LocalizationExtensions.LocalizeText(key);
				myTextP.text = LocalizationExtensions.LocalizeText(key, null, null, applyColorCodes);
				hasLoadedKey = true;
			}
		}


		public string GetTargetText()
		{
			return targetText;
		}


		public void UpdateText_Concat(string add)
		{
			//Adds an asterisk if the text coult not be loaded from the language file
			if (myTextM)
			{
				myTextM.text = LocalizationExtensions.LocalizeText(key) + " " + add;
				hasLoadedKey = true;
			}
			if (myText)
			{
				myText.text = LocalizationExtensions.LocalizeText(key) + " " + add;
				hasLoadedKey = true;
			}
			if (myTextP)
			{
				myTextP.text = LocalizationExtensions.LocalizeText(key) + " " + add;
				hasLoadedKey = true;
			}
		}

		public void PlayThisSpeechText()
		{
			LocalizationExtensions.PlayTTS(key);
		}

		#region Editor Buttons
		[Button()]
		public void SaveToLanguageJson()
		{
#if UNITY_EDITOR
			if (!HasKey)
			{
				Debug.Log("<color=red>Please insert a key to this text or it won't be saved. [Click to highlight]", this);
				return;
			}

			var textValue = "";
			LoadPossibleTextComponents();
			if (myText) 
			{
				UnityEditor.Undo.RecordObject(myTextM, "LoadSDK Save Text");
				textValue = myText.text;			
				UnityEditor.EditorUtility.SetDirty(myText);				
			}
			if (myTextM) 
			{ 
				UnityEditor.Undo.RecordObject(myTextM, "LoadSDK Save Text");
				textValue = myTextM.text;
				UnityEditor.EditorUtility.SetDirty(myTextM);				
			}
			if (myTextP)
			{
				UnityEditor.Undo.RecordObject(myTextM, "LoadSDK Save Text");
				textValue = myTextP.text;				
				UnityEditor.EditorUtility.SetDirty(myTextP);				
			}
			LocalizationExtensions.EditorSaveToLanguageJson(key, textValue, this);
#endif
		}

		[Button()]
		public void LoadFromLanguageJson()
		{
#if UNITY_EDITOR
			if (!HasKey)
			{
				Debug.Log("<color=red>There's no key to load this text from, insert a key to this text. [Click to highlight]", this);
				return;
			}
			var text = LocalizationExtensions.EditorLoadFromLanguageJson(key, this);
			if (!string.IsNullOrEmpty(text))
			{
				LoadPossibleTextComponents();
				if (myText)
				{
					UnityEditor.Undo.RecordObject(myTextM, "LoadSDK Load Text");
					myText.text = text;
					UnityEditor.EditorUtility.SetDirty(myTextM);
				}
				if (myTextM)
				{
					UnityEditor.Undo.RecordObject(myTextM, "LoadSDK Load Text");
					myTextM.text = text;
					UnityEditor.EditorUtility.SetDirty(myTextM);
				}
				if (myTextP)
				{
					UnityEditor.Undo.RecordObject(myTextM, "LoadSDK Load Text");
					myTextP.text = text;
					UnityEditor.EditorUtility.SetDirty(myTextM);
				}
			}
#endif
		}
#endregion
	}
}