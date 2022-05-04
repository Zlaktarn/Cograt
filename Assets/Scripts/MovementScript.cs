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

    float x = 0;
    bool isGrounded = false;
    Vector3 yVelocity;
    #endregion

    public Vector2 startPos;

    [SerializeField]
    GameObject cog = default;

    float t = 0;


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

    [SerializeField]
    BoxCollider2D groundBox = default;
    public bool hooked = false;

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
        anim = ratObject.GetComponent<Animator>();
        sprite = ratObject.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        x = 0.0f;
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
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        else
            rb.simulated = false;

    }

    private void Update()
    {
        if (!GoalScript.Victory)
        {
            if (!death)
            {
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
            }
        }
    }

    public bool IsGrounded()
    {
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
}