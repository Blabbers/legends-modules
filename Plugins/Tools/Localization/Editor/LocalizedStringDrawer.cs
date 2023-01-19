using UnityEditor;
using UnityEngine;
using System;
using Blabbers.Game00;
using static UnityEditor.PlayerSettings;
using static Animancer.Validate;

[CustomPropertyDrawer(typeof(LocalizedString))]
public class LocalizedStringDrawer : PropertyDrawer
{
	//Criar alguma variável para guardar a ultima key, deletar a antiga quando criar uma nova
	int selectedLanguage = 0;
	bool editingKey = false;
	bool editTriggeredOnce = false;
	bool languageTriggeredOnce = false;
	string internalText = "";

	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
	{

		string controlKeyName;
		string controlTextName;

		var internalKeyProp = property.FindPropertyRelative("key");
		var internalTextProp = property.FindPropertyRelative("text");

		var internalKey = internalKeyProp.stringValue;
		internalText = internalTextProp.stringValue;

		Rect horizontalLine1, leftBlock, rightBlock;
		float sizePercent = 0.075f;
		float spacing = 0.025f;

		#region Confgiure horizontal line 1
		horizontalLine1 = rect;
		horizontalLine1.height = 20f;
		horizontalLine1.width = rect.width * 1.0f; 
		#endregion


		leftBlock = horizontalLine1;
		rightBlock = horizontalLine1;

		leftBlock.width = horizontalLine1.width * 0.60f;
		rightBlock.width = horizontalLine1.width * 0.40f;
		rightBlock.x = leftBlock.width;


		#region Left block
		EditorGUILayout.BeginHorizontal();
		{
			Rect currentRect;
			float remainingSize;
			float startX, finalX;
			float buttonSize = 27.5f;

			startX = 0;
			finalX = startX + (leftBlock.width * 1.0f);
			currentRect = leftBlock;

			GUIContent buttonContent;


			//Buttons first

			var icon = EditorGUIUtility.IconContent(@"d_Refresh").image;
			buttonContent = new GUIContent(null, icon, "Load text from language.json");

			currentRect.x = finalX - buttonSize - 5;
			currentRect.width = buttonSize;

			if (GUI.Button(currentRect, buttonContent))
			{
				internalText = LoadText(internalTextProp, internalKey);
				GUI.FocusControl(null);
			}


			currentRect.x -= buttonSize;
			currentRect.width = buttonSize;

			icon = EditorGUIUtility.IconContent(@"d_SaveAs").image;
			buttonContent = new GUIContent(null, icon, "Save text to language.json");

			if (GUI.Button(currentRect, buttonContent))
			{
				//if(overrideKey) internalKey = 
				SaveBtn(internalKey, internalText);
			}

			remainingSize = leftBlock.width - ((buttonSize * 2) + 10);
			currentRect.x = 10;
			currentRect.width = remainingSize -10;

			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.richText = true;
			EditorGUI.LabelField(currentRect, $"Story Text <color=yellow><i><b>({GetLanguageCode()})</b></i></color>", style);

		}
		EditorGUILayout.EndHorizontal();
		#endregion

		#region Right block
		EditorGUILayout.BeginHorizontal();
		{
			Rect currentRect;
			float startX, finalX;

			currentRect = rightBlock;
			startX = leftBlock.width;
			finalX = startX + (rightBlock.width * 1.0f);


			currentRect.x = finalX - 10;

			//internalBoolProp.boolValue = EditorGUI.Toggle(currentRect, overrideKey);

			currentRect.width = 20;
			var icon = EditorGUIUtility.IconContent(@"_Menu@2x").image;
			GUIContent buttonContent = new GUIContent(null, icon, null);

			Rect temp =currentRect;


			EditorGUI.BeginDisabledGroup(!editingKey);
			currentRect.x = startX;
			currentRect.width = rightBlock.width - 15;

			//controlKeyName = "ValueFld" + GUIUtility.GetControlID(FocusType.Keyboard);
			controlKeyName = "controlKeyName";
			GUI.SetNextControlName(controlKeyName);
			internalKeyProp.stringValue = EditorGUI.TextField(currentRect, internalKey);

			EditorGUI.EndDisabledGroup();

			//EditorGUIUtility.
			//EditorGUI.foc


			if (EditorGUI.DropdownButton(temp, buttonContent, FocusType.Passive))
			{
				GenericMenu menu = new GenericMenu();
				menu.AddItem(new GUIContent("Select Language/English"), selectedLanguage == 0, SetEnglish);
				menu.AddItem(new GUIContent("Select Language/Spanish"), selectedLanguage == 1, SetSpanish);

				menu.AddItem(new GUIContent("Edit"), false, EnableEdit);

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

				menu.ShowAsContext();
			}






		}
		EditorGUILayout.EndHorizontal();
		#endregion

		Rect horizontalLine2;
		horizontalLine2 = horizontalLine1;

		horizontalLine2.y += 25f;
		horizontalLine2.x = 10;
		horizontalLine2.width = horizontalLine1.width - 10;


		if (languageTriggeredOnce)
		{
			languageTriggeredOnce = false;
			internalText = LoadText(internalTextProp, internalKey);
			GUI.FocusControl(null);
		}


		EditorGUILayout.BeginHorizontal();
		{
			Rect currentRect;

			currentRect = horizontalLine2;
			currentRect.height = 60f;


			EditorStyles.textField.wordWrap = true;

			//controlTextName = "ValueFld" + GUIUtility.GetControlID(FocusType.Keyboard);
			controlTextName = "controlTextName";
			GUI.SetNextControlName(controlTextName);
			internalTextProp.stringValue = EditorGUI.TextArea(currentRect, internalText);
			EditorStyles.textField.wordWrap = false;
		}
		EditorGUILayout.EndHorizontal();


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
				//Debug.Log($"Está editando [{currentFocusedControlName}] | {Event.current.type}");

				if (!(currentFocusedControlName == controlKeyName))
				{
					//Debug.Log($"Deu certo! [{currentFocusedControlName}] != [{controlKeyName}] | {Event.current.type}");
					GenerateLocKey(internalKeyProp, internalKey);
					editingKey = false;
				}
			}
			else
			{
				//Debug.Log("NÃOO Está editando");

				GenerateLocKey(internalKeyProp, internalKey);
				editingKey = false;

			}
		}



	}

	private string LoadText(SerializedProperty internalTextProp, string internalKey)
	{
		string internalText;
		internalTextProp.stringValue = LoadBtn(internalKey);
		internalText = internalTextProp.stringValue;
		return internalText;
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

	void SaveBtn(string key, string value)
	{
		LocalizationExtensions.EditorSaveToLanguageJson(key, value, langCode: GetLanguageCode());
	}

	string LoadBtn(string key)
	{
		return LocalizationExtensions.EditorLoadFromLanguageJson(key, langCode: GetLanguageCode());
	}

	// Acho que um ultimo passo legal vai ser tipo um botao ou um atalho generico que faz dar load em TOOODOS os Flowcharts/LoadSDKtext da scene que voce ta aberto e tal.
	//Mas isso é uma outra tool, não seria aqui pelo drawer.
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float extraHeight = 20.0f + 20f + 20f + 10f;
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
			// TEEM QUE DAR "SET PROPERTY" DAQUI COM O NOME NOVO DA KEY.
			//property..key = System.Guid.NewGuid().ToString();

			property.stringValue = System.Guid.NewGuid().ToString();
		}
	}
}