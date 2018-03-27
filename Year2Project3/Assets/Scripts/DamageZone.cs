using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{

    [HideInInspector]
    public bool canDamage;

    public int myDamage;
    private int myHealth;
    public int myMaxHealth;

    public Animator anim;

    public string myObjectPool;

    private void OnEnable()
    {
        myHealth = myMaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage)
        {
            return;
        }

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(myDamage);
            myHealth--;

            if (anim != null)
            {
                anim.SetTrigger("Damage");
            }
        }

        if (myHealth <= 0)
        {
            ObjectPooler.instance.GrabFromPool("destroy particle", transform.position, Quaternion.identity);
            ObjectPooler.instance.AddToPool(myObjectPool, gameObject);
        }
    }
}
