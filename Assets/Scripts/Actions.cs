using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Actions : MonoBehaviour
{
    MovementScript player = default;

    [SerializeField]
    LayerMask layerMask;

    Vector2 position;

    LineRenderer line;
    Vector2 connectedLinePos;
    DistanceJoint2D hooker = default;

    bool groundSlamming = false;
    public bool smashing = false;
    [SerializeField]
    float groundSlamVelocity = 8.0f;
    [SerializeField]
    float groundSlamInterval = 0.2f;
    float groundSlamTimer = 0.0f;

    Rigidbody2D rb;

    bool isGrounded;

    bool hooked;

    void Awake()
    {
        player = GetComponent<MovementScript>();
        line = GameObject.Find("HookLine").GetComponent<LineRenderer>();
        line.enabled = false;
        hooker = GetComponent<DistanceJoint2D>();
        hooker.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        //line.SetPosition(0, transform.position);
    }

    void Update()
    {
        isGrounded = player.IsGrounded();


         position = transform.position;

        Hook();
        GroundSlam();
    }

    void Hook()
    {
        Vector2 cameraPoint = Input.mousePosition;
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(cameraPoint);
        RaycastHit2D hit = Physics2D.Raycast(position, worldPoint - position, Mathf.Infinity, layerMask);

        if (Input.GetMouseButtonDown(0))
        {
            if (hit)
            {
                Debug.DrawLine(transform.position, worldPoint, Color.green);
                Debug.DrawRay(position, hit.point - position, Color.red);
                connectedLinePos = hit.point;
                line.SetPosition(1, connectedLinePos - position);
                hooker.connectedAnchor = connectedLinePos;
                ActivateHook(true);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ActivateHook(false);
            line.SetPosition(1, position);
            connectedLinePos = position;
        }
        else
        {
            line.SetPosition(1, connectedLinePos - position);
            hooker.connectedAnchor = connectedLinePos;
        }
    }

    void ActivateHook(bool isHooking)
    {
        line.enabled = isHooking;
        hooker.enabled = isHooking;
        hooked = isHooking;
    }

    void GroundSlam()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isGrounded && !groundSlamming/* && !hooked*/)
        {
            groundSlamTimer = 0;
            groundSlamming = true;
        }

        if (groundSlamming)
            GroundSlamMechanics();

        if(isGrounded || hooked)
            smashing = false;
    }

    void GroundSlamMechanics()
    {
        float t = Time.deltaTime;

        groundSlamTimer += t;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (groundSlamTimer >= groundSlamInterval)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.velocity = Vector2.up * -groundSlamVelocity;

            smashing = true;
            groundSlamming = false;
        }
    }
}
