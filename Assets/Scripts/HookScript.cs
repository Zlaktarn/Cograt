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

    void Awake()
    {

        playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<MovementScript>();
        }

        rb = GetComponent<Rigidbody2D>();
        player.hookPos = transform.position;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        player.hookPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            player.hookHitPos = transform.position;
            rb.velocity = Vector2.zero;
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            box.enabled = false;
            hitTarget = true;
            audioSource.Play();
        }
    }


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Ground")
    //    {
    //        Vector3 thisPos = transform.localPosition;
    //        player.hookHitPos = thisPos;

    //        //print(player.hookHitPos);
    //        rb.velocity = Vector2.zero;
    //        BoxCollider2D box = GetComponent<BoxCollider2D>();
    //        box.enabled = false;
    //        hitTarget = true;
    //    }
    //}
}
