using BeauRoutine;
using Blabbers.Game00;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconAlternator : MonoBehaviour
{
    public SpriteRenderer mySprite;
    public Image myImage;

    public List<Sprite> spriteList;
    float delay = 0.75f;
    [SerializeField] int id = 0;


    private void Awake()
	{
        if (!mySprite)
        {         
            if(GetComponent<SpriteRenderer>() != null)
			{
                mySprite = GetComponent<SpriteRenderer>();
            }

        }

        if (!myImage)
        {
            if (GetComponent<Image>() != null)
            {
                myImage = GetComponent<Image>();
            }
        }

		if (!Game.IsMobile)
		{
            Routine.Start(_IconCoroutine(delay));
        }

    }

    IEnumerator _IconCoroutine(float delay)
	{
        UpdateSprite(0);

        while (true)
		{
            yield return new WaitForSecondsRealtime(delay);

            id++;

            if (id >= spriteList.Count)
            {
                id = 0;
            }

            UpdateSprite(id);
        }
    }

    void UpdateSprite(int id)
	{

		if (myImage) myImage.sprite = spriteList[id];
        if (mySprite) mySprite.sprite = spriteList[id];
    }


}
