using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryBowManz : Soldier {
    public Transform bowPos;
    public float fireRate;

    void OnTriggerEnter(Collider other) {
        if(targetTransform != null && targetTransform == other.transform) {
            if(other.GetComponent<Enemy>() is EnemyBowman) {
                targetTransform.gameObject.GetComponent<Enemy>().StartBattle(this);
            }
            anim.SetBool("Attack", true);
            anim.SetBool("Idle", false);
            StopCoroutine(Attack());
            StartCoroutine(Attack());
        }
    }

    void OnTriggerExit(Collider other) {
        anim.SetBool("Attack", false);
        anim.SetBool("Idle", true);
        StopCoroutine(Attack());
    }

    void OnCollisionStay(Collision collision) {
        if(targetTransform == null) {
            targetTransform = collision.gameObject.transform;
        }
    }

    public IEnumerator Attack() {
        Transform _attackingCurrently = targetTransform;
        yield return new WaitForSeconds(attackCooldown);

        float distance = Vector3.Distance(bowPos.position, targetTransform.position);
        Transform _currentArrow = ObjectPooler.instance.GrabFromPool("Ally Arrow", bowPos.position, Quaternion.Euler(new Vector3(0, 0, 0))).transform;
        _currentArrow.LookAt(targetTransform);
        _currentArrow.GetChild(0).GetComponent<Arrow>().distance = distance;
        _currentArrow.position += _currentArrow.forward * distance / 2;
        _currentArrow.GetChild(0).GetComponent<Arrow>().myArrow.position -= new Vector3(0, _currentArrow.GetChild(0).GetComponent<Arrow>().minAmount, 0);
        _currentArrow.GetChild(0).transform.position = bowPos.position;
        _currentArrow.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(-55, -45, 0));

        if(targetTransform != null && targetTransform.tag == "Enemy" && targetTransform == _attackingCurrently) {
            targetTransform.GetComponent<Enemy>().TakeDamage(myStats.damage.currentValue);
            StartCoroutine(Attack());
        }
        else {
            anim.SetBool("Attack", false);
            anim.SetBool("Idle", false);
        }
    }

    public override void TakeDamage(float damage) {
        myStats.health.currentValue -= damage;

        if(myStats.health.currentValue <= 0) {
            StopAllCoroutines();
            if(targetTransform != null) {
                targetTransform.GetComponent<Enemy>().RemoveCounter(this);
            }
            ObjectPooler.instance.AddToPool("Stationary Bowman", gameObject);
            ResourceManager.instance.AddGold(ResourceManager.instance.normalEnemyGoldReward);
        }
    }
}
