using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blabbers.Game00;

public class UI_TutorialController : MonoBehaviour, ISingleton
{
    public static HashSet<string> AlreadyTriggeredInThisLevel = new HashSet<string>();
    public bool IsEnabled = false;
    
    public void OnCreated()
    {
        if (!SceneLoader.isStuckOnThisLevel)
        {
            AlreadyTriggeredInThisLevel.Clear();
        }
    }
    public bool CheckTutorialEnabled()
    {
        IsEnabled = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var isTutorial = child.GetComponent<UI_TutorialLevel1>() != null;
            if (child.gameObject.activeSelf && isTutorial)
            {
                IsEnabled = true;
                break;
            }
        }

        return IsEnabled;
    }
}
