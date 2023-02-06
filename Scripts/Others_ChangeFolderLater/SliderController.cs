using Blabbers.Game00;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public UnityEvent<float> onValueChanged;


	//public UnityEvent<float> onValueChangedMoreThan;

	[Foldout("Runtime")] [SerializeField] bool isInteractable = true;
	[Foldout("Runtime")] [SerializeField] Vector3 originalHandlePos, targetPos;
	[Foldout("Runtime")][SerializeField] Vector3 minHandlePos, maxHandlePos;
	float percent;
	bool initialized = false;

	[Foldout("Configs")] public bool valueToColor = false;
	[Foldout("Configs")] public Gradient colorRange;
	[Foldout("Configs")] public float startValue;
	[Foldout("Configs")] public Vector2 sliderRange;

    [Foldout("Components")] public Slider slider;
	[Foldout("Components")] public TextMeshProUGUI minDisplay;
	[Foldout("Components")] public TextMeshProUGUI maxDisplay, currentDisplay;
	[Foldout("Components")] public Transform originalHandle, ghostHandle;
	[Foldout("Components")][SerializeField] Image fillImage;

	#region Testing
	[Button]
	void GetHandlePosition()
	{
		GetOriginalHandlePos();
	}

	[Button]
	void AdjustGhostSlider()
	{
		ghostHandle.gameObject.SetActive(true);
		RectUtility.UpdateRectScreenPosition(ref ghostHandle, originalHandlePos);
	} 
	#endregion


	private void Awake()
    {
		if (initialized) return;
        SetStartingConfigs();
		slider.onValueChanged.AddListener(delegate { OnValueChanged();});
		//GetOriginalHandlePos();
		//originalPos = scoreText.transform.localPosition;

		initialized = true;
	}

	private void OnEnable()
	{
		//Debug.Log("ENABLED + " + this.name);
		if (initialized) return;
		GetOriginalHandlePos();
	}

	private void OnDisable()
	{
		//Debug.Log("DISABLED + " + this.name);
	}

	#region External

	public float GetPercent(float value)
	{
		return Mathf.InverseLerp(slider.minValue, slider.maxValue, value);
	}

	public void Reset()
	{
		slider.value = startValue;
	}

	public void SetUpData(float startValue, Vector2 range)
	{
		this.startValue = startValue;
		
		// HACK FOR BETA BUILD
		//sliderRange = range; // I disabled this, to manually change the text on the others
		// HACK FOR BETA BULD

		SetStartingConfigs();
	}

	public void GetOriginalHandlePos()
	{
		Debug.Log("<SliderController> GetOriginalHandlePos()".Colored("white"));
		originalHandlePos = RectUtility.GetRectPosition(originalHandle);

		slider.value = sliderRange.x;
		minHandlePos = RectUtility.GetRectPosition(originalHandle);

		slider.value = sliderRange.y;
		maxHandlePos = RectUtility.GetRectPosition(originalHandle);

		slider.value = startValue;

	} 
	#endregion

	#region Editor
#if UNITY_EDITOR

	[Button]
	void ConfigureSlider()
	{
		SetStartingConfigs();

	}

#endif

	#endregion

	void SetStartingConfigs()
    {
		//minDisplay.text = sliderRange.x.ToString();
		//maxDisplay.text = (sliderRange.y * UI_GameplayHUD.HACK_MULTIPLIER_FOR_SPEED_TEXT).ToString();

		//currentDisplay.text = (startValue * UI_GameplayHUD.HACK_MULTIPLIER_FOR_SPEED_TEXT).ToString();

		minDisplay.text = (0).ToString();
		//maxDisplay.text = (AirplaneConfigsData.Instance.maxSpeedDisplay).ToString();
		maxDisplay.text = (sliderRange.y).ToString();

		currentDisplay.text = (0).ToString();


		slider.minValue = sliderRange.x;
		slider.maxValue = sliderRange.y;

		slider.value = sliderRange.x;
		minHandlePos = RectUtility.GetRectPosition(originalHandle);

		slider.value = sliderRange.y;
		maxHandlePos = RectUtility.GetRectPosition(originalHandle);

		slider.value = startValue;
	}

    void OnValueChanged()
    {
		//currentDisplay.text = (Mathf.Round(slider.value * UI_GameplayHUD.HACK_MULTIPLIER_FOR_SPEED_TEXT)).ToString();
		currentDisplay.text = (Mathf.Round(slider.value)).ToString();
		targetPos = originalHandle.position;
		percent = slider.value / slider.maxValue;
		onValueChanged?.Invoke(slider.value);


		if (valueToColor)
		{
			fillImage.color = colorRange.Evaluate(percent);
		}

			//Debug.Log("OnValueChanged".Colored("red"));
	}

	public void UpdateValue(float value)
	{
		slider.value = value;
		//currentDisplay.text = (Mathf.Round(slider.value * UI_GameplayHUD.HACK_MULTIPLIER_FOR_SPEED_TEXT)).ToString();
		currentDisplay.text = (Mathf.Round(slider.value)).ToString();
	}

	public void UpdateDisplay(float value)
	{
		//currentDisplay.text = (Mathf.Round(value * UI_GameplayHUD.HACK_MULTIPLIER_FOR_SPEED_TEXT)).ToString();
		currentDisplay.text = (Mathf.Round(slider.value)).ToString();
	}


	public void UpdateRange(Vector2 range)
	{
		sliderRange = range;

		slider.minValue = sliderRange.x;
		slider.maxValue = sliderRange.y;
	}

	public void ToggleInteractable(bool active)
	{
		isInteractable = active;
		slider.interactable = isInteractable;
	}


	#region Ghost Handle
	public void ToggleGhostHandle(bool active, float startLerp = 0.0f)
	{
		//LerpGhostHandle(startLerp);
		//ghostHandle.position = originalHandlePos;
		RectUtility.UpdateRectScreenPosition(ref ghostHandle, originalHandlePos);

		ghostHandle.gameObject.SetActive(active);
	}

	public void UpdateGhostTarget()
	{
		Debug.Log("UpdateGhostTarget()\n".Colored("red"));
		targetPos = RectUtility.GetRectPosition(originalHandle);
		ToggleGhostHandle(true, 0);
	}


	public void SetBothHandles(float value)
	{
		UpdateValue(value);
		ghostHandle.position = RectUtility.GetRectPosition(originalHandle);
	}

	public void SetGhostHandle(float percent)
	{
		ghostHandle.position = Vector2.Lerp(minHandlePos, maxHandlePos, percent);
	}

	#endregion


	// Update is called once per frame
	void Update()
    {
        
    }
}
