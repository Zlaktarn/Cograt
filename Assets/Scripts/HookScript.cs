using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HookScript : MonoBehaviour
{
    GameObject playerObj = default;
    MovementScript player = default;

    private Vector3 shootDir;
    public bool hitTarget = false;
    Rigidbody2D rb;
    Vector3 thisPos;
    AudioSource audioSource;

    [SerializeField]
    LayerMask layerMask;
    Transform mousePos;

    RaycastHit2D hit;

    [SerializeField]
    LineRenderer line;
    Vector2 connectedLinePos = Vector2.zero;

    void Awake()
    {
        player = GetComponent<MovementScript>();
        line.enabled = false;
        //line.SetPosition(0, transform.position);
    }

    void Update()
    {
        player.hookPos = transform.position;


        // Does the ray intersect any objects excluding the player layer


        Vector2 cameraPoint = Input.mousePosition;
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(cameraPoint);

        Vector2 position = transform.position;
        float distance = Vector2.Distance(position, worldPoint);

        RaycastHit2D hit = Physics2D.Raycast(position, worldPoint - position, Mathf.Infinity, layerMask);


        if(Input.GetMouseButtonDown(0))
        {
            if (hit)
            {
                Debug.DrawLine(transform.position, worldPoint, Color.green);
                Debug.DrawRay(position, hit.point - position, Color.red);
                line.enabled = true;
                connectedLinePos = hit.point;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            line.enabled = false;
            line.SetPosition(1, position);
            connectedLinePos = position;
        }
        else
        {
            line.SetPosition(1, connectedLinePos - position);
        }
    }


}
