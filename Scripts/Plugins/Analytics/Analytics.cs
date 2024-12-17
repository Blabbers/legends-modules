using Blabbers.Game00;
using System.Collections.Generic;
using UnityEngine;

namespace Blabbers
{
	public static class Analytics
	{
		private static Dictionary<string, object> dataDictionary = new Dictionary<string, object>();


		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void Init()
		{
			dataDictionary = new Dictionary<string, object>();
		}


		public static void OnGameStart()
		{
			dataDictionary.Clear();
			var isNewGame = GameData.Instance.Progress.isNewGame;
			dataDictionary.Add("isNewGame", isNewGame);
			if (!isNewGame)
			{
				dataDictionary.Add("reachedLevel", GameData.Instance.Progress.reachedLevel);
			}
			SendEvent("gameStart", dataDictionary);
		}

		public static void OnLevelStart()
		{
			dataDictionary.Clear();
			dataDictionary.Add("level", GameData.Instance.Progress.currentLevelId + 1);
			var situation = "isFirstTime";
			if (GameData.Instance.Progress.reachedLevel > GameData.Instance.Progress.currentLevelId)
			{
				situation = "isReplaying";
			}
			else if (SceneLoader.isStuckOnThisLevel)
			{
				situation = "isStuck";
			}
			dataDictionary.Add("situation", situation);
			SendEvent("levelStart", dataDictionary);
		}

		public static void OnSubLevelStart(string minigame, int minigameId)
		{
			dataDictionary.Clear();
			dataDictionary.Add("level", $"{GameData.Instance.Progress.currentLevelId + 1}_{minigame}_{minigameId}");
			var situation = "isFirstTime";
			if (GameData.Instance.Progress.reachedLevel > GameData.Instance.Progress.currentLevelId)
			{
				situation = "isReplaying";
			}
			else if (SceneLoader.isStuckOnThisLevel)
			{
				situation = "isStuck";
			}
			dataDictionary.Add("situation", situation);
			SendEvent("levelStart", dataDictionary);
		}

		public static void OnSubLevelVictory(string minigame, int minigameId)
		{
			dataDictionary.Clear();
			dataDictionary.Add("level", $"{GameData.Instance.Progress.currentLevelId + 1}_{minigame}_{minigameId}");
			var situation = "isFirstTime";
			if (GameData.Instance.Progress.reachedLevel > GameData.Instance.Progress.currentLevelId)
			{
				situation = "isReplaying";
			}
			else if (SceneLoader.isStuckOnThisLevel)
			{
				situation = "isStuck";
			}
			dataDictionary.Add("situation", situation);
			SendEvent("levelVictory", dataDictionary);

		}

		public static void OnDefeat(string reason)
		{
			dataDictionary.Clear();
			dataDictionary.Add("level", GameData.Instance.Progress.currentLevelId + 1);
			dataDictionary.Add("reason", reason);
			SendEvent("defeat", dataDictionary);
		}

		public static void OnTutorialShown(string tutorialName)
		{
			dataDictionary.Clear();
			dataDictionary.Add("level", GameData.Instance.Progress.currentLevelId + 1);
			dataDictionary.Add("name", tutorialName);
			SendEvent("tutorial", dataDictionary);
		}

		public static void OnLevelVictory(int starAmount)
		{
			dataDictionary.Clear();
			dataDictionary.Add("level", GameData.Instance.Progress.currentLevelId + 1);
			dataDictionary.Add("duration", Time.timeSinceLevelLoad);
			dataDictionary.Add("starAmount", starAmount);
			var popupQuestion = Singleton.Get<UI_PopupQuestion>();
			if (popupQuestion)
			{
				var hasQuestion = popupQuestion.QuestionWasAnsweredThisLevel;
				var answeredCorrectly = popupQuestion.ChoseCorrectly;
				if (hasQuestion && answeredCorrectly)
				{
					dataDictionary.Add("correctAnswer", answeredCorrectly);
				}
			}
			SendEvent("levelVictory", dataDictionary);
		}

		public static void OnGameFinished()
		{
			dataDictionary.Clear();
			dataDictionary.Add("level", GameData.Instance.Progress.currentLevelId + 1);
			dataDictionary.Add("all3Stars", GameData.Instance.Progress.AllLevelsWith3Stars());
			dataDictionary.Add("duration", Time.timeSinceLevelLoad);
			dataDictionary.Add("totalScore", GameData.Instance.Progress.score);
			SendEvent("gameFinished", dataDictionary);
		}

		public static void OnCutsceneStart(string cutsceneTitle)
		{
			dataDictionary.Clear();
			dataDictionary.Add("cutscene", cutsceneTitle);
			dataDictionary.Add("timeSinceStart", Time.realtimeSinceStartup);
			SendEvent("cutsceneStart", dataDictionary);
		}

		public static void OnCutsceneEnd(string cutsceneTitle)
		{
			dataDictionary.Clear();
			dataDictionary.Add("cutscene", cutsceneTitle);
			dataDictionary.Add("timeSinceStart", Time.realtimeSinceStartup);
			dataDictionary.Add("duration", Time.timeSinceLevelLoad);
			SendEvent("cutsceneEnd", dataDictionary);
		}

		public static void OnSkinSelected()
		{
			return;
			// Disabled for now
			dataDictionary.Clear();
			var skinSet = "";
			var first = true;
			foreach (var customization in GameData.Instance.Progress.customizations)
			{
				skinSet += $"{(first ? "" : ", ")}{customization.name}:{customization.id}";
				if (first) first = false;
			}

			dataDictionary.Add("skinSet", skinSet);
			dataDictionary.Add("reachedLevel", GameData.Instance.Progress.currentLevelId + 1);
			dataDictionary.Add("timeSinceStart", Time.realtimeSinceStartup);
			dataDictionary.Add("enabledTTS", ProgressController.enableAutomaticTTS);
			SendEvent("skinSelected", dataDictionary);
		}

		private static void SendEvent(string eventName, Dictionary<string, object> dataDictionary)
		{

#if UNITY_EDITOR || DEVELOPMENT_BUILD
			var parameters = "";
			foreach (var data in dataDictionary)
			{
				parameters += $"\n[{data.Key}]: {data.Value}";
			}

			Debug.Log(
				$"Analytics → <color=cyan>[{eventName}]</color> event was sent. With the parameters:{parameters}");
#else
            
            //UnityEngine.Analytics.Analytics.CustomEvent(eventName, dataDictionary);
#endif



		}
	}
}