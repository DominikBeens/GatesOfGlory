using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour 
{

    public static ObjectPooler instance;

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
        for (int i = 0; i < goldPoolSize; i++)
        {
            GameObject newGoldObject = Instantiate(goldPrefab);
            newGoldObject.SetActive(false);

            goldPool.Enqueue(newGoldObject);
        }
    }
}
