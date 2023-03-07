using UnityEditor;
using UnityEngine;
using Blabbers.Game00;
using System.Reflection;
using System;
using System.Collections.Generic;
using Animancer.Editor;


[CustomPropertyDrawer(typeof(LocalizedString), true)]
public class LocalizedStringDrawer : PropertyDrawerWithEvents
{
	public static HashSet<LocalizedStringDrawer> LocStringsActiveInEditor = new HashSet<LocalizedStringDrawer>();

	//Criar alguma variavel para guardar a ultima key, deletar a antiga quando criar uma nova
	int selectedLanguage = 0;
	bool editingKey = false;
	bool editTriggeredOnce = false;
	bool languageTriggeredOnce = false;

	LocalizedStringOptionsAttribute options;
	bool hasBigTextArea = true;
	bool shouldShowTextArea = true;
	string internalText = "";

	public LocalizedString thisLocString;
	public SerializedProperty thisProperty;

	public LocalizedString GetThisLocString(SerializedProperty property)
	{
		//var field = fieldInfo.GetValue(property.serializedObject.targetObject);		
		var field = property.GetValue();

		//var possibleIndex = property.GetIndexInArray();
		//if (possibleIndex >= 0)
		//{			
		//	// Then this is inside an array
		//	if(field is LocalizedString[])
		//	{
		//		var array = (LocalizedString[])field;
		//		return array[possibleIndex];
		//	}
		//	if (field is List<LocalizedString>)
		//	{
		//		var array = (List<LocalizedString>)field;
		//		return array[possibleIndex];
		//	}
		//	// TODO: But... if the object is son of another object it still bugs out. The "GameData.TextConfigs" is not working...
		//	// TODO: Maybe we find it through reflection? Maybe another solution for the "GetValue" function instead
		//}

		return (LocalizedString)field;
	}

	public override void OnEnable(Rect position, SerializedProperty property, GUIContent label)
	{
		options = fieldInfo.GetCustomAttribute<LocalizedStringOptionsAttribute>(true);
		shouldShowTextArea = options == null || (options != null && !options.hideArea);
		hasBigTextArea = options != null && options.hasBigTextArea;
		LocalizationExtensions.ResetLanguageJson();
		thisLocString = GetThisLocString(property);
		thisProperty = property;

		// Add to save later
		LocStringsActiveInEditor.Remove(this);
		LocStringsActiveInEditor.Add(this);
	}

	public override void OnDisable()
	{
		LocStringsActiveInEditor.Remove(this);
	}

	public override void OnDestroy()
	{
		LocStringsActiveInEditor.Remove(this);
	}

	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
	{
		base.OnGUI(rect, property, label);

		thisProperty = property;

		string controlKeyName;
		string controlTextName;

		var internalKeyProp = property.FindPropertyRelative("key");
		var internalTextProp = property.FindPropertyRelative("text");
		var internalKey = internalKeyProp.stringValue;

		internalText = internalTextProp.stringValue;

		Rect horizontalLine1, leftBlock, rightBlock;

		horizontalLine1 = rect;
		horizontalLine1.height = 20f;
		horizontalLine1.width = rect.width * 1.0f;

		leftBlock = horizontalLine1;
		rightBlock = horizontalLine1;

		leftBlock.width = horizontalLine1.width * 0.60f;
		rightBlock.width = horizontalLine1.width * 0.40f;
		rightBlock.x = leftBlock.width;

		#region Left block
		{
			Rect currentRect;
			float remainingSize;
			float startX, finalX;
			float buttonSize = 27.5f;

			startX = 0;
			finalX = startX + (leftBlock.width * 1.0f);
			currentRect = leftBlock;

			GUIContent buttonContent;

			var icon = EditorGUIUtility.IconContent(@"d_Refresh").image;
			buttonContent = new GUIContent(null, icon, "Load text from language.json");

			currentRect.x = finalX - buttonSize - 5;
			currentRect.width = buttonSize;

			if (GUI.Button(currentRect, buttonContent))
			{
				internalText = LoadText(property, internalTextProp, internalKey);
				GUI.FocusControl(null);
			}

			currentRect.x -= buttonSize;
			currentRect.width = buttonSize;

			icon = EditorGUIUtility.IconContent(@"d_SaveAs").image;
			buttonContent = new GUIContent(null, icon, "Save text to language.json");

			if (GUI.Button(currentRect, buttonContent))
			{
				SaveBtn(property, internalKey, internalText);
			}

			remainingSize = leftBlock.width - ((buttonSize * 2) + 10);
			currentRect.x = 10;
			currentRect.width = remainingSize - 10;

			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.richText = true;

			///var locStringObj = (LocalizedString)fieldInfo.GetValue(property.serializedObject.targetObject);
			var hasUnsavedChanges = thisLocString.HasUnsavedChanges();
			EditorGUI.LabelField(currentRect, $"{property.displayName} <color=yellow><i><b>({GetLanguageCode()})</b></i></color> {(hasUnsavedChanges ? "<color=red>* UNSAVED CHANGES *</color>" : "")}", style);
		}
		#endregion

		#region Right block		
		{
			Rect currentRect;
			float startX, finalX;

			currentRect = rightBlock;
			startX = leftBlock.width;
			finalX = startX + (rightBlock.width * 1.0f);

			currentRect.x = finalX - 10;

			currentRect.width = 20;
			var icon = EditorGUIUtility.IconContent(@"_Menu@2x").image;
			GUIContent buttonContent = new GUIContent(null, icon, null);

			Rect temp = currentRect;

			EditorGUI.BeginDisabledGroup(!editingKey);
			currentRect.x = startX;
			currentRect.width = rightBlock.width - 15;
			controlKeyName = "controlKeyName";
			GUI.SetNextControlName(controlKeyName);
			internalKeyProp.stringValue = EditorGUI.TextField(currentRect, internalKey);

			EditorGUI.EndDisabledGroup();


			if (EditorGUI.DropdownButton(temp, buttonContent, FocusType.Passive))
			{
				GenericMenu menu = new GenericMenu();
				menu.AddItem(new GUIContent("Select Language/English"), selectedLanguage == 0, SetEnglish);
				menu.AddItem(new GUIContent("Select Language/Spanish"), selectedLanguage == 1, SetSpanish);

				menu.AddItem(new GUIContent("Edit"), false, EnableEdit);

				menu.AddItem(new GUIContent("New random key"), false, GenerateNewKey);

				void SetEnglish()
				{
					languageTriggeredOnce = true;
					selectedLanguage = 0;

				}
				void SetSpanish()
				{
					languageTriggeredOnce = true;
					selectedLanguage = 1;
				}

				void EnableEdit()
				{
					editTriggeredOnce = true;
				}

				void GenerateNewKey()
				{
					// TODO: This needs to work properly
					GenerateLocKey(internalKeyProp, "");
				}
				menu.ShowAsContext();
			}
		}
		#endregion

		Rect horizontalLine2;
		horizontalLine2 = horizontalLine1;

		horizontalLine2.y += 25f;
		horizontalLine2.x = 10;
		horizontalLine2.width = horizontalLine1.width - 10;


		if (languageTriggeredOnce)
		{
			languageTriggeredOnce = false;
			internalText = LoadText(property, internalTextProp, internalKey);
			GUI.FocusControl(null);
		}

		if (shouldShowTextArea)
		{
			Rect currentRect;

			currentRect = horizontalLine2;
			currentRect.height = hasBigTextArea ? 60f : 20;


			EditorStyles.textField.wordWrap = true;
			controlTextName = "controlTextName";
			GUI.SetNextControlName(controlTextName);
			if (hasBigTextArea)
			{
				internalTextProp.stringValue = EditorGUI.TextArea(currentRect, internalText);
			}
			else
			{
				internalTextProp.stringValue = EditorGUI.TextField(currentRect, internalText);
			}
			EditorStyles.textField.wordWrap = false;
		}

		if (Event.current.type == EventType.Repaint)
		{
			if (editTriggeredOnce)
			{
				editingKey = true;
				editTriggeredOnce = false;
				EditorGUI.FocusTextInControl(controlKeyName);
			}

			if (EditorGUIUtility.editingTextField)
			{
				var currentFocusedControlName = GUI.GetNameOfFocusedControl();
				if (!(currentFocusedControlName == controlKeyName))
				{
					GenerateLocKey(internalKeyProp, internalKey);
					editingKey = false;
				}
			}
			else
			{
				GenerateLocKey(internalKeyProp, internalKey);
				editingKey = false;

			}
		}
	}

	void DrawSpace(ref Rect rect, float size, float totalWidth)
	{
		rect.x += totalWidth * size;
	}

	void DrawVerticalBar(ref Rect rect)
	{
		Rect current = rect;

		var icon = EditorGUIUtility.IconContent(@"VerticalSplit").image;
		current.height = 20;
		current.width = 10;

		rect.x += current.width;
		EditorGUI.DrawPreviewTexture(current, icon);
	}

	public void SaveBtn(SerializedProperty property, string key, string value)
	{
		LocalizationExtensions.EditorSaveToLanguageJson(key, value, langCode: GetLanguageCode());

		var locString = GetThisLocString(property);
		locString.OnSave?.Invoke();
	}

	private string LoadText(SerializedProperty property, SerializedProperty internalTextProp, string internalKey)
	{
		string internalText;
		internalTextProp.stringValue = LocalizationExtensions.EditorLoadFromLanguageJson(internalKey, langCode: GetLanguageCode());
		internalText = internalTextProp.stringValue;

		var locString = GetThisLocString(property);
		locString.OnLoad?.Invoke(internalText);

		return internalText;
	}

	// Acho que um ultimo passo legal vai ser tipo um botao ou um atalho generico que faz dar load em TOOODOS os Flowcharts/LoadSDKtext da scene que voce ta aberto e tal.
	//Mas isso é uma outra tool, não seria aqui pelo drawer.
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float extraHeight = hasBigTextArea ? 20.0f + 20f + 20f + 10f : 27f;

		if (!shouldShowTextArea)
		{
			extraHeight = 10.0f;
		}

		return base.GetPropertyHeight(property, label) + extraHeight;
	}


	string GetLanguageCode()
	{
		switch (selectedLanguage)
		{
			case 0:
				return "en";

			case 1:
				return "es";

			default:
				return "en";
		}

	}

	public void GenerateLocKey(SerializedProperty property, string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			property.stringValue = GetNewSmallGUID();
		}

		string GetNewSmallGUID()
		{
			var guid = System.Guid.NewGuid().ToString();
			var smallGuid = guid.Substring(0, Math.Min(8, guid.Length));
			// Tries to load it from the json, if this key already existis, we generate another one
			var hasKey = !string.IsNullOrEmpty(LocalizationExtensions.EditorLoadFromLanguageJson(smallGuid, displayMessages: false));
			return hasKey ? GetNewSmallGUID() : smallGuid;
		}
	}
}
public class LocalizedStringSaveAssets : UnityEditor.AssetModificationProcessor
{
	static string[] OnWillSaveAssets(string[] paths)
	{
		// Saves the localized string throught the property drawer, during Unity's ctrl+S Save function
		if (LocalizedStringDrawer.LocStringsActiveInEditor != null)
		{
			foreach (var item in LocalizedStringDrawer.LocStringsActiveInEditor)
			{
				if (item.thisLocString.HasUnsavedChanges())
				{
					item.SaveBtn(item.thisProperty, item.thisLocString.Key, item.thisLocString.GetRawText());
				}
			}
		}
		return paths;
	}
}
