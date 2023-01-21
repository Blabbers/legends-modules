using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderValueCheck : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public float valueBiggerThan;
    public UnityEvent OnValueBiggerThan;
    public float valueEqualsTo;
    public UnityEvent OnValueEqualsTo;
    public float valueSmallerThan;
    public UnityEvent OnValueSmallerThan;    

	private void Start()
	{
		if (!slider)
		{
            slider = GetComponent<Slider>();
		}
        slider.onValueChanged.AddListener(HandleValueChanged);
    }

    void HandleValueChanged(float value)
	{
        if (!isActiveAndEnabled) return;
        if (value > valueBiggerThan) OnValueBiggerThan?.Invoke();
        if (value == valueEqualsTo) OnValueEqualsTo?.Invoke();
        if (value < valueSmallerThan) OnValueSmallerThan?.Invoke();
    }
}
