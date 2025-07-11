using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NaughtyAttributes;
using UnityEngine;

public class UI_FollowWorldComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    public Transform target;

    [SerializeField]
    Camera mainCam;

    //[HorizontalLine(color: EColor.White)]

    [BoxGroup("Movement")]
    [SerializeField]
    bool lerpToPos = false;

    [BoxGroup("Movement")]
    [ShowIf(nameof(lerpToPos))]
    [SerializeField]
    [MinValue(10)]
    float lerpSpeed;

    [BoxGroup("Offset")]
    [SerializeField]
    bool hasPositionOffset = true;

    [BoxGroup("Offset")]
    [ShowIf(nameof(hasPositionOffset))]
    [SerializeField]
    Vector2 positionOffset;

    [BoxGroup("Bounds")]
    [SerializeField]
    bool clampAtBounds = true;

    [BoxGroup("Bounds")]
    [ShowIf(nameof(clampAtBounds))]
    [SerializeField]
    [Range(0, 0.25f)]
    float paddingX;

    [BoxGroup("Bounds")]
    [ShowIf(nameof(clampAtBounds))]
    [SerializeField]
    [Range(0, 0.25f)]
    float paddingY;

    //[ShowIf(nameof(clampAtBounds))][SerializeField] Vector2 boundsPadding = Vector2.zero;

    Transform transformUI;

    private void Awake()
    {
        transformUI = this.transform;
        if (!mainCam)
            mainCam = Camera.main;
    }

    private void Update()
    {
        PositionUI();
    }

    void PositionUI()
    {
        Vector2 targetPos;

        //targetPos = CameraToWorldUtility.WorldToCameraPosition_Clamped(target, mainCam, boundsPadding.x, boundsPadding.y);]
        if (clampAtBounds)
        {
            targetPos = CameraToWorldUtility.WorldToCameraPosition_Clamped(
                target,
                mainCam,
                positionOffset.x,
                positionOffset.y,
                paddingX,
                paddingY
            );
        }
        else
        {
            targetPos = CameraToWorldUtility.WorldToCameraPosition(
                target,
                mainCam,
                positionOffset.x,
                positionOffset.y
            );
        }

        if (lerpToPos)
        {
            transformUI.position = Vector3.MoveTowards(
                transformUI.position,
                targetPos,
                Time.deltaTime * lerpSpeed
            );
        }
        else
        {
            transformUI.position = targetPos;
        }
    }
}
