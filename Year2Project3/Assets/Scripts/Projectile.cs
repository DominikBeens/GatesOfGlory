using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{

    private bool canRotate;
    private bool canDealDamage = true;

    [HideInInspector]
    public float myDamage;

    private Material myMat;
    public float destroyTime;
    public float destroyFadeSpeed;

    private void Awake()
    {
        Renderer myRenderer = transform.GetChild(0).GetComponent<Renderer>();
        myRenderer.material = new Material(myRenderer.material);
        myMat = myRenderer.material;
    }

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

        if (!canDealDamage)
        {
            return;
        }

        Enemy enemy = collision.transform.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(myDamage);
            Destroy(gameObject);
        }

        canDealDamage = false;
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(destroyTime);

        while (myMat.color.a > 0.1f)
        {
            myMat.color -= new Color(0, 0, 0, Time.deltaTime * destroyFadeSpeed);
            yield return null;
        }

        Destroy(gameObject);
    }
}
