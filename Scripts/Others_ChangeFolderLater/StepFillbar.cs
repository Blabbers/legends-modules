using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepFillbar : MonoBehaviour
{
    [Range(0,10)]
    [SerializeField]
    [OnValueChanged(nameof(UpdateVisual))]
    public int value = 10;    
    private int lastValue;
    [SerializeField]
    private List<GameObject> fillbarObjects;
    void Start()
    {
        UpdateVisual();
    }

    void Update()
    {
        if(value != lastValue)
		{
            lastValue = value;
            UpdateVisual();
        }
    }

    void UpdateVisual()
	{
		for (int i = 0; i < fillbarObjects.Count; i++)
		{
            var obj = fillbarObjects[i];
            obj.SetActive(i < value);
        }
	}
}