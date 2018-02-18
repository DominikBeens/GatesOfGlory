using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Soldier : Damagebles{
    public float attackCooldown;
    public NavMeshAgent agent;
    public Transform targetTransform;

    [Header("Healthbar")]
    public Transform healthbarCanvas;
    public Image healthbarFill;
    public Animator healthbarAnim;

    public override void TakeDamage(float damage){
        myStats.health.currentValue -= damage;
        healthbarFill.fillAmount = (myStats.health.currentValue / myStats.health.baseValue);
        if(myStats.health.currentValue <= 0){
            Destroy(gameObject);
        }
    }

    public virtual void Update()
    {
        healthbarCanvas.LookAt(Camera.main.transform);
    }

    public void ShowHealthbar()
    {
        healthbarAnim.ResetTrigger("Hide");
        healthbarAnim.SetTrigger("Show");
    }

    public void HideHealthbar()
    {
        healthbarAnim.ResetTrigger("Show");
        healthbarAnim.SetTrigger("Hide");
    }
}
