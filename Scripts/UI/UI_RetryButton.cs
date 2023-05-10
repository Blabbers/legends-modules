using Blabbers.Game00;
using UnityEngine;
using UnityEngine.Events;

public class UI_RetryButton : MonoBehaviour, ISingleton
{
    public static bool HardReset;
    public UnityEvent OnRetry;

    public void OnCreated()
    {

    }

    public void ResetLevelFromScratch()
    {
        // Resets so everything plays again if they try from the victory screen
        SceneLoader.isStuckOnThisLevel = false;
        HardReset = true;
#if UNITY_EDITOR
        Debug.Log("→ Hard reset. Level will be reloaded from scratch.");
#endif
    }


    public void Retry()
    {
        OnRetry?.Invoke();
    }
}
