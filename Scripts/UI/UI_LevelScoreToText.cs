using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blabbers.Game00
{
    public class UI_LevelScoreToText : MonoBehaviour
    {
        private Text scoreText;
        private TextMeshProUGUI scoreTextM;

        private void Awake()
        {
            scoreText = GetComponent<Text>();
            scoreTextM = GetComponent<TextMeshProUGUI>();
            ProgressController.OnLevelScoreChanged.AddListener(UpdateScore);
        }

        private void Start()
        {
            var score = ProgressController.GameProgress.CurrentLevel.score;
            UpdateScore(score);
        }

        private void OnDestroy()
        {
            ProgressController.OnLevelScoreChanged.RemoveListener(UpdateScore);
        }

        void UpdateScore(int value)
        {
            if (ProgressController.GameProgress.levels == null)
                return;

            Debug.Log("LevelScore: " + value);

            if (scoreText) scoreText.text = value.ToString();
            if (scoreTextM) scoreTextM.text = value.ToString();
        }
    }
}