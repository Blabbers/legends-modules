using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Values/TextureList", order = 1)]
public class TextureList : GenericSOList
{
	public List<Texture2D> textures = new List<Texture2D>();
}
