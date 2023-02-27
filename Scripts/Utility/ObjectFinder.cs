using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectFinder
{
	public static bool FindObjectByName(string name,out GameObject obj)
	{
		var allTargets = UnityEngine.Object.FindObjectsOfType<GameObject>();
		bool found = false;
		obj = null;

		foreach (var target in allTargets)
		{
			if(target.name == name)
			{
				obj = target;
				found = true;
				break;
			}
		}
		
		return found;
	}


}
