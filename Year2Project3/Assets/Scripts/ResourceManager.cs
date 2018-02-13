using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour 
{

    public static ResourceManager instance;

    [Header("Gold")]
    public static int gold;
    public const int goldPerPhysicalCoin = 10;
    public GameObject goldPrefab;
    public Transform goldSpawn;
    public float goldSpawnInterval;
    private bool canSpawnGold = true;
    private int goldToSpawn;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddGold(1000);
        }

        if (goldToSpawn > 0)
        {
            if (canSpawnGold)
            {
                StartCoroutine(SpawnGold());
            }
        }
    }

    private void AddGold(int amount)
    {
        gold += amount;
        goldToSpawn += (amount / goldPerPhysicalCoin);
    }

    private IEnumerator SpawnGold()
    {
        canSpawnGold = false;

        Instantiate(goldPrefab, goldSpawn.transform.position, Quaternion.Euler(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)));
        goldToSpawn--;

        yield return new WaitForSeconds(goldSpawnInterval);

        canSpawnGold = true;
    }
}
