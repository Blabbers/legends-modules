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

        //Debug.Log($"IconAlternator Awake()");


        if (!mySprite)
        {         
            if(GetComponent<SpriteRenderer>() != null)
			{
                mySprite = GetComponent<SpriteRenderer>();
            }

        }

        //Debug.Log($"IconAlternator Awake() Got sprite");

        if (!myImage)
        {
            if (GetComponent<Image>() != null)
            {
                myImage = GetComponent<Image>();
            }
        }

		if (!Game.IsMobile)
		{
            //Debug.Log($"Device.isMobile: {Singleton.Get<Loader>().isMobile}".Colored("orange"));
            Routine.Start(_IconCoroutine(delay));
        }


		

        //Debug.Log($"IconAlternator Awake() Got image");

        //Routine.Start(_IconCoroutine(delay));
    }

    IEnumerator _IconCoroutine(float delay)
	{
        UpdateSprite(0);

        while (true)
		{
            //Debug.Log("Change sprite");
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
        //Debug.Log($"UpdateSprite {id}");

		if (myImage) myImage.sprite = spriteList[id];
        if (mySprite) mySprite.sprite = spriteList[id];
    }


}
