using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringUtility
{

	public static string FindTermInString(string term, string mainText, out bool success)
	{
		success = true;
		string termFormat = "";

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

		updated = mainText.Replace(term, newTerm);

		return updated;
	}


}
