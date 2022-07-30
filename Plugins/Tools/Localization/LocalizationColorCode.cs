using Blabbers.Game00;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu]
public class LocalizationColorCode : ScriptableObject
{
    public string key;
    public string text;
    public List<string> extraKeys;

    public Color color;
    public bool includePlural = true;

	#region SaveLoadFromEditor
	public bool HasKey => !string.IsNullOrEmpty(key);	
	[Button()]
	public void SaveToLanguageJson()
	{
		if (!HasKey)
		{
			key = this.name;
		}
		LocalizationExtensions.EditorSaveToLanguageJson(key, text, this);
	}
	[Button()]
	public void LoadFromLanguageJson()
	{
		if (!HasKey)
		{
			key = this.name;
		}
				
		this.text = LocalizationExtensions.EditorLoadFromLanguageJson(key, this);
	}
	#endregion
}
