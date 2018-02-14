using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleWeapon : MonoBehaviour 
{

    public enum WeaponType
    {
        Ballista,
        Canon,
        Catapult
    }
    public WeaponType weaponType;

    public enum Side
    {
        Left,
        Right
    }
    public Side side;

    public string weaponName;
    public int weaponLevel;
    public int buildCost;
    public Stat upgradeCost;

    protected bool usingWeapon;
    protected bool shooting;

    public Transform rotatableWeapon;
    public GameObject projectile;
    public List<Transform> projectileSpawns = new List<Transform>();
    public int amountOfProjectiles = 1;

    public GameObject useUI;

    [Header("Properties")]
    public Stat damage;
    public Stat force;
    [Space(10)]
    public Stat coolDown;
    private float nextTimeToFire;

    [Header("Rotation")]
    public float maxXRotation;
    public float minXRotation;
    [Space(10)]
    public LayerMask mouseLayerMask;

    public virtual void Update()
    {
        if (usingWeapon)
        {
            useUI.transform.LookAt(Camera.main.transform);

            if (shooting)
            {
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + coolDown.currentValue;
                    Shoot();
                }
            }

            if (Input.GetButtonDown("Cancel"))
            {
                StopUsing();
            }
        }
    }

    public virtual void Shoot()
    {

    }

    public void Upgrade()
    {

    }

    public void ToggleAutoShoot()
    {
        shooting = !shooting;
    }

    public virtual void StartUsing()
    {
        usingWeapon = true;
        GameManager.instance.StartUsingWeapon(this);

        useUI.SetActive(true);
    }

    public virtual void StopUsing()
    {
        StartCoroutine(EventStopUsing());
    }

    public IEnumerator EventStopUsing()
    {
        useUI.GetComponent<Animator>().SetTrigger("CloseUI");

        yield return new WaitForSeconds(0.5f);

        usingWeapon = false;
        GameManager.instance.StopUsingWeapon();

        shooting = false;

        useUI.SetActive(false);
    }

    public void SetLeftSide()
    {
        side = Side.Left;
    }

    public void SetRightSide()
    {
        side = Side.Right;
        transform.rotation = Quaternion.Euler(0, 180, 0);
        useUI.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
