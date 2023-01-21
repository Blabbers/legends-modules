using Animancer;
using Blabbers.Game00;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class TextLocalized : TextMeshProUGUI
{
	[HideLocalizationTextArea]
	[SerializeField] LocalizedString localization;

	[SerializeField] bool playTTSOnEnable = false;
	[SerializeField] bool isAnimated = false;
	[SerializeField] UnityEvent OnAnimationFinished;

	protected override void OnEnable()
	{
		var key = "";
		if(localization != null)
		{
			key = localization.Key;
		}

		if (!string.IsNullOrEmpty(key))
		{
			UpdateText();
			if (playTTSOnEnable)
			{
				LocalizationExtensions.PlayTTS(key);
			}
		}
		else
		{
			Debug.LogWarning("Key variable is empty at:" + this.gameObject.name, this.gameObject);
		}
	}

	public void UpdateText()
	{
		if (isAnimated)
		{
			
		}
		else
		{
			//LocalizationExtensions.LocalizeText(localization.Key, null, null, localization.applyColorCodes);
		}



	}


	public void PlayThisSpeechText()
	{
		LocalizationExtensions.PlayTTS(localization.Key);
	}
}
