using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Animancer.Validate;


[CreateAssetMenu]
public class PossibleCustomizations : ScriptableObject
{
    #region Instance
    private static PossibleCustomizations _instance = null;
    public static PossibleCustomizations Instance
    {
        get
        {
            if (!_instance) _instance = Resources.Load<PossibleCustomizations>("PossibleCustomizations");
            return _instance;
        }
    }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init()
	{
		_instance = null;
	}

	#endregion

	public int NumberOfSlots = 7;

    //[Expandable]
    //public GenericSOList test;

	public List<GenericSOList> possibleCustomizations;

	[Header("Colors")]
    public List<Texture2D> SkinColors = new List<Texture2D>();
    public List<Material> HairColors = new List<Material>();

    [Header("Body Parts")]
    public List<GameObject> HairStyles = new List<GameObject>();
    public List<GameObject> Faces = new List<GameObject>();
    public List<GameObject> Torsos = new List<GameObject>();
    public List<GameObject> Legs = new List<GameObject>();
    public List<GameObject> ShoesLeft = new List<GameObject>();
    public List<GameObject> ShoesRight = new List<GameObject>();


    //This can be removed, it is editor only
    public int GetMaxOptions(CustomizationSlot slot)
    {
		if (slot == CustomizationSlot.Hair)
		{
            return HairStyles.Count;
		}
		else if (slot == CustomizationSlot.Face)
		{
			return Faces.Count;
		}
		else if (slot == CustomizationSlot.Torso)
		{
			return Torsos.Count;
		}
		else if (slot == CustomizationSlot.Legs)
		{
			return Legs.Count;
		}
		else if (slot == CustomizationSlot.SkinColor)
		{
			return SkinColors.Count;
		}
		else if (slot == CustomizationSlot.HairColor)
		{
			return HairColors.Count;
		}
		else
		{
			return ShoesLeft.Count;
		}

	}

	public int GetSlotSize(int groupId)
	{
		GenericSOList list = possibleCustomizations[groupId];


		//
		//if(list is GameObjectList)
		//{

		//}

		if(list.GetType() == typeof(GameObjectList))
		{
			var objList = (GameObjectList)list;
			return objList.gameObjects.Count;
		}
		else if (list.GetType() == typeof(MaterialList))
		{
			var objList = (MaterialList)list;
			return objList.materials.Count;
		}
		else
		{
			var objList = (TextureList)list;
			return objList.textures.Count;
		}

		//if (type == SlotType.GameObject)
		//{
		//	var objList = (GameObjectList)list;
		//	return objList.gameObjects.Count;
		//}
		//else if (type == SlotType.Material)
		//{
		//	var objList = (MaterialList)list;
		//	return objList.materials.Count;
		//}
		//else
		//{
		//	var objList = (TextureList)list;
		//	return objList.textures.Count;
		//}
	}

	public string GetName(int groupId)
	{
		GenericSOList list = possibleCustomizations[groupId];
		return list.name;

	}

	//Would need to find a way to make this generic, and change CustomizationSlot for an int
	public SlotData GetSlotData(CustomizationSlot slot)
    {

       // Debug.Log($"GetSlotData: {slot}");

        SlotData data;

        if (slot == CustomizationSlot.Hair)
        {
            data = new SlotData(HairStyles.Count, CustomizationSlot.Hair);
        }
        else if (slot == CustomizationSlot.Face)
        {
            data = new SlotData(Faces.Count, CustomizationSlot.Face);
        }
        else if (slot == CustomizationSlot.Torso)
        {
            data = new SlotData(Torsos.Count, CustomizationSlot.Torso);
        }
        else if (slot == CustomizationSlot.Legs)
        {
            data = new SlotData(Legs.Count, CustomizationSlot.Legs);
        }
        else if (slot == CustomizationSlot.SkinColor)
        {
            data = new SlotData(3, CustomizationSlot.SkinColor);
        }
        else if (slot == CustomizationSlot.HairColor)
        {
            data = new SlotData(4, CustomizationSlot.HairColor);
        }
        else
        {
            data = new SlotData(ShoesLeft.Count, CustomizationSlot.Shoes);
        }
        return data;
    }

	//Would need to find a way to make this generic, and change CustomizationSlot for an int
	public GameObject GetSlotObject(CustomizationSlot slot, int id)
	{
        //Debug.Log($"GetSlotObject| CustomizationSlot: {slot.ToString()} - {(int)id}");

        //GenericSOList list = possibleCustomizations[0];
        //GameObjectList objList = (GameObjectList)list;


        //return objList.gameObjects[id];


		if (slot == CustomizationSlot.Hair)
		{
			return HairStyles[id];
             //return possibleCustomizations[3];
		}
		else if (slot == CustomizationSlot.Face)
		{
			return Faces[id];
		}
		else if (slot == CustomizationSlot.Torso)
		{
			return Torsos[id];
		}
		else if (slot == CustomizationSlot.Legs)
		{
			return Legs[id];
		}

		return null;
	}

	public T GetSlotObject<T>(SlotType type, int groupId ,int id)
	{
		GenericSOList list = possibleCustomizations[groupId];



        if(type == SlotType.GameObject)
        {
			var objList = (GameObjectList)list;
			var value = objList.gameObjects[id];

			return (T)Convert.ChangeType(value, typeof(T));
		}
		else if (type == SlotType.Material)
		{
			var objList = (MaterialList)list;
			var value = objList.materials[id];

			return (T)Convert.ChangeType(value, typeof(T));
		}
		else
		{
			var objList = (TextureList)list;
			var value = objList.textures[id];

			return (T)Convert.ChangeType(value, typeof(T));
		}
	}


	public Texture2D GetSkinColor(int id)
    {
        return SkinColors[id];
    }

    public Material GetHairColor(int id)
    {
        return HairColors[id];
    }

    public List<GameObject> GetShoesData(int id)
    {
        List<GameObject> shoes = new List<GameObject>();

        shoes.Add(ShoesLeft[id]);
        shoes.Add(ShoesRight[id]);

        return shoes;
    }

}


public enum CustomizationSlot
{
    Hair, Face, Torso, Legs, Shoes, HairColor, SkinColor
}

public enum SlotType { 
    GameObject, Material, Texture
}



