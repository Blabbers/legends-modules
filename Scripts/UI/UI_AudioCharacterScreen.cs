using DG.Tweening;
using Blabbers.Game00;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AudioCharacterScreen : MonoBehaviour
{
	#region Init
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init()
	{
		_instance = null;
	}
	#endregion

	[SerializeField] UI_CharacterBlock characterBlock;
	[SerializeField] Transform originPos;
	[SerializeField] Transform dialoguePos;

	#region Instance
	private static UI_AudioCharacterScreen _instance = null;
	public static UI_AudioCharacterScreen Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = Instantiate(Resources.Load<UI_AudioCharacterScreen>("UI/--Popup--StreamAudioCharacter"));
				_instance.gameObject.name = "--Popup--SpeakingCharacter";
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	#endregion

	#region Test Buttons

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

	#endregion


	public void AnimateIn(float duration)
	{

		gameObject.SetActive(true);
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

	public void ToggleCharacterSpeaking(bool active)
	{

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
			gameObject.SetActive(false);
		}
	}
}