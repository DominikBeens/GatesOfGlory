using UnityEngine;
using System.Collections;

public class GoldCoin : MonoBehaviour 
{

    public Rigidbody myRb;
    public Collider myCollider;

    public float rBDisableTimer = 3;

    public void StartRBTimer()
    {
        StartCoroutine(Timer(rBDisableTimer));
    }

    private IEnumerator Timer(float f)
    {
        yield return new WaitForSeconds(f);
        myRb.isKinematic = true;
    }
}
