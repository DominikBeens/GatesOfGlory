using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void UseRoom()
    {
        base.UseRoom();

        currentAmountToSpawn += amountToSpawnPerBuy;
    }

    public IEnumerator SpawnMinions()
    {
        canSpawn = false;

        Vector3 spawnOffset = new Vector3(Random.Range(minionSpawnPoint.position.x - spawnPointOffsetRandomizer, minionSpawnPoint.position.x + spawnPointOffsetRandomizer),
                                          minionSpawnPoint.position.y,
                                          Random.Range(minionSpawnPoint.position.z - spawnPointOffsetRandomizer, minionSpawnPoint.position.z + spawnPointOffsetRandomizer));

        GameObject newMinion = Instantiate(minionToSpawn, minionSpawnPoint.position, Quaternion.identity, minionSpawnPoint);
        newMinion.transform.position = Vector3.zero + spawnOffset;

        currentAmountToSpawn--;

        yield return new WaitForSeconds(spawnInterval);

        canSpawn = true;
    }
}
