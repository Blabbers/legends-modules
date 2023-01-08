using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectCreatorUtility
{
	public static GameObject CreateAndDeleteOld(string name, Transform parent = null)
	{
		GameObject tempParent;


		if(parent ==null)
		{
			if (GameObject.Find(name) != null)
			{
				tempParent = GameObject.Find(name);
				GameObject.DestroyImmediate(tempParent.gameObject);
			}

			tempParent = new GameObject(name);
		}
		else
		{
			if (parent.transform.Find(name) != null)
			{
				tempParent = GameObject.Find(name);
				GameObject.DestroyImmediate(tempParent.gameObject);
			}

			tempParent = new GameObject(name);
			tempParent.transform.SetParent(parent);
		}

		return tempParent;
	}



}


