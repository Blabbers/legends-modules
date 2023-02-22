using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro.EditorUtilities;
using Animancer.Editor;
using Blabbers.Game00;

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
		applyKeyCodesProp = serializedObject.FindProperty("applyKeyCodes");

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
		EditorGUILayout.PropertyField(applyKeyCodesProp);
		
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

		if (GUI.changed) { 		
			EditorUtility.SetDirty(target);
		}
	}


}
