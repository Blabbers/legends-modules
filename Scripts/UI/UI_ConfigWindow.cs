using System;
using System.Collections;
using System.Collections.Generic;
using Blabbers.Game00;
using UnityEngine;

public class UI_ConfigWindow : MonoBehaviour //can be based on UI PopupWindow later
{
    public void OnYesButton()
    {
        ProgressController.GameProgress.enableAutomaticTTS = true;
    }
    public void OnNoButton()
    {
        ProgressController.GameProgress.enableAutomaticTTS = false;
    }
}
