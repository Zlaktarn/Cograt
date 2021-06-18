using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShooter : MonoBehaviour
{
    Vector3 mousePos = Vector2.zero;

    void Start()
    {
    }

    public void Shoot()
    {
        Vector3 shootDir = (mousePos - transform.position).normalized;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, shootDir);

        if(raycastHit.collider != null)
        {
            Debug.DrawRay(transform.position, raycastHit.collider.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            //hookTransform = Instantiate(hook, transform.position, Quaternion.identity);
            //Vector3 shootDir = (mousePos - this.transform.position).normalized;
            //hookTransform.GetComponent<HookScript>().Setup(shootDir);

            Shoot();
        }
        else if(Input.GetMouseButton(0))
        {
        }

    }
}
