using Blabbers.Game00;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UI_ProgressiveScoreDisplay : MonoBehaviour
{
	[Header("Configs")]
	public bool hasLerp = false;
	public float lerpSpeed = 1.0f;

	private Text scoreText;
	private TextMeshProUGUI scoreTextM;
	private float targetScore;
	private float currentScoreShown;
	Vector3 originalScale, originalPos;


	private void Awake()
	{
		scoreText = GetComponent<Text>();
		scoreTextM = GetComponent<TextMeshProUGUI>();

		originalScale = Vector3.one;



		//if (hasLerp)
		//{
		//	ProgressController.OnLevelScoreChanged.AddListener(UpdateTargetScore);
		//}
		//else
		//{
		//	ProgressController.OnLevelScoreChanged.AddListener(UpdateTextOnly);
		//}


		if (scoreText)
		{
			scoreText.transform.localScale = Vector3.one;
			originalPos = scoreText.transform.localPosition;
		}
		if (scoreTextM)
		{
			scoreTextM.transform.localScale = Vector3.one;
			originalPos = scoreTextM.transform.localPosition;
		}

	}


	public void UpdateScore(int targetValue, int startValue = 0)
	{
		if (hasLerp)
		{
			UpdateTextOnly(startValue);
			UpdateTargetScore(targetValue);
		}
		else
		{
			UpdateTextOnly(targetValue);
		}
	}


	 void UpdateTargetScore(int value)
	{
		if (ProgressController.GameProgress.levels == null)
			return;

		targetScore = value;

		if (scoreText)
		{
			scoreText.transform.DOKill();
			scoreText.transform.localPosition = originalPos;
			//scoreText.transform.DOPunchPosition(Vector3.one * 20f, 0.2f); 
		}

		if (scoreTextM)
		{
			scoreTextM.transform.DOKill();
			scoreTextM.transform.localPosition = originalPos;
			//scoreTextM.transform.DOPunchPosition(Vector3.one * 20f, 0.2f);
		}

	}

	void UpdateTextOnly(int value)
	{
		if (ProgressController.GameProgress.levels == null)
			return;

		UpdateTextVisual(value);
	}


	void UpdateTextVisual(int value)
	{

		//Debug.Log("LevelScore: " + value);

		if (scoreText) scoreText.text = value.ToString();
		if (scoreTextM) scoreTextM.text = value.ToString();
	}

	private void Update()
	{
		if (!hasLerp) return;

		if (currentScoreShown <= targetScore)
		{
			currentScoreShown = Mathf.Lerp(currentScoreShown, targetScore, Time.deltaTime * lerpSpeed);
			UpdateTextVisual(Mathf.CeilToInt(currentScoreShown));
		}
	}
}
