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

        if (myLevel < myMaxLevel)
        {
            upgradeStatsText.text = "Damage: " + damage.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(damage.increaseValue) + "</color>)\n" +
                                    "Force: " + force.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(force.increaseValue) + "</color>)\n" +
                                    "Fire Rate: " + cooldown.currentValue.ToString("f2") + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(cooldown.increaseValue) + "</color>)";
        }
        else
        {
            upgradeStatsText.text = "Damage: " + damage.currentValue + "\n" +
                                    "Force: " + force.currentValue + "\n" +
                                    "Fire Rate: " + cooldown.currentValue.ToString("f2");
        }

        if (myLevel == (autoFireLevelReq - 1))
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
        GameObject newProjectile;

        switch (amountOfProjectiles)
        {
            case AmountOfProjectiles.One:

                newProjectile = ObjectPooler.instance.GrabFromPool("ballista projectile", projectileSpawns[0].position, projectileSpawns[0].rotation);

                newProjectile.GetComponent<Projectile>().myDamage = damage.currentValue;
                newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * (force.currentValue + Random.Range(-15, 15)));
                break;
            case AmountOfProjectiles.Three:

                for (int i = 0; i < projectileSpawns.Count; i++)
                {
                    newProjectile = ObjectPooler.instance.GrabFromPool("ballista projectile", projectileSpawns[i].position, projectileSpawns[i].rotation);

                    newProjectile.GetComponent<Projectile>().myDamage = damage.currentValue;
                    newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * (force.currentValue + Random.Range(-15, 15)));
                }
                break;
        }
    }

    public override void Upgrade()
    {
        if (!ResourceManager.instance.HasEnoughGold((int)myUpgradeCost.currentValue) || myLevel >= myMaxLevel)
        {
            return;
        }

        base.Upgrade();

        damage.currentValue += damage.increaseValue;
        force.currentValue += force.increaseValue;
        cooldown.currentValue += cooldown.increaseValue;
        anim.speed = 1 / cooldown.currentValue;

        if (myLevel >= myMaxLevel)
        {
            buyUpgradeButton.interactable = false;
        }

        if (myLevel >= autoFireLevelReq)
        {
            autoFireToggle.SetActive(true);
        }

        SetupUI();
    }
}
