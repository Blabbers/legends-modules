using Blabbers.Game00;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(+1000)]
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
		// Internally, TMPro runs [ExecuteAlways]. But this script should execute only during runtime. 
		if (!Application.isPlaying)
			return;

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
		//text = LocalizationExtensions.LocalizeText(Localization.Key,null,null);
		//text = LocalizationExtensions.LocalizeText(Localization.Key, null, null, localization.ApplyKeyCodes);
		text = LocalizationExtensions.LocalizeText(Localization.Key, null, null, true);
	}

	public void PlayThisSpeechText()
	{
		LocalizationExtensions.PlayTTS(localization.Key);
	}
}
