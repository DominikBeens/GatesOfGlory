using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleRoom_Ambush : CastleRoom
{

    [Header("Stats")]
    public Stat useCooldown;
    public Stat damageAmount;
    [Space(10)]
    public Stat projectileRows;
    public Stat projectileColumns;
    public float projectileDistOffset;
    public Stat projectileSpawnDelay;

    [Header("Ambush Room Setup")]
    public Image cooldownFill;
    public TextMeshProUGUI descriptionText;
    private Transform cameraTarget;
    public float camZoomSpeed;
    private CameraManager mainCamManager;

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

        descriptionText.text = "Ambushes the enemies with a strike from above. Deals " + "<color=green>" + damageAmount.currentValue + "</color> damage.";

        upgradeStatsText.text = "Damage amount: " + damageAmount.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(damageAmount.increaseValue) + "</color>)" + "\n" +
                                "Cooldown: " + useCooldown.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(useCooldown.increaseValue) + "</color>)";
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

        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(Volley());
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

        StartCoroutine(Volley());
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

        for (int i = 0; i < projectileRows.currentValue; i++)
        {
            int toSpawnLeft = (int)projectileRows.currentValue;
            int toSpawnRight = (int)projectileRows.currentValue;

            for (int ii = 0; ii < ambushSpawns.Length; ii++)
            {
                if (ambushSpawns[ii].transform.position.x < 0)
                {
                    Vector3 spawnPos = new Vector3(ambushSpawns[ii].transform.position.x - (projectileDistOffset * toSpawnLeft), ambushSpawns[ii].transform.position.y, ambushSpawns[ii].transform.position.z);
                    GameObject newProjectile = ObjectPooler.instance.GrabFromPool("ballista projectile", spawnPos, ambushSpawns[ii].transform.rotation);
                    newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * 150);

                    toSpawnLeft--;
                }
                else
                {
                    Vector3 spawnPos = new Vector3(ambushSpawns[ii].transform.position.x + (projectileDistOffset * toSpawnRight), ambushSpawns[ii].transform.position.y, ambushSpawns[ii].transform.position.z);
                    GameObject newProjectile = ObjectPooler.instance.GrabFromPool("ballista projectile", spawnPos, ambushSpawns[ii].transform.rotation);
                    newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * 150);

                    toSpawnRight--;
                }
            }

            yield return new WaitForSeconds(projectileSpawnDelay.currentValue);
        }
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
