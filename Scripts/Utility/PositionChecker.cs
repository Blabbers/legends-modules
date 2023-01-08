using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[ExecuteInEditMode]
public class PositionChecker : MonoBehaviour
{
    public Transform obj;
    public Vector3 position;

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (obj == null) return;
        position = obj.position;
    }
#endif
}

