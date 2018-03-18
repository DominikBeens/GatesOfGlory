using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleWeapon_Catapult : CastleWeapon
{

    [Header("Catapult Setup")]
    public Slider forceSlider;
    [Space(10)]
    public float minForce;
    public float maxForce;
    [Space(10)]
    public GameObject autoFireNotification;

    public override void Awake()
    {
        base.Awake();

        forceSlider.minValue = minForce;
        forceSlider.maxValue = maxForce;

        forceSlider.value = 0.5f * maxForce;
    }

    public override void SetupUI()
    {
        base.SetupUI();

        if (myLevel < myMaxLevel)
        {
            upgradeStatsText.text = "Damage: " + damage.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(damage.increaseValue) + "</color>)\n" +
                                    "Fire Rate: " + cooldown.currentValue.ToString("f2") + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(cooldown.increaseValue) + "</color>)";
        }
        else
        {
            upgradeStatsText.text = "Damage: " + damage.currentValue + "\n" +
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
    }

    public override void Shoot()
    {
        base.Shoot();

        anim.SetTrigger("Fire");
    }

    public void FireProjectile()
    {
        GameObject newProjectile = ObjectPooler.instance.GrabFromPool("catapult projectile", projectileSpawns[0].position, projectileSpawns[0].rotation);

        newProjectile.GetComponent<Projectile>().myDamage = damage.currentValue;
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * forceSlider.value, ForceMode.Impulse);
    }

    public override void Upgrade()
    {
        if (!ResourceManager.instance.HasEnoughGold((int)myUpgradeCost.currentValue) || myLevel >= myMaxLevel)
        {
            return;
        }

        base.Upgrade();

        damage.currentValue += damage.increaseValue;
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
