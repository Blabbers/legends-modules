using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blabbers.Game00
{
	public class UI_FinalScoreToText : MonoBehaviour
	{
		private Text scoreText;
		private TextMeshProUGUI scoreTextM;

		void OnEnable()
		{
			var score = ProgressController.GameProgress.score.ToString();

			scoreText = GetComponent<Text>();
			if (scoreText) scoreText.text = score;

			scoreTextM = GetComponent<TextMeshProUGUI>();
			if (scoreTextM) scoreTextM.text = score;
		}
	}
}