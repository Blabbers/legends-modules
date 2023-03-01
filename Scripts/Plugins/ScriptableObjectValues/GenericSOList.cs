using System.Collections.Generic;
using UnityEngine;

public class GenericSOList : ScriptableObject
{
	public int GetLength()
	{

		if (this is GameObjectList)
		{
			return ((GameObjectList)this).gameObjects.Count;

		}
		if (this is MaterialList)
		{
			return ((MaterialList)this).materials.Count;

		}
		if (this is TextureList)
		{
			return ((TextureList)this).textures.Count;
		}

		return 0;

	}
}