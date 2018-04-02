using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
    public bool enemyArrow;

    [HideInInspector]
    public Rigidbody rb;

    private bool canRotate;

    public enum Type
    {
        BallistaProjectile,
        CatapultProjectile,
        AmbushProjectile_Volley,
        AmbushProjectile_Meteor,
        AttackProjectile_Arrow
    }
    public Type type;

    [HideInInspector]
    public float myDamage;

    private Material myMat;
    private bool hit;

    public bool freezeOnImpact;
    [Space(10)]
    public int maxTargetsToHit;
    private int targetsHit;
    [Space(10)]
    public float destroyTime;
    public float destroyFadeSpeed;
    [Space(10)]
    public GameObject trailParticle;

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
        targetsHit = 0;
        trailParticle.SetActive(true);
        hit = false;
    }

    public void Fire(float force, float randomizer)
    {
        rb.AddForce(transform.forward * (force + Random.Range(-randomizer, randomizer)));
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
        if (collision.transform.tag == "Enemy" || collision.transform.tag == "Ally")
        {
            return;
        }

        if (hit)
        {
            return;
        }

        hit = true;
        // Sets targetsHit to max so that it cant deal damage to enemies passing by
        targetsHit = maxTargetsToHit;

        canRotate = false;

        if (freezeOnImpact)
        {
            rb.isKinematic = true;
        }
        else
        {
            trailParticle.SetActive(false);
        }

        StartCoroutine(Destroy());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hit)
        {
            return;
        }

        if (targetsHit >= maxTargetsToHit)
        {
            return;
        }

        if (enemyArrow){
            Allie allie = other.transform.GetComponent<Allie>();
            if (allie != null)
            {
                allie.TakeDamage(myDamage);
                ReAddToPool();
                targetsHit++;

                if (targetsHit >= maxTargetsToHit)
                {
                    hit = true;
                }
            }
        }
        else
        {
            Enemy enemy = other.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(myDamage);

                if (type == Type.CatapultProjectile)
                {
                    ObjectPooler.instance.GrabFromPool("meteor explode particle", transform.position, Quaternion.identity);
                }

                ReAddToPool();
                targetsHit++;

                if (targetsHit >= maxTargetsToHit)
                {
                    hit = true;
                }
            }
        }

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

                ObjectPooler.instance.AddToPool("catapult projectile", gameObject);
                break;
            case Type.AmbushProjectile_Volley:

                ObjectPooler.instance.AddToPool("ambush volley", gameObject);
                break;

            case Type.AmbushProjectile_Meteor:

                ObjectPooler.instance.AddToPool("ambush meteor", gameObject);
                break;
            case Type.AttackProjectile_Arrow:

                ObjectPooler.instance.AddToPool("Attacking Arrow", gameObject);
                break;
        }
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }
}
