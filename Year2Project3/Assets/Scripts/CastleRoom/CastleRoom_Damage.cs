using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleRoom_Damage : CastleRoom
{

    [Header("Stats")]
    public Stat useCooldown;
    public Stat damageAmount;
    public Stat amountOfEnemiesToDamage;

    [Header("Damage Room Setup")]
    public Image cooldownFill;
    public TextMeshProUGUI descriptionText;

    private float currentCooldown = 0.95f;

    public override void SetupUI()
    {
        base.SetupUI();

        descriptionText.text = "Strikes <color=green>" + amountOfEnemiesToDamage.currentValue + "</color> enemies for <color=green>" + damageAmount.currentValue + "</color> hp.";

        upgradeStatsText.text = "Damage amount: " + damageAmount.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(damageAmount.increaseValue) + "</color>)" + "\n" +
                                "Amount of targets: " + amountOfEnemiesToDamage.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(amountOfEnemiesToDamage.increaseValue) + "</color>)" + "\n" +
                                "Cooldown: " + useCooldown.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(useCooldown.increaseValue) + "</color>)";
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

        List<Enemy> toDamage = new List<Enemy>();
        for (int i = 0; i < amountOfEnemiesToDamage.currentValue; i++)
        {
            List<Enemy> canBeDamaged = new List<Enemy>(WaveManager.instance.enemiesInScene);

            for (int ii = 0; ii < canBeDamaged.Count; ii++)
            {
                if (toDamage.Contains(canBeDamaged[ii]))
                {
                    canBeDamaged.Remove(canBeDamaged[ii]);
                }
            }

            if (canBeDamaged.Count == 0)
            {
                return;
            }

            toDamage.Add(canBeDamaged[Random.Range(0, canBeDamaged.Count)]);
        }

        for (int i = 0; i < toDamage.Count; i++)
        {
            WaveManager.instance.enemiesInScene[i].TakeDamage(damageAmount.currentValue);

            Vector3 particleSpawn = new Vector3(WaveManager.instance.enemiesInScene[i].transform.position.x, WaveManager.instance.enemiesInScene[i].transform.position.y + 1.5f);
            ObjectPooler.instance.GrabFromPool("damage particle", particleSpawn, Quaternion.identity);
        }
    }

    public override void Upgrade()
    {
        if (!ResourceManager.instance.HasEnoughGold((int)upgradeCost.currentValue) || roomLevel >= maxRoomLevel)
        {
            return;
        }

        base.Upgrade();

        useCooldown.currentValue += useCooldown.increaseValue;
        damageAmount.currentValue += damageAmount.increaseValue;
        amountOfEnemiesToDamage.currentValue += amountOfEnemiesToDamage.increaseValue;

        SetupUI();
    }
}
