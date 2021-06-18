using UnityEngine;
using System.Collections;

public class SmoothFollow2D : MonoBehaviour
{
    public Transform target;
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    float smoothSpeed = 0.125f;

    void Start()
    {
        transform.position = new Vector3(target.position.x, target.position.y, -offset.z);
        //if (target == null)
        //{
        //    target = GameObject.FindGameObjectWithTag("Player").transform;
        //}
    }

    void FixedUpdate()
    {
        if (target)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, -offset.z);

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}