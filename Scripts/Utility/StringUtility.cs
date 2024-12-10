using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringUtility
{

	public static string FindTermInString(string term, string mainText, out bool success)
	{
		success = true;
		string termFormat = "";

		if (mainText.Length == 0)
		{
			success = false;
			return mainText;
		}

		//First check
		termFormat = term;
		if (mainText.Contains(termFormat)) return termFormat;

		//Check all lowercase
		termFormat = term.ToLower();
		if (mainText.Contains(termFormat)) return termFormat;

		//Check all uppercase
		termFormat = term.ToUpper();
		if (mainText.Contains(termFormat)) return termFormat;

		//Check first letter capitalized
		termFormat = term.ToLower();

		if (termFormat.Length == 1) termFormat = "" + char.ToUpper(termFormat[0]);
		else termFormat = char.ToUpper(termFormat[0]) + termFormat.Substring(1);

		if (mainText.Contains(termFormat)) {

			return termFormat;
		}
		else
		{
			//Debug.Log($"mainText [{mainText}] doens't Contains -> termFormat: [{termFormat}]");
		}

		success = false;
		return termFormat;
	}

	public static string ReplaceTermInString(string term, string newTerm,string mainText)
	{
		string updated = "";
		updated = mainText;

		if(term.Length ==0 || mainText.Length==0) return updated;

		updated = mainText.Replace(term, newTerm);

		return updated;
	}


	public static int ConvertSceneNameToLevel(string scenePath)
	{
		int level = 1;

		string sceneName = ConvertScenePathToName(scenePath);
		if (!sceneName.Contains("-")) return -1;


		string[] split = sceneName.Split("-");
		int result = level;

		string clean = RemoveNonNumericCharacters(split[1]);

		if (int.TryParse(clean, out result))
		{
			level = result;
		}


		return level;
	}

	public static string ConvertScenePathToName(string path)
	{
		string sceneName;
		string[] split;
		string[] split2;

		split = path.Split('/');
		sceneName = split[split.Length - 1];
		split2 = sceneName.Split('.');

		return split2[0];
	}

	public static string RemoveNonNumericCharacters(string input)
	{
		if (string.IsNullOrEmpty(input))
			return input;

		// Regex to match any character that is not a digit
		return Regex.Replace(input, "[^0-9]", "");
	}

}
