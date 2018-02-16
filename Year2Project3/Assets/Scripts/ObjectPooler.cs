using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour 
{

    [System.Serializable]
    public class ObjectPool
    {
        public string poolName;
        public GameObject poolPrefab;
        public int poolSize;
        public Queue<GameObject> poolQueue = new Queue<GameObject>();
    }

    public static ObjectPooler instance;

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    public List<ObjectPool> pools = new List<ObjectPool>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            for (int ii = 0; ii < pools[i].poolSize; ii++)
            {
                GameObject newPooledObject = Instantiate(pools[i].poolPrefab);
                newPooledObject.SetActive(false);

                pools[i].poolQueue.Enqueue(newPooledObject);
            }

            poolDictionary.Add(pools[i].poolName, pools[i].poolQueue);
        }
    }

    public GameObject GrabFromPool(string poolName, Vector3 position, Quaternion rotation)
    {
        GameObject toGrab = poolDictionary[poolName].Dequeue();
        poolDictionary[poolName].Enqueue(toGrab);

        toGrab.transform.position = position;
        toGrab.transform.rotation = rotation;

        toGrab.SetActive(true);

        return toGrab;
    }
}
