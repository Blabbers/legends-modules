using Blabbers.Game00;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(+1000)]
public class TextLocalized : TextMeshProUGUI
{
	[LocalizedStringOptions(hideArea: true)]
	[SerializeField]
	private LocalizedString localization;
	public LocalizedString Localization => localization;

	[SerializeField] bool playTTSOnEnable = false;	
	bool applyKeyCodes = false;

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

		base.OnEnable();
	}

	public void UpdateText()
	{
		text = LocalizationExtensions.LocalizeText(Localization.Key, null, null, applyKeyCodes);
	}

	public void PlayThisSpeechText()
	{
		LocalizationExtensions.PlayTTS(localization.Key);
	}

	#region Setters
	public void SetApplyKeyCodes(bool enable)
	{
		applyKeyCodes = enable;
	}

	public void SetPlayTTSOnEnable(bool enable)
	{
		playTTSOnEnable = enable;
	}

	public void SetLocalization(LocalizedString loc)
	{
		localization = loc;
	}
	#endregion

}
