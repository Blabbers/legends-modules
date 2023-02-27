using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Values/GameObjectList", order = 1)]
public class GameObjectList : GenericSOList
{
	public List<GameObject> gameObjects = new List<GameObject>();
}
