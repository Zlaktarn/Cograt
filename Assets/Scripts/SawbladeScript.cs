using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawbladeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            MovementScript player = collision.gameObject.GetComponent<MovementScript>();
            
            if(player != null)
            {
                MovementScript.death = true;
            }
        }
    }
}
