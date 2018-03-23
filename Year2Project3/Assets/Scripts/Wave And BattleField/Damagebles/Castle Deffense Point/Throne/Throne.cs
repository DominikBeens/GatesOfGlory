using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throne : CastleDeffensePoint
{

    public Image throneHealthBarLeft;
    public Image throneHealthBarRight;
    public Image gateHealthBarLeft;
    public Image gateHealthBarRight;

    public CastleDeffensePoint leftGate, rightGate;

    public float lerpSpeed;

    private void Update() {

        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(10);
        }
    }

    private void Start() {
        HPBar();
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
            HPBar();

            if (myStats.health.currentValue <= 0)
            {
                if (GameManager.instance.gameState == GameManager.GameState.Playing)
                {
                    StartCoroutine(UIManager.instance.GameOver());
                }
            }
        }
    }

    public void HPBar(){
        gateHealthBarLeft.fillAmount = (leftGate.myStats.health.currentValue + myStats.health.currentValue) / (leftGate.myStats.health.baseValue + myStats.health.baseValue);
        throneHealthBarLeft.fillAmount = myStats.health.currentValue / (myStats.health.baseValue + leftGate.myStats.health.baseValue);
        gateHealthBarRight.fillAmount = (rightGate.myStats.health.currentValue + myStats.health.currentValue) / (rightGate.myStats.health.baseValue + myStats.health.baseValue);
        throneHealthBarRight.fillAmount = myStats.health.currentValue / (myStats.health.baseValue + rightGate.myStats.health.baseValue);
    }
}
