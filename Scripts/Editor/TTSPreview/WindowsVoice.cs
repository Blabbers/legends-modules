using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
using Blabbers.Game00;

public static class WindowsVoice
{
    [DllImport("WindowsVoice")]
    public static extern void initSpeech();

    [DllImport("WindowsVoice")]
    public static extern void destroySpeech();

    [DllImport("WindowsVoice")]
    public static extern void addToSpeechQueue(string s);

    [DllImport("WindowsVoice")]
    public static extern void clearSpeechQueue();

    [DllImport("WindowsVoice")]
    public static extern void statusMessage(StringBuilder str, int length);

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        initSpeech();
        Game.OnGameQuit -= destroy;
        Game.OnGameQuit += destroy;
    }

    public static void speak(string msg)
    {
        destroySpeech();
        initSpeech();
        // Destroying the speech and initializing it all again was the only way I found to pause it
        addToSpeechQueue(msg);
        //speak(msg);
    }

    public static void destroy()
    {
        destroySpeech();
    }

    public static string GetStatusMessage()
    {
        StringBuilder sb = new StringBuilder(40);
        statusMessage(sb, 40);
        return sb.ToString();
    }
}