using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : CastleDeffensePoint {


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
    }

    public void ChangeGateOpen() {
        gateOpen = !gateOpen;
    }

    public void Heal(float healAmount) {
        myStats.health.currentValue += healAmount;

        if(myStats.health.currentValue > myStats.health.baseValue) {
            myStats.health.currentValue = myStats.health.baseValue;
        }

        healthbarFill.fillAmount = myStats.health.currentValue / myStats.health.baseValue;
    }
}
