using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomizationSelector : MonoBehaviour
{

	public Action<int, int> OnOptionChanged;

	[SerializeField] string groupName;
	[SerializeField] int groupId;


	[Space(10)]
	[SerializeField] int selectedId;
	[SerializeField] int lastId;
	[SerializeField] TextLocalized title;


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


	public void SetupSelector(int lastId, string name)
	{
		this.lastId = lastId;
		groupName = name;
	}

	public void Next()
	{
		selectedId++;

		if(selectedId > lastId)
		{
			selectedId = 0;
		}

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

		Debug.Log($"{groupName} ".Colored("orange") +
	$"\nPrevious() groupId: {groupId} |selectedId: {selectedId}");
		OnOptionChanged?.Invoke(groupId, selectedId);
	}

	public void SelectOption(int id)
	{
		selectedId = id;
		OnOptionChanged?.Invoke(groupId, selectedId);
	}


}
