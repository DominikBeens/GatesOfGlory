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
    public Toggle autoFireToggle;
    public GameObject autoFireLockedOverlay;

    protected bool shooting;
    private bool autoFire;

    [Header("Weapon Setup")]
    public GameObject projectile;
    public List<Transform> projectileSpawns = new List<Transform>();
    public enum AmountOfProjectiles
    {
        One,
        Three
    }
    public AmountOfProjectiles amountOfProjectiles;

    public WeaponStats stats;
    private float nextTimeToFire;

    public override void Awake()
    {
        base.Awake();

        stats = Instantiate(stats);
    }

    public virtual void Update()
    {
        if (usingBuilding || autoFire)
        {
            useUI.transform.parent.parent.LookAt(mainCam.transform);

            if (shooting || autoFire)
            {
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + stats.myFireRate.currentValue;
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
        myNameText.text = "Level <color=green>" + info.myLevel.ToString() + "</color> " + info.myName;
        upgradeCostText.text = info.myUpgradeCost.currentValue.ToString();
    }

    public virtual void Shoot()
    {

    }

    public override void Upgrade()
    {
        base.Upgrade();
    }

    public void ToggleAutoShoot()
    {
        shooting = !shooting;
    }

    public void ToggleAutoFire()
    {
        if (info.myLevel < stats.autoFireLevelRequirement)
        {
            return;
        }

        autoFire = !autoFire;
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
