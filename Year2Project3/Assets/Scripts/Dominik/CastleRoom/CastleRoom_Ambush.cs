﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleRoom_Ambush : CastleRoom
{

    private int selectedAmbush;

    [Header("Stats")]
    public Stat useCooldown;
    public Stat damageAmount;

    [Header("Ambush Options")]
    public AmbushOptions volleyOptions;
    public AmbushOptions meteorOptions;
    public AmbushOptions spearOptions;

    [Header("Ambush Room Setup")]
    public Image cooldownFill;
    public TextMeshProUGUI descriptionText;
    private Transform cameraTarget;
    public float camZoomSpeed;

    [Header("Upgrades")]
    public TextMeshProUGUI nextLevelExtraUpgradeText;
    public GameObject nextLevelExtraUpgradePanel;
    public GameObject meteorStrikeButton;
    public GameObject spearRainButton;

    private GameObject[] ambushSpawns;
    private GameObject[] spearRainSpawns;

    private float currentCooldown = 0.95f;

    private CameraManager[] cams;
    private Camera[] camComponents;

    [System.Serializable]
    public struct AmbushOptions
    {
        public int rows;
        public int columns;
        public float distOffset;
        public float spawnDelay;
        public float spawnOffset;
    }

    public override void Awake()
    {
        base.Awake();

        cameraTarget = GameObject.FindWithTag("CameraTarget").transform;

        ambushSpawns = GameObject.FindGameObjectsWithTag("AmbushSpawn");
        spearRainSpawns = GameObject.FindGameObjectsWithTag("SpearRainSpawn");

        cams = FindObjectsOfType<CameraManager>();
        camComponents = FindObjectsOfType<Camera>();
    }

    public override void SetupUI()
    {
        base.SetupUI();

        descriptionText.text = "Ambushes the enemies with a strike from above.\n Deals " + "<color=green>" + damageAmount.currentValue + "</color> damage.";

        if (info.myLevel < info.myMaxLevel)
        {
            upgradeStatsText.text = "Damage amount: " + damageAmount.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(damageAmount.increaseValue) + "</color>)" + "\n" +
                                    "Cooldown: " + useCooldown.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(useCooldown.increaseValue) + "</color>)";
        }
        else
        {
            upgradeStatsText.text = "Damage amount: " + damageAmount.currentValue + "\n" +
                                    "Cooldown: " + useCooldown.currentValue;
        }

        if (info.myLevel == 4)
        {
            nextLevelExtraUpgradePanel.SetActive(true);
            nextLevelExtraUpgradeText.text = "Next level will unlock the ambush: meteor strike";
        }
        else if (info.myLevel == 9)
        {
            nextLevelExtraUpgradePanel.SetActive(true);
            nextLevelExtraUpgradeText.text = "Next level will unlock the ambush: spear rain";
        }
        else
        {
            nextLevelExtraUpgradePanel.SetActive(false);
            nextLevelExtraUpgradeText.text = string.Empty;
        }

        if (info.myLevel >= 5)
        {
            meteorStrikeButton.SetActive(true);
        }

        if (info.myLevel >= 10)
        {
            spearRainButton.SetActive(true);
        }
    }

    public override void Update()
    {
        base.Update();

        if (currentCooldown < 1)
        {
            currentCooldown += 1 / useCooldown.currentValue * Time.deltaTime;
            cooldownFill.fillAmount = currentCooldown;
        }
        else
        {
            currentCooldown = 1;
        }
    }

    public override void UseRoom()
    {
        base.UseRoom();

        if (currentCooldown < 1)
        {
            return;
        }

        currentCooldown = 0;

        switch (selectedAmbush)
        {
            case 0:

                StartCoroutine(ProjectileRain("ambush volley", volleyOptions.rows, volleyOptions.columns, volleyOptions.distOffset, volleyOptions.spawnDelay, volleyOptions.spawnOffset, false, 2, ambushSpawns));
                break;

            case 1:

                StartCoroutine(ProjectileRain("ambush meteor", meteorOptions.rows, meteorOptions.columns, meteorOptions.distOffset, meteorOptions.spawnDelay, meteorOptions.spawnOffset, true, 4, ambushSpawns));
                break;

            case 2:

                StartCoroutine(ProjectileRain("ambush spear", spearOptions.rows, spearOptions.columns, spearOptions.distOffset, spearOptions.spawnDelay, spearOptions.spawnOffset, false, 2, spearRainSpawns));
                break;
        }

        StopUsingButton();
    }

    private IEnumerator ProjectileRain(string projectile, int rainRows, int rainColumns, float rainDistOffset, float rainSpawnDelay, float rainSpawnOffset, bool addTorque, float screenShakeAmount, GameObject[] spawns)
    {
        Vector3 zoomTo = new Vector3(0, 15, -25);

        for (int i = 0; i < cams.Length; i++)
        {
            cams[i].canMove = false;
        }

        while (Vector3.Distance(cameraTarget.position, zoomTo) > 0.1f)
        {
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, zoomTo, Time.deltaTime * camZoomSpeed);

            for (int i = 0; i < camComponents.Length; i++)
            {
                camComponents[i].fieldOfView = Mathf.Lerp(camComponents[i].fieldOfView, 60, Time.deltaTime * (camZoomSpeed * 5));
            }
            yield return null;
        }

        mainCam.GetComponent<CameraShake>().Shake(rainSpawnDelay * rainRows + 5, screenShakeAmount, screenShakeAmount, 0, 0, 1);

        int toSpawnLeft = rainRows;
        int toSpawnRight = rainRows;

        for (int i = 0; i < rainRows; i++)
        {
            for (int ii = 0; ii < spawns.Length; ii++)
            {
                Vector3 spawnPos = new Vector3();

                if (spawns[ii].transform.position.x < 0)
                {
                    spawnPos = new Vector3(spawns[ii].transform.position.x - (rainDistOffset * toSpawnLeft + GetRandomOffset(rainSpawnOffset)), spawns[ii].transform.position.y + GetRandomOffset(rainSpawnOffset), ambushSpawns[ii].transform.position.z);
                    toSpawnLeft--;
                }
                else
                {
                    spawnPos = new Vector3(spawns[ii].transform.position.x + (rainDistOffset * toSpawnRight + GetRandomOffset(rainSpawnOffset)), spawns[ii].transform.position.y + GetRandomOffset(rainSpawnOffset), ambushSpawns[ii].transform.position.z);
                    toSpawnRight--;
                }

                GameObject newProjectile = SpawnProjectile(projectile, spawnPos, spawns[ii].transform.rotation, addTorque);

                Vector3 columnSpawnPos = new Vector3();
                int columns = rainColumns;
                for (int iii = 0; iii < rainColumns; iii++)
                {
                    if (spawns[ii].transform.position.x < 0)
                    {
                        columnSpawnPos = new Vector3(newProjectile.transform.position.x + (0.2f * columns + GetRandomOffset(rainSpawnOffset)), newProjectile.transform.position.y + (2f * columns + GetRandomOffset(rainSpawnOffset)), newProjectile.transform.position.z);
                    }
                    else
                    {
                        columnSpawnPos = new Vector3(newProjectile.transform.position.x - (0.2f * columns + GetRandomOffset(rainSpawnOffset)), newProjectile.transform.position.y + (2f * columns + GetRandomOffset(rainSpawnOffset)), newProjectile.transform.position.z);
                    }

                    SpawnProjectile(projectile, columnSpawnPos, spawns[ii].transform.rotation, addTorque);

                    columns--;
                }

                yield return new WaitForSeconds(rainSpawnDelay);
            }
        }

        yield return new WaitForSeconds(4);

        zoomTo = new Vector3(0, 2, 0);

        while (Vector3.Distance(cameraTarget.position, zoomTo) > 0.1f)
        {
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, zoomTo, Time.deltaTime * camZoomSpeed);
            yield return null;
        }

        cameraTarget.position = zoomTo;
        for (int i = 0; i < cams.Length; i++)
        {
            cams[i].canMove = true;
        }
    }

    private GameObject SpawnProjectile(string projectile, Vector3 position, Quaternion rotation, bool addTorque)
    {
        GameObject newProjectile = ObjectPooler.instance.GrabFromPool(projectile, position, rotation);

        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.myDamage = damageAmount.currentValue;
        projectileComponent.Fire(150, 0, ForceMode.Force);

        if (addTorque)
        {

        }

        return newProjectile;
    }

    private float GetRandomOffset(float variable)
    {
        float random = Random.Range(-variable, variable);
        return random;
    }

    public void SetSelectedAmbush(int i)
    {
        selectedAmbush = i;
    }

    public override void Upgrade()
    {
        if (!ResourceManager.instance.HasEnoughGold((int)info.myUpgradeCost.currentValue) || info.myLevel >= info.myMaxLevel)
        {
            return;
        }

        base.Upgrade();

        useCooldown.currentValue += useCooldown.increaseValue;
        damageAmount.currentValue += damageAmount.increaseValue;

        SetupUI();
    }
}
