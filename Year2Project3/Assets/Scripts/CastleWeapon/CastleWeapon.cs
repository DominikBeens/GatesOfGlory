using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleWeapon : MonoBehaviour 
{

    public enum WeaponType
    {
        Ballista,
        Catapult,
        Canon
    }
    public WeaponType weaponType;

    public string weaponName;
    public int weaponLevel;

    protected bool usingWeapon;

    public Transform rotatableWeapon;
    public GameObject projectile;
    public Transform projectileSpawn;
    protected Transform mouseObject;

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

    public virtual void Awake()
    {
        mouseObject = GameObject.FindWithTag("MouseObject").transform;
    }

    public virtual void Update()
    {
        if (usingWeapon)
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + coolDown.currentValue;
                Shoot();
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

    public virtual void StartUsing()
    {
        usingWeapon = true;
        GameManager.instance.StartUsingWeapon(this);
    }

    public virtual void StopUsing()
    {
        usingWeapon = false;
        GameManager.instance.StopUsingWeapon();
    }
}
