using UnityEditor;
using UnityEngine;
using Blabbers.Game00;
using Animancer.Editor;
using System.Reflection;
using Fungus;

[CustomPropertyDrawer(typeof(LocalizedString), true)]
public class LocalizedStringDrawer : PropertyDrawer
{
	//Criar alguma variável para guardar a ultima key, deletar a antiga quando criar uma nova
	int selectedLanguage = 0;
	bool editingKey = false;
	bool editTriggeredOnce = false;
	bool languageTriggeredOnce = false;

	bool firstCheck = true;
	LocalizedStringOptionsAttribute options;
	bool hasBigTextArea = true;
	bool shouldShowTextArea = true;
	string internalText = "";

	public void OnEnable()
	{
		options = fieldInfo.GetCustomAttribute<LocalizedStringOptionsAttribute>(true);
		shouldShowTextArea = options == null || (options != null && !options.hideArea);
		hasBigTextArea = options != null && options.hasBigTextArea;
		LocalizationExtensions.ResetLanguageJson();
	}

	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
	{
		if (firstCheck)
		{
			OnEnable();
			firstCheck = false;
		}

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
		//EditorGUILayout.BeginHorizontal();
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
				internalText = LoadText(property,internalTextProp, internalKey);
				GUI.FocusControl(null);
			}

			currentRect.x -= buttonSize;
			currentRect.width = buttonSize;

			icon = EditorGUIUtility.IconContent(@"d_SaveAs").image;
			buttonContent = new GUIContent(null, icon, "Save text to language.json");

			if (GUI.Button(currentRect, buttonContent))
			{
				//if(overrideKey) internalKey = 
				SaveBtn(property, internalKey, internalText);
			}

			remainingSize = leftBlock.width - ((buttonSize * 2) + 10);
			currentRect.x = 10;
			currentRect.width = remainingSize -10;

			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.richText = true;
			EditorGUI.LabelField(currentRect, $"{property.displayName} <color=yellow><i><b>({GetLanguageCode()})</b></i></color>", style);

		}
		//EditorGUILayout.EndHorizontal();
		#endregion

		#region Right block
		//EditorGUILayout.BeginHorizontal();
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

			Rect temp =currentRect;


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
		//EditorGUILayout.EndHorizontal();
		#endregion

		Rect horizontalLine2;
		horizontalLine2 = horizontalLine1;

		horizontalLine2.y += 25f;
		horizontalLine2.x = 10;
		horizontalLine2.width = horizontalLine1.width - 10;


		if (languageTriggeredOnce)
		{
			languageTriggeredOnce = false;
			internalText = LoadText(property,internalTextProp, internalKey);
			GUI.FocusControl(null);
		}

		if (shouldShowTextArea)
		{
			//EditorGUILayout.BeginHorizontal();
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
			//EditorGUILayout.EndHorizontal();
		}




		if(Event.current.type == EventType.Repaint)
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

	void SaveBtn(SerializedProperty property, string key, string value)
	{
		LocalizationExtensions.EditorSaveToLanguageJson(key, value, langCode: GetLanguageCode());

		LocalizedString locString;
		locString = (LocalizedString)property.GetValue();
		locString.OnSave?.Invoke();
	}

	private string LoadText(SerializedProperty property, SerializedProperty internalTextProp, string internalKey)
	{
		string internalText;
		internalTextProp.stringValue = LocalizationExtensions.EditorLoadFromLanguageJson(internalKey, langCode: GetLanguageCode());
		internalText = internalTextProp.stringValue;

		LocalizedString locString;
		locString = (LocalizedString)property.GetValue();
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
			property.stringValue = System.Guid.NewGuid().ToString();
		}
	}
}