using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void Start()
    {
        
    }

    void Update()
    {
        this.transform.position = target.position + offset;
    }
}
