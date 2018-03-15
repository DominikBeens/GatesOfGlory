using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile{
    public Transform myArrow;
    public float distance;
    public Rigidbody rb;
    public int arrowSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.isKinematic == false){
            myArrow.Rotate(Vector3.right, Time.deltaTime * arrowSpeed * distance);
        }
    }
}
