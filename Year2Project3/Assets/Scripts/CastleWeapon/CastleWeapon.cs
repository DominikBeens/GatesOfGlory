using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleWeapon : MonoBehaviour 
{

    public enum WeaponType
    {
        Ballista,
        Canon,
        Catapult
    }
    public WeaponType weaponType;

    public enum Side
    {
        Left,
        Right
    }
    public Side side;

    [Header("Basic Properties")]
    public string weaponName;
    public int weaponLevel;
    public int weaponMaxLevel;
    public int buildCost;
    public Stat upgradeCost;

    protected bool usingWeapon;
    protected bool shooting;

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

    public GameObject useUI;
    public Animator anim;

    [HideInInspector]
    public CastleBuilder myBuilder;

    [Header("Stats")]
    public Stat damage;
    public Stat force;
    public Stat cooldown;
    private float nextTimeToFire;

    [Header("Rotation Options")]
    public float maxXRotation;
    public float minXRotation;

    private void Awake()
    {
        useUI.SetActive(false);
    }

    public virtual void Update()
    {
        if (usingWeapon)
        {
            useUI.transform.parent.parent.LookAt(Camera.main.transform);

            if (shooting)
            {
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + cooldown.currentValue;
                    Shoot();
                }
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

    public void Upgrade()
    {

    }

    public void ToggleAutoShoot()
    {
        shooting = !shooting;
    }

    public virtual void StartUsing()
    {
        usingWeapon = true;
        useUI.SetActive(true);

        CastleUpgradeManager.instance.CloseAllUI(this);
    }

    public virtual void StopUsing()
    {
        StartCoroutine(EventStopUsing());
    }

    public IEnumerator EventStopUsing()
    {
        if (useUI.activeInHierarchy)
        {
            useUI.GetComponent<Animator>().SetTrigger("CloseUI");
        }

        yield return new WaitForSeconds(0.5f);

        myBuilder.useButton.SetActive(true);

        usingWeapon = false;
        shooting = false;

        useUI.SetActive(false);
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
