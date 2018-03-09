using System.Collections;
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
    [Space(10)]
    public Stat projectileRows;
    public Stat projectileColumns;
    public float projectileDistOffset;
    public Stat projectileSpawnDelay;
    public float randomProjectileSpawnOffset;

    [Header("Ambush Room Setup")]
    public Image cooldownFill;
    public TextMeshProUGUI descriptionText;
    private Transform cameraTarget;
    public float camZoomSpeed;
    private CameraManager mainCamManager;

    [Header("Upgrades")]
    public TextMeshProUGUI nextLevelExtraUpgradeText;
    public GameObject meteorStrikeButton;
    public GameObject spearRainButton;

    private GameObject[] ambushSpawns;

    private float currentCooldown = 0.95f;

    public override void Awake()
    {
        base.Awake();

        cameraTarget = GameObject.FindWithTag("CameraTarget").transform;
        mainCamManager = Camera.main.GetComponent<CameraManager>();

        ambushSpawns = GameObject.FindGameObjectsWithTag("AmbushSpawn");
    }

    public override void SetupUI()
    {
        base.SetupUI();

        descriptionText.text = "Ambushes the enemies with a strike from above.\n Deals " + "<color=green>" + damageAmount.currentValue + "</color> damage.";

        upgradeStatsText.text = "Damage amount: " + damageAmount.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(damageAmount.increaseValue) + "</color>)" + "\n" +
                                "Cooldown: " + useCooldown.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(useCooldown.increaseValue) + "</color>)";

        if (roomLevel == 4)
        {
            nextLevelExtraUpgradeText.text = "Next level will unlock the ambush: meteor strike";
        }
        else if (roomLevel == 9)
        {
            nextLevelExtraUpgradeText.text = "Next level will unlock the ambush: spear rain";
        }
        else
        {
            nextLevelExtraUpgradeText.text = string.Empty;
        }

        if (roomLevel >= 5)
        {
            meteorStrikeButton.SetActive(true);
        }

        if (roomLevel >= 10)
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

                StartCoroutine(Volley());
                break;

            case 1:
                break;

            case 2:
                break;
        }

        StopUsing();
    }

    private IEnumerator Volley()
    {
        Vector3 zoomTo = new Vector3(0, 15, -25);
        mainCamManager.canMove = false;

        while (Vector3.Distance(cameraTarget.position, zoomTo) > 0.1f)
        {
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, zoomTo, Time.deltaTime * camZoomSpeed);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * (camZoomSpeed * 5));
            yield return null;
        }

        Camera.main.GetComponent<CameraShake>().Shake(projectileSpawnDelay.currentValue * projectileRows.currentValue + 5, 2, 2, 0, 0, 1);

        int toSpawnLeft = (int)projectileRows.currentValue;
        int toSpawnRight = (int)projectileRows.currentValue;

        for (int i = 0; i < projectileRows.currentValue; i++)
        {
            for (int ii = 0; ii < ambushSpawns.Length; ii++)
            {
                Vector3 spawnPos = new Vector3();
                GameObject newProjectile = null;

                if (ambushSpawns[ii].transform.position.x < 0)
                {
                    spawnPos = new Vector3(ambushSpawns[ii].transform.position.x - (projectileDistOffset * toSpawnLeft + GetRandomOffset()), ambushSpawns[ii].transform.position.y + GetRandomOffset(), ambushSpawns[ii].transform.position.z);
                    toSpawnLeft--;
                }
                else
                {
                    spawnPos = new Vector3(ambushSpawns[ii].transform.position.x + (projectileDistOffset * toSpawnRight + GetRandomOffset()), ambushSpawns[ii].transform.position.y + GetRandomOffset(), ambushSpawns[ii].transform.position.z);
                    toSpawnRight--;
                }

                newProjectile = ObjectPooler.instance.GrabFromPool("ambush projectile", spawnPos, ambushSpawns[ii].transform.rotation);
                newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * 150);
                newProjectile.GetComponent<Projectile>().myDamage = damageAmount.currentValue;

                Vector3 columnSpawnPos = new Vector3();
                int columns = (int)projectileColumns.currentValue;
                for (int iii = 0; iii < projectileColumns.currentValue; iii++)
                {
                    if (ambushSpawns[ii].transform.position.x < 0)
                    {
                        columnSpawnPos = new Vector3(newProjectile.transform.position.x + (0.2f * columns + GetRandomOffset()), newProjectile.transform.position.y + (2f * columns + GetRandomOffset()), newProjectile.transform.position.z);
                    }
                    else
                    {
                        columnSpawnPos = new Vector3(newProjectile.transform.position.x - (0.2f * columns + GetRandomOffset()), newProjectile.transform.position.y + (2f * columns + GetRandomOffset()), newProjectile.transform.position.z);
                    }

                    GameObject newNewProjectile = ObjectPooler.instance.GrabFromPool("ambush projectile", columnSpawnPos, ambushSpawns[ii].transform.rotation);
                    newNewProjectile.GetComponent<Rigidbody>().AddForce(newNewProjectile.transform.forward * 150);
                    newNewProjectile.GetComponent<Projectile>().myDamage = damageAmount.currentValue;

                    columns--;
                }

                yield return new WaitForSeconds(projectileSpawnDelay.currentValue);
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
        mainCamManager.canMove = true;
    }

    private float GetRandomOffset()
    {
        float random = Random.Range(-randomProjectileSpawnOffset, randomProjectileSpawnOffset);
        return random;
    }

    public void SetSelectedAmbush(int i)
    {
        selectedAmbush = i;
    }

    public override void Upgrade()
    {
        if (ResourceManager.instance.goldPrefabsInScene.Count < upgradeCost.currentValue || roomLevel >= maxRoomLevel)
        {
            return;
        }

        base.Upgrade();

        useCooldown.currentValue += useCooldown.increaseValue;
        damageAmount.currentValue += damageAmount.increaseValue;

        SetupUI();
    }
}
