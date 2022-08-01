using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorWheelLerp : MonoBehaviour
{
    public Gradient gradient;
    public float duration;
    void Start()
    {
        this.GetComponent<Image>().DOGradientColor(gradient, duration).SetLoops(-1);
    }
}
