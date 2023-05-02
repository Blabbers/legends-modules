using BeauRoutine;
using Blabbers.Game00;
using DG.Tweening;
using LoLSDK;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SimulationEnvironment : MonoBehaviour, ISingleton
{
	public static SimulationEnvironment Instance => Singleton.Get<SimulationEnvironment>();
	public UnityEvent OnStart;
	public UnityEvent OnFinish;
	public bool isPaused;

	#region Start / Awake / OnCreated
	public void OnCreated()
	{
	}

	void Awake()
	{
	}

	void Start()
	{
		OnStart?.Invoke();
		ProgressController.AddProgress();
	} 
	#endregion


	void Update()
	{

#if UNITY_EDITOR || DEVELOPMENT_BUILD || UNITY_CLOUD_BUILD
		if (Input.GetKeyDown(KeyCode.End))
		{
			FinishSimulation();
		}
#endif


		if (isPaused) return;
	}

	public void FinishSimulation()
	{
		Instance.OnFinish?.Invoke();
	}

	public void TogglePauseSimulation(bool active)
	{
		Instance.isPaused = active;
	}
}
