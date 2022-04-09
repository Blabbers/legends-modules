using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blabbers.Game00;

public class UI_TutorialController : MonoBehaviour, ISingleton
{

    public bool IsEnabled = false;

    public bool CheckTutorialEnabled()
    {

        IsEnabled = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                IsEnabled = true;
                break;
            }
        }

        return IsEnabled;
    }



    public void OnCreated()
    {
    }
}
