using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleDeffensePoint : Damagebles {
    public bool gateOpen;
    public List<Enemy> attackingMe = new List<Enemy>();
    public CastleGate myGate;
    public Image healthbarFill;

    public override void TakeDamage(float damage){
        myStats.health.currentValue -= damage;
        healthbarFill.fillAmount = myStats.health.currentValue / myStats.health.baseValue;
        if(myStats.health.currentValue < 0){
            myGate.OpenGate();
            gateOpen = true;
            myGate.locked = true;
            foreach (Enemy e in attackingMe){
                if(e != null){
                    e.attackingCastle = false;
                    e.FindNewTarget();
                }
            }
        }
    }

    public void ChangeGateOpen(){
        gateOpen = !gateOpen;
    }
}
