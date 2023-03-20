using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Editor/GizmosConfigs", order = 1)]

public class GizmosConfigs : ScriptableObject
{

	#region Instance
	private static GizmosConfigs _instance = null;
	public static GizmosConfigs Instance
	{
		get
		{
			if (!_instance) _instance = Resources.Load<GizmosConfigs>("GizmosConfigs");
			return _instance;
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init()
	{
		_instance = null;
	}

	#endregion


	public bool enemyGizmos;
	public bool playerGizmos;
	public bool colliderGizmos;
	public bool triggerGizmos;
	public bool cameraGizmos;
	public bool toolGizmos = false;

}
