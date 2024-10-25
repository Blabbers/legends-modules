﻿using Blabbers.Game00;
using LoLSDK;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonPlayTTS : MonoBehaviour
{
    [InfoBox("This script plays the TTS from a parent text with a LoadSDKText component attached. Unless there is a manual OverrideTTSkey added on the field.")]
    [SerializeField]
    private Button button;
    private TextLocalized localizedText;
    private bool loaded;
    public string overrideTTSkey;
    void OnEnable()
    {
        if (!string.IsNullOrEmpty(overrideTTSkey))
        {
			button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ManualSpeakText);
            return;
        }

        if (loaded)
            return;

        if (!localizedText)
        {
            localizedText = GetComponentInParent<TextLocalized>();
        }

        if (localizedText)
        {
            loaded = true;

            //Changed this line to override the normal Speech to text that is dependent of the bool
            //button.onClick.AddListener(sdkText.PlayThisSpeechText);    
            button.onClick.AddListener(() => ForceSpeakText(localizedText.Localization.Key));
        }

    }

    public void ExternalSetup(string key)
    {
        overrideTTSkey = key;

		if (!string.IsNullOrEmpty(overrideTTSkey))
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(ManualSpeakText);
			return;
		}
	}

    void ManualSpeakText()
    {
        Debug.Log("Btn SpeakText: " + overrideTTSkey);
        LOLSDK.Instance.SpeakText(overrideTTSkey);
        LocalizationExtensions.AlreadyPlayedTTS.Add(overrideTTSkey);

        //LOLSDK.Instance.SpeakText(key);
        //LOLSDK.Instance?.SpeakText(key);
        //LocalizationExtensions.AlreadyPlayedTTS.Add(key);
    }


    void ForceSpeakText(string key)
    {
        LOLSDK.Instance?.SpeakText(key);
        LocalizationExtensions.AlreadyPlayedTTS.Add(key);
    }
}
