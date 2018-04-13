using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBowman : Enemy {
    public Transform bowPos;
    public float fireRate;

    public override void TakeDamage(float damage) {
        myStats.health.currentValue -= damage;

        if(myStats.health.currentValue <= 0) {
            StopAllCoroutines();
            ObjectPooler.instance.AddToPool("Enemy Bowman", gameObject);
            ResourceManager.instance.AddGold(ResourceManager.instance.normalEnemyGoldReward);
        }
    }

    public override IEnumerator Attack() {
        yield return new WaitForSeconds(attackCooldown);
        if (target == null) {
            if (attackingSoldiers.Count > 0) {
                for (int i = 0; i < attackingSoldiers.Count; i++) {
                    if (attackingSoldiers[i].inFight == true) {

                        target = attackingSoldiers[i];
                        break;
                    }
                }
            }
            StopBattle();
            StopCoroutine(Attack());
        }
        float distance = Vector3.Distance(bowPos.position, target.transform.position);
        Transform _currentArrow = ObjectPooler.instance.GrabFromPool("Attacking Arrow", bowPos.position, Quaternion.Euler(new Vector3(0, 0, -45))).transform;
        print(_currentArrow);
        _currentArrow.LookAt(target.transform);
        _currentArrow.GetChild(0).GetComponent<Arrow>().distance = distance;
        _currentArrow.position += _currentArrow.forward * (distance / 2 - 5);
        _currentArrow.GetChild(0).GetComponent<Arrow>().myArrow.position -= new Vector3(0, _currentArrow.GetChild(0).GetComponent<Arrow>().minAmount, 0);
        _currentArrow.GetChild(0).transform.position = bowPos.position;
        _currentArrow.GetChild(0).transform.LookAt(targetTransform);
        _currentArrow.GetChild(0).transform.localEulerAngles += new Vector3(-70, 0, 0);

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

    void OnTriggerStay(Collider collision){
        if(collision.tag == "Defense"){
            CastleGatePoint castleGatePoint = collision.transform.GetComponent<CastleGatePoint>();

            if (castleGatePoint != null)
            {
                if (castleGatePoint.myGate.isOpen)
                {
                    FindNewTarget();
                    attackingCastle = false;
                    StopAllCoroutines();
                    return;
                }
            }

            agent.isStopped = true;
        }

    }


    void OnTriggerEnter(Collider collision) {
        if(collision.transform == targetTransform){
            StartBattle(targetTransform.GetComponent<Damagebles>());
            targetTransform.GetComponent<CastleDeffensePoint>().attackingMe.Add(this);
            agent.isStopped = true;
            attackingCastle = true;
            target = collision.gameObject.GetComponent<Damagebles>();
            StartCoroutine(Attack());
        }
    }

    void OnTriggerExit(Collider collision) {
        if(collision.transform == targetTransform && targetTransform.tag == "Defense") {
            StopAllCoroutines();
            FindNewTarget();
            targetTransform.GetComponent<CastleDeffensePoint>().attackingMe.Remove(this);
        }
    }
}
