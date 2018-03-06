using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour 
{

    public static ResourceManager instance;

    [Header("Gold")]
    public int goldToStartWith;
    public Transform goldSpawn;
    public float goldSpawnInterval;
    private bool canDumpGold = true;
    private int goldToDump;
    public List<GameObject> goldPrefabsInScene = new List<GameObject>();

    [Header("Enemy Kill Rewards")]
    public int normalEnemyGoldReward;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        AddGold(goldToStartWith);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddGold(100);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            RemoveGold(100);
        }

        if (goldToDump > 0)
        {
            if (canDumpGold)
            {
                StartCoroutine(DumpGold());
            }
        }
    }

    public void AddGold(int amount)
    {
        goldToDump += amount;

        int extraGoldToSpawn = amount;

        for (int i = 0; i < extraGoldToSpawn; i++)
        {
            GameObject newGold = ObjectPooler.instance.GrabFromPool("gold", goldSpawn.transform.position, Quaternion.Euler(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)));
            goldPrefabsInScene.Add(newGold);
        }
    }

    private IEnumerator DumpGold()
    {
        canDumpGold = false;

        for (int i = 0; i < goldPrefabsInScene.Count; i++)
        {
            Rigidbody rb = goldPrefabsInScene[i].GetComponent<Rigidbody>();

            if (rb.isKinematic)
            {
                Collider col = goldPrefabsInScene[i].GetComponentInChildren<Collider>();

                rb.isKinematic = false;
                col.enabled = true;

                goldToDump--;

                yield return new WaitForSeconds(goldSpawnInterval);
            }
        }

        canDumpGold = true;
    }

    public void RemoveGold(int amount)
    {
        if (goldPrefabsInScene.Count < amount)
        {
            return;
        }

        if (goldToDump >= amount)
        {
            goldToDump -= amount;
            return;
        }
        else
        {
            if (goldToDump > 0)
            {
                goldToDump = 0;
            }
        }

        int goldPrefabsToDelete = amount;

        for (int i = 0; i < goldPrefabsToDelete; i++)
        {
            Rigidbody rb = goldPrefabsInScene[goldPrefabsInScene.Count - 1].GetComponent<Rigidbody>();
            Collider col = goldPrefabsInScene[goldPrefabsInScene.Count - 1].GetComponentInChildren<Collider>();

            rb.isKinematic = true;
            col.enabled = false;

            ObjectPooler.instance.AddToPool("gold", goldPrefabsInScene[goldPrefabsInScene.Count - 1]);
            goldPrefabsInScene.RemoveAt(goldPrefabsInScene.Count - 1);
        }
    }
}
