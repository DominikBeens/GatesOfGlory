using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTerrorist : Enemy {

    public override void TakeDamage(float damage) {
        myStats.health.currentValue -= damage;

        if(myStats.health.currentValue <= 0) {
            ObjectPooler.instance.AddToPool("Enemy Terrorist", gameObject);
            ResourceManager.instance.AddGold(1000); // edit plz
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.transform == targetTransform) {
            targetTransform.GetComponent<CastleDeffensePoint>().attackingMe.Add(this);
            collision.gameObject.GetComponent<Damagebles>().TakeDamage(myStats.damage.currentValue);
            ObjectPooler.instance.AddToPool("Enemy Terrorist", gameObject);
        }
    }

    private void OnCollisionStay(Collision collision) {
        
    }
}
