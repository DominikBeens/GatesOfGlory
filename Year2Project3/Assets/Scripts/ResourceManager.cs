using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour 
{

    public static ResourceManager instance;

    [Header("Gold")]
    public static int gold;
    public const int goldPerPhysicalCoin = 10;
    public Transform goldSpawn;
    public float goldSpawnInterval;
    private bool canSpawnGold = true;
    private int goldToSpawn;
    private List<GameObject> goldPrefabsInScene = new List<GameObject>();


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
        if (Input.GetKeyDown(KeyCode.H))
        {
            RemoveGold(100);
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

        GameObject newGold = ObjectPooler.instance.GrabFromPool("gold", goldSpawn.transform.position, Quaternion.Euler(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)));

        goldPrefabsInScene.Add(newGold);

        goldToSpawn--;

        yield return new WaitForSeconds(goldSpawnInterval);

        canSpawnGold = true;
    }

    public void RemoveGold(int amount)
    {
        gold -= amount;

        int goldPrefabsToDelete = (amount / goldPerPhysicalCoin);

        for (int i = 0; i < goldPrefabsToDelete; i++)
        {
            goldPrefabsInScene[i].SetActive(false);
            goldPrefabsInScene.RemoveAt(0);
        }
    }
}
