using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Blabbers.Game00
{
    public class UI_AnimatedScore : MonoBehaviour
    {


        [Header("Runtime")]
        public float targetScore;
        public float currentScoreShown;
        public string prefix;
        public string suffix;


        [Header("Configs")]
        public bool hasLerp = false;
        public float lerpSpeed = 1.0f;
        public float textUpscale = 1.25f;
        public float animationDuration = 0.1f;

        private Text scoreText;
        private TextMeshProUGUI scoreTextM;

        Vector3 originalScale, originalPos;

        private void Awake()
        {
            scoreText = GetComponent<Text>();
            scoreTextM = GetComponent<TextMeshProUGUI>();

            originalScale = Vector3.one;
            targetScore = 0;

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

        private void OnDestroy()
        {

        }


		#region External
        public void UpdateScore(int value, string prefix="", string suffix="")
		{
            this.prefix = prefix;
            this.suffix = suffix;

            UpdateTargetScore(value);
        }

        public void ResetScore(float target = 0)
		{
            targetScore = target;
            currentScoreShown = 0;

            if (scoreText)
            {
                transform.localScale = Vector2.one;
            }

            if (scoreTextM)
            {
                transform.localScale = Vector2.one;
            }


            UpdateTextVisual("");
        }
		#endregion


		void UpdateTargetScore(int value)
        {
            targetScore = value;

            if (scoreText)
            {
                //scoreText.transform.DOKill();
                //scoreText.transform.localPosition = originalPos;
                //scoreText.transform.DOPunchPosition(Vector3.one * 20f, 0.2f);

                scoreText.transform.DOKill();
                scoreText.transform.localScale = Vector2.one;
                scoreText.transform.DOScale(textUpscale, animationDuration).OnComplete(() => scoreText.transform.localScale = Vector2.one);
            }

            if (scoreTextM)
            {
                //scoreTextM.transform.DOKill();
                //scoreTextM.transform.localPosition = originalPos;
                //scoreTextM.transform.DOPunchPosition(Vector3.one * 20f, 0.2f);

                scoreTextM.transform.DOKill();
                scoreTextM.transform.localScale = Vector2.one;
                //scoreTextM.transform.DOScale(textUpscale, animationDuration).OnComplete(() => scoreText.transform.localScale = Vector2.one);
                scoreTextM.transform.DOPunchPosition(Vector3.one * 20f, animationDuration);
                //scoreTextM.transform.DOScale(textUpscale, animationDuration).OnComplete(() => scoreText.transform.localScale = Vector2.one);
            }
        }

        void UpdateTextOnly(int value)
        {
            UpdateTextVisual(value.ToString());
        }

        //void UpdateTextVisual(int value)
        //{

        //    //Debug.Log("LevelScore: " + value);

        //    if (scoreText) scoreText.text = $"{prefix}{value}{suffix}";
        //    if (scoreTextM) scoreTextM.text = $"{prefix}{value}{suffix}";
        //}

        void UpdateTextVisual(string text)
		{
            if (scoreText) scoreText.text = $"{prefix}{text}{suffix}";
            if (scoreTextM) scoreTextM.text = $"{prefix}{text}{suffix}";
        }

        private void Update()
        {
            if (!hasLerp) return;

            if (currentScoreShown <= targetScore)
            {
                currentScoreShown = Mathf.Lerp(currentScoreShown, targetScore, Time.deltaTime * lerpSpeed);
                UpdateTextVisual(Mathf.CeilToInt(currentScoreShown).ToString());
            }
        }
    }
}