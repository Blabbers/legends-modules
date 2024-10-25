﻿using Blabbers.Game00;
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
using System.Xml.Linq;

public class UI_CustomizationScreen : MonoBehaviour, ISingleton
{

	#region Variables

	//[Foldout("Runtime")] public CustomizationSlot Type;
	[Foldout("Runtime")] public bool isNewGame = false;
	[Foldout("Runtime")][SerializeField] float targetRotation;
	bool hairActive, skinActive;

	//[Foldout("Runtime")][ReorderableList] public List<SlotData> slotData = new List<SlotData>();

	//Make this a one time input
	[Foldout("Runtime")] Customization[] savedOptions;
	[Foldout("Runtime")][ReorderableList][SerializeField] CustomizationData[] configuredOptions;

	//[Foldout("Runtime")][ReorderableList][SerializeField] Customization[] configuredOptions;

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

		selectors.Sort((x, y) => x.GroupIds[0].CompareTo(y.GroupIds[0]));

		//objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));


		for (int i = 0; i < selectors.Count; i++)
		{
			selectors[i].OnOptionChanged = null;

			var lastId = PossibleCustomizations.Instance.GetSlotSize(selectors[i].GroupIds[0]) - 1;
			var name = PossibleCustomizations.Instance.GetName(selectors[i].GroupIds[0]);

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

			var lastId = PossibleCustomizations.Instance.GetSlotSize(selectors[i].GroupIds[0]) - 1;
			var name = PossibleCustomizations.Instance.GetName(selectors[i].GroupIds[0]);
			var selected = savedOptions[i].id;

			selectors[i].SetupSelector(lastId, name, selected);
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
		GenerateConfigureOptions();

		GetSDKTexts();
		RefreshSDKTexts();

		targetRotation = 0;
		SetRotation(targetRotation);

		hairActive = skinActive = false;

		if(customization) customization.UpdateVisual(configuredOptions);

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
		int size = PossibleCustomizations.Instance.NumberOfSlots;
		savedOptions = new Customization[size];

		if(GameData.Instance.Progress == null || GameData.Instance.Progress.customizations == null)
		{
			for (int i = 0; i < size; i++)
			{
				savedOptions[i] = new Customization();
				savedOptions[i].id = 0;

				savedOptions[i].name = $"{savedOptions[i].id + 1} - {PossibleCustomizations.Instance.GetName(i)}";
			}

			return;
		}


		if (GameData.Instance.Progress.customizations.Length == size)
		{
			

			for (int i = 0; i < size; i++)
			{
				savedOptions[i] = new Customization();
				//savedOptions[i].name = GameData.Instance.Progress.customizations[i].name;
				savedOptions[i].id = GameData.Instance.Progress.customizations[i].id;

				savedOptions[i].name = $"{savedOptions[i].id + 1} - {PossibleCustomizations.Instance.GetName(i)}";

			}

		}
		else
		{
			for (int i = 0; i < size; i++)
			{
				savedOptions[i] = new Customization();
				savedOptions[i].id = 0;

				savedOptions[i].name = $"{savedOptions[i].id + 1} - {PossibleCustomizations.Instance.GetName(i)}";
			}

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

	void GenerateConfigureOptions()
	{

		int size = savedOptions.Length;

		if(configuredOptions == null || configuredOptions.Length == 0)
		{

			configuredOptions = new CustomizationData[size];

			for (int i = 0; i < savedOptions.Length; i++)
			{
				configuredOptions[i] = new CustomizationData(savedOptions[i], PossibleCustomizations.Instance.GetSlotList(i));
			}
		}


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


	void HandleSelectionUpdate(List<int> groupId, int selectedId)
	{
		string display ="";

		for (int i = 0; i < groupId.Count; i++)
		{
			display = display + $"{groupId[i]} ";
		}

		//Debug.Log($"HandleSelectionUpdate()\ngroupId: {display} |selectedId: {selectedId}");

		for (int i = 0; i < groupId.Count; i++)
		{
			savedOptions[groupId[i]].id = selectedId;
			savedOptions[groupId[i]].name = $"{savedOptions[groupId[i]].id + 1} - {PossibleCustomizations.Instance.GetName(groupId[i])}";
		}

		GenerateConfigureOptions();
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
		if (!playerTransform) return;
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

		//foreach (var item in configuredOptions)
		//{
		//	//item.GenerateRandomOption();

		//	item.GenerateRandomOption();

		//}

		foreach(var item in selectors)
		{
			item.GenerateRandomOption();
		}

		for (int i = 0; i < selectors.Count; i++)
		{
			for (int j = 0; j < selectors[i].GroupIds.Count; j++)
			{
				var id = selectors[i].GroupIds[j];
				configuredOptions[id].RefreshId(selectors[i].SelectedId);
			}
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

		//Type = (CustomizationSlot)id;
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
	//public void ToggleColorPick(int id)
	//{
	//	if (id == 0)
	//	{
	//		hairActive = !hairActive;
	//		ToggleColorPick_Hair(hairActive);
	//		Type = CustomizationSlot.HairColor;
	//	}
	//	else
	//	{
	//		skinActive = !skinActive;
	//		ToggleColorPick_Face(skinActive);
	//		Type = CustomizationSlot.SkinColor;
	//	}

	//}


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
		if (customization) customization.UpdateVisual(configuredOptions);
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


		int size = configuredOptions.Length;
		savedOptions = new Customization[size];

		for (int i = 0; i < savedOptions.Length; i++)
		{
			savedOptions[i] = configuredOptions[i].SavedData;
			//configuredOptions[i] = new CustomizationData(savedOptions[i], PossibleCustomizations.Instance.GetSlotList(i));
		}
		GameData.Instance.Progress.customizations = new Customization[size];
		for (int i = 0; i < size; i++)
		{
			GameData.Instance.Progress.customizations[i] = savedOptions[i];
		}



	}


	public void OnCreated()
	{

	}
}

