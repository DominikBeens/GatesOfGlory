using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Allie : Soldier{

    public override void Update(){
        base.Update();
        if (targetTransform == null){
            targetTransform = BattleManager.instance.AllyGetTarget(transform.position.x,this);
            agent.SetDestination(targetTransform.position);
            agent.isStopped = false;
        }
        else if(agent.isStopped == false){
            agent.SetDestination(targetTransform.position);
        }
    }

    public void GetNewTarget(){
        if(targetTransform != null && targetTransform.tag != "Enemy"){
            targetTransform = BattleManager.instance.AllyGetTarget(transform.position.x, this);
            agent.SetDestination(targetTransform.position);
            agent.isStopped = false;
        }
    }

    public override void TakeDamage(float damage){
        myStats.health.currentValue -= damage;
        if (myStats.health.currentValue <= 0){
            targetTransform.GetComponent<Enemy>().RemoveCounter(this);
            Destroy(gameObject);
        }
    }

    public IEnumerator Attack(){
        Transform _attackingCurrently = targetTransform;
        yield return new WaitForSeconds(attackCooldown);
        if (targetTransform == _attackingCurrently){
            targetTransform.GetComponent<Enemy>().TakeDamage(myStats.damage.currentValue);
            //death Animation
            StartCoroutine(Attack());
        }
    }

    private void OnDestroy()
    {
        WaveManager.instance.alliesInScene.Remove(this);
    }
}
