using Blabbers.Game00;
using UnityEngine;

public static class TTSPreview
{
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    //static void Init()
    //{
    //    LocalizationExtensions.EditorPreviewTTS = PreviewTTSInEditor;
    //}
    public static void PreviewTTSInEditor(string key)
    {
#if UNITY_EDITOR
        var text = LocalizationExtensions.EditorLoadFromLanguageJson(key, displayMessages: false);
        Debug.Log("TTS: ".Colored("cyan") + text);
        //WindowsVoice.speak(text);
#endif
    }
}