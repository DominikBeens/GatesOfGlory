using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleRoom_Heal : CastleRoom 
{

    [Header("Stats")]
    public Stat useCooldown;
    public Stat healAmount;

    [Space(10)]
    public Image cooldownFill;
    public TextMeshProUGUI descriptionText;
    public GameObject nextLevelExtraUpgradePanel;

    private float currentCooldown = 0.95f;

    public override void SetupUI()
    {
        base.SetupUI();

        descriptionText.text = "Heals all allies for <color=green>" + healAmount.currentValue + "</color> hp.";

        upgradeStatsText.text = "Heal amount: " + healAmount.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(healAmount.increaseValue) + "</color>)" + "\n" +
                                "Cooldown: " + useCooldown .currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(useCooldown.increaseValue) + "</color>)";

        if (myLevel == 4)
        {
            nextLevelExtraUpgradePanel.SetActive(true);
        }
        else
        {
            nextLevelExtraUpgradePanel.SetActive(false);
        }
    }

    public override void Update()
    {
        base.Update();

        if (currentCooldown < 1)
        {
            currentCooldown += 1 / useCooldown.currentValue * Time.deltaTime;
            cooldownFill.fillAmount = currentCooldown;
        }
        else
        {
            currentCooldown = 1;
        }
    }

    public override void UseRoom()
    {
        base.UseRoom();

        if (currentCooldown < 1)
        {
            return;
        }

        currentCooldown = 0;

        for (int i = 0; i < WaveManager.instance.alliesInScene.Count; i++)
        {
            WaveManager.instance.alliesInScene[i].TakeDamage(-healAmount.currentValue);

            Vector3 particleSpawn = new Vector3(WaveManager.instance.alliesInScene[i].transform.position.x, WaveManager.instance.alliesInScene[i].transform.position.y + 1.5f);
            ObjectPooler.instance.GrabFromPool("heal particle", particleSpawn, Quaternion.identity);
        }
    }

    public override void Upgrade()
    {
        if (!ResourceManager.instance.HasEnoughGold((int)myUpgradeCost.currentValue) || myLevel >= myMaxLevel)
        {
            return;
        }

        base.Upgrade();

        useCooldown.currentValue += useCooldown.increaseValue;
        healAmount.currentValue += healAmount.increaseValue;

        SetupUI();
    }
}
