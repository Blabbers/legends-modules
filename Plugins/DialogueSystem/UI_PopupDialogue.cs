using System;
using System.Collections;
using BeauRoutine;
using Blabbers.Game00;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopupDialogue : UI_PopupWindow, ISingleton
{
	public Sprite detectivePortrait;
	public Sprite guardPortrait;
	public Image portraitImg;
	public TextMeshProUGUI text;

	private bool firstTime;
	private Vector3 initialPortraitPos;

	public TextMeshProUGUI textContinue;

	private string finalText;
	[SerializeField] private bool allowContinue;
	[SerializeField] private bool allowSkip;
	private CharacterSay currentCharacterSay;

	private Sprite overrideSprite;

	public void OverrideSprite(Sprite newSprite)
	{
		overrideSprite = newSprite;
	}
	public void Execute(CharacterSay characterSay, bool allowContinue, bool allowSkip = true)
	{
		currentCharacterSay = characterSay;
		this.allowContinue = allowContinue;
		this.allowSkip = allowSkip;

		finalText = LocalizationExtensions.LocalizeText(characterSay.key);
		if (firstTime)
		{
			firstTime = false;
			initialPortraitPos = portraitImg.transform.position;
		}

		ShowPopup();

		portraitImg.sprite = overrideSprite != null ? overrideSprite : characterSay.character;
		portraitImg.transform.DOKill();
		portraitImg.DOKill();

		portraitImg.DOFade(0f, 0f);
		portraitImg.transform.position = initialPortraitPos - Vector3.up * 100f;
		var duration = 0.3f;
		portraitImg.transform.DOMove(initialPortraitPos, duration);
		portraitImg.DOFade(1f, duration * 0.3f);

		text.text = "";
		ReproduceText(characterSay);
	}

	public override void ShowPopup()
	{
		base.ShowPopup();
	}

	public override void HidePopup()
	{
		if (currentCharacterSay)
		{
			currentCharacterSay.OnIsOver?.Invoke();
			currentCharacterSay = null;
		}

		base.HidePopup();
	}

	private void Update()
	{
		if (allowContinue && Input.anyKeyDown)
		{
			//Debug.Log("<UI_PopupDialogue> Valid input");

			if (textContinue.gameObject.activeSelf)
			{
				HidePopup();
			}
			else if (started && allowSkip)
			{
				Stop();
				textContinue.gameObject.SetActive(true);
				text.text = finalText;
			}
		}


#if UNITY_EDITOR || DEVELOPMENT_BUILD
		if (Input.GetKey(KeyCode.PageDown))
		{
			if (textContinue.gameObject.activeSelf)
			{
				HidePopup();
			}
			else if (started)
			{
				Stop();
				textContinue.gameObject.SetActive(true);
				text.text = finalText;
			}
		}
#endif


	}


	public PauseInfo pauseInfo;

	private Coroutine speechRoutine;

	private void Stop()
	{
		textContinue.gameObject.SetActive(false);
		if (speechRoutine != null)
		{
			StopCoroutine(speechRoutine);
			speechRoutine = null;
		}
	}

	private bool started;

	private void ReproduceText(CharacterSay characterSay)
	{
		Stop();
		var narratorText = finalText;
		started = false;
		var actualText = "";
		var index = 0;

		speechRoutine = StartCoroutine(Routine());

		IEnumerator Routine()
		{
			yield return new WaitForSecondsRealtime(0.3f);
			started = true;
			//if didnt read all letters
			while (index < narratorText.Length)
			{
				//get one letter
				char letter = narratorText[index];

				//update on screen
				text.text = Write(letter);
				index++;

				switch (letter)
				{
					case ':':
					case '?':
					case '!':
					case '.':
						yield return new WaitForSecondsRealtime(pauseInfo.dotPause);
						break;
					case ';':
					case ',':
						yield return new WaitForSecondsRealtime(pauseInfo.commaPause);
						break;
					case ' ':
						yield return new WaitForSecondsRealtime(pauseInfo.spacePause);
						break;
					case '<':
						// if we find a start of a tag, we dont pause until we find the end of it
						var maxSearch = 2048;
						while (index < narratorText.Length && maxSearch > 0)
						{
							var possibleEndTag = narratorText[index];							
							text.text = Write(possibleEndTag);
							index++;
							if (possibleEndTag == '>')
							{
								break;
							}							
							maxSearch--;
						}
						break;
					default:
						yield return new WaitForSecondsRealtime(pauseInfo.normalPause);
						break;
				}
			}

			yield return new WaitForSecondsRealtime(0.2f);
			if (allowContinue) textContinue.gameObject.SetActive(true);
		}

		string Write(char letter)
		{
			actualText += letter;
			return actualText;
		}
	}
}

[Serializable]
public class PauseInfo
{
	public float dotPause;
	public float commaPause;
	public float spacePause;
	public float normalPause;
}