using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : Soldier {
    public List<Soldier> attackingSoldiers = new List<Soldier>();
    public int maxAttacking;
    public bool attackingCastle;
    public Damagebles target;

    void Start() {
        targetTransform = BattleManager.instance.EnemyGetTarget(transform.position.x);
        FindNewTarget();
        agent.SetDestination(targetTransform.position);
        BattleManager.instance.freeEnemys.Add(gameObject);
    }

    void Update() {
        FindNewTarget();
    }

    public void RemoveCounter(Soldier _attacking) {
        attackingSoldiers.Remove(_attacking);
        if(attackingSoldiers.Count == maxAttacking - 1) {
            BattleManager.instance.freeEnemys.Add(gameObject);
        }
        else if(attackingSoldiers.Count <= 0) {
            FindNewTarget();
            if(attackingCastle) {
                target = targetTransform.GetComponent<Damagebles>();
            }
            else {
                agent.isStopped = false;
            }
        }
        else {
            target = attackingSoldiers[0];
        }
    }

    public void AddCounter(Allie _attacking) {
        attackingSoldiers.Add(_attacking);
        if(attackingSoldiers.Count >= maxAttacking) {
            target = _attacking;
            BattleManager.instance.freeEnemys.Remove(gameObject);
        }
    }

    public void StartBattle(Damagebles _target) {
        anim.SetBool("Attack", true);
        target = _target;
        if(_target != null) {
            transform.LookAt(_target.transform);
        }
        else {
            if(transform.position.x > 0) {
                transform.localEulerAngles = new Vector3(0, -90, 0);
            }
            else {
                transform.localEulerAngles = new Vector3(0, 90, 0);
            }
        }
        agent.isStopped = true;
        StartCoroutine(Attack());
    }

    public void StopBattle() {
        if(attackingCastle == true) {
            StartBattle(targetTransform.GetComponent<Damagebles>());
        }
        else {
            anim.SetBool("Attack", false);
            target = null;
            agent.isStopped = false;
            FindNewTarget();
        }
    }

    public virtual void FindNewTarget() {
        Transform newTarget = BattleManager.instance.EnemyGetTarget(transform.position.x);
        if(targetTransform != newTarget) {
            anim.SetBool("Attack", false);
            StopCoroutine(Attack());
            targetTransform = newTarget;
            agent.SetDestination(targetTransform.position);
            attackingCastle = false;
            if(attackingSoldiers.Count <= 0) {
                agent.isStopped = false;
            }
        }
    }

    public virtual IEnumerator Attack() {
        Damagebles _targetToAttack = targetTransform.GetComponent<Damagebles>();
        yield return new WaitForSeconds(attackCooldown);
        if(target == null) {
            if(attackingSoldiers.Count > 0) {
                for(int i = 0; i < attackingSoldiers.Count; i++) {
                    if(attackingSoldiers[i].inFight == true) {

                        target = attackingSoldiers[i];
                        break;
                    }
                }
            }
            StopBattle();

        }
        else {
            StopAllCoroutines();
            StartCoroutine(Attack());
            target.TakeDamage(myStats.damage.currentValue);
        }

    }

    private void OnDisable() {
        WaveManager.instance.enemiesInScene.Remove(this);
    }
}