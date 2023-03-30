using Blabbers;
using UnityEngine;
using Blabbers.Game00;

public class UI_ConfigWindow : MonoBehaviour, ISingleton //can be based on UI PopupWindow later
{
    public bool showOnInitialize = true;

    public void OnCreated()
    {
        if (showOnInitialize)
        {
            this.gameObject.SetActive(true);
        }
    }
    public void OnYesButton()
    {
		if (ProgressController.GameProgress == null)
		{
			Debug.Log("OnYesButton() | ProgressController.GameProgress == null");
			return;
		}

		ProgressController.GameProgress.enableAutomaticTTS = true;
        ProgressController.enableAutomaticTTS = true;
    }
    public void OnNoButton()
    {
        if(ProgressController.GameProgress == null)
        {
            Debug.Log("OnNoButton() | ProgressController.GameProgress == null");
            return;
        }

		ProgressController.GameProgress.enableAutomaticTTS = false;
        ProgressController.enableAutomaticTTS = false;
    }
}
