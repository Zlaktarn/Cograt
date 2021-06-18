using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool ButtonPushed = false;
    SpriteRenderer button;
    [SerializeField]
    Sprite buttonUp = default;
    [SerializeField]
    Sprite buttonDown = default;

    private void Start()
    {
        button = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(!ButtonPushed)
        {
            button.sprite = buttonUp;
        }
        else
            button.sprite = buttonDown;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Hook")
        {
            ButtonPushed = true;
        }
    }
}
