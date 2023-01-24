using Blabbers.Game00;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu]
public class LocalizationColorCode : ScriptableObject
{

	public LocalizedString localization;

    //public string key;
    //public string text;

	//To remove
    //public List<string> extraKeys;
	public Color color;
    //public bool includePlural = true;


	public List<string> tags = new List<string>();

	#region SaveLoadFromEditor
	//public bool HasKey => !string.IsNullOrEmpty(key);	
	//[Button()]
	//public void SaveToLanguageJson()
	//{
	//	if (!HasKey)
	//	{
	//		key = this.name;
	//	}
	//	LocalizationExtensions.EditorSaveToLanguageJson(key, text, this);
	//}
	//[Button()]
	//public void LoadFromLanguageJson()
	//{
	//	if (!HasKey)
	//	{
	//		key = this.name;
	//	}
				
	//	this.text = LocalizationExtensions.EditorLoadFromLanguageJson(key, this);
	//}
	#endregion
}
