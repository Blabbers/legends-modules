using Fungus;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCustomization : MonoBehaviour
{
	public bool isInGame = true;
	[ReorderableList][SerializeField] Customization[] savedOptions;
	public CustomizationVisuals customization;


	#region Editor
	[Button]
	void SetupVisuals_Editor()
	{
		int size = Enum.GetNames(typeof(CustomizationSlot)).Length;
		//slotData.Clear();

		savedOptions = new Customization[size];

		Debug.Log($"SetupVisuals_Editor() \nsize: {size}");

		for (int i = 0; i < size; i++)
		{
			//slotData.Add(new SlotData());
			//slotData[i].Type = (CustomizationSlot)i;
			//slotData[i].Id = Random.Range(0, PossibleCustomizations.Instance.GetMaxOptions(slotData[i].Type));

			//slotData[i].Name = $"{slotData[i].Id} - {slotData[i].Type.ToString()}";


			savedOptions[i] = new Customization();

			//savedOptions[i].id = Random.Range(0, PossibleCustomizations.Instance.GetMaxOptions((CustomizationSlot)i));
			savedOptions[i].id = Random.Range(0, PossibleCustomizations.Instance.GetSlotSize(i));
			savedOptions[i].name = $"{savedOptions[i].id} - {(CustomizationSlot)i}";
		}

		customization.SetUpVisuals();

	}

	[Button]
	void UpdateVisuals_Editor()
	{

		#region MyRegion
		//for (int i = 0; i < slotData.Count; i++)
		//{

		//	slotData[i].Name = $"{slotData[i].Id} - {slotData[i].Type.ToString()}";

		//}


		//UpdateVisual(slotData); 
		#endregion

		for (int i = 0; i < savedOptions.Length; i++)
		{

			savedOptions[i].name = $"{savedOptions[i].id} - {(CustomizationSlot)i}";

		}

		UpdateVisual(savedOptions);

	}

	#endregion

	private void Start()
	{
		if (isInGame)
		{
			LoadFromGameData();
			UpdateVisual(savedOptions);
		}
	}

	void LoadFromGameData()
	{

		//int size = Enum.GetNames(typeof(CustomizationSlot)).Length;
		int size = PossibleCustomizations.Instance.NumberOfSlots;

		savedOptions = GameData.Instance.Progress.customizations;

		if(savedOptions == null) savedOptions = new Customization[size];


		Debug.Log($"savedOptions.Length: {savedOptions.Length}");

		if (savedOptions.Length == 0)
		{
			savedOptions = new Customization[size];

			for (int i = 0; i < size; i++)
			{
				savedOptions[i] = new Customization();

				savedOptions[i].id = 0;
				savedOptions[i].name = $"{savedOptions[i].id} - {(CustomizationSlot)i}";
			}


		}

	}


	public void UpdateVisual(List<SlotData> slotData)
	{
		//Debug.Log("PlayerCustomization.UpdateVisual()\n".Colored("orange"));


		savedOptions = ConvertSlotData(slotData);
		customization.UpdateVisual(savedOptions);

		//this.slotData = slotData;
		//customization.UpdateVisual(slotData);
	}




	public void UpdateVisual(Customization[] savedOptions)
	{
		Debug.Log($"PlayerCustomization.UpdateVisual()\nsavedOptions.Length: {savedOptions.Length}".Colored("orange"));
		customization.UpdateVisual(savedOptions);
	}

	Customization[] ConvertSlotData(List<SlotData> slotData)
	{
		Customization[] savedOptions = new Customization[slotData.Count];

		for (int i = 0; i < slotData.Count; i++)
		{
			savedOptions[i] = new Customization();

			savedOptions[i].id = slotData[i].Id;
			savedOptions[i].name = $"{savedOptions[i].id} - {(CustomizationSlot)i}";
		}

		return savedOptions;
	}

}

[Serializable]
public class CustomizationVisuals
{
	[Header("Runtime")]
	public GameObject currentHead, currentHeadChild;
	public Texture2D currentTex;
	public Renderer HeadRenderer;

	public GameObject currentHair;
	public Renderer hairRenderer;
	public Material currentMat;

	[SerializeField] GameObject faceParent, hairParent;

	//public Customization[] configs;



	[Header("Components")]
	public Transform headParent;
	public List<Renderer> hands;
	[ReorderableList] public List<SlotReferences> Slots;


	public void SetUpVisuals()
	{
		ClearCurrent();

		bool found = false;

		if (!headParent)
		{
			GameObject temp;
			found = ObjectFinder.FindObjectByName("+ Head", out temp);

			if (found)
			{
				headParent = temp.transform;
			}

		}

		if(headParent.Find("+ Face") == null)
		{
			faceParent = new GameObject("+ Face");
			faceParent.transform.SetParent(headParent);
			faceParent.transform.localPosition = Vector3.zero;
			faceParent.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
		else
		{
			faceParent = headParent.Find("+ Face").gameObject;
		}


		if (headParent.Find("+ Hair") == null)
		{
			hairParent = new GameObject("+ Hair");
			hairParent.transform.SetParent(headParent);
			hairParent.transform.localPosition = Vector3.zero;
			hairParent.transform.localRotation = Quaternion.Euler(Vector3.zero);

		}
		else
		{
			hairParent = headParent.Find("+ Hair").gameObject;
		}


		Slots[0].Parent = hairParent.transform;
		Slots[1].Parent = faceParent.transform;
	}


	public void UpdateVisual(Customization[] savedOption)
	{
		int size, currentId;
		CustomizationSlot currentSlot;

		size = savedOption.Length;
		//configs = savedOption;

		//Debug.Log($"CustomizationVisuals.UpdateVisual()\nsavedOptions.Length: {size}".Colored("orange"));

		for (int i = 0; i < size; i++)
		{
			currentSlot = (CustomizationSlot)i;
			currentId = savedOption[i].id;


			//Debug.Log($"UpdateVisual() \n" +
			//	$"{currentSlot} | ".Colored("white") +
			//	$"currentId: {currentId}".Colored());
			ReplaceCurrent(currentSlot, currentId);
		}
	}

	#region MyRegion

	//public void UpdateVisual(List<SlotData> slotData)
	//{
	//	int size, currentId;
	//	CustomizationSlot currentSlot;


	//	size = PossibleCustomizations.Instance.NumberOfSlots;
	//	//this.Slots = slotData;

	//	//Get skins on game data
	//	for (int i = 0; i < size; i++)
	//	{
	//		currentSlot = (CustomizationSlot)i;
	//		currentId = slotData[i].Id;
	//		//currentId = GameData.Instance.progress.customizations[i].id;
	//		//currentId = 0;

	//		Debug.Log($"UpdateVisual() \n" +
	//			$"{currentSlot} | ".Colored("white") +
	//			$"currentId: {currentId}".Colored());
	//		ReplaceCurrent(currentSlot, currentId);
	//	}
	//} 
	#endregion

	#region MyRegion
	//public void UpdateVisual()
	//{
	//	int size, currentId;
	//	CustomizationSlot currentSlot;
	//	bool foundSave = true;

	//	size = PossibleCustomizations.Instance.NumberOfSlots;
	//	//configs = GameData.Instance.progress.customizations;

	//	CustomizationInitializer.instance.Initialize();

	//	//Get skins on game data
	//	for (int i = 0; i < size; i++)
	//	{
	//		currentSlot = (CustomizationSlot)i;
	//		//currentId = GameData.Instance.progress.customizations[i].id;
	//		currentId = 0;

	//		ReplaceCurrent(currentSlot, currentId);
	//	}

	//	// Debug.Log($"<PlayerAnimations> UpdateVisual\nCustomizations new size: {  GameData.Instance.progress.customizations.Length}".Colored());
	//} 
	#endregion

	public void ReplaceCurrent(CustomizationSlot slot, int id)
	{
		GameObject temp, target;
		List<GameObject> targets;
		Texture2D texture;

		int slotId;
		slotId = (int)slot;

		int groupId = 0;
		SlotType type = SlotType.GameObject;



		//if (slot == CustomizationSlot.Hair || slot == CustomizationSlot.Face || slot == CustomizationSlot.Torso || slot == CustomizationSlot.Legs)
		//{
		//	//Debug.Log($"ReplaceCurrent|  new id:{id}\nslotId: {slotId} | CustomizationSlot: {slot.ToString()} - {(int)slot}");

		//	//target = PossibleCustomizations.Instance.GetSlotObject(slot, id);

		//	target = (GameObject)PossibleCustomizations.Instance.GetSlotObject<GameObject>(type, groupId, id);


		//	if (slot == CustomizationSlot.Face)
		//	{
		//		Slots[slotId].DestroyCurrent(1);
		//		temp = MonoBehaviour.Instantiate(target, Slots[slotId].Parent);
		//		temp.transform.SetAsLastSibling();
		//	}
		//	else if (slot == CustomizationSlot.Hair)
		//	{
		//		Slots[slotId].DestroyCurrent(0);
		//		temp = MonoBehaviour.Instantiate(target, Slots[slotId].Parent);
		//		temp.transform.SetAsFirstSibling();
		//	}
		//	else
		//	{
		//		Slots[slotId].DestroyCurrent();
		//		temp = MonoBehaviour.Instantiate(target, Slots[slotId].Parent);

		//	}



		//	//Debug.Log($"Target: {target.name} \nparent: {Slots[slotId].Parent}");
		//	temp.transform.localPosition = Vector3.zero;
		//	temp.transform.localRotation = Quaternion.Euler(Vector3.zero);

		//	//HeadRenderer = currentHead.GetComponent<Renderer>();

		//	if (slot == CustomizationSlot.Face)
		//	{
		//		currentHead = temp;
		//		currentHeadChild = currentHead.transform.GetChild(0).gameObject;
		//		HeadRenderer = currentHeadChild.GetComponent<Renderer>();
		//	}


		//	if (slot == CustomizationSlot.Hair)
		//	{
		//		currentHair = temp;
		//		hairRenderer = currentHair.GetComponent<Renderer>();
		//	}


		//}
		//else
		//{

		//	if (slot == CustomizationSlot.Shoes)
		//	{
		//		targets = PossibleCustomizations.Instance.GetShoesData(id);


		//		//Delete left
		//		Slots[slotId].DestroyAllChildren();
		//		temp = MonoBehaviour.Instantiate(targets[0], Slots[slotId].Parent);

		//		//Delete right
		//		Slots[slotId + 1].DestroyAllChildren();
		//		temp = MonoBehaviour.Instantiate(targets[1], Slots[slotId + 1].Parent);

		//	}
		//	else if (slot == CustomizationSlot.SkinColor)

		//	{
		//		currentTex = PossibleCustomizations.Instance.GetSkinColor(id);

		//		if (Application.isPlaying)
		//		{
		//			hands[0].material.mainTexture = currentTex;
		//			hands[1].material.mainTexture = currentTex;
		//		}
		//		else
		//		{
		//			hands[0].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = currentTex;
		//			hands[1].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = currentTex;

		//		}


		//	}
		//	else if (slot == CustomizationSlot.HairColor)
		//	{
		//		currentMat = PossibleCustomizations.Instance.GetHairColor(id);
		//	}

		//}

		//if (HeadRenderer != null)
		//{
		//	if(Application.isPlaying) HeadRenderer.material.mainTexture = currentTex;
		//	else HeadRenderer.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = currentTex;

		//}

		//if (hairRenderer != null)
		//{
		//	if (Application.isPlaying) hairRenderer.material = currentMat;
		//	else hairRenderer.material = currentMat;
		//}

		//HeadRenderer.material.mainTexture = currentTex;
	}


	public void ClearCurrent()
	{

		if(Slots != null)
		{
			for (int i = 0; i < Slots.Count; i++)
			{
				Slots[i].DestroyAllChildren();
			}
		}

		currentHead = null;
		currentHeadChild = null;
		currentTex = null;
		HeadRenderer = null;
		currentHair = null;
		hairRenderer = null;
		currentMat = null;
		faceParent = null;
		hairParent = null;

		if (headParent == null) return;

		//GameObject temp;
		//int size = headParent.childCount;

		//for (int i = 0; i < headParent.childCount; i++)
		//{
		//	temp = headParent.GetChild(i).gameObject;
		//	if (temp == null) return;

		//	if (Application.isPlaying)
		//	{
		//		MonoBehaviour.Destroy(temp);
		//	}
		//	else
		//	{
		//		MonoBehaviour.DestroyImmediate(temp);
		//	}

		//	i--;
		//	i = Mathf.Clamp(i, 0, headParent.childCount);

		//}
	}

}

[Serializable]
public class SlotReferences
{
	public string Name;
	public Transform Parent;
	public GameObject Current;

	public void GetCurrent()
	{
		Current = Parent.GetChild(0).gameObject;
	}

	public void DestroyCurrent(int id = 0)
	{
		if (Parent == null) return;
		if (Parent.childCount <= 0) return;

		Current = Parent.GetChild(0).gameObject;

		if (Current == null) return;

		if (Application.isPlaying) MonoBehaviour.Destroy(Current);
		else MonoBehaviour.DestroyImmediate(Current);

	}



	public void DestroyAllChildren()
	{
		int limiter = 0;
		int size;
		GameObject temp;

		//Debug.Log($"{Name}");
		if (Parent == null) return;

		size = Parent.childCount;

		if (size <= 0) return;

		for (int i = 0; i < Parent.childCount; i++)
		{
			temp = Parent.GetChild(i).gameObject;
			if (temp == null) return;

			if (Application.isPlaying)
			{
				MonoBehaviour.Destroy(temp);
			}
			else
			{
				MonoBehaviour.DestroyImmediate(temp);
			}

			i--;
			i = Mathf.Clamp(i, 0, Parent.childCount);

		}
	}

}
