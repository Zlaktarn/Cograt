using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakScript : MonoBehaviour
{
    GameObject playerObject;
    MovementScript player;

    void Awake()
    {
        playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<MovementScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(player.groundSlamming)
        {
            Destroy(gameObject);
        }
    }
}
