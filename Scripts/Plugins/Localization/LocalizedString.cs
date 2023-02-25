using UnityEngine;
using Blabbers.Game00;
using System;

[System.Serializable]
public class LocalizedString
{
	//Operators
	public static implicit operator string(LocalizedString locString) { return locString.ToString(); }

	//This has to be hidden from view or "read only" in inspector by default
	[SerializeField] private string key;
	[SerializeField] private string text;
	[SerializeField] public bool applyKeyCodes = false;
	public bool ApplyKeyCodes => applyKeyCodes;
	public Action<string> OnLoad;
	public Action OnSave;

	public string Text
	{
		get {
			return LocalizationExtensions.LocalizeText(key, applyColorCode: true);
		}
		set
		{
			//TODO: Salvar o texto no load tambem
			//if (!value.Equals(text))
			//{
			//	LocalizationExtensions.EditorSaveToLanguageJson(key, value);
			//}
			text = value;
		}
	}

	public string Key => key;
	/// <summary>
	/// This text will only be different thant the "Text" prop if the string is not saved to the laguage file yet.
	/// </summary>
	public string InternalText => text;

	public bool HasUnsavedChanges()
	{
		var textInFile = LocalizationExtensions.EditorLoadFromLanguageJson(key, null, false, GameData.Instance.currentSelectedLangCode);
		return text != textInFile;
	}

	public void OverrideLocKey(string key)
	{
		this.key = key;
	}

	public override string ToString()
	{
		return Text;
	}

	public string GetRawText()
	{
		return text;
	}

	public string GetLocalizedText(bool applyKeyCode)
	{
		return LocalizationExtensions.LocalizeText(key, applyColorCode: applyKeyCode);
	}

	public static string GenerateLocKey()
	{
		return GetNewSmallGUID();	

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