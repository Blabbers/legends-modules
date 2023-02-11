using Blabbers.Game00;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsInitializer : MonoBehaviour
{

    private static StatsInitializer _instance = null;
    public static StatsInitializer instance
    {
        get
        {

            Debug.Log("<StatsInitializer> get instance");

            if (_instance == null)
            {
                if (GameObject.Find("_StatsInitializer"))
                {
                    _instance = GameObject.Find("_StatsInitializer").GetComponent<StatsInitializer>();
                }
                else
                {
                    GameObject aux = new GameObject();
                    aux.name = "_StatsInitializer";
                    _instance = aux.AddComponent<StatsInitializer>();
                    currentAirplane = AirplaneConfigsData.Instance.airplanes[0];


				}
                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }


    //[Tooltip("Should be empty on editor")]
    //[HideInInspector]public List<SlotData> slotData = new List<SlotData>();
    public static bool firstTimeStatsScreen = true;
    public static int currentAirplaneId = 0;
    public static AirplaneData currentAirplane;



	public void Initialize()
    {
        //GetDataFromSave();
    }

    public void ShowStatsTutorial()
    {
        firstTimeStatsScreen = false;
	}

    public void UpdateCurrentAirplane(AirplaneData current, int id)
    {
        currentAirplane = current;
        currentAirplaneId = id;
	}

    public AirplaneData GetCurrentAirplane()
    {
        return currentAirplane;
	}


    #region MyRegion
    //void GetDataFromSave()
    //{

    //    //Debug.LogError("Initialize".Colored("green"));

    //    int size = 5;
    //    CustomizationSlot current;
    //    slotData = new List<SlotData>();


    //    if (GameData.Instance.progress == null)
    //    {
    //        size = 0;
    //    }
    //    else if (GameData.Instance.progress.customizations == null)
    //    {
    //        size = 0;
    //    }
    //    else
    //    {

    //        size = GameData.Instance.progress.customizations.Length;
    //    }



    //    if (size <= 0)
    //    {
    //        //Debug.Log("No saved data found".Colored("red"));

    //        size = PossibleCustomizations.Instance.NumberOfSlots;



    //        for (int i = 0; i < size; i++)
    //        {
    //            current = (CustomizationSlot)i;
    //            slotData.Add(PossibleCustomizations.Instance.GetSlotData(current));
    //            slotData[i].UpdateCurrent(0);
    //        }

    //    }
    //    else
    //    {
    //        //Debug.Log("Found Saved Data".Colored("red"));

    //        for (int i = 0; i < size; i++)
    //        {
    //            current = (CustomizationSlot)i;
    //            slotData.Add(PossibleCustomizations.Instance.GetSlotData(current));
    //            slotData[i].UpdateCurrent(GameData.Instance.progress.customizations[i].id);
    //        }

    //    }

    //    UpdateGameData();
    //}

    //void UpdateGameData()
    //{
    //    GameData.Instance.progress.customizations = new Customization[PossibleCustomizations.Instance.NumberOfSlots];

    //    int size = PossibleCustomizations.Instance.NumberOfSlots;
    //    string[] titles = new string[] { "Hair", "Face", "Torso", "Legs", "Shoes", "Hair Color", "Skin Color" };

    //    for (int i = 0; i < size; i++)
    //    {
    //        GameData.Instance.progress.customizations[i] = new Customization
    //        {
    //            name = titles[i] + " - " + slotData[i].Id,
    //            id = slotData[i].Id
    //        };
    //    }

    //    //Debug.Log($"Creating {size} new customization".Colored());
    //    //Debug.Log($"Customizations new size: {  GameData.Instance.progress.customizations.Length}".Colored());
    //}

    #endregion
    public void OnCreated()
    {

    }


}
