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
	public bool showOnlyOnce;
    public bool autoHideShowHUD;
    [Foldout("Components")]
    public Writer writer;
    [Foldout("Components")]
    public TextLocalized text;

    public override void ShowScreen()
    {
        //if (showOnlyOnce && UI_TutorialController.AlreadyTriggeredInThisLevel.Contains(gameObject.name)) return;
        //UI_TutorialController.AlreadyTriggeredInThisLevel.Add(gameObject.name);

        AudioController.Instance.FadeGameplayVolume(0.1f);

        Routine.Start(Run());

        IEnumerator Run()
        {
            if (delay > 0)
            {
                yield return Routine.WaitSeconds(delay);
            }

            this.gameObject.SetActive(true);
            Analytics.OnTutorialShown(this.name);

            //Debug.Log($"<UI_TutorialLevel1> ShowScreen(): {pause}");

            if(!ignorePause) Singleton.Get<GameplayController>()?.TogglePause(pause);

			if (autoHideShowHUD)
            {
                Singleton.Get<UI_GameplayHUD>()?.HideFullHUD();                
            }

			// Needs to wait a frame, for execution order purposes.
			if (!writer.HasInitialized)
			{
                yield return new WaitUntil(() => writer.HasInitialized);
			}
            StartCoroutine(writer.Write(text.Localization));
        }
    }

    public override void HideScreen()
    {
	    System.GC.Collect();
        this.gameObject.SetActive(false);
        if (Singleton.Get<GameplayController>() != null)
        {
			if (!ignorePause) Singleton.Get<GameplayController>()?.TogglePause(false);
		}
		if (autoHideShowHUD)
		{
            Singleton.Get<UI_GameplayHUD>()?.ShowFullHUD();
        }
		AudioController.Instance?.FadeResetGameplayVolume(0.25f);
    }
}