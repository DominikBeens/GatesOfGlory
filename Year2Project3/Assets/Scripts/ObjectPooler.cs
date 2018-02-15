using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour 
{

    public static ObjectPooler instance;

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    [Header("Gold")]
    public int goldPoolSize;
    public GameObject goldPrefab;
    public Queue<GameObject> goldPool = new Queue<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        poolDictionary.Add("gold", goldPool);

        for (int i = 0; i < goldPoolSize; i++)
        {
            GameObject newGoldObject = Instantiate(goldPrefab);
            newGoldObject.SetActive(false);

            goldPool.Enqueue(newGoldObject);
        }
    }

    public void GrabFromPool(string poolName, Vector3 position, Quaternion rotation)
    {
        GameObject toGrab = null;

        if (poolName == "gold")
        {
            toGrab = goldPool.Dequeue();
            goldPool.Enqueue(toGrab);
        }

        toGrab.transform.position = position;
        toGrab.transform.rotation = rotation;

        toGrab.SetActive(true);
    }
}
