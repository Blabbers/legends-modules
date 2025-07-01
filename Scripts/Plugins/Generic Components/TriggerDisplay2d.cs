using System;
using System.Collections;
using System.Collections.Generic;
using Blabbers;
using NaughtyAttributes;
using UnityEngine;

public class TriggerDisplay2d : MonoBehaviour
{
    public GenericTriggerDisplay triggerDisplay;

    [Button]
    void GetTriggerData()
    {
        //if (!triggerDisplay.transform) triggerDisplay.transform = this.transform;

        if (!triggerDisplay.transform)
        {
            if (FindValidCollider())
            {
                triggerDisplay.GetTriggerData();
            }
        }
        else
        {
            if (triggerDisplay.transform.gameObject.GetComponent<Collider2D>() == null)
            {
                if (FindValidCollider())
                {
                    triggerDisplay.GetTriggerData();
                }
            }

            if (triggerDisplay.collider == null)
            {
                if (FindValidCollider())
                {
                    triggerDisplay.GetTriggerData();
                }
            }
        }
    }

    bool FindValidCollider()
    {
        bool hasCollider = false;

        if (gameObject.GetComponent<Collider2D>() != null)
        {
            hasCollider = true;
            triggerDisplay.transform = transform;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Collider2D>() != null)
            {
                hasCollider = true;
                triggerDisplay.transform = transform.GetChild(i);

                break;
            }
        }

        return hasCollider;
    }

    private void OnDrawGizmos()
    {
        triggerDisplay.DrawGizmos();
    }
}

[Serializable]
public class GenericTriggerDisplay
{
    [Header("Configs")]
    public TriggerShape2d triggerShape;
    public Color gizmosColor = Color.green;
    public Color borderColor = Color.green;

    [Header("Components")]
    public Transform transform;
    public Transform parent;
    public Collider2D collider;

    [SerializeField]
    BoxCollider2D boxCollider;

    [SerializeField]
    CircleCollider2D circleCollider;

    public void GetTriggerData()
    {
        if (!transform)
            return;
        if (!collider)
            collider = transform.GetComponent<Collider2D>();
        if (!parent)
            parent = transform.parent;

        if (!parent)
            parent = transform;

        if (transform.GetComponent<BoxCollider2D>() != null)
        {
            boxCollider = transform.GetComponent<BoxCollider2D>();
            triggerShape = TriggerShape2d.Box;
        }

        if (transform.GetComponent<CircleCollider2D>() != null)
        {
            circleCollider = transform.GetComponent<CircleCollider2D>();
            triggerShape = TriggerShape2d.Circle;
        }
    }

    //public BoxCollider2D collider;

    public void DrawGizmos()
    {
        if (boxCollider != null)
        {
            float scaleX = boxCollider.size.x * parent.localScale.x;
            float scaleY = boxCollider.size.y * parent.localScale.y;

            if (this.transform.localScale.x > 0)
            {
                scaleX = boxCollider.size.x * parent.localScale.x * transform.localScale.x;
                scaleY = boxCollider.size.y * parent.localScale.y * transform.localScale.y;
            }

            GizmosUtility.DrawWireRectangle2d(
                transform.position,
                new Vector2(scaleX, scaleY),
                0,
                borderColor
            );
            GizmosUtility.DrawRectangle(
                transform.position,
                new Vector2(scaleX, scaleY),
                gizmosColor,
                collider.offset
            );
        }

        if (circleCollider != null)
        {
            GizmosUtility.DrawSphere(
                transform.position,
                circleCollider.radius,
                gizmosColor,
                new Vector3(0, 0, 10),
                0.25f
            );
        }
    }
}
