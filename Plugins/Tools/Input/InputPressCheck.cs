using Blabbers.Game00;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InputPressCheck : MonoBehaviour
{
    public string inputName;
    public UnityEvent OnButtonDown;

	void Update()
    {
		if (Input.GetButtonDown(inputName)) 
        {
            ButtonDownInvoke();
        }

    }

    // Public for the possibility of external inspector re-use
    public void ButtonDownInvoke()
	{
        if (!this.isActiveAndEnabled) return;
        OnButtonDown?.Invoke();
    }
}
