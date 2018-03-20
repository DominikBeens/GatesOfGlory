using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throne : CastleDeffensePoint
{
    public override void TakeDamage(float damage)
    {
        if (ResourceManager.instance.goldPrefabsInScene.Count >= Mathf.RoundToInt(damage / 10))
        {
            ResourceManager.instance.RemoveGold(Mathf.RoundToInt(damage / 10), false);
        }
        else if(ResourceManager.instance.goldPrefabsInScene.Count > 0)
        {
            damage = Mathf.Abs(ResourceManager.instance.goldPrefabsInScene.Count - Mathf.RoundToInt(damage / 10));
            ResourceManager.instance.RemoveGold(ResourceManager.instance.goldPrefabsInScene.Count, false);
            myStats.health.currentValue -= damage;
        }
        else
        {
            myStats.health.currentValue -= damage;

            if (myStats.health.currentValue <= 0)
            {
                if (GameManager.instance.gameState == GameManager.GameState.Playing)
                {
                    StartCoroutine(UIManager.instance.GameOver());
                }
            }
        }

        if (healthbarFill != null)
        {
            healthbarFill.fillAmount = (myStats.health.currentValue / myStats.health.baseValue);
        }
    }
}
