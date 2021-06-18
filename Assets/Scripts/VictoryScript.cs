using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScript : MonoBehaviour
{
    bool VictoryBool = false;
    [SerializeField]
    GameObject text = default;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (VictoryBool)
        {
            text.SetActive(true);
        }
        else
            text.SetActive(false);

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            VictoryBool = true;
        }
    }
}

