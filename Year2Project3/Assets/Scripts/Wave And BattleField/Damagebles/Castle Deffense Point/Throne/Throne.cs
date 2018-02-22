using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throne : CastleDeffensePoint
{
    public override void TakeDamage(float damage)
    {
        if (ResourceManager.gold >= damage)
        {
            ResourceManager.instance.RemoveGold(Mathf.RoundToInt(damage));
        }
        else if(ResourceManager.gold > 0)
        {
            damage = Mathf.Abs(ResourceManager.gold - Mathf.RoundToInt(damage));
            ResourceManager.instance.RemoveGold(ResourceManager.gold);
            myStats.health.currentValue -= damage;
        }
        else
        {
            myStats.health.currentValue -= damage;
        }

        if (healthbarFill != null)
        {
            healthbarFill.fillAmount = (myStats.health.currentValue / myStats.health.baseValue);
        }
    }
}
