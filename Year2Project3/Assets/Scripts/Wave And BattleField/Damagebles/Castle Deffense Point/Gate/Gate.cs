using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : CastleDeffensePoint {
    public Throne daThrone;

    public override void TakeDamage(float damage) {
        if(gateOpen) {
            return;
        }

        myStats.health.currentValue -= damage;
        healthbarFill.fillAmount = (myStats.health.currentValue / myStats.health.baseValue);
        if(myStats.health.currentValue < 0) {
            myGate.OpenGate();
            gateOpen = true;
            myGate.locked = true;
            foreach(Enemy e in attackingMe) {
                if(e != null) {
                    e.attackingCastle = false;
                    e.FindNewTarget();
                }
            }
        }

        daThrone.HPBar();
    }

    public void ChangeGateOpen() {
        gateOpen = !gateOpen;
    }

    public void Heal(float healAmount) {
        if (myStats.health.currentValue == myStats.health.baseValue)
        {
            return;
        }

        myStats.health.currentValue += healAmount;

        Vector3 healParticlePos = new Vector3(transform.position.x + 0.5f, transform.position.y + 3, transform.position.z - 5);
        ObjectPooler.instance.GrabFromPool("heal particle", healParticlePos, Quaternion.identity);

        if(myStats.health.currentValue > myStats.health.baseValue) {
            myStats.health.currentValue = myStats.health.baseValue;
        }

        healthbarFill.fillAmount = myStats.health.currentValue / myStats.health.baseValue;
    }
}
