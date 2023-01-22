using Blabbers.Game00;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class TextLocalized : TextMeshProUGUI
{
	[SerializeField] 
	[HideLocalizationTextArea]
	private LocalizedString localization;
	public LocalizedString Localization => localization;

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
				PlayThisSpeechText();
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
