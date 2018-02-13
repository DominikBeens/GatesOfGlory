using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : Damagebles{
    public float attackCooldown;
    public NavMeshAgent agent;
    public Transform targetTransform;

    public override void TakeDamage(float damage){
        myStats.health.currentValue -= damage;
        if(myStats.health.currentValue <= 0){
            Destroy(gameObject);
        }
    }
}
