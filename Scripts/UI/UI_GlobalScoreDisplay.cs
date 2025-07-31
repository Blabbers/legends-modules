using System.Collections;
using System.Collections.Generic;
using Blabbers.Game00;
using NaughtyAttributes;
using UnityEngine;

public class UI_GlobalScoreDisplay : MonoBehaviour
{
    [BoxGroup("Configs")]
    [SerializeField]
    float startDelay = 0f;

    [Foldout("Components")]
    [SerializeField]
    GameObject display;

    [Foldout("Components")]
    [SerializeField]
    UI_ProgressiveScoreDisplay control;

    [SerializeField]
    int score;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_Delay());
        IEnumerator _Delay()
        {
            if (startDelay > 0)
                yield return new WaitForSeconds(startDelay);
            else
                yield return new WaitForEndOfFrame();

            if (display != null)
            {
                display.SetActive(true);
            }

            score = ProgressController.GameProgress.score;
            if (control)
            {
                control.UpdateScore(score, Mathf.RoundToInt(score / 2));
            }
        }
    }
}
