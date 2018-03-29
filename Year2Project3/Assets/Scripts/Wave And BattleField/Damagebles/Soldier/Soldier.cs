using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Soldier : Damagebles{
    public NavMeshAgent agent;
    public float attackCooldown;
    public Transform targetTransform;
    public Animator anim;
    public bool inFight;

    public override void TakeDamage(float damage){
        myStats.health.currentValue -= damage;
        if(myStats.health.currentValue <= 0){
            //Destroy(gameObject);
        }
    }
}
