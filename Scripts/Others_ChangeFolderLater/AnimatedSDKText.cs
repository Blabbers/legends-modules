using Blabbers.Game00;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AnimatedSDKText : MonoBehaviour
{

	[Header("Configs")]
	public bool isAnimated = true;
	public PauseInfo pauseInfo;
	public UnityEvent onAnimationFinished;

	[Header("Components")]
	public TextMeshProUGUI myText;
	public LoadSDKText loadSDK;

	private bool started;
	bool hasTriggered = false;
	private Coroutine speechRoutine;

	#region Initialization
	private void Awake()
	{
		LoadPossibleTextComponents();
		//if (isAnimated) ReproduceText(myText);
	}

	private void LoadPossibleTextComponents()
	{
		if (!myText)
		{
			myText = GetComponent<TextMeshProUGUI>();
		}

		if (!loadSDK)
		{
			loadSDK = GetComponent<LoadSDKText>();
		}
	} 
	#endregion

	private void ReproduceText(TextMeshProUGUI text)
	{
		Debug.Log($"ReproduceText: {text.text}".Colored("white"));
		var narratorText = text.text;
		text.text = "";



		hasTriggered = true;
		Stop();
		
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
			onAnimationFinished?.Invoke();

			//if (allowContinue) textContinue.gameObject.SetActive(true);
		}

		string Write(char letter)
		{
			actualText += letter;
			return actualText;
		}
	}

	private void Stop()
	{
		//textContinue.gameObject.SetActive(false);
		if (speechRoutine != null)
		{
			StopCoroutine(speechRoutine);
			speechRoutine = null;
		}
	}

	// Update is called once per frame
	void Update()
    {
		if (hasTriggered || !isAnimated) return;
		if (loadSDK.hasLoadedKey) ReproduceText(myText);


	}
}

