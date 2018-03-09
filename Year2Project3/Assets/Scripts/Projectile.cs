using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{

    private Rigidbody rb;

    private bool canRotate;
    private bool canDealDamage = true;

    public enum Type
    {
        BallistaProjectile,
        CatapultProjectile,
        AmbushProjectile_Volley
    }
    public Type type;

    [HideInInspector]
    public float myDamage;

    private Material myMat;
    public float destroyTime;
    public float destroyFadeSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Renderer myRenderer = transform.GetChild(0).GetComponent<Renderer>();
        myRenderer.material = new Material(myRenderer.material);
        myMat = myRenderer.material;
    }

    private void OnEnable()
    {
        myMat.color = new Color(myMat.color.r, myMat.color.g, myMat.color.b, 1);

        canRotate = true;
        rb.isKinematic = false;
        canDealDamage = true;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        canRotate = false;
        rb.isKinematic = true;

        if (!canDealDamage)
        {
            return;
        }

        canDealDamage = false;

        Enemy enemy = collision.transform.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(myDamage);
            ReAddToPool();
            return;
        }

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

        ReAddToPool();
    }

    private void ReAddToPool()
    {
        switch (type)
        {
            case Type.BallistaProjectile:

                ObjectPooler.instance.AddToPool("ballista projectile", gameObject);
                break;
            case Type.CatapultProjectile:


                break;
            case Type.AmbushProjectile_Volley:

                ObjectPooler.instance.AddToPool("ambush projectile", gameObject);
                break;
        }
    }
}
