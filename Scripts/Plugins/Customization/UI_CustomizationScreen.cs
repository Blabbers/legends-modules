using Blabbers.Game00;
using System.Collections;
using System;
using System.Collections.Generic;
using Blabbers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine.Events;
using System.Linq;

public class UI_CustomizationScreen : MonoBehaviour, ISingleton
{

	#region Variables

	[Foldout("Runtime")] public CustomizationSlot Type;
	[Foldout("Runtime")] public bool isNewGame = false;
	[Foldout("Runtime")][SerializeField] float targetRotation;
	bool hairActive, skinActive;

	//[Foldout("Runtime")][ReorderableList] public List<SlotData> slotData = new List<SlotData>();
	[Foldout("Runtime")][ReorderableList][SerializeField] Customization[] savedOptions;

	[Range(15, 45)]
	[Foldout("Configs")] public float rotationAngle = 30;
	[Range(0, 0.5f)]
	[Foldout("Configs")] public float rotationDuration = 0.25f;



	//[Foldout("Configs")] public List<string> sdkKeys;
	//bodyCustomize_Hair //bodyCustomize_Face //bodyCustomize_Torso //bodyCustomize_Legs //bodyCustomize_Shoes

	[Foldout("Components")] public CanvasGroup buttonParent;
	[Foldout("Components")] public List<TextLocalized> displayTexts;
	[Foldout("Components")] public PlayerCustomization customization;
	[Foldout("Components")] public GameObject popupSkinColor;
	[Foldout("Components")] public GameObject popupHairColor;
	[Foldout("Components")][SerializeField] Transform playerTransform;
	[Foldout("Components")]
	[SerializeField] List<CustomizationSelector> selectors;

	#endregion

	[Foldout("Events")] public UnityEvent OnFirstCustomization;
	[Foldout("Events")] public UnityEvent OnCustomizationStart;
	[Foldout("Events")] public UnityEvent OnCustomizationEnd;


	#region Editor

	[Button]
	void SetupSelectors_Editor()
	{

		var allTargets = UnityEngine.Object.FindObjectsOfType<CustomizationSelector>();
		selectors.Clear();
		selectors= new List<CustomizationSelector>(allTargets);

		selectors.Sort((x, y) => x.GroupId.CompareTo(y.GroupId));

		//objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));


		for (int i = 0; i < selectors.Count; i++)
		{
			selectors[i].OnOptionChanged = null;

			var lastId = PossibleCustomizations.Instance.GetSlotSize(selectors[i].GroupId) - 1;
			var name = PossibleCustomizations.Instance.GetName(selectors[i].GroupId);

			selectors[i].SetupSelector(lastId, name);
			//selectors[i].OnOptionChanged += HandleSelectionUpdate;
		}

		//SetSelectorEvents();
	}

	#endregion


	void SetSelectorEvents()
	{
		for (int i = 0; i < selectors.Count; i++)
		{
			selectors[i].OnOptionChanged = null;

			var lastId = PossibleCustomizations.Instance.GetSlotSize(selectors[i].GroupId) - 1;
			var name = PossibleCustomizations.Instance.GetName(selectors[i].GroupId);

			selectors[i].SetupSelector(lastId, name);
			selectors[i].OnOptionChanged += HandleSelectionUpdate;

			if (selectors[i].HasTitle)
			{
				selectors[i].DisplayTitle = LocalizationExtensions.LocalizeText(selectors[i].LanguageKey);
				selectors[i].UpdateDisplay();
			}

		}
	}



	#region Awake
	void Awake()
	{
		//slotData.Clear();
		GetDataFromSave();

		GetSDKTexts();
		RefreshSDKTexts();

		targetRotation = 0;
		SetRotation(targetRotation);

		hairActive = skinActive = false;
		//customization.UpdateVisual(slotData);
		customization.UpdateVisual(savedOptions);

		SetSelectorEvents();
		//characterVisual.UpdateVisual();

		isNewGame = ProgressController.GameProgress.isNewGame;
		
		Delay(0.25f);
	}

	void Start()
	{
		if (IsFirstCustomization())
		{
			Debug.Log("UI_CustomizationScreen.OnFirstCustomization()".Colored());
			OnFirstCustomization?.Invoke();
		}

		OnCustomizationStart?.Invoke();
	}

	bool IsFirstCustomization()
	{
		if (GameData.Instance.Progress.customizations == null) return true;
		if (GameData.Instance.Progress.customizations.Length == 0) return true;

		return false;
	}

	void GetSDKTexts()
	{
		//Transform parent, current, title;
		//LoadSDKText sdkText;
		//TextMeshProUGUI text;


		//Debug.LogError($"GetSDKTexts(): {slotData.Count}".Colored());


		Transform parent;
		parent = buttonParent.transform;


		//for (int i = 0; i < displayTexts.Count; i++)
		//{
		//	if (slotData[i].display == null) slotData[i].display = displayTexts[i];
		//}

	}

	void RefreshSDKTexts()
	{
		//int size = 5;
		//size = sdkKeys.Count;

		//Debug.Log($"RefreshSDKTexts(): {slotData.Count}".Colored());

		//for (int i = 0; i < size; i++)
		//{
		//	//slotData[i].UpdateName();
		//	slotData[i].SetTitle(LocalizationExtensions.LocalizeText(sdkKeys[i]));
		//}

		for (int i = 0; i < selectors.Count; i++)
		{


			if (selectors[i].HasTitle)
			{
				//string value = LocalizationExtensions.LocalizeText(selectors[i].LanguageKey);
				//Debug.Log($"RefreshSDKTexts()\n{i} | key:{selectors[i].LanguageKey} | value: {value}");
				selectors[i].DisplayTitle = LocalizationExtensions.LocalizeText(selectors[i].LanguageKey);
				selectors[i].UpdateDisplay();

				//savedOptions[i].name = selectors[i].DisplayTitle;
			}


			//savedOptions[i].name = $"[{savedOptions[i].id}] {selectors[i].GroupName}";
		}




	}

	void GetDataFromSave()
	{
		#region MyRegion
		//int size = 5;
		//CustomizationSlot current;

		//if (GameData.Instance.progress == null)
		//{
		//    size = 0;
		//}
		//else if (GameData.Instance.progress.customizations == null)
		//{
		//    size = 0;
		//}
		//else
		//{

		//    size = GameData.Instance.progress.customizations.Length;
		//}



		//if (size <= 0)
		//{
		//    Debug.Log("No saved data found".Colored("red"));

		//    size = PossibleCustomizations.Instance.NumberOfSlots;

		//    //for (int i = 0; i < size; i++)
		//    //{
		//    //    selectIds.Add(0);
		//    //}

		//    for (int i = 0; i < size; i++)
		//    {
		//        current = (CustomizationSlot)i;
		//        slotData.Add(PossibleCustomizations.Instance.GetSlotData(current));
		//        slotData[i].UpdateCurrent(0);
		//    }

		//}
		//else
		//{
		//    //for (int i = 0; i < size; i++)
		//    //{
		//    //    selectIds.Add(GameData.Instance.progress.customizations[i].id);
		//    //}

		//    Debug.Log("Found Saved Data".Colored("red"));

		//    for (int i = 0; i < size; i++)
		//    {
		//        current = (CustomizationSlot)i;
		//        slotData.Add(PossibleCustomizations.Instance.GetSlotData(current));
		//        slotData[i].UpdateCurrent(GameData.Instance.progress.customizations[i].id);
		//    }

		//}

		//UpdateGameData(); 
		#endregion

		//Debug.Log($"GetDataFromSave()".Colored("green"));
		int size = 5;
		CustomizationSlot current;


		//size = Enum.GetNames(typeof(CustomizationSlot)).Length;
		//Debug.Log($"is PossibleCustomizations.Instance null? {PossibleCustomizations.Instance == null}");

		size = PossibleCustomizations.Instance.NumberOfSlots;
		savedOptions = new Customization[size];

		if (GameData.Instance.Progress.customizations.Length == size)
		{
			

			for (int i = 0; i < size; i++)
			{
				savedOptions[i] = new Customization();
				savedOptions[i].name = GameData.Instance.Progress.customizations[i].name;
				savedOptions[i].id = GameData.Instance.Progress.customizations[i].id;

			}

		}
		else
		{
			for (int i = 0; i < size; i++)
			{
				savedOptions[i] = new Customization();
				savedOptions[i].name = "";
				savedOptions[i].id = 0;
			}

				//		slotData.Add(PossibleCustomizations.Instance.GetSlotData(current));
				//		slotData[i].UpdateCurrent(0);
		}


		//if (GameData.Instance.Progress.customizations != null)
		//{

		//	Debug.Log($"UI_CustomizationScreen.GetDataFromSave() " +
		//		$"\nNumberOfSlots: {size} | GameData customizations: {GameData.Instance.Progress.customizations.Length}".Colored("orange"));
		//}

		//if (GameData.Instance.Progress.customizations.Length == size)
		//{

		//	for (int i = 0; i < size; i++)
		//	{
		//		//Debug.Log($"GetDataFromSave() i = {i}");

		//		current = (CustomizationSlot)i;
		//		slotData.Add(PossibleCustomizations.Instance.GetSlotData(current));
		//		slotData[i].UpdateCurrent(GameData.Instance.Progress.customizations[i].id);
		//	}

		//}
		//else
		//{



		//	for (int i = 0; i < size; i++)
		//	{
		//		//Debug.Log($"GetDataFromSave() i = {i}");

		//		current = (CustomizationSlot)i;
		//		slotData.Add(PossibleCustomizations.Instance.GetSlotData(current));
		//		slotData[i].UpdateCurrent(0);
		//	}
		//}



		//UpdateGameData();
	}




	void Delay(float time)
	{
		StartCoroutine(_Delay(time));
		IEnumerator _Delay(float delay)
		{
			yield return new WaitForSeconds(delay);
			RefreshSDKTexts();

		}
	}

	#endregion

	#region Buttons


	void HandleSelectionUpdate(int groupId, int selectedId)
	{
		Debug.Log($"HandleSelectionUpdate()\ngroupId: {groupId} |selectedId: {selectedId}");


		//slotData[groupId].UpdateCurrent(selectedId);

		//ArrowButtonsGeneric(id);
		//slotData[id].Previous();

		savedOptions[groupId].id = selectedId;
		//savedOptions[groupId].name = 
		UpdateVisualGeneric();
	}


	#region Rotation
	public void RotateForward()
	{

		targetRotation += rotationAngle;
		UpdateRotation(targetRotation);
	}

	public void RotateBack()
	{
		targetRotation -= rotationAngle;
		UpdateRotation(targetRotation);
	}

	void UpdateRotation(float angle)
	{
		//playerTransform.Rotate(new Vector3(0, angle, 0));

		playerTransform.DOKill();
		playerTransform.DORotate(new Vector3(0, angle, 0), rotationDuration);


	}

	void SetRotation(float angle)
	{
		targetRotation = angle;
		playerTransform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
	}

	#endregion

	//public void Next(int id)
	//{
	//	//Debug.Log("Next: " + (CustomizationSlot)id);
	//	//button.GetComponent<MotionTweenPlayer>().OnAnimationFinished.AddListener(() => { Singleton.Get<SceneLoader>().LoadGameLevel(myLevel); })

	//	ArrowButtonsGeneric(id);

	//	slotData[id].Next();
	//	UpdateVisualGeneric();
	//}

	//public void Previous(int id)
	//{
	//	//Debug.Log("Previous: " + (CustomizationSlot)id);
	//	ArrowButtonsGeneric(id);

	//	slotData[id].Previous();
	//	UpdateVisualGeneric();
	//}

	public void RandomizeVisuals()
	{
		//foreach (var item in slotData)
		//{
		//	item.GenerateRandomOption();
		//}

		foreach (var item in savedOptions)
		{
			//item.GenerateRandomOption();
		}

		UpdateVisualGeneric();
	}

	//void GenerateRandomOption()
	//{
	//	int newId;
	//	newId = UnityEngine.Random.Range(0, TotalOptions);

	//	UpdateCurrent(newId);

	//}


	void ArrowButtonsGeneric(int id)
	{
		//Debug.Log("ArrowButtonsGeneric: " + (CustomizationSlot)id);

		Type = (CustomizationSlot)id;
	}

	//Methods to swap visuals
	public void Finish()
	{
		buttonParent.DOFade(0.0f, 0.75f).OnComplete(PostAnimation);
		Analytics.OnSkinSelected();

		UpdateGameData();

		isNewGame = ProgressController.GameProgress.isNewGame;

		void PostAnimation()
		{
			OnCustomizationEnd?.Invoke();

			if (isNewGame)
			{
				Singleton.Get<SceneLoader>().LoadSceneByName("cutscene-start");
			}
			else
			{
				Singleton.Get<SceneLoader>().LoadLevelSelectScene();
			}

			Singleton.Get<ProgressController>().Save();
		}
	}

	#region Color picks
	public void ToggleColorPick(int id)
	{
		if (id == 0)
		{
			hairActive = !hairActive;
			ToggleColorPick_Hair(hairActive);
			Type = CustomizationSlot.HairColor;
		}
		else
		{
			skinActive = !skinActive;
			ToggleColorPick_Face(skinActive);
			Type = CustomizationSlot.SkinColor;
		}

	}


	//Being used to select hair color, weird
	//public void ToggleHairColorPick(int optionId)
	//{
	//	hairActive = !hairActive;
	//	//ToggleColorPick_Hair(hairActive);
	//	Type = CustomizationSlot.HairColor;
	//	slotData[(int)CustomizationSlot.HairColor].UpdateCurrent(optionId);
	//	UpdateVisualGeneric();
	//}

	////Being used to select skin color, weird
	//public void ToggleSkinColorPick(int optionId)
	//{
	//	skinActive = !skinActive;
	//	ToggleColorPick_Face(skinActive);
	//	Type = CustomizationSlot.SkinColor;
	//	slotData[(int)CustomizationSlot.SkinColor].UpdateCurrent(optionId);
	//	UpdateVisualGeneric();
	//}

	void ToggleColorPick_Hair(bool active)
	{
		//OptionButton(0);
		//popupHairColor.SetActive(active);
		//popupSkinColor.SetActive(false);
		skinActive = false;
	}

	void ToggleColorPick_Face(bool active)
	{
		//popupSkinColor.SetActive(active);
		//popupHairColor.SetActive(false);
		hairActive = false;
	} 
	#endregion


	void UpdateVisualGeneric()
	{

		//Debug.Log("UpdateVisualGeneric()");

		UpdateGameData();
		//customization.UpdateVisual(slotData);
		customization.UpdateVisual(savedOptions);

		//characterVisual.UpdateVisual();
	}



	#endregion

	#region PopupButtons
	//public void OptionButton(int id)
	//{

	//	if (hairActive)
	//	{
	//		Type = CustomizationSlot.HairColor;
	//	}

	//	if (skinActive)
	//	{
	//		Type = CustomizationSlot.SkinColor;
	//	}

	//	//if (Type == CustomizationSlot.HairColor)
	//	//{
	//	//	//Debug.Log($"OptionButton {Type.ToString()} new id: {id}".Colored());
	//	//	slotData[(int)CustomizationSlot.HairColor].UpdateCurrent(id);
	//	//}
	//	//else if (Type == CustomizationSlot.SkinColor)
	//	//{
	//	//	slotData[(int)CustomizationSlot.SkinColor].UpdateCurrent(id);
	//	//}

	//	UpdateVisualGeneric();
	//}
	#endregion

	void UpdateGameData()
	{
		//GameData.Instance.progress.customizations = new Customization[PossibleCustomizations.Instance.NumberOfSlots];

		//int size = PossibleCustomizations.Instance.NumberOfSlots;
		//string[] titles = new string[] { "Hair", "Face", "Torso", "Legs", "Shoes", "Hair Color", "Skin Color" };

		//GameData.Instance.Progress.customizations = new Customization[size];

		//for (int i = 0; i < size; i++)
		//{
		//	GameData.Instance.Progress.customizations[i] = new Customization
		//	{
		//		name = titles[i] + " - " + slotData[i].Id,
		//		id = slotData[i].Id
		//	};
		//}

		//Debug.Log($"Creating {size} new customization".Colored());
		// Debug.Log($"Customizations new size: {  GameData.Instance.progress.customizations.Length}".Colored());
	}


	public void OnCreated()
	{

	}
}


[Serializable]
public class SlotData
{
	public string Name;
	[SerializeField] string title = "";
	public CustomizationSlot Type;
	public int Id;
	public int TotalOptions;
	public TextLocalized display;

	#region Constructor
	public SlotData()
	{

	}

	public SlotData(int max, CustomizationSlot type, int id = 0)
	{
		TotalOptions = max;
		Id = id;
		Type = type;

		//Debug.LogError($"new SlotData {Type}".Colored("green"));
		UpdateName();
		//Name = Id + " - " + Type.ToString();
	}


	public void UpdateName()
	{
		//if (loadSDK != null)
		//{
		//    loadSDK.UpdateText_Concat("" + (Id + 1));

		//    Debug.LogError($"UpdateName {Type} \nFoundSDK".Colored());
		//}
		//else
		//{
		//    Debug.LogError($"UpdateName {Type} \nloadSDK is null".Colored("red"));
		//}

		Name = Id + " - " + Type.ToString();
		//Debug.Log(title);
		////title = "A";

		if (display != null)
		{
			display.text = title + " " + (Id + 1);
		}
	}

	public void SetTitle(string title)
	{
		this.title = title;
		//display.text = title + " " + (Id + 1);
		UpdateName();
	}

	#endregion

	public void UpdateCurrent(int id)
	{
		//Debug.Log($"UpdateCurrent {Type} \nid: {id}".Colored());

		Id = id;
		UpdateName();
	}


	public void GenerateRandomOption()
	{
		int newId;
		newId = UnityEngine.Random.Range(0, TotalOptions);

		UpdateCurrent(newId);

	}


	public void Next()
	{
		Id++;

		if (Id >= TotalOptions)
		{
			Id = 0;
		}

		UpdateCurrent(Id);
	}

	public void Previous()
	{
		Id--;

		if (Id < 0)
		{
			Id = TotalOptions - 1;
		}

		UpdateCurrent(Id);
	}

}
