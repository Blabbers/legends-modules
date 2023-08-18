using DG.Tweening;
using Blabbers.Game00;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AudioCharacterScreen : MonoBehaviour
{
	//[SerializeField] Transform characterBlock;
	//[SerializeField] Transform helpButton;
	//[SerializeField] Transform characterBox;

	[SerializeField] UI_CharacterBlock characterBlock;

	[SerializeField] Transform originPos;
	[SerializeField] Transform dialoguePos;

	[Button]
    void AnimateInTest()
    {
		AnimateIn(0.5f);

	}

	[Button]
	void AnimateOutTest()
	{
		AnimateOut(0.5f);
	}



	public void AnimateIn(float duration)
	{

		characterBlock.gameObject.SetActive(true);
		Singleton.Get<UI_GameplayHUD>().ToggleDisplay(false);

		StartCoroutine(_Delay());
		IEnumerator _Delay()
		{
			yield return new WaitForSeconds(0.5f);
			//characterBlock.DOMove(dialoguePos.position, duration);
			characterBlock.AnimateIn(dialoguePos.position, duration);
		}

		
	}


	public void AnimateOut(float duration)
	{
		//characterBlock.gameObject.SetActive(true);
		//Singleton.Get<UI_GameplayHUD>().ToggleDisplay(false);

		StartCoroutine(_Delay());
		IEnumerator _Delay()
		{
			yield return new WaitForSeconds(0.25f);
			characterBlock.AnimateOut(originPos.position, duration, ()=> FinishAnimationOut());
		}
	}


	void FinishAnimationOut()
	{
		Singleton.Get<UI_GameplayHUD>().ToggleDisplay(true);

		StartCoroutine(_Delay());
		IEnumerator _Delay()
		{
			yield return new WaitForSeconds(0.5f);
			characterBlock.gameObject.SetActive(false);
		}
	}
}
