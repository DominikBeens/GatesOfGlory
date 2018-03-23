using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throne : CastleDeffensePoint
{
    public Image uiHealthBarLeft;
    public Image uiHealthBarRight;
    public Transform rightGatesUI, leftGatesUI;
    public Transform nextRightGatesUI, nextLeftGatesUI;
    public float lerpSpeed;

    private void Update() {

    }

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
            uiHealthBarLeft.fillAmount = (myStats.health.currentValue / myStats.health.baseValue);
            uiHealthBarRight.fillAmount = (myStats.health.currentValue / myStats.health.baseValue);
            StopCoroutine(Lerp());
            StartCoroutine(Lerp());
        }
    }

    public IEnumerator Lerp() {
        while(Mathf.Abs(rightGatesUI.position.x - nextRightGatesUI.position.x) * 1 - uiHealthBarRight.fillAmount > 0.1f ) {
            rightGatesUI.position = new Vector3(Mathf.Lerp(rightGatesUI.position.x, nextRightGatesUI.position.x, lerpSpeed) * 1 - uiHealthBarRight.fillAmount, rightGatesUI.position.y, rightGatesUI.position.z);
            yield return null;
        }
    }
}
