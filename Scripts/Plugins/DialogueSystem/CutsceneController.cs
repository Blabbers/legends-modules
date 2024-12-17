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
		public bool fadeMusicVolume = true;
		public float musicVolume = 0.05f;
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

			if (fadeMusicVolume)
			{
				Singleton.Get<AudioController>().FadeMusicVolume(musicVolume);
			}
			Analytics.OnCutsceneStart(cutsceneNameKey);

			//Fade.Out(3f);
			Fade.In(3f);
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

		}

		public void FinishCutscene()
		{
			Analytics.OnCutsceneEnd(cutsceneNameKey);
			LoadNextScene();			
		}

		public void LoadNextScene()
		{
			if (sceneToLoadAfter != null)
			{

				string sceneName = ConvertScenePathToName(sceneToLoadAfter.ScenePath);
				Debug.Log($"CutsceneController.LoadNextScene()\nScene name: {sceneName}");

				if (sceneName.Equals("level-select"))
				{
					Singleton.Get<SceneLoader>().LoadLevelSelectScene();
				}
				else
				{
					Singleton.Get<SceneLoader>().LoadSceneByName(sceneToLoadAfter);
				}
			}
			else
			{
				//Debug.Log("There's no scene to load for this cutscene.".Colored("red"), this);
			}
		}

		private string ConvertScenePathToName(string path)
		{
			string sceneName;
			string[] split;
			string[] split2;

			split = path.Split('/');
			sceneName = split[split.Length - 1];

			split2 = sceneName.Split('.');

			return split2[0];
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