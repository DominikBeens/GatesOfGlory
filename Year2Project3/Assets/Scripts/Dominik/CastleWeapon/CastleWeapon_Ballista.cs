using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleWeapon_Ballista : CastleWeapon
{

    [Header("Ballista Setup")]
    public float maxXRotation;
    public float minXRotation;
    [Space(10)]
    public float rotationSpeed;
    public Slider rotationSlider;
    [Space(10)]
    public Transform rotatableWeapon;
    [Space(10)]
    public GameObject autoFireNotification;

    public override void Awake()
    {
        base.Awake();

        rotationSlider.minValue = minXRotation;
        rotationSlider.maxValue = maxXRotation;
    }

    public override void SetupUI()
    {
        base.SetupUI();

        if (info.myLevel < info.myMaxLevel)
        {
            upgradeStatsText.text = "Damage: " + stats.myDamage.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(stats.myDamage.increaseValue) + "</color>)\n" +
                                    "Fire Rate: " + stats.myFireRate.currentValue.ToString("f2") + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(stats.myFireRate.increaseValue) + "</color>)";
        }
        else
        {
            upgradeStatsText.text = "Damage: " + stats.myDamage.currentValue + "\n" +
                                    "Fire Rate: " + stats.myFireRate.currentValue.ToString("f2");
        }

        if (info.myLevel == (stats.autoFireLevelRequirement - 1))
        {
            autoFireNotification.SetActive(true);
        }
        else
        {
            autoFireNotification.SetActive(false);
        }
    }

    public override void Update()
    {
        base.Update();

        if (usingBuilding)
        {
            Quaternion newRotation;

            if (side == Side.Left)
            {
                newRotation = Quaternion.Euler(rotationSlider.value, -90, 0);
            }
            else
            {
                newRotation = Quaternion.Euler(rotationSlider.value, 90, 0);
            }

            rotatableWeapon.transform.rotation = Quaternion.RotateTowards(rotatableWeapon.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        anim.SetTrigger("Fire");
    }

    public void FireProjectile()
    {
        fireSound.pitch = Random.Range(0.85f,1);
        fireSound.Play();

        GameObject newProjectile;
        Projectile projectile;

        switch (amountOfProjectiles)
        {
            case AmountOfProjectiles.One:

                newProjectile = ObjectPooler.instance.GrabFromPool("ballista projectile", projectileSpawns[0].position, projectileSpawns[0].rotation);

                projectile = newProjectile.GetComponent<Projectile>();
                projectile.myDamage = stats.myDamage.currentValue;
                projectile.Fire(stats.myForce.currentValue, 15, ForceMode.Force);
                break;
            case AmountOfProjectiles.Three:

                for (int i = 0; i < projectileSpawns.Count; i++)
                {
                    newProjectile = ObjectPooler.instance.GrabFromPool("ballista projectile", projectileSpawns[i].position, projectileSpawns[i].rotation);

                    projectile = newProjectile.GetComponent<Projectile>();
                    projectile.myDamage = stats.myDamage.currentValue;
                    projectile.Fire(stats.myForce.currentValue, 15, ForceMode.Force);
                }
                break;
        }
    }

    public override void Upgrade()
    {
        if (!ResourceManager.instance.HasEnoughGold((int)info.myUpgradeCost.currentValue) || info.myLevel >= info.myMaxLevel)
        {
            return;
        }

        base.Upgrade();

        stats.myDamage.currentValue += stats.myDamage.increaseValue;
        stats.myForce.currentValue += stats.myForce.increaseValue;
        stats.myFireRate.currentValue += stats.myFireRate.increaseValue;
        anim.speed = 1 / stats.myFireRate.currentValue;

        if (info.myLevel >= info.myMaxLevel)
        {
            buyUpgradeButton.interactable = false;
        }

        if (info.myLevel >= stats.autoFireLevelRequirement)
        {
            autoFireLockedOverlay.SetActive(false);
            autoFireToggle.interactable = true;
        }

        SetupUI();
    }
}
