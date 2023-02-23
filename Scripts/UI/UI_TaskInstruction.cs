using TMPro;
using UnityEngine;

public class UI_TaskInstruction : MonoBehaviour
{
	public TextMeshProUGUI text;
	public UI_ButtonPlayTTS buttonPlayTTS;

	public void Setup(LocalizedString localizedString)
	{
		text.text = localizedString;
		buttonPlayTTS.overrideTTSkey = localizedString.Key;
	}
}
