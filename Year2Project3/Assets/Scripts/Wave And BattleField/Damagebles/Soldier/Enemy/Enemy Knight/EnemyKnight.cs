using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnight : Enemy{
    void OnCollisionStay(Collision collision){
        if (collision.collider.tag == "Defense"){
            if (collision.transform.GetComponent<CastleDeffensePoint>().gateOpen == true){
                FindNewTarget();
                agent.isStopped = false;
            }
            else{
                agent.isStopped = true;
            }
        }

    }

    void OnCollisionEnter(Collision collision){
        if (collision.transform == targetTransform){
            StartBattle(target);
            targetTransform.GetComponent<CastleDeffensePoint>().attackingMe.Add(this);
            agent.isStopped = true;
            attackingCastle = true;
            target = collision.gameObject.GetComponent<Damagebles>();
        }
    }

    void OnCollisionExit(Collision collision){
        if (collision.transform == targetTransform){
            targetTransform.GetComponent<CastleDeffensePoint>().attackingMe.Remove(this);
        }
    }
}
