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
    }

    public override void Shoot()
    {
        base.Shoot();

        anim.SetTrigger("Fire");
    }

    public void FireProjectile()
    {
        fireSound.pitch = Random.Range(0.6f,0.75f);
        fireSound.Play();
        GameObject newProjectile = ObjectPooler.instance.GrabFromPool("catapult projectile", projectileSpawns[0].position, projectileSpawns[0].rotation);

        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.myDamage = stats.myDamage.currentValue;
        projectile.Fire(forceSlider.value, 0, ForceMode.Impulse);
    }

    public override void Upgrade()
    {
        if (!ResourceManager.instance.HasEnoughGold((int)info.myUpgradeCost.currentValue) || info.myLevel >= info.myMaxLevel)
        {
            return;
        }

        base.Upgrade();

        stats.myDamage.currentValue += stats.myDamage.increaseValue;
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
