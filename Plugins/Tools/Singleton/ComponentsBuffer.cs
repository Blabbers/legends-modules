using System;
using System.Collections.Generic;
using Blabbers.Game00;
using UnityEngine;

public class ComponentsBuffer : MonoBehaviour
{
	private static readonly Dictionary<GameObject, ComponentsBuffer> buffers = new Dictionary<GameObject, ComponentsBuffer>();
	
	private Dictionary<Type, Component> componentByType;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void Init()
	{
		buffers.Clear();
	}
	
	public static T Get<T>(Component component) where T : Component
	{
		return Get<T>(component.gameObject);
	}

	public static T Get<T>(GameObject gameObject) where T : Component
	{
		if (!buffers.TryGetValue(gameObject, out var buffer))
		{
			return null;
		}

		return buffer.Get<T>();
	}

	private void Awake()
	{
		componentByType = new Dictionary<Type, Component>();
		
		// Fills up with all MonoBehaviours from this object
		var monoBehaviours = GetComponentsInChildren<MonoBehaviour>();
		foreach (var component in monoBehaviours)
		{
			if (component == this)
				continue;
			
			componentByType[component.GetType()] = component;
		}
		
		buffers.Add(gameObject, this);
	}

	public T Get<T>() where T : Component
	{
		var key = typeof(T);
		var match = default(T);
		
		if (componentByType.ContainsKey(key))
		{
			match = (T)componentByType[key];
		}

		if (match != default) return match;
		
		if (gameObject.TryGetComponentAboveAndBelow(out match))
		{
			componentByType[key] = match;
		}

		return match;
	}
}