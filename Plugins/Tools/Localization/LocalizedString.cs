using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blabbers.Game00;
using NaughtyAttributes;

[System.Serializable]
public class LocalizedString
{
	//Operators
	public static implicit operator string(LocalizedString locString) { return locString.ToString(); }

	//This has to be hidden from view or "read only" in inspector by default
	[SerializeField] private string key;
	[SerializeField] private string text;

	public void Changed()
	{
		Debug.Log("VALUE CHANGED!! " + text);
	}
	public string Text
	{
		get { return LocalizationExtensions.LocalizeText(key); }
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

	public void OverrideLocKey(string key)
	{
		this.key = key;
	}

	public override string ToString()
	{
		return Text;
	}
}
