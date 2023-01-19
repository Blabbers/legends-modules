using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FungusSettings : ScriptableObject
{
    #region Instance
    private static FungusSettings _instance = null;
    public static FungusSettings Instance
    {
        get
        {
            if (!_instance) _instance = Resources.Load<FungusSettings>("FungusGlobalSettings");
            return _instance;
        }
    }
    #endregion

    // Flowchart global settings
    [field: Header("Flowchart Global Settings (see tooltips)")]
    [field: Range(0f, 5f)]
    [field: Tooltip("Adds a pause after each execution step to make it easier to visualise program flow. Editor only, has no effect in platform builds.")]
    [field: SerializeField] public float StepPause { get; private set; } = 0f;

    [field: Tooltip("Use command color when displaying the command list in the Fungus Editor window")]
    [field: SerializeField] public bool ColorCommands { get; private set; } = true;

    [field: Tooltip("Hides the Flowchart block and command components in the inspector. Deselect to inspect the block and command components that make up the Flowchart.")]
    [field: SerializeField] public bool HideComponents { get; private set; } = true;

    [field: Tooltip("Saves the selected block and commands when saving the scene. Helps avoid version control conflicts if you've only changed the active selection.")]
    [field: SerializeField] public bool SaveSelection { get; private set; } = true;

    [field: Tooltip("Display line numbers in the command list in the Block inspector.")]
    [field: SerializeField] public bool ShowLineNumbers { get; private set; } = false;

    [field: Tooltip("List of commands to hide in the Add Command menu. Use this to restrict the set of commands available when editing a Flowchart.")]
    [field: SerializeField] public List<string> HideCommands { get; private set; } = new List<string>();
}
