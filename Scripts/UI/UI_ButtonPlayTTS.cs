using Blabbers.Game00;
using LoLSDK;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonPlayTTS : MonoBehaviour
{
    [InfoBox("This script plays the TTS from a parent text with a LoadSDKText component attached. Unless there is a manual OverrideTTSkey added on the field.")]
    [SerializeField]
    private Button button;
    private LoadSDKText sdkText;
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
        
        if(loaded)
            return;

        if (!sdkText)
        {
            sdkText = GetComponentInParent<LoadSDKText>();
        }
            
        if (sdkText)
        {
            loaded = true;
            button.onClick.AddListener(sdkText.PlayThisSpeechText);    
        }
    }

    void ManualSpeakText()
    {
        Debug.Log("Btn SpeakText: " + overrideTTSkey);
        LOLSDK.Instance.SpeakText(overrideTTSkey);
        LocalizationExtensions.AlreadyPlayedTTS.Add(overrideTTSkey);
    }
}
