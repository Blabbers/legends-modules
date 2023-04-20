using BennyKok.RuntimeDebug.Actions;
using BennyKok.RuntimeDebug.Systems;
using UnityEngine;

namespace BennyKok.RuntimeDebug.Components
{
    public abstract class RuntimeDebugBehaviour : MonoBehaviour
    {
        private BaseDebugAction[] actions;

        protected virtual void Awake()
        {
			if (!RuntimeDebugSystem.IsSystemEnabled)
            {
                Debug.Log("RuntimeDebugBehaviour Awake() SystemNotEnabled");
				return;
			}
			actions = RuntimeDebugSystem.RegisterActionsAuto(this);
        }

        protected virtual void OnDestroy()
        {
            if (!RuntimeDebugSystem.IsSystemEnabled) {
				Debug.Log("RuntimeDebugBehaviour OnDestroy() SystemNotEnabled");
				return;
			} 
            RuntimeDebugSystem.UnregisterActions(actions);
        }
    }
}