using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    ButtonScript button = null;

    Vector2 position;

    [SerializeField]
    bool horizontal = false;

    float timer;
    float timerInterval = 1;

    void Start()
    {
        position = transform.position;
    }

    void Update()
    {
        if(button.ButtonPushed)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            timer += Time.deltaTime;
            if(horizontal)
                position.x += 14 * Time.deltaTime;
            else
                position.y -= 14 * Time.deltaTime;

            if (timer >= timerInterval)
                Destroy(gameObject);
        }

        transform.position = position;
    }
}
