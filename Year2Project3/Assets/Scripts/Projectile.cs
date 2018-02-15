using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{

    private bool canRotate;

    private void OnEnable()
    {
        canRotate = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        canRotate = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
