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
    }

    private void AddGold(int amount)
    {
        gold += amount;
        goldToSpawn += (amount / goldPerPhysicalCoin);

        for (int i = 0; i < goldToSpawn; i++)
        {
            GameObject newGold = ObjectPooler.instance.GrabFromPool("gold", goldSpawn.transform.position, Quaternion.Euler(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)));
            goldPrefabsInScene.Add(newGold);
        }

        StartCoroutine(DumpGold());
    }

    private IEnumerator DumpGold()
    {
        for (int i = 0; i < goldPrefabsInScene.Count; i++)
        {
            Rigidbody rb = goldPrefabsInScene[i].GetComponent<Rigidbody>();
            Collider col = goldPrefabsInScene[i].GetComponentInChildren<Collider>();

            if (rb.isKinematic)
            {
                rb.isKinematic = false;
                col.enabled = true;
                yield return new WaitForSeconds(goldSpawnInterval);
            }
        }
    }

    public void RemoveGold(int amount)
    {
        if (gold < amount)
        {
            amount = gold;
        }

        gold -= amount;

        int goldPrefabsToDelete = (amount / goldPerPhysicalCoin);

        for (int i = 0; i < goldPrefabsToDelete; i++)
        {
            goldPrefabsInScene[i].SetActive(false);
            goldPrefabsInScene.RemoveAt(i);
        }
    }
}
