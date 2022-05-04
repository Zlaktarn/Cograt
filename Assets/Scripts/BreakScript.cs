using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakScript : MonoBehaviour
{
    GameObject playerObject;
    Actions player;

    void Awake()
    {
        playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<Actions>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(player.smashing)
        {
            Destroy(gameObject);
        }
    }
}
