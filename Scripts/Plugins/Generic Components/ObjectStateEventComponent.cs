using UnityEngine;
using UnityEngine.Events;

public class ObjectStateEventComponent : MonoBehaviour
{
	public UnityEvent OnAwake;
	public UnityEvent OnStart;
	public UnityEvent OnEnabled;
	public UnityEvent OnDisabled;
	public UnityEvent OnDestroyed;

	private void Awake()
	{
		OnAwake?.Invoke();
	}
	private void Start()
	{
		OnStart?.Invoke();
	}
	private void OnEnable()
	{
		OnEnabled?.Invoke();
	}
	private void OnDisable()
	{
		OnDisabled?.Invoke();
	}
	private void OnDestroy()
	{
		OnDestroyed?.Invoke();
	}
}
