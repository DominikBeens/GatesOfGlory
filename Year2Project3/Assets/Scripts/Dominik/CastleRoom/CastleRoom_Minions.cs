using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleRoom_Minions : CastleRoom
{

    [Header("Minion Room Setup")]
    public Transform minionSpawnPoint;

    [Header("Minion Room Stats")]
    public Stat knightSpawnCost;
    public Stat archerSpawnCost;
    public Stat spearmanSpawnCost;
    public Stat amountToSpawnPerBuy;
    public float spawnInterval;
    public float spawnPointOffsetRandomizer;
    public float statMultiplier;
    public float buttonClickTimer;

    private bool canSpawn = true;
    private int currentAmountToSpawn;

    private int knightsToSpawn;
    private int archersToSpawn;
    private int spearmenToSpawn;

    private float buyKnightsTimer = 1;
    public Image buyKnightsTimerFill;
    private float buyArchersTimer = 1;
    public Image buyArchersTimerFill;
    private float buySpearmenTimer = 1;
    public Image buySpearmenTimerFill;

    [Header("UI")]
    public TextMeshProUGUI roomTypeText;
    public TextMeshProUGUI knightCostText;
    public TextMeshProUGUI archerCostText;
    public TextMeshProUGUI spearmanCostText;

    public override void Update()
    {
        base.Update();

        if (currentAmountToSpawn > 0)
        {
            if (canSpawn)
            {
                StartCoroutine(SpawnMinions());
            }
        }

        buyKnightsTimer = (buyKnightsTimer < 1) ? buyKnightsTimer += 1 / buttonClickTimer * Time.deltaTime : 1;
        buyKnightsTimerFill.fillAmount = buyKnightsTimer;

        buyArchersTimer = (buyArchersTimer < 1) ? buyArchersTimer += 1 / buttonClickTimer * Time.deltaTime : 1;
        buyArchersTimerFill.fillAmount = buyArchersTimer;

        buySpearmenTimer = (buySpearmenTimer < 1) ? buySpearmenTimer += 1 / buttonClickTimer * Time.deltaTime : 1;
        buySpearmenTimerFill.fillAmount = buySpearmenTimer;
    }

    public override void SetupUI()
    {
        base.SetupUI();

        knightCostText.text = "Cost: <color=yellow>" + knightSpawnCost.currentValue + "</color>";
        archerCostText.text = "Cost: <color=yellow>" + archerSpawnCost.currentValue + "</color>";
        spearmanCostText.text = "Cost: <color=yellow>" + spearmanSpawnCost.currentValue + "</color>";

        if (info.myLevel < info.myMaxLevel)
        {
            upgradeStatsText.text = "All minions get a <color=green>" + ((info.myLevel * statMultiplier) * 100) + "%</color>" + " stat boost." + "\n" +
                                    "Stats include: health and damage.";
            //"Spawn cost: " + spawnCost.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(spawnCost.increaseValue) + "</color>)" + "\n" +
            //"Spawns per buy: " + amountToSpawnPerBuy.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(amountToSpawnPerBuy.increaseValue) + "</color>)";
        }
        else
        {
            upgradeStatsText.text = "All upgrades maxed";
        }
    }

    public void BuyMinionsButton(int type)
    {
        switch (type)
        {
            case 0:
                if (!ResourceManager.instance.HasEnoughGold((int)knightSpawnCost.currentValue))
                {
                    return;
                }
                if (buyKnightsTimer < 1)
                {
                    return;
                }

                buyKnightsTimer = 0;
                knightsToSpawn += (int)amountToSpawnPerBuy.currentValue;
                ResourceManager.instance.RemoveGold((int)knightSpawnCost.currentValue, true);
                break;
            case 1:
                if (!ResourceManager.instance.HasEnoughGold((int)archerSpawnCost.currentValue))
                {
                    return;
                }
                if (buyArchersTimer < 1)
                {
                    return;
                }

                buyArchersTimer = 0;
                archersToSpawn += (int)amountToSpawnPerBuy.currentValue;
                ResourceManager.instance.RemoveGold((int)archerSpawnCost.currentValue, true);
                break;
            case 2:
                if (!ResourceManager.instance.HasEnoughGold((int)spearmanSpawnCost.currentValue))
                {
                    return;
                }
                if (buySpearmenTimer < 1)
                {
                    return;
                }

                buySpearmenTimer = 0;
                spearmenToSpawn += (int)amountToSpawnPerBuy.currentValue;
                ResourceManager.instance.RemoveGold((int)spearmanSpawnCost.currentValue, true);
                break;
        }

        currentAmountToSpawn += (int)amountToSpawnPerBuy.currentValue;
    }

    public override void UseRoom()
    {
        base.UseRoom();
    }

    public IEnumerator SpawnMinions()
    {
        canSpawn = false;

        Vector3 spawnOffset = new Vector3(Random.Range(minionSpawnPoint.position.x - spawnPointOffsetRandomizer, minionSpawnPoint.position.x + spawnPointOffsetRandomizer),
                                          minionSpawnPoint.position.y,
                                          Random.Range(minionSpawnPoint.position.z - spawnPointOffsetRandomizer, minionSpawnPoint.position.z + spawnPointOffsetRandomizer));

        GameObject newMinion = null;
        if (knightsToSpawn > 0)
        {
            newMinion = ObjectPooler.instance.GrabFromPool("Ally Knight", Vector3.zero + spawnOffset, Quaternion.Euler(Vector3.zero));
            knightsToSpawn--;
        }
        else if (archersToSpawn > 0)
        {
            newMinion = ObjectPooler.instance.GrabFromPool("Ally Bowman", Vector3.zero + spawnOffset, Quaternion.Euler(Vector3.zero));
            archersToSpawn--;
        }
        else if (spearmenToSpawn > 0)
        {
            newMinion = ObjectPooler.instance.GrabFromPool("Ally Spearmen", Vector3.zero + spawnOffset, Quaternion.Euler(Vector3.zero));
            spearmenToSpawn--;
        }
        newMinion.GetComponent<AudioSource>().pitch = Random.Range(0.75f, 1.25f);
        newMinion.GetComponent<AudioSource>().volume = Random.Range(0.01f, 0.08f);
        newMinion.transform.SetParent(null);

        if (info.myLevel > 1)
        {
            Damagebles minion = newMinion.GetComponent<Damagebles>();
            minion.myStats.health.currentValue *= (info.myLevel * statMultiplier);
            minion.myStats.damage.currentValue *= (info.myLevel * statMultiplier);
        }

        WaveManager.instance.alliesInScene.Add(newMinion.GetComponent<Allie>());

        currentAmountToSpawn--;

        yield return new WaitForSeconds(spawnInterval);

        canSpawn = true;
    }

    public override void Upgrade()
    {
        if (!ResourceManager.instance.HasEnoughGold((int)info.myUpgradeCost.currentValue) || info.myLevel >= info.myMaxLevel)
        {
            return;
        }

        base.Upgrade();

        amountToSpawnPerBuy.currentValue += amountToSpawnPerBuy.increaseValue;

        SetupUI();
    }
}
