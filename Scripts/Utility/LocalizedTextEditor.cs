using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LocalizedTextEditor : MonoBehaviour
{
#if UNITY_EDITOR
	[MenuItem("LocalizedString/Save: Save all LocalizedString %#S")]
	static void SaveAll()
	{

	}

	[MenuItem("LocalizedString/Load: Load all LocalizedString %#L")]
	static void LoadAll()
	{
		//Get all the localized text on scene
		//Save their current text
		//Load the text using the method
		//Compare new and old text
		//List all text that changed and their old version
		//Put all this information in a screen
	}

#endif
}
