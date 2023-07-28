using Blabbers.Game00;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Clickblock : MonoBehaviour, ISingleton
{

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init()
	{
		_instance = null;
	}


	#region Instance
	private static UI_Clickblock _instance = null;
	public static UI_Clickblock Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = Instantiate(Resources.Load<UI_Clickblock>("UI/--Popup--Clickblock"));
				_instance.gameObject.name = "--Popup--Clickblock";
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	#endregion


	public void ToggleClickBlock(bool active)
	{
		Debug.Log($"ToggleClickBlock: {active}");
		_instance.gameObject.SetActive(active);
	}


	public void OnCreated()
	{

	}

}
