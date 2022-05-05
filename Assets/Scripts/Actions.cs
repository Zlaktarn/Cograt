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
    Vector2 hookDestination;
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
    public GameObject hookObject;
    public float hookShootSpeed = 50;
    public float hookReturnSpeed = 80;
    public float hookLength = 10;
    public float hookInterval = 0;
    float hookDistance = 0;
    Vector2 prevHookPos;
    bool hookDestinationReached;
    float step = 0;

    float timer = 0;

    RaycastHit2D hit;

    Vector2 hookPos;

    void Awake()
    {
        player = GetComponent<MovementScript>();
        hooker = GetComponent<DistanceJoint2D>();
        rb = GetComponent<Rigidbody2D>();

        line = GameObject.Find("HookLine").GetComponent<LineRenderer>();
        hookPos = hookObject.transform.position;
        ActivateHook(false);
    }

    void Update()
    {
        isGrounded = player.IsGrounded();
        position = transform.position;

        Hook();
        GroundSlam();

        hookObject.transform.position = hookPos;
    }

    void Hook()
    {
        Vector2 cameraPoint = Input.mousePosition;
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(cameraPoint);

        if (Input.GetMouseButtonDown(0))
        {
            hit = Physics2D.Raycast(position, worldPoint - position, hookLength, layerMask);
            timer = 0;
            hookDestinationReached = false;
            prevHookPos = transform.position;

            if (hit)
            {
                Debug.DrawLine(transform.position, worldPoint, Color.green);
                Debug.DrawRay(position, hit.point - position, Color.red);
                hookDestination = hit.point;
                hookDistance = Vector2.Distance(prevHookPos, hookDestination);
            }
            else
            {
                Vector2 direction = (position - worldPoint).normalized;
                hookDestination = position - direction * hookLength;
                hookDistance = Vector2.Distance(prevHookPos, hookDestination);
            }
            
            hookInterval = hookDistance / hookShootSpeed;
            hookPos = Vector3.zero;
            hookObject.SetActive(true);
        }
        else if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;

            if(hookPos != hookDestination && !hookDestinationReached)
                hookPos = Vector2.MoveTowards(prevHookPos, hookDestination, hookShootSpeed * timer);

            line.enabled = true;
            line.SetPosition(1, hookPos - position);

            if (timer >= hookInterval)
            {
                hookDestinationReached = true;

                if (hit)
                {
                    hooked = true;
                    hooker.connectedAnchor = hookDestination;
                    hooker.enabled = true;
                }
                else
                    if(timer >= hookInterval + 0.2f)
                        ReturnHook();
            }
        }
        else
            ReturnHook();
    }

    void ReturnHook()
    {
        hooked = false;
        hooker.enabled = false;

        line.SetPosition(1, hookPos - position);
        hookPos = Vector2.MoveTowards(hookPos, position, hookReturnSpeed * Time.deltaTime);

        if (hookPos == position)
        {
            line.SetPosition(1, position);
            hookDestination = position;
            ActivateHook(false);
            hookDistance = 0;
        }
    }
    void ActivateHook(bool isHooking)
    {
        hooked = isHooking;
        line.enabled = isHooking;
        hooker.enabled = isHooking;
        hookObject.SetActive(isHooking);
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