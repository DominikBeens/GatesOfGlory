using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Allie
{

    void OnTriggerEnter(Collider other)
    {
        if (targetTransform != null && targetTransform == other.transform)
        {
            targetTransform.gameObject.GetComponent<Enemy>().StartBattle(this);
            inFight = true;
            agent.isStopped = true;
            StartCoroutine(Attack());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == targetTransform)
        {
            agent.isStopped = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.transform != targetTransform)
        {
            GetNewTarget();
        }
        else
        {
            GetNewTarget();
        }
    }
}
