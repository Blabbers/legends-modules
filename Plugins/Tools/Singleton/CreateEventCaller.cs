using System;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blabbers.Game00
{
	[DefaultExecutionOrder(-1000)]
	public class CreateEventCaller : MonoBehaviour
	{
		public static CreateEventCaller instance;
		public List<MonoBehaviour> instances;

#if UNITY_EDITOR
		private void Update()
		{
			instances = new List<MonoBehaviour>(Singleton.Instances.Values);
		}
#endif

		void Awake()
		{
			if (instance)
			{
				Destroy(this.gameObject);
				return;
			}

			instance = this;
			DontDestroyOnLoad(this);
            SceneManager.sceneUnloaded += HandleSceneLoaded;
		}

        private void HandleSceneLoaded(Scene arg0)
        {
            InitializeSingleton();
        }

        private void InitializeSingleton()
		{
			Singleton.ClearAllSingletonInstances();
			Singleton.InitializeAllSingletonInstances();
		}
    }

	public interface ISingleton
	{
		/// <summary>
		/// Invoked when the scene is loaded, even if the object is disabled.
		/// </summary>
		void OnCreated();
	}

	public static class Singleton
	{
		private static bool hasInitialized;
        private static GameObject globalInstance;
		public static Dictionary<Type, MonoBehaviour> Instances { get; private set; } = new Dictionary<Type, MonoBehaviour>();
        public static readonly Relay<MonoBehaviour> OnAssigned = new Relay<MonoBehaviour>();

        public static T Get<T>() where T : MonoBehaviour
		{
			if (!Application.isPlaying)
				return null;

			if (!hasInitialized)
			{
				hasInitialized = true;
				InitializeAllSingletonInstances();
			}
			
			var type = typeof(T);
			
			// Remove all deleted instances
			if (Has<T>() && Instances[type].Equals(null))
			{
				Instances.Remove(type);
			}

			MonoBehaviour mono;
			if (Instances.ContainsKey(type))
			{
				// This instance was already initialized, then we just get it
				mono = Instances[type];
				Instances[type] = mono;
				OnAssigned?.Invoke(mono);
			}
			else
			{
				// Crates it the first time
				mono = FindObjectOfTypeAll<T>();
				if(mono != null)
				{
					Initialize(mono);
					// Calls "OnCreated" method the first time this singleton is created or accessed
                    (mono as ISingleton).OnCreated();
                }
			}
			return mono as T;
		}

		public static bool Has<T>() where T : MonoBehaviour
		{
			var type = typeof(T);
			return Instances.ContainsKey(type);
		}

		public static void Initialize(MonoBehaviour monoBehaviour)
		{
			if (monoBehaviour != null)
			{
				var type = monoBehaviour.GetType();
				// Adds or replaces value to the dictionary
				if (!Instances.ContainsKey(type) || Instances[type] == null)
				{
					Instances[type] = monoBehaviour;
					OnAssigned?.Invoke(monoBehaviour);
				}
			}
		}

		public static void ClearAllSingletonInstances()
		{
			hasInitialized = false;
			Instances.Clear();
		}

		/// <summary>
		/// Finds the first object (active or inactive)
		/// </summary>
		public static T FindObjectOfTypeAll<T>() where T : MonoBehaviour
		{
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				var s = SceneManager.GetSceneAt(i);
				var allGameObjects = s.GetRootGameObjects();
				for (int j = 0; j < allGameObjects.Length; j++)
				{
					var go = allGameObjects[j];
					var obj = go.GetComponentInChildren<T>(true);
					if (obj != null && obj is T)
					{
						return (T)obj;
					}
				}
			}
			return default;
		}
        
        // TODO: Chamar esse initialize em um callback fora do monobehaviour da Unity (caso ainda nao exista "globalInstance" setada)
        // Porque por enquanto ele só roda e cria o primeiro [Global] se alguem chamar um Singleton.Get<> (que por coincidencia acontece ja no menu inicial)
		public static void InitializeAllSingletonInstances()
		{
			if (!hasInitialized)
			{
				hasInitialized = true;
			}

            if (!globalInstance)
            {
                var globalResource = Resources.Load<GameObject>("[Global]");
                globalInstance = GameObject.Instantiate(globalResource);
            }

            for (int i = -1; i < SceneManager.sceneCount; i++)
			{
				Scene scene;
				if(i == -1)
				{
					// Current scene from this guy is "DontDestroyOnLoad" and we use it to access others that are possibly in it
					scene = CreateEventCaller.instance.gameObject.scene;
				}
				else
				{
					scene = SceneManager.GetSceneAt(i);
				}

				var allGameObjects = scene.GetRootGameObjects();
				for (int j = 0; j < allGameObjects.Length; j++)
				{
					var go = allGameObjects[j];
					var objs = go.GetComponentsInChildren<ISingleton>(true);
					foreach (var item in objs)
					{
						var mono = (MonoBehaviour)item;
						// OnCreated message is called inside the singleton initialization
						Initialize(mono);
					}
				}
			}
			Action onCreatedCalls = ()=> { };
			foreach (var mono in Instances)
			{
				// Calls "OnCreated" method the first time this singleton is created or accessed
				if (mono.Value)
				{
					onCreatedCalls += (mono.Value as ISingleton).OnCreated;
				}
			}

			var delegates = onCreatedCalls.GetInvocationList();
			for (int i = 0; i < delegates.Length; i++)
			{
				var action = delegates[i];
				try
				{
					action.DynamicInvoke();
				}
				catch(Exception ex)
				{
					Debug.LogError(ex);
				}
			}
			//onCreatedCalls?.Invoke();
		}
	}
}