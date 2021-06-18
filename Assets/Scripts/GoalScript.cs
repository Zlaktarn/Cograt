using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    BoxCollider2D coll;
    public static bool Victory = false;

    // Start is called before the first frame update
    void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        Victory = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Victory = true;
        }
    }
}
