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

	[SerializeField] int lastPlayedId = -1;

	[SerializeField] UI_CharacterBlock characterBlock;
	[SerializeField] Transform originPos;
	[SerializeField] Transform dialoguePos;
	bool lastHudState;

	IEnumerator animationOutDelay;
	bool isAnimatingIn;


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

	public int GetNextPlayId()
	{
		lastPlayedId++;
		return lastPlayedId;
	}

	public void ResetPlayId()
	{
		lastPlayedId = 0;
	}

	#region Animation

	public void AnimateIn(float duration)
	{
		isAnimatingIn = true;

		if (animationOutDelay !=null)
		{
			StopCoroutine(animationOutDelay);
		}

		gameObject.SetActive(true);
		characterBlock.gameObject.SetActive(true);

		lastHudState = Singleton.Get<UI_GameplayHUD>().IsActive();
		//Debug.Log($"AnimateIn\nlastHudState: {lastHudState}");

		Singleton.Get<UI_GameplayHUD>().ToggleDisplay(false);



		StartCoroutine(_Delay());
		IEnumerator _Delay()
		{
			yield return new WaitForSeconds(0.5f);
			characterBlock.AnimateIn(dialoguePos.position, duration, FinishAnimationIn);
		}


	}

	void FinishAnimationIn()
	{
		isAnimatingIn = false;
	}

	public void ToggleCharacterSpeaking(bool active)
	{

	}

	public void AnimateOut(float duration, bool showHudOnCharacterOut = true)
	{
		//characterBlock.gameObject.SetActive(true);
		//Singleton.Get<UI_GameplayHUD>().ToggleDisplay(false);

		if (showHudOnCharacterOut) lastHudState = showHudOnCharacterOut;
		else lastHudState = false;

		StartCoroutine(_Delay());
		IEnumerator _Delay()
		{
			yield return new WaitForSeconds(0.25f);
			characterBlock.AnimateOut(originPos.position, duration, () => FinishAnimationOut());
		}
	}


	void FinishAnimationOut()
	{
		//Debug.Log($"FinishAnimationOut\nlastHudState: {lastHudState}");
		Singleton.Get<UI_GameplayHUD>().ToggleDisplay(lastHudState);


		animationOutDelay = _OutDelay();
		StartCoroutine(animationOutDelay);
	} 

	IEnumerator _OutDelay()
	{
		yield return new WaitForSeconds(1.0f);

		if (isAnimatingIn) yield break;

		Debug.Log($"_OutDelay() disable stuff\nisAnimatingIn? {isAnimatingIn}");
		characterBlock.gameObject.SetActive(false);
		gameObject.SetActive(false);

	}


	#endregion
}
