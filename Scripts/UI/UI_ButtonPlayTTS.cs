using Blabbers.Game00;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonPlayTTS : MonoBehaviour
{
    [SerializeField]
    private Button button;
    private LoadSDKText sdkText;
    private bool loaded;
    void OnEnable()
    {
        if(loaded)
            return;
        
        if (!sdkText)
        {
            sdkText = GetComponentInParent<LoadSDKText>();
        }
            
        if (sdkText)
        {
            loaded = true;
            button.onClick.AddListener(sdkText.PlaySpeechText);    
        }
    }
}
