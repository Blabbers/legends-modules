using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_TaskInstruction : MonoBehaviour
{
	public TextMeshProUGUI text;
	public UI_ButtonPlayTTS buttonPlayTTS;
	public CanvasGroup canvasGroup;
	public float fadeOutDuration = 0.5f;

	public void Setup(LocalizedString localizedString)
	{
		canvasGroup.alpha = 1;
		text.text = localizedString;
		buttonPlayTTS.overrideTTSkey = localizedString.Key;
	}

	public void Hide()
	{
		canvasGroup.DOFade(0f, fadeOutDuration).OnComplete(HandleHideComplete);
		void HandleHideComplete()
		{
			this.gameObject.SetActive(false);
		}
	}
}
