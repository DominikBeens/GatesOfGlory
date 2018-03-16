using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleWeapon : CastleBuild 
{

    public enum WeaponType
    {
        Ballista,
        Canon,
        Catapult
    }
    public WeaponType weaponType;

    [Header("Auto-Fire")]
    public int autoFireLevelReq;
    public GameObject autoFireToggle;

    protected bool usingWeapon;
    protected bool shooting;
    private bool autoFire;

    [Header("Weapon Setup")]
    public Transform rotatableWeapon;
    public GameObject projectile;
    public List<Transform> projectileSpawns = new List<Transform>();
    public enum AmountOfProjectiles
    {
        One,
        Three
    }
    public AmountOfProjectiles amountOfProjectiles;

    public Animator anim;

    [Header("Stats")]
    public Stat damage;
    public Stat force;
    public Stat cooldown;
    private float nextTimeToFire;

    [Header("Rotation Options")]
    public float maxXRotation;
    public float minXRotation;

    public override void Awake()
    {
        base.Awake();
    }

    public virtual void Update()
    {
        if (usingWeapon || autoFire)
        {
            useUI.transform.parent.parent.LookAt(Camera.main.transform);

            if (shooting || autoFire)
            {
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + cooldown.currentValue;
                    Shoot();
                }
            }

            if (Input.GetButtonDown("Cancel"))
            {
                StopUsingButton();
            }
        }
    }

    public override void SetupUI()
    {

    }

    public virtual void Shoot()
    {

    }

    public override void Upgrade()
    {

    }

    public void ToggleAutoShoot()
    {
        shooting = !shooting;
    }

    public void ToggleAutoFire()
    {
        autoFire = !autoFire;
    }

    public override void StopUsingButton()
    {
        base.StopUsingButton();
    }

    public void SetLeftSide()
    {
        side = Side.Left;
    }

    public void SetRightSide()
    {
        side = Side.Right;
        transform.rotation = Quaternion.Euler(0, 180, 0);
        useUI.transform.parent.parent.rotation = Quaternion.Euler(Vector3.zero);
    }
}
