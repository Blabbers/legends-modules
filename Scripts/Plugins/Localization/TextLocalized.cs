using Blabbers.Game00;
using Fungus;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class TextLocalized : TextMeshProUGUI
{

	//[HideLocalizationTextArea]
	[SerializeField]
	private LocalizedString localization;
	public LocalizedString Localization => localization;

	[SerializeField] bool playTTSOnEnable = false;
	[SerializeField] bool isAnimated = false;
	[SerializeField] UnityEvent OnAnimationFinished;

	protected override void OnEnable()
	{

		Debug.Log("TextLocalized - OnEnable()");

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
		text = LocalizationExtensions.LocalizeText(Localization.Key,null,null);
	}

	public void PlayThisSpeechText()
	{
		LocalizationExtensions.PlayTTS(localization.Key);
	}
}
