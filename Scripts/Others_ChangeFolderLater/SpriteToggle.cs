using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteToggle : MonoBehaviour
{

	public Image image;
	public Sprite normalIcon, pressedIcon;


	public void TogglePressed(bool active)
	{
		if (active)
		{
			image.sprite = normalIcon;
		}
		else
		{
			image.sprite = pressedIcon;
		}
	}

}
