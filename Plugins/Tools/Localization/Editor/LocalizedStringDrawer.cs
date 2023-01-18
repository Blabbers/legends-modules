using UnityEditor;
using UnityEngine;
using System;
using Blabbers.Game00;
using static UnityEditor.PlayerSettings;

[CustomPropertyDrawer(typeof(LocalizedString))]
public class LocalizedStringDrawer : PropertyDrawer
{
	//Criar alguma variável para guardar a ultima key, deletar a antiga quando criar uma nova

	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
	{
		var internalKeyProp = property.FindPropertyRelative("key");
		var internalTextProp = property.FindPropertyRelative("text");
		var internalBoolProp = property.FindPropertyRelative("overrideKey");

		var internalKey = internalKeyProp.stringValue;
		var internalText = internalTextProp.stringValue;
		var overrideKey = internalBoolProp.boolValue;

		#region MyRegion
		//GenerateLocKey(property, internalKey);

		//EditorGUI.PropertyField(rect, internalTextProp, label, true);
		//rect.y += 20;

		//var keyLabel = new GUIContent("BATATA", "aaaaa eu sou batata");
		//EditorGUI.PropertyField(rect, internalKeyProp, keyLabel, true);

		////------------------------------------------ 
		#endregion


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
				internalTextProp.stringValue = LoadBtn(property, internalKey);
				internalText = internalTextProp.stringValue;

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
			EditorGUI.LabelField(currentRect, "Story Text");

		}
		EditorGUILayout.EndHorizontal();
		#endregion


		#region Left block anchored left
		//EditorGUILayout.BeginHorizontal();
		//{
		//	Rect currentRect;
		//	float buttonWidth, remainingSize;
		//	float startX, finalX;
		//	float buttonSize = 30.0f;

		//	startX = 0;
		//	finalX = startX + (leftBlock.width * 1.0f);

		//	currentRect = leftBlock;
		//	currentRect.width = leftBlock.width * 0.75f;
		//	EditorGUI.LabelField(currentRect, "Story Text");

		//	currentRect.x += leftBlock.width * 0.75f;
		//	remainingSize = leftBlock.width - currentRect.x;



		//	buttonWidth = buttonSize;
		//	if (remainingSize / 2 < buttonSize)
		//	{
		//		buttonWidth = remainingSize / 2;
		//	}

		//	currentRect.width = buttonWidth;

		//	#region Button save
		//	var icon = EditorGUIUtility.IconContent(@"d_SaveAs").image;
		//	GUIContent buttonContent = new GUIContent(null, icon, "Save text to language.json");

		//	if (GUI.Button(currentRect, buttonContent))
		//	{
		//		//if(overrideKey) internalKey = 
		//		SaveBtn(property, internalKey, internalText);
		//	}
		//	#endregion

		//	currentRect.x += buttonWidth;
		//	currentRect.width = buttonWidth;

		//	#region Button load


		//	icon = EditorGUIUtility.IconContent(@"d_Refresh").image;
		//	buttonContent = new GUIContent(null, icon, "Load text from language.json");

		//	if (GUI.Button(currentRect, buttonContent))
		//	{
		//		internalTextProp.stringValue = LoadBtn(property, internalKey);
		//		internalText = internalTextProp.stringValue;

		//		GUI.FocusControl(null);
		//	}

		//	#endregion
		//}
		//EditorGUILayout.EndHorizontal();
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
			internalBoolProp.boolValue = EditorGUI.Toggle(currentRect, overrideKey);

			//DrawSpace(ref currentRect, spacing, horizontalLine1.width);

			EditorGUI.BeginDisabledGroup(overrideKey);

			currentRect.x = startX;
			currentRect.width = rightBlock.width - 15;
			internalKeyProp.stringValue = EditorGUI.TextField(currentRect, internalKey);


			EditorGUI.EndDisabledGroup();
			//currentRect.x = leftBlock.width + (rightBlock.width * 1.0f) - 10;
			//internalBoolProp.boolValue = EditorGUI.Toggle(currentRect, overrideKey);

		}
		EditorGUILayout.EndHorizontal();
		#endregion

		Rect horizontalLine2;
		horizontalLine2 = horizontalLine1;

		horizontalLine2.y += 25f;
		horizontalLine2.x = 10;
		horizontalLine2.width = horizontalLine1.width - 10;

		EditorGUILayout.BeginHorizontal();
		{
			Rect currentRect;

			currentRect = horizontalLine2;
			currentRect.height = 60f;

			internalTextProp.stringValue = EditorGUI.TextArea(currentRect, internalText);

			if (!overrideKey)
			{
				GenerateLocKey(internalKeyProp, internalKey);
			}

		}
		EditorGUILayout.EndHorizontal();

		#region old

		#region Internal key
		//DrawSpace(ref rect, spacing, horizontalLine1.width);

		////GUI.enabled = overrideKey;
		//EditorGUI.BeginDisabledGroup(overrideKey);

		//rect.width = horizontalLine1.width * 0.5f;
		//rect.x += horizontalLine1.width * sizePercent;
		//internalKeyProp.stringValue = EditorGUI.TextField(rect, internalKey);
		#endregion

		//DrawSpace(ref rect, spacing, horizontalLine1.width);

		//EditorGUI.EndDisabledGroup();
		////GUI.enabled = true;

		//rect.x += horizontalLine1.width * 0.5f;
		//internalBoolProp.boolValue = EditorGUI.Toggle(rect, overrideKey);

		//rect.y += 25f;
		//rect.x = 0;
		//rect.height = 60f;
		//rect.width = horizontalLine1.width * 0.95f;

		//DrawSpace(ref rect, spacing, horizontalLine1.width);

		//internalTextProp.stringValue = EditorGUI.TextArea(rect, internalText);

		//if (!overrideKey)
		//{
		//	GenerateLocKey(internalKeyProp, internalKey);
		//} 
		#endregion
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
		// Em caso de salvar e ainda nao tiver uma key, a gente cria ela aqui em runtime, seta no objeto e só depois salva.
		LocalizationExtensions.EditorSaveToLanguageJson(key, value);
	}

	string LoadBtn(SerializedProperty property, string key)
	{
		// Dai no btn de load, quando clicar atualiza o texto da property para o texto que tava no arquivo.
		//var internalTextProp = property.FindPropertyRelative("text");
		//internalTextProp.stringValue = LocalizationExtensions.EditorLoadFromLanguageJson(key);


		return LocalizationExtensions.EditorLoadFromLanguageJson(key);
		//LocalizationExtensions.EditorLoadFromLanguageJson(key);
	}

	// Acho que um ultimo passo legal vai ser tipo um botao ou um atalho generico que faz dar load em TOOODOS os Flowcharts/LoadSDKtext da scene que voce ta aberto e tal.
	//Mas isso é uma outra tool, não seria aqui pelo drawer.
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float extraHeight = 20.0f + 20f + 20f + 10f;
		return base.GetPropertyHeight(property, label) + extraHeight;
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