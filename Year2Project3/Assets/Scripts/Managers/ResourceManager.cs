using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public Animator goldAnim;
    public TextMeshProUGUI goldText;

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
            RemoveGold(5, false);
        }

        goldText.text = goldPrefabsInScene.Count.ToString();

        if (goldToDump > 0)
        {
            if (canDumpGold)
            {
                StartCoroutine(DumpGold());
            }

            goldAnim.ResetTrigger("Close");
            goldAnim.SetTrigger("Open");
        }
        else
        {
            goldAnim.ResetTrigger("Open");
            goldAnim.SetTrigger("Close");
        }
    }

    public void AddGold(int amount)
    {
        goldToDump += amount;

        int extraGoldToSpawn = amount;

        for (int i = 0; i < extraGoldToSpawn; i++)
        {
            GameObject newGold = ObjectPooler.instance.GrabFromPool("gold", goldSpawn.transform.position, Quaternion.Euler(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)));

            int random = Random.Range(0, 2);
            if (random == 0)
            {
                newGold.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                newGold.transform.GetChild(0).gameObject.SetActive(false);
            }

            goldPrefabsInScene.Add(newGold);
            Notary.goldAccumulated++;
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

    public void RemoveGold(int amount, bool spentByPlayer)
    {
        if (goldPrefabsInScene.Count < amount)
        {
            return;
        }

        if (spentByPlayer)
        {
            Notary.goldSpent += amount;
        }
        else
        {
            Notary.goldStolen += amount;
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

    public bool HasEnoughGold(int price)
    {
        if (goldPrefabsInScene.Count >= price)
        {
            return true;
        }
        else
        {
            UIManager.instance.DisplayNotEnoughGoldIcon();
            return false;
        }
    }
}
