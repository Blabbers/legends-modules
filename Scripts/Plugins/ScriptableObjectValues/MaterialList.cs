using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Values/MaterialList", order = 1)]
public class MaterialList : GenericSOList
{
	public List<Material> materials = new List<Material>();
}
