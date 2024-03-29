using BeauRoutine;
using Fungus;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PlayerCustomization : MonoBehaviour
{
	#region Variables
	public bool isInGame = true;
	public bool scaleOverride = false;
	[SerializeField] Vector3 scale;

	//[ReorderableList][SerializeField] 
	Customization[] savedOptions;

	//[ReorderableList][SerializeField]
	CustomizationData[] configuredOptions;

	[ReorderableList][SerializeField] CustomizationParentData[] parentData;

	public UnityEvent OnCustomizationFinsihed;

	//[ReorderableList][SerializeField] SlotData[] slots;
	//public CustomizationVisuals customization;

	#endregion

	#region Editor
	[Button]
	void SetupBaseData()
	{
		int size = PossibleCustomizations.Instance.NumberOfSlots;
		savedOptions = new Customization[size];

		for (int i = 0; i < size; i++)
		{
			savedOptions[i] = new Customization();
			savedOptions[i].name = $"{savedOptions[i].id + 1} - {PossibleCustomizations.Instance.GetName(i)}";

		}

		GenerateConfigureOptions();

		Transform[] parents = new Transform[size];

		for (int i = 0; i < size; i++)
		{
			parents[i] = null;
		}


		for (int i = 0; i < parentData.Length; i++)
		{
			parents[i] = parentData[i].parent;
		}


		parentData = new CustomizationParentData[size];


		for (int i = 0; i < size; i++)
		{
			parentData[i] = new CustomizationParentData(configuredOptions[i].ListReference);
			if (parents[i] != null) parentData[i].parent = parents[i];
		}

	} 
	#endregion

	private void Start()
	{
		if (isInGame)
		{
			LoadFromGameData();
			GenerateConfigureOptions();
			//UpdateVisual(savedOptions);
			UpdateVisual(configuredOptions);

			if (scaleOverride)
			{
				var child = this.transform.GetChild(0);
				child.localScale = scale;
			}

			OnCustomizationFinsihed?.Invoke();
		}
	}

	void LoadFromGameData()
	{

		int size = PossibleCustomizations.Instance.NumberOfSlots;
		savedOptions = new Customization[size];


		if (GameData.Instance.Progress == null || GameData.Instance.Progress.customizations == null)
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
			savedOptions = GameData.Instance.Progress.customizations;
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



		#region OLD
		//savedOptions = GameData.Instance.Progress.customizations;
		//if (savedOptions == null) savedOptions = new Customization[size];
		//if(GameData.Instance.Progress.customizations == null)
		//{
		//	Debug.Log("LoadFromGameData() customizations is null!");
		//}
		//else
		//{
		//	var customizations = "";
		//	for (int i = 0; i < GameData.Instance.Progress.customizations.Length; i++)
		//	{
		//		if (GameData.Instance.Progress.customizations[i] != null)
		//		{
		//			customizations = $"{customizations}{GameData.Instance.Progress.customizations[i]}\n";
		//		}
		//		else
		//		{
		//			customizations = customizations + "null" + "\n";
		//		}

		//	}

		//	Debug.Log($"LoadFromGameData(isInGame = {isInGame}) " +
		//		$"\nsavedOptions.Length: {savedOptions.Length} | customizations.Length: {GameData.Instance.Progress.customizations.Length}" +
		//		$"\n{customizations}");

		//}


		//if (savedOptions.Length == 0 || savedOptions[0] == null)
		//{
		//	Debug.Log($"LoadFromGameData() savedOptions.Length == 0");


		//	savedOptions = new Customization[size];

		//	for (int i = 0; i < size; i++)
		//	{
		//		savedOptions[i] = new Customization();

		//		savedOptions[i].id = 0;
		//		savedOptions[i].name = $"{savedOptions[i].id} - {PossibleCustomizations.Instance.GetSlotList(i).name}";
		//	}


		//} 
		#endregion


		//Debug
		//var saved = "";
		//for (int i = 0; i < savedOptions.Length; i++)
		//{
		//	if (savedOptions[i] != null)
		//	{
		//		saved = $"{saved}{savedOptions[i]}\n";
		//	}
		//	else
		//	{
		//		saved = saved + "null" + "\n";
		//	}

		//}



		//Debug.Log($"LoadFromGameData(isInGame = {isInGame}) " +
		//	$"\nsavedOptions.Length: {savedOptions.Length} | customizations.Length: {GameData.Instance.Progress.customizations.Length}" +
		//	$"\n{saved}");

	}


	void GenerateConfigureOptions()
	{
		//Debug.Log($"GenerateConfigureOptions()" +
		//	$"\nis savedOptions null? {savedOptions == null} | is PossibleCustomizations null? {PossibleCustomizations.Instance == null}\n-");


	

		int size = savedOptions.Length;
		configuredOptions = new CustomizationData[size];

		//Debug.Log($"GenerateConfigureOptions()" +
		//	$"\nsavedOptions.Length: {savedOptions.Length} |\nconfiguredOptions.Length {configuredOptions.Length}");

		//Debug.Log($"GenerateConfigureOptions()\nb4 loop \n-");

		for (int i = 0; i < savedOptions.Length; i++)
		{
			//Debug.Log($"GenerateConfigureOptions()" +
			//	$"\nsavedOptions[{i}] null? {savedOptions[i] == null} " +
			//	$"\nGetSlotList({i}) null? {PossibleCustomizations.Instance.GetSlotList(i) == null}");


			//Debug.Log($"GenerateConfigureOptions() {savedOptions[i].name} | {PossibleCustomizations.Instance.GetSlotList(i)}");

			configuredOptions[i] = new CustomizationData(savedOptions[i], PossibleCustomizations.Instance.GetSlotList(i));
		}

	}
	public void UpdateVisual(CustomizationData[] configuredOptions)
	{
		this.configuredOptions = configuredOptions;


		for (int i = 0; i < parentData.Length; i++)
		{
			//Debug.Log($"\nSetup {i} - {parentData[i].listRef}");

			if (parentData[i].listRef is GameObjectList)
			{
				var id = this.configuredOptions[i].Id;
				parentData[i].Setup(id);
			}


		}


		for (int i = 0; i < parentData.Length; i++)
		{
			//Debug.Log($"\nSetup {i} - {parentData[i].listRef}");

			if(parentData[i].oldComponent != null)
			{
				Destroy(parentData[i].oldComponent);
			}
		}



		//delay
		//Routine.Start(Routine.Delay(() => PostDelay(), 0.05f));

		//PostDelay();

		for (int i = 0; i < parentData.Length; i++)
		{
			//Debug.Log($"\nSetup {i} - {parentData[i].listRef}");

			if (parentData[i].listRef is GameObjectList)
			{

			}
			else
			{
				var id = this.configuredOptions[i].Id;
				parentData[i].Setup(id);
			}


		}


	}

	public CustomizationParentData[] GetCustomizationData()
	{
		return parentData;
	}
 
	void PostDelay()
	{
		for (int i = 0; i < parentData.Length; i++)
		{
			//Debug.Log($"\nSetup {i} - {parentData[i].listRef}");

			if (parentData[i].listRef is GameObjectList)
			{

			}
			else
			{
				var id = this.configuredOptions[i].Id;
				parentData[i].Setup(id);
			}


		}
	}


	#region MyRegion
	//Customization[] ConvertSlotData(List<SlotData> slotData)
	//{
	//	Customization[] savedOptions = new Customization[slotData.Count];

	//	for (int i = 0; i < slotData.Count; i++)
	//	{
	//		savedOptions[i] = new Customization();

	//		savedOptions[i].id = slotData[i].Id;
	//		savedOptions[i].name = $"{savedOptions[i].id} - {(CustomizationSlot)i}";
	//	}

	//	return savedOptions;
	//} 
	#endregion

	#region MyRegion

	//public void UpdateVisual(List<SlotData> slotData)
	//{
	//	//Debug.Log("PlayerCustomization.UpdateVisual()\n".Colored("orange"));


	//	savedOptions = ConvertSlotData(slotData);
	//	customization.UpdateVisual(savedOptions);

	//	//this.slotData = slotData;
	//	//customization.UpdateVisual(slotData);
	//}

	#endregion

}

[Serializable]
public class CustomizationData
{
	[SerializeField] string name;
	[SerializeField] GenericSOList listReference;
	[SerializeField] Customization savedData;


	public CustomizationData(Customization savedData, GenericSOList listReference)
	{
		this.savedData = savedData;
		this.listReference = listReference;

		name = savedData.name;
	}

	public GenericSOList ListReference { get { return listReference; } }


	public Customization SavedData { get { return savedData; } }

	public int Id
	{
		get { return savedData.id; }
	}

	public void RefreshId(int id)
	{
		savedData.id = id;
	}
}

[Serializable]
public class CustomizationParentData
{
	public string name;
	public Transform parent;
	public GenericSOList listRef;

	[Header("Runtime")]
	public Transform tempRef;
	public GameObject oldComponent;
	

	public CustomizationParentData(GenericSOList listRef)
	{
		this.listRef = listRef;
		name = listRef.name;
	}

	public void Setup(int selectedId)
	{
		if(parent == null)
		{
			Debug.Log($"Missing parent for Setup: {listRef.name} ");
			return;
		}

		List<Renderer> renderers = new List<Renderer>();

	
		if (listRef is GameObjectList)
		{

	//		Debug.Log($"GameObjectList | selectedId: {selectedId} " +
	//$"\nlistRef: {listRef.name} | {parent.GetChild(0)}".Colored());

			var obj = ((GameObjectList)listRef).gameObjects[selectedId];

			if (parent.childCount > 0)
			{
				oldComponent = parent.GetChild(0).gameObject;
				if (oldComponent != null) oldComponent.transform.parent = null;
			}
		
			//DestroyAllChildren(parent);
			var temp = GameObject.Instantiate(obj, parent: this.parent);
			temp.transform.localPosition = Vector3.zero;
			temp.transform.localRotation = Quaternion.Euler(Vector3.zero);

			tempRef = temp.transform;
		}
		if (listRef is MaterialList)
		{
			//Debug.Log($"MaterialList | selectedId: {selectedId} " +
			//	$"\nlistRef: {listRef.name} | {parent.GetChild(0)}".Colored());

			var mat = ((MaterialList)listRef).materials[selectedId];
			//parent.GetComponent<Renderer>().material = mat;



			if (parent.GetChild(0).GetComponent<Renderer>() != null)
			{
				parent.GetChild(0).GetComponent<Renderer>().material = mat;
				tempRef = parent.GetChild(0);
			}
			else
			{
				Debug.Log($"parent.GetChild(0) is null".Colored("orange"));
			}

			//renderers = GetAllRenderers(parent);

			//Debug.Log($"Found renderer {renderers[0].name} | {renderers[0].materials[0].name}");

			//for (int i = 0; i < renderers.Count; i++)
			//{
			//	renderers[i].material = mat;
			//}

		}
		if (listRef is TextureList)
		{
			var mat = ((TextureList)listRef).textures[selectedId];

			if (parent.GetChild(0) == null) return;
			

			if (parent.GetChild(0).GetComponent<Renderer>() != null)
			{
				parent.GetChild(0).GetComponent<Renderer>().material.mainTexture = mat;

				tempRef = parent.GetChild(0);
				return;
			}

			//Debug.Log($"TextureList \nlistRef: {listRef.name} | {parent.GetChild(0).GetChild(0)}");
			parent.GetChild(0).GetChild(0).GetComponent<Renderer>().material.mainTexture = mat;

			tempRef = parent.GetChild(0).GetChild(0);


			//parent.GetChild(0).GetComponent<Renderer>().material.mainTexture = mat;
		}
	}

	List<Renderer> GetAllRenderers(Transform parent)
	{
		var renderers = new List<Renderer>();

		if (parent.GetChild(0).GetComponent<Renderer>()!= null)
		{
			renderers.Add(parent.GetChild(0).GetComponent<Renderer>());
		
			return renderers;
		}

		renderers.Add(parent.GetChild(0).GetChild(0).GetComponent<Renderer>());
		return renderers;
	}

	void DestroyAllChildren(Transform parent)
	{
		int limiter = 0;
		int size;
		GameObject temp;

		//Debug.Log($"{Name}");
		if (parent == null) return;

		//size = parent.childCount;


		for (int i = 0; i < parent.childCount; i++)
		{
			temp = parent.GetChild(i).gameObject;
			if (temp == null) return;

			if (Application.isPlaying)
			{
				MonoBehaviour.DestroyImmediate(temp);
			}
			else
			{
				MonoBehaviour.DestroyImmediate(temp);
			}

			i--;
			limiter++;

			if (limiter > 20) return;

			if (i < 0) i = 0;
			//i = Mathf.Clamp(i, 0, parent.childCount);

		}
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


[Serializable]
public class SlotData
{
	public string Name;
	public Transform Parent;
	public GameObject Current;
	public Customization savedData;

}







//[Serializable]
//public class SlotData
//{
//	public string Name;
//	[SerializeField] string title = "";
//	public CustomizationSlot Type;
//	public int Id;
//	public int TotalOptions;
//	public TextLocalized display;

//	#region Constructor
//	public SlotData()
//	{

//	}

//	public SlotData(int max, CustomizationSlot type, int id = 0)
//	{
//		TotalOptions = max;
//		Id = id;
//		Type = type;

//		//Debug.LogError($"new SlotData {Type}".Colored("green"));
//		UpdateName();
//		//Name = Id + " - " + Type.ToString();
//	}


//	public void UpdateName()
//	{
//		//if (loadSDK != null)
//		//{
//		//    loadSDK.UpdateText_Concat("" + (Id + 1));

//		//    Debug.LogError($"UpdateName {Type} \nFoundSDK".Colored());
//		//}
//		//else
//		//{
//		//    Debug.LogError($"UpdateName {Type} \nloadSDK is null".Colored("red"));
//		//}

//		Name = Id + " - " + Type.ToString();
//		//Debug.Log(title);
//		////title = "A";

//		if (display != null)
//		{
//			display.text = title + " " + (Id + 1);
//		}
//	}

//	public void SetTitle(string title)
//	{
//		this.title = title;
//		//display.text = title + " " + (Id + 1);
//		UpdateName();
//	}

//	#endregion

//	public void UpdateCurrent(int id)
//	{
//		//Debug.Log($"UpdateCurrent {Type} \nid: {id}".Colored());

//		Id = id;
//		UpdateName();
//	}


//	public void GenerateRandomOption()
//	{
//		int newId;
//		newId = UnityEngine.Random.Range(0, TotalOptions);

//		UpdateCurrent(newId);

//	}


//	public void Next()
//	{
//		Id++;

//		if (Id >= TotalOptions)
//		{
//			Id = 0;
//		}

//		UpdateCurrent(Id);
//	}

//	public void Previous()
//	{
//		Id--;

//		if (Id < 0)
//		{
//			Id = TotalOptions - 1;
//		}

//		UpdateCurrent(Id);
//	}

//}
