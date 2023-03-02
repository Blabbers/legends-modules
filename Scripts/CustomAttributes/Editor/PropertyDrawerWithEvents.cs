using UnityEditor;
using UnityEngine;

public abstract class PropertyDrawerWithEvents : PropertyDrawer
{
	bool init = true;

	~PropertyDrawerWithEvents()
	{
		Destroy();
	}

	private void PlayModeStateChanged(PlayModeStateChange obj)
	{
		switch (obj)
		{
			case PlayModeStateChange.ExitingEditMode:
			case PlayModeStateChange.ExitingPlayMode:
				Destroy();
				break;
		}
	}

	private void SelectionChanged()
	{
		Disable();
	}

	/// <summary>
	/// Write code for when the property is first displayed or redisplayed.
	/// </summary>
	public abstract void OnEnable(Rect position, SerializedProperty property, GUIContent label);

	/// <summary>
	/// Write code for when the property may be $$anonymous$$dden.
	/// </summary>
	public abstract void OnDisable();

	/// <summary>
	/// Write code for when the property is destroyed. (e.g. Releasing resources.)
	/// </summary>
	public abstract void OnDestroy();

	//public abstract void OnGUIWithEvent(Rect position, SerializedProperty property, GUIContent label);

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (init) Enable(position, property, label);

		//OnGUIWithEvent(position, property, label);
	}

	public void Enable(Rect position, SerializedProperty property, GUIContent label)
	{
		init = false;
		EditorApplication.playModeStateChanged += PlayModeStateChanged;
		Selection.selectionChanged += SelectionChanged;
		OnEnable(position, property, label);
	}

	public void Disable()
	{
		OnDisable();
		EditorApplication.playModeStateChanged -= PlayModeStateChanged;
		Selection.selectionChanged -= SelectionChanged;
		init = true;
	}

	public void Destroy()
	{
		OnDestroy();
		EditorApplication.playModeStateChanged -= PlayModeStateChanged;
		Selection.selectionChanged -= SelectionChanged;
		init = true;
	}
}
