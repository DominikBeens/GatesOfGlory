using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile{
    public Transform arrow;
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
            arrow.Rotate(Vector3.right, Time.deltaTime * arrowSpeed * distance);
        }
    }
}
