using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Soldier{
    public List<Allie> attackingSoldiers = new List<Allie>();
    public int maxAttacking;
    public bool attackingCastle;
    public Damagebles target;

    void Start(){
        targetTransform = BattleManager.instance.EnemyGetTarget(transform.position.x);
        FindNewTarget();
        agent.SetDestination(targetTransform.position);
        BattleManager.instance.freeEnemys.Add(gameObject);
    }

    void Update(){
        FindNewTarget();
    }

    public void RemoveCounter(Allie _attacking){
        attackingSoldiers.Remove(_attacking);
        if(attackingSoldiers.Count <= 0){
            FindNewTarget();
            BattleManager.instance.freeEnemys.Add(gameObject);
            if (attackingCastle){
                target = targetTransform.GetComponent<Damagebles>();
            }
            else{
                agent.isStopped = false;
            }
        }
        else{
            target = attackingSoldiers[0];
        }
    }

    public void AddCounter(Allie _attacking){
        attackingSoldiers.Add(_attacking);
        if (attackingSoldiers.Count >= maxAttacking){
            target = _attacking;
            BattleManager.instance.freeEnemys.Remove(gameObject);
        }
    }

    public void StartBattle(Damagebles _target){
        target = _target;
        agent.isStopped = true;
        StartCoroutine(Attack());
    }

    public void StopBattle(){
        BattleManager.instance.freeEnemys.Add(gameObject);
        if(attackingCastle == true){
            StartBattle(targetTransform.GetComponent<Damagebles>());
        }
        else{
            agent.isStopped = false;
            FindNewTarget();
        }
    }

    public void FindNewTarget(){
        Transform newTarget = BattleManager.instance.EnemyGetTarget(transform.position.x);
        if (targetTransform != newTarget){
            targetTransform = newTarget;
            agent.SetDestination(targetTransform.position);
            attackingCastle = false;
            if(attackingSoldiers.Count <= 0){
                agent.isStopped = false;
            }
        }
    }

    public IEnumerator Attack(){
        Damagebles _targetToAttack = targetTransform.GetComponent<Damagebles>();
        yield return new WaitForSeconds(attackCooldown);
        if(target == null){
            if (attackingSoldiers.Count > 0){
                target = attackingSoldiers[0];
            }
            else{
                StopBattle();
            }
        }
        else{
            StartCoroutine(Attack());
            target.TakeDamage(myStats.damage.currentValue);
        }
    }
}