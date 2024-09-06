using System.Collections;
using BeauRoutine;
using Blabbers;
using Blabbers.Game00;
using Fungus;
using NaughtyAttributes;
using UnityEngine;

public class UI_Tutorial : UI_TutorialWindowBase
{
	public float delay;

	public bool pause;
	public bool ignorePause = false;
	[HideIf("ignorePause")]
	public bool ignoreCountdown = false;
	public bool showOnlyOnce;
	public bool autoHideShowHUD;
	[Foldout("Components")]
	public Writer writer;
	[Foldout("Components")]
	public TextLocalized text;
	public float writerDelay = 1.0f;
	public bool fadeoutGameVolume = true;

	public override void ShowScreen()
	{
		//if (showOnlyOnce && UI_TutorialController.AlreadyTriggeredInThisLevel.Contains(gameObject.name)) return;
		//UI_TutorialController.AlreadyTriggeredInThisLevel.Add(gameObject.name);

		if (writerDelay > 0)
		{
			//text.enabled = false;
			text.AllowPlay(false);
		}

		Routine.Start(Run());

		IEnumerator Run()
		{
			if (!this.gameObject.activeSelf)
			{
				if (delay > 0)
				{
					yield return new WaitUntil(() => Application.isFocused);
					yield return Routine.WaitSeconds(delay);
				}

				this.gameObject.SetActive(true);
			}

			Analytics.OnTutorialShown(this.name);
			if (fadeoutGameVolume)
			{
				AudioController.Instance.FadeGameplayVolume(0.1f);
				AudioController.Instance.FadeMusicVolume(AudioController.MusicVolumeHalf);
			}

			if (!ignorePause)
			{
				Singleton.Get<GameplayController>()?.TogglePause(pause);
				if (ignoreCountdown) Singleton.Get<UI_PopupCountdown>()?.SetEnableCountdown(false);
			}

			if (autoHideShowHUD)
			{
				Singleton.Get<UI_GameplayHUD>()?.HideFullHUD();
			}

			// Needs to wait a frame, for execution order purposes.
			if (!writer.HasInitialized)
			{
				yield return new WaitUntil(() => writer.HasInitialized);
			}
			if (writerDelay > 0)
			{
				text.enabled = false;
				yield return new WaitUntil(() => Application.isFocused);
				yield return new WaitForSecondsRealtime(writerDelay);

				text.AllowPlay(true);
				text.enabled = true;
			}
			Routine.Start(writer.Write(text.Localization));
		}
	}

	public override void HideScreen()
	{
		System.GC.Collect();
		this.gameObject.SetActive(false);
		if (Singleton.Get<GameplayController>() != null)
		{
			if (!ignorePause)
			{
				if (ignoreCountdown) Singleton.Get<GameplayController>()?.TogglePause(false, true);
				else Singleton.Get<GameplayController>()?.TogglePause(false);
			}


		}
		if (autoHideShowHUD)
		{
			Singleton.Get<UI_GameplayHUD>()?.ShowFullHUD();
		}
		AudioController.Instance?.FadeResetGameplayVolume(0.25f);
		AudioController.Instance?.FadeResetMusicVolume(0.25f);
	}
}