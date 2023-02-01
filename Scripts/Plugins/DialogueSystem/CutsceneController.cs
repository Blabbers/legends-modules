using BeauRoutine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;
using UnityEngine.Events;

namespace Blabbers.Game00
{
	public class CutsceneController : MonoBehaviour, ISingleton
	{
		public bool HasCutscene = false;
		public static CutsceneController Instance => Singleton.Get<CutsceneController>();
		
		public SceneReference sceneToLoadAfter;
		public float musicVolume = 0.05f;
		public PlayableDirector playableDirector;
		[Header("Analytics")]
		public string cutsceneNameKey = "cutscene-start";
		public UnityEvent OnStart;

		public void OnCreated()
		{
		}

		private void Start()
		{
			if (!HasCutscene)
			{
				//Debug.Log("Jumping cutscene as requested.".Colored("yellow"), this);
				LoadNextScene();
				return;
			}

			Singleton.Get<AudioController>().FadeMusicVolume(musicVolume, 0.5f);
			Analytics.OnCutsceneStart(cutsceneNameKey);

			Fade.Out(0.2f);
			OnStart?.Invoke();
		}

		private void Update()
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			if (Input.GetKeyDown(KeyCode.End))
			{
				LoadNextScene();
			}
#endif
		}

		public void PauseTimeline(bool value)
		{
			if (value)
			{
				//Pause
				playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
			}
			else
			{
				//Resume
				playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
			}
		}

		public void FinishCutscene()
		{
			Routine.Start(Run());

			IEnumerator Run()
			{
				Fade.In(0.5f);
				yield return Routine.WaitSeconds(2f);
				Analytics.OnCutsceneEnd(cutsceneNameKey);
				LoadNextScene();
			}
		}

		public void LoadNextScene()
		{
			if (sceneToLoadAfter != null)
			{
				Singleton.Get<SceneLoader>().LoadSceneByName(sceneToLoadAfter);
			}
			else
			{
				//Debug.Log("There's no scene to load for this cutscene.".Colored("red"), this);
			}
		}

		public void ScreenShake(float duration)
		{
			Camera.main.DOShakePosition(duration, 1);
			//Debug.Log("Screen Shaked");
		}

		public void DebugLog(string text)
		{
			Debug.Log(text);
		}
	}
}