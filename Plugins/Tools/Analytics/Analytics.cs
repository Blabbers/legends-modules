using System.Collections.Generic;
using Blabbers.Game00;
using UnityEngine;
using UnityEngine.Analytics;

namespace Blabbers
{
    public static class Analytics
    {
        private static readonly Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

        public static void OnGameStart()
        {
            dataDictionary.Clear();
            var isNewGame = GameData.Instance.progress.isNewGame;
            dataDictionary.Add("isNewGame", isNewGame);
            if (!isNewGame)
            {
                dataDictionary.Add("reachedLevel", GameData.Instance.progress.reachedLevel);
            }

            SendEvent("gameStart", dataDictionary);
        }

        public static void OnChooseAutomaticTTS()
        {
            dataDictionary.Clear();
            dataDictionary.Add("enabled", ProgressController.enableAutomaticTTS);
            SendEvent("automaticTTS", dataDictionary);
        }
        
        public static void OnQuestionAnswered(bool answeredCorrectly)
        {
            dataDictionary.Clear();
            dataDictionary.Add("level", GameData.Instance.progress.currentLevelId + 1);
            dataDictionary.Add("isCorrect", answeredCorrectly);
            SendEvent("questionAnswered", dataDictionary);
        }

        public static void OnLevelStart()
        {
            dataDictionary.Clear();
            dataDictionary.Add("level", GameData.Instance.progress.currentLevelId + 1);
            var situation = "isFirstTime";
            if (GameData.Instance.progress.reachedLevel > GameData.Instance.progress.currentLevelId)
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
        
        public static void OnDefeat(string reason)
        {
            dataDictionary.Clear();
            dataDictionary.Add("level", GameData.Instance.progress.currentLevelId + 1);
            dataDictionary.Add("reason", reason);
            SendEvent("defeat", dataDictionary);
        }
        
        public static void OnTutorialShown(string tutorialName)
        {
            dataDictionary.Clear();
            dataDictionary.Add("level", GameData.Instance.progress.currentLevelId + 1);
            dataDictionary.Add("name", tutorialName);
            SendEvent("tutorial", dataDictionary);
        }
        
        public static void OnLevelVictory(int starAmount)
        {
            dataDictionary.Clear();
            dataDictionary.Add("level", GameData.Instance.progress.currentLevelId + 1);
            dataDictionary.Add("duration", Time.timeSinceLevelLoad);
            dataDictionary.Add("starAmount", starAmount);
            SendEvent("levelVictory", dataDictionary);
        }
        
        public static void OnGameFinished()
        {
            dataDictionary.Clear();
            dataDictionary.Add("level", GameData.Instance.progress.currentLevelId + 1);
            dataDictionary.Add("all3Stars", GameData.Instance.progress.AllLevelsWith3Stars());
            dataDictionary.Add("duration", Time.timeSinceLevelLoad);
            dataDictionary.Add("totalScore", GameData.Instance.progress.score);
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
            dataDictionary.Clear();
            var skinSet = "";
            var first = true;
            foreach (var customization in GameData.Instance.progress.customizations)
            {
                skinSet += $"{(first ? "" : ", ")}{customization.name}:{customization.id}";
                if (first) first = false;
            }

            dataDictionary.Add("skinSet", skinSet);
            dataDictionary.Add("reachedLevel", GameData.Instance.progress.currentLevelId + 1);
            dataDictionary.Add("timeSinceStart", Time.realtimeSinceStartup);
            SendEvent("skinSelected", dataDictionary);
        }

        private static void SendEvent(string eventName, Dictionary<string, object> dataDictionary)
        {
            var result = UnityEngine.Analytics.Analytics.CustomEvent(eventName, dataDictionary);
#if UNITY_EDITOR
            var parameters = "";
            foreach (var data in dataDictionary)
            {
                parameters += $"\n[{data.Key}]: {data.Value}";
            }

            Debug.Log(
                $"Analytics → <color=cyan>[{eventName}]</color> event was sent <color=white>[{result.ToString()}]</color>. With the parameters:{parameters}");
#endif
        }
    }
}