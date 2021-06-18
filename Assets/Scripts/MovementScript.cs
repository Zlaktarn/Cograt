using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    Vector2 position = Vector2.zero;

    #region Movement
    [SerializeField]
    float speed = 3.0f;
    [SerializeField]
    float maxSpeed = 4.0f;
    [SerializeField]
    float jumpVelocity = 4.0f;
    [SerializeField]
    float breaks = 2.0f;
    [SerializeField]
    float groundSlamVelocity = 8.0f;
    [SerializeField]
    float groundSlamInterval = 0.2f;
    float groundSlamTimer = 0.0f;
    float x = 0;
    bool isGrounded = false;
    Vector3 yVelocity;
    #endregion

    public Vector2 startPos;

    [SerializeField]
    GameObject cog = default;

    float t = 0;

    [SerializeField]
    LayerMask hookAble = default;
    DistanceJoint2D hooker = default;

    #region Animation
    [SerializeField]
    GameObject ratObject = null;
    [SerializeField]
    GameObject ratAndCogObject = null;
    [SerializeField]
    GameObject deathObj = null;
    Animator anim = null;
    SpriteRenderer sprite = null;
    #endregion
    Rigidbody2D rb;

    #region Layers
    [SerializeField]
    LayerMask layerMask = default;
    [SerializeField]
    LayerMask breakableLayer = default;
    #endregion

    public static bool death = false;

    public bool groundSlamming = false;

    [SerializeField]
    BoxCollider2D groundBox = default;
    public Vector2 playerPos = Vector3.zero;
    Vector2 mousePos = Vector2.zero;
    Vector2 formerMousePos = Vector2.zero;
    float hookDistance;
    public bool hooked = false;
    CircleCollider2D coll;

    [SerializeField]
    LineRenderer hookLine = default;
    bool hooking = false;
    bool hookSpawned = false;
    GameObject hook = default;
    public Vector2 hookHitPos = Vector2.zero;
    public Vector2 hookPos = Vector2.zero;
    Quaternion qAngle = default;


    Vector3 rotation = Vector3.zero;
    Quaternion cogRotation;

    [SerializeField]
    GameObject projectile = default;
    [SerializeField]
    float projectileSpeed = 40f;

    AudioSource audioSource;
    [SerializeField]
    AudioClip jumpClip;
    [SerializeField]
    AudioClip bounceClip;
    [SerializeField]
    AudioClip deathClip;
    [SerializeField]
    AudioClip breakClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startPos = transform.position;
        hookLine.enabled = false;
        anim = ratObject.GetComponent<Animator>();
        sprite = ratObject.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        x = 0.0f;
        coll = GetComponent<CircleCollider2D>();
        hooker = GetComponent<DistanceJoint2D>();
        death = false;
    }

    void FixedUpdate()
    {

        if (!GoalScript.Victory)
        {
            rb.simulated = true;

            Animations();

            t = Time.deltaTime;

            if (!death)
            {
                ratAndCogObject.SetActive(true);
                deathObj.SetActive(false);
                Movement();
            }
            else
            {
                ratAndCogObject.SetActive(false);
                deathObj.SetActive(true);
                HookRelease();
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        else
            rb.simulated = false;

        if (groundSlamming)
            GroundSlam();
    }

    private void Update()
    {
        playerPos = transform.position;

        if (!GoalScript.Victory)
        {
            if (!death)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    hooking = true;

                    if (!hookSpawned)
                        HookShot();
                }
                if (hooking)
                {
                    hookLine.SetPosition(0, hookPos);
                    hookLine.SetPosition(1, playerPos);
                    hookLine.enabled = true;

                    if (hookHitPos != Vector2.zero)
                        HookHold();
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    if (hook != null)
                        Destroy(hook);
                    HookRelease();
                    hookSpawned = false;
                    hooked = false;
                }

                isGrounded = IsGrounded();

                if (isGrounded)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Vector2 momentum = rb.velocity;
                        momentum.y = jumpVelocity;
                        rb.velocity = momentum;

                        audioSource.clip = jumpClip;
                        audioSource.Play();
                    }
                }
                if (Input.GetKeyDown(KeyCode.LeftControl) && !isGrounded && !groundSlamming && !hooked)
                {
                    groundSlamTimer = 0;
                    groundSlamming = true;
                }

                
            }
        }
    }

    void GroundSlam()
    {
        groundSlamTimer += t;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (groundSlamTimer >= groundSlamInterval)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.velocity = Vector2.up * -groundSlamVelocity;
            RaycastHit2D raycastHit2d = Physics2D.BoxCast(groundBox.bounds.center, groundBox.transform.localScale / 2, 0, Vector2.down, 0, breakableLayer);

            if (isGrounded || hooked)
                groundSlamming = false;
        }
    }

    void HookShot()
    {
        if (groundSlamming)
            hookSpawned = true;

        hookHitPos = Vector2.zero;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 hookDir = mousePos - (Vector2)transform.position;

        float hookAngle = AngleBetweenTwoPoints(transform.position, mousePos);
        qAngle = Quaternion.Euler(new Vector3(0, 0, hookAngle + 45));
        hookDir.Normalize();
        Vector3 playerCenter = transform.localScale / 2;

        hook = Instantiate(projectile, transform.position + playerCenter + hookDir / 2, qAngle) as GameObject;
        hook.GetComponent<Rigidbody2D>().velocity = hookDir * projectileSpeed;

        Vector3 hookPosition = hook.transform.position;
        //hooker.distance = Vector2.Distance(playerPos, hookPosition);

    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    void HookHold()
    {
        hooked = true;


        if (hookHitPos != Vector2.zero)
            hooker.connectedAnchor = hookHitPos;

        hooker.enabled = true;
    }

    void HookRelease()
    {
        hooking = false;
        hookLine.enabled = false;
        hooker.enabled = false;
    }

    #region OldHook
    void Hook()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hookLine.SetPosition(0, transform.position);

        if (Input.GetMouseButtonDown(0))
        {
            formerMousePos = mousePos;
        }
        else if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, hookAble);

            if (hit.collider != null)
            {
                Debug.DrawLine(transform.position, formerMousePos, Color.red);
                //Hooked();
                hooked = true;
            }
        }
        else
        {
            hookLine.enabled = false;
            hooker.enabled = false;
            hooked = false;
        }
    }

    //void Hooked()
    //{
    //    Vector2 playerPos = transform.position;
    //    hooker.distance = Vector2.Distance(playerPos, formerMousePos);
    //    hooker.enabled = true;

    //    hookLine.enabled = true;

    //    hookLine.SetPosition(1, formerMousePos);

    //    hooker.connectedAnchor = formerMousePos;
    //}
    #endregion

    private bool IsGrounded()
    {
        //CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        //RaycastHit2D raycastHit2d = Physics2D.CircleCast(circleCollider.bounds.center, circleCollider.radius, Vector2.down * .01f, 0, layerMask);
        RaycastHit2D raycastHit2d;

        raycastHit2d = Physics2D.BoxCast(groundBox.bounds.center, groundBox.transform.localScale / 2, 0, Vector2.down, 0, layerMask);


        return raycastHit2d.collider != null;

    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            sprite.flipX = true;

            if (x >= -maxSpeed)
                x -= speed * Time.deltaTime;
        }
        else if (x < 0)
            x += breaks * t;

        if (Input.GetKey(KeyCode.D))
        {
            sprite.flipX = false;

            if (x <= maxSpeed)
                x += speed * Time.deltaTime;
        }
        else if (x > 0)
            x -= breaks * t;

        if (isGrounded)
        {
            if (rb.velocity.x > 0.01f)
                rb.AddForce(Vector2.right * -breaks / 4);
            else if (rb.velocity.x < -0.01f)
                rb.AddForce(Vector2.right * breaks / 4);
        }

        rb.AddForce(Vector2.right * x);

        cog.transform.Rotate(0, 0, -x, Space.Self);
    }

    private void Animations()
    {
        if (!death)
        {
            if (x >= 1 || x <= -1)
                anim.SetBool("Moving", true);
            else
                anim.SetBool("Moving", false);

            if (isGrounded)
                anim.SetBool("IsGrounded", true);
            else
                anim.SetBool("IsGrounded", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.name == "Bounce" && rb.velocity.y >= -1f)
        {
            audioSource.clip = bounceClip;
            audioSource.Play();
        }

        if(collision.transform.name == "Lava")
        {
            audioSource.clip = deathClip;
            audioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && groundSlamming)
        {
            audioSource.clip = breakClip;
            audioSource.Play();
        }
    }
}