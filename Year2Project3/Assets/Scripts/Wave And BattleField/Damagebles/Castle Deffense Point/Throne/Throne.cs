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

    public RectTransform rightGatesUI, leftGatesUI;

    public float lerpSpeed;

    private void Update() {

        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(10);
        }
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
    }
}
