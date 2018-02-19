using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Allie : Soldier
{

    void Start()
    {
        targetTransform = BattleManager.instance.AllyGetTarget(transform.position.x, this);
        agent.SetDestination(targetTransform.position);
    }

    public override void Update()
    {
        base.Update();
        GetNewTarget();
    }

    public void GetNewTarget()
    {
        if (targetTransform != null)
        {
            Transform newTarget = BattleManager.instance.AllyGetTarget(transform.position.x, this); //
            if (newTarget != targetTransform)
            {
                if (targetTransform.tag == "Enemy")
                {
                    if (newTarget.tag != "Enemy")
                    {
                        targetTransform.GetComponent<Enemy>().RemoveCounter(this);
                        targetTransform = newTarget;
                        agent.SetDestination(targetTransform.position);
                        agent.isStopped = false;
                    }
                    else if (agent.isStopped == false)
                    {
                        agent.SetDestination(targetTransform.position);
                    }
                }
                else
                {
                    if (newTarget.tag == "Enemy")
                    {
                        newTarget.GetComponent<Enemy>().AddCounter(this);
                    }
                    targetTransform = newTarget;
                    agent.SetDestination(targetTransform.position);
                    agent.isStopped = false;
                }
            }
            else if (targetTransform.tag == "Enemy" && agent.isStopped == false)
            {
                agent.SetDestination(targetTransform.position);
            }
        }
        else
        {
            targetTransform = BattleManager.instance.AllyGetTarget(transform.position.x, this); //
            agent.SetDestination(targetTransform.position);
            agent.isStopped = false;
        }
    }

    public override void TakeDamage(float damage)
    {
        myStats.health.currentValue -= damage;
        if (myStats.health.currentValue <= 0)
        {
            if(targetTransform != null)
            {
            targetTransform.GetComponent<Enemy>().RemoveCounter(this);
            }
            Destroy(gameObject);
        }
    }

    public IEnumerator Attack()
    {
        Transform _attackingCurrently = targetTransform;
        yield return new WaitForSeconds(attackCooldown);
        if (targetTransform != null && targetTransform.tag == "Enemy" && targetTransform == _attackingCurrently)
        {
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
