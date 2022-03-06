/*
Video to GIF
https://ezgif.com/video-to-gif

Gif to spritesheet [1]
https://jacklehamster.github.io/utils/gif2sprite/
Gif to spritesheet [2]
https://onlinegiftools.com/convert-gif-to-sprite-sheet 
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_AnimatedSprite : MonoBehaviour
{
	public Sprite sprite;
	[NaughtyAttributes.OnValueChanged(nameof(OnDelayChange))]
	public bool isLoop = true;
	public bool playOnEnable = true;
	public float frameDelay = 0.07f;

	private Image image;
	private Sprite[] sprites;
	private WaitForSeconds wait;

	private void Awake()
	{
		image = GetComponent<Image>();
		sprites = Resources.LoadAll<Sprite>(sprite.texture.name);
		OnDelayChange();
	}

	private void OnEnable()
	{
		if (playOnEnable)
		{
			PlayAnimation();
		}
	}

	private void PlayAnimation()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			for (int i = 0; i < sprites.Length; i++)
			{
				image.sprite = sprites[i];
				yield return new WaitForSeconds(frameDelay);
			}
			if (isLoop)
			{
				PlayAnimation();
			}
		}
	}

	void OnDelayChange()
	{
		if (Application.isPlaying)
		{
			wait = new WaitForSeconds(frameDelay);
		}
	}
	void OnTextureAdded()
	{
		image.sprite = sprite;
	}
}
