using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBowman : Enemy {
    private Transform myTransform;
    public Transform bowPos;
    public float fireRate;

    public override void TakeDamage(float damage) {
        myStats.health.currentValue -= damage;
        if(myStats.health.currentValue <= 0) {
            Destroy(gameObject);
        }
    }

    public override IEnumerator Attack() {
        Damagebles _targetToAttack = targetTransform.GetComponent<Damagebles>();
        bowPos.LookAt(target.transform);
        Transform _currentArrow = ObjectPooler.instance.GrabFromPool("Attacking Arrow", bowPos.position, Quaternion.Euler(new Vector3(0,0,-45))).transform;
        _currentArrow.LookAt(target.transform);
        float distance = Vector3.Distance(bowPos.position, target.transform.position);
        _currentArrow.GetChild(0).GetComponent<Arrow>().distance = distance;
        _currentArrow.position += _currentArrow.forward * distance / 2;
        _currentArrow.GetChild(0).GetComponent<Arrow>().myArrow.position -= new Vector3(0, _currentArrow.GetChild(0).GetComponent<Arrow>().minAmount, 0);
        _currentArrow.GetChild(0).transform.position = bowPos.position;
        _currentArrow.GetChild(0).transform.rotation = Quaternion.Euler( new Vector3(-55, 90, 0));

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
            StartCoroutine(Attack());
            target.TakeDamage(myStats.damage.currentValue);
        }
    }

    void OnTriggerStay(Collider collision) {
        if(collision.tag == "Defense") {
            if(collision.transform.GetComponent<CastleDeffensePoint>().gateOpen == true) {
                FindNewTarget();
                agent.isStopped = false;
            }
            else {
                agent.isStopped = true;
            }
        }

    }

    void OnTriggerEnter(Collider collision) {
        if(collision.transform == targetTransform) {
            StartBattle(target);
            targetTransform.GetComponent<CastleDeffensePoint>().attackingMe.Add(this);
            agent.isStopped = true;
            attackingCastle = true;
            target = collision.gameObject.GetComponent<Damagebles>();
            StartCoroutine(Attack());
        }
    }

    void OnTriggerExit(Collider collision) {
        if(collision.transform == targetTransform) {
            StopAllCoroutines();
            targetTransform.GetComponent<CastleDeffensePoint>().attackingMe.Remove(this);
        }
    }
}
