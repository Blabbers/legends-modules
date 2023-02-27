using Blabbers.Game00;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[System.Serializable]
public class EventOptionClick : UnityEvent<int>
{
}

public class UI_PopupOptions : UI_PopupWindow
{
    public EventOptionClick OnOptionClick;
    [SerializeField] Transform buttonsParent;
    
    public void ButtonClick(int id)
    {
        OnOptionClick.Invoke(id);
    }

}
