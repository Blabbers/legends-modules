using UnityEditor;
using UnityEngine;
using System;
using Blabbers.Game00;

[CustomPropertyDrawer(typeof(LocalizedString))]
public class LocalizedStringDrawer : PropertyDrawer
{
	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
	{
		var internalKeyProp = property.FindPropertyRelative("key");
		var internalTextProp = property.FindPropertyRelative("text");

		var internalKey = internalKeyProp.stringValue;
		var internalText = internalTextProp.stringValue;

		//GenerateLocKey(property, internalKey);

		EditorGUI.PropertyField(rect, internalTextProp, label, true);
		rect.y += 20;
		
		var keyLabel = new GUIContent("BATATA", "aaaaa eu sou batata");
		EditorGUI.PropertyField(rect, internalKeyProp, keyLabel, true);

		rect.y += 20f;
		rect.height = 20f;
		if (GUI.Button(rect, "Save"))
		{
			SaveBtn(property, internalKey, internalText);
		}		

	}

	void SaveBtn(SerializedProperty property, string key, string value)
	{
		// Em caso de salvar e ainda nao tiver uma key, a gente cria ela aqui em runtime, seta no objeto e só depois salva.
		LocalizationExtensions.EditorSaveToLanguageJson(key, value);
	}

	void LoadBtn()
	{
		// Dai no btn de load, quando clicar atualiza o texto da property para o texto que tava no arquivo.
		//LocalizationExtensions.EditorLoadFromLanguageJson(key);
	}

	// Acho que um ultimo passo legal vai ser tipo um botao ou um atalho generico que faz dar load em TOOODOS os Flowcharts/LoadSDKtext da scene que voce ta aberto e tal.
	//Mas isso é uma outra tool, não seria aqui pelo drawer.
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float extraHeight = 20.0f + 20f + 10f;
		return base.GetPropertyHeight(property, label) + extraHeight;
	}

	public void GenerateLocKey(SerializedProperty property, string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			// TEEM QUE DAR "SET PROPERTY" DAQUI COM O NOME NOVO DA KEY.
			//property..key = System.Guid.NewGuid().ToString();
		}
	}
}