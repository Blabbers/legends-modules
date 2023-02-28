using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomizationSelector : MonoBehaviour
{

	[SerializeField] string groupName;
	[SerializeField] int groupId;

	[SerializeField] bool hasTitle = true;
	[SerializeField] string languageKey;

	public Action<int, int> OnOptionChanged;
	[Foldout("Runtime")][SerializeField] int selectedId;
	[Foldout("Runtime")][SerializeField] int lastId;
	[Foldout("Runtime")][SerializeField] string displayTitle;

	[Foldout("Components")][SerializeField] TextLocalized title;


	#region Get/Set

	public int LastId
	{
		get { return lastId; }
		set { lastId = value; }
	}

	public int SelectedId
	{
		get { return selectedId; }
	}


	public int GroupId
	{
		get { return groupId; }
	}
	public string GroupName
	{
		get { return groupName; }
	}

	public string DisplayTitle { 
		get { return displayTitle; }
		set { displayTitle = value; }
	}

	public string LanguageKey {
		get { return languageKey; }
	}

	public bool HasTitle
	{
		get { return hasTitle;}
	}





	#endregion



	#region Editor

	[Button]
	void SetupSelectors_Editor()
	{
		var lastId = PossibleCustomizations.Instance.GetSlotSize(groupId);
		var name = PossibleCustomizations.Instance.GetName(groupId);


		SetupSelector(lastId, name);
	}

	#endregion


	public void SetupSelector(int lastId, string name, int selectedId = 0)
	{
		this.lastId = lastId;
		this.selectedId = selectedId;
		groupName = name;

		if (string.IsNullOrEmpty(languageKey)){
			hasTitle = false;
		}
	}

	public void Next()
	{
		selectedId++;

		if(selectedId > lastId)
		{
			selectedId = 0;
		}

		UpdateDisplay();

		Debug.Log($"{groupName} ".Colored() +
			$"\nNext() groupId: {groupId} |selectedId: {selectedId}");
		OnOptionChanged?.Invoke(groupId, selectedId);
	}

	public void Previous()
	{
		selectedId--;

		if (selectedId < 0)
		{
			selectedId = lastId;
		}

		UpdateDisplay();

		Debug.Log($"{groupName} ".Colored("orange") +
	$"\nPrevious() groupId: {groupId} |selectedId: {selectedId}");
		OnOptionChanged?.Invoke(groupId, selectedId);
	}

	public void UpdateDisplay()
	{
		//Debug.Log($"UpdateDisplay() \n groupId: {groupId} | selected: {selectedId}");
		title.text = $"{displayTitle} {selectedId + 1}";
	}



	public void SelectOption(int id)
	{
		selectedId = id;
		OnOptionChanged?.Invoke(groupId, selectedId);
	}


}
