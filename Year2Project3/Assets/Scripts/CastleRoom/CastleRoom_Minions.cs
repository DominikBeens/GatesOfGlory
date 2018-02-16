using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CastleRoom_Minions : CastleRoom 
{

    [Space(10)]
    public GameObject minionToSpawn;
    public Transform minionSpawnPoint;
    [Space(10)]
    public int spawnCost;
    public int amountToSpawnPerBuy;
    public float spawnInterval;
    public float spawnPointOffsetRandomizer;

    private bool canSpawn = true;
    private int currentAmountToSpawn;

    [Header("UI")]
    public TextMeshProUGUI roomTypeText;
    public TextMeshProUGUI roomMinionCostText;

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
    }

    public override void SetupUI()
    {
        base.SetupUI();

        roomTypeText.text = "Type: <color=green>" + roomType + "</color>";
        roomMinionCostText.text = "Spawn Cost: " + spawnCost;
    }

    public override void UseRoom()
    {
        base.UseRoom();

        if (ResourceManager.gold >= spawnCost)
        {
            currentAmountToSpawn += amountToSpawnPerBuy;
        }
    }

    public IEnumerator SpawnMinions()
    {
        canSpawn = false;

        Vector3 spawnOffset = new Vector3(Random.Range(minionSpawnPoint.position.x - spawnPointOffsetRandomizer, minionSpawnPoint.position.x + spawnPointOffsetRandomizer),
                                          minionSpawnPoint.position.y,
                                          Random.Range(minionSpawnPoint.position.z - spawnPointOffsetRandomizer, minionSpawnPoint.position.z + spawnPointOffsetRandomizer));

        GameObject newMinion = Instantiate(minionToSpawn, minionSpawnPoint.position, Quaternion.identity, minionSpawnPoint);
        newMinion.transform.position = Vector3.zero + spawnOffset;

        WaveManager.instance.alliesInScene.Add(newMinion.GetComponent<Allie>());

        currentAmountToSpawn--;

        yield return new WaitForSeconds(spawnInterval);

        canSpawn = true;
    }
}
