using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro.EditorUtilities;
using Animancer.Editor;
using Blabbers.Game00;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(TextLocalized))]
public class TextLocalizedEditor : TMP_EditorPanelUI
{
	//[HideTextField]
	SerializedProperty localizedStringProp;
	SerializedProperty playTTSOnEnableProp;
	SerializedProperty applyKeyCodesProp;

	LocalizedString locString;
	string loadedText = "";
	bool loaded = false;

	#region Enable / Disable
	protected override void OnEnable()
	{
		localizedStringProp = serializedObject.FindProperty("localization");
		playTTSOnEnableProp = serializedObject.FindProperty("playTTSOnEnable");
		//applyKeyCodesProp = serializedObject.FindProperty("applyKeyCodes");

		locString = (LocalizedString)localizedStringProp.GetValue();
		locString.OnLoad += HandleLocalizedStringLoad;

		//Debug.Log($"locString: {locString}");

		base.OnEnable();
	}

	protected override void OnDisable()
	{
		locString.OnLoad -= HandleLocalizedStringLoad;
		base.OnDisable();
	}

	#endregion

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(localizedStringProp);
		EditorGUILayout.PropertyField(playTTSOnEnableProp);
		//EditorGUILayout.PropertyField(applyKeyCodesProp);

		#region Get string value and pass it to other variable
		string textValue;
		textValue = (string)m_TextProp.GetValue();

		if (!string.IsNullOrEmpty(loadedText))
		{
			textValue = loadedText;
			loadedText = string.Empty;
			m_TextProp.SetValue(textValue);
		}

		locString.Text = textValue;
		#endregion

		ApplyChanges();
		base.OnInspectorGUI();
	}

	void HandleLocalizedStringLoad(string text)
	{
		loadedText = text;
	}

	void ApplyChanges()
	{
		serializedObject.ApplyModifiedProperties();

		if (GUI.changed)
		{
			EditorUtility.SetDirty(target);
		}
	}

	[MenuItem("Component/Scripts/Text Localized")]
	private static void OnAddComponent()
	{
		var textLoc = Selection.activeGameObject.AddComponent<TextLocalized>();
		try
		{
			// Generates new localization key
			textLoc.Localization.OverrideLocKey(LocalizedString.GenerateLocKey());
		}
		catch { }
	}

	[MenuItem("GameObject/UI/Text - Localized")]
	private static void CreateMenu()
	{
		//Creates
		var gameObject = ObjectFactory.CreateGameObject("Text", typeof(TextLocalized));

		// Place the object in the proper scene, with a relevant name
		StageUtility.PlaceGameObjectInCurrentStage(gameObject);
		GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

		// Sets parent if any
		if (Selection.activeGameObject)
		{
			gameObject.transform.SetParent(Selection.activeGameObject.transform);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
		}

		var textLoc = gameObject.GetComponent<TextLocalized>();
		if (textLoc)
		{
			// Generates new localization key
			textLoc.Localization.OverrideLocKey(LocalizedString.GenerateLocKey());
		}

		// Record undo and select
		Undo.RegisterCreatedObjectUndo(gameObject, $"Create Object: {gameObject.name}");
		Selection.activeGameObject = gameObject;

		// Mark the scene as dirty, for saving
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
	}
}
