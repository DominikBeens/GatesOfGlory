using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryBowManz : Soldier {
    public Transform bowPos;
    public float fireRate;
    [HideInInspector]
    public ArcherSpot mySpot;

    void OnTriggerEnter(Collider other) {
        if(targetTransform == null) {
            anim.SetBool("Attack", true);
            anim.SetBool("Idle", false);
            targetTransform = other.transform;
            if(other.GetComponent<EnemyBowman>() != null) {
            print("l");
            targetTransform.gameObject.GetComponent<Enemy>().StartBattle(this);
            }
            StopCoroutine(Attack());
            StartCoroutine(Attack());
        }
    }

    void OnTriggerExit(Collider other) {
        if(targetTransform == other.transform) {
            anim.SetBool("Attack", false);
            anim.SetBool("Idle", true);
            targetTransform = null;
            StopCoroutine(Attack());
        }
    }

    void OnCollisionStay(Collision collision) {
        if(targetTransform == null) {
            targetTransform = collision.gameObject.transform;
        }
    }

    public IEnumerator Attack() {
        Transform _attackingCurrently = targetTransform;
        if(targetTransform != null) {
            float distance = Vector3.Distance(bowPos.position, targetTransform.position);
            Transform _currentArrow = ObjectPooler.instance.GrabFromPool("Staonairy arrows", bowPos.position, Quaternion.Euler(new Vector3(0, 0, 0))).transform;
            _currentArrow.LookAt(targetTransform);
            _currentArrow.GetChild(0).GetComponent<Arrow>().distance = distance;
            _currentArrow.position += _currentArrow.forward * distance / 2;
            _currentArrow.GetChild(0).GetComponent<Arrow>().myArrow.position -= new Vector3(0, 0, 14f);
            _currentArrow.GetChild(0).transform.position = bowPos.position;
            _currentArrow.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(-55, -45, 0));
        }

            if(targetTransform.gameObject.activeSelf == true && targetTransform != null && targetTransform.tag == "Enemy" && targetTransform == _attackingCurrently) {
                targetTransform.GetComponent<Enemy>().TakeDamage(myStats.damage.currentValue);
                StartCoroutine(Attack());
            }
            else {
                StopCoroutine(Attack());
                targetTransform = null;
                anim.SetBool("Attack", false);
                anim.SetBool("Idle", false);
            }
        yield return new WaitForSeconds(attackCooldown);
    }

    public override void TakeDamage(float damage) {
        myStats.health.currentValue -= damage;

        if(myStats.health.currentValue <= 0) {
            StopAllCoroutines();
            if(targetTransform != null) {
                targetTransform.GetComponent<Enemy>().RemoveCounter(this);
            }
            ResourceManager.instance.AddGold(ResourceManager.instance.normalEnemyGoldReward);
            mySpot.RemoveArcher();
            mySpot = null;
        }
    }
}
