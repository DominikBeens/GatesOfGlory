using UnityEngine;

public class PooledObject : MonoBehaviour 
{

    public string myPoolName;

    public enum ReAddType
    {
        WaitForDisable,
        DisableAfterTimer
    }
    [Space(10)]
    public ReAddType reAddType;
    public float selfReAddTimer;

    private float reAddTimer;

    private void OnEnable()
    {
        reAddTimer = selfReAddTimer;    
    }

    private void Update()
    {
        if (reAddType == ReAddType.DisableAfterTimer)
        {
            if (reAddTimer > 0)
            {
                reAddTimer -= Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        ObjectPooler.instance.AddToPool(myPoolName, gameObject);
    }
}
