using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blabbers.Game00;
using NaughtyAttributes;
using System;

[System.Serializable]
public class LocalizedString
{
	//Operators
	public static implicit operator string(LocalizedString locString) { return locString.ToString(); }

	//This has to be hidden from view or "read only" in inspector by default
	[SerializeField] private string key;
	[SerializeField] private string text;
	//[field: SerializeField] public bool applyKeyCodes { get; private set; }
	[SerializeField] public bool applyKeyCodes = false;
	public bool ApplyKeyCodes => applyKeyCodes;
	public Action<string> OnLoad;

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
		return text != Text;
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

}