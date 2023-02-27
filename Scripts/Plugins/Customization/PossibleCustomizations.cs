using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    [Expandable]
    public GenericSOList test;

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

		if (slot == CustomizationSlot.Hair)
		{
			return HairStyles[id];
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


