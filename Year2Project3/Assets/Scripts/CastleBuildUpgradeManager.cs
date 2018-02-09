using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBuildUpgradeManager : MonoBehaviour 
{

    public static CastleBuildUpgradeManager instance;

    private static CastleBuilder selectedBuild;

    public GameObject buildUI;
    public GameObject upgradeUI;

    [Header("Buttons")]
    public GameObject buildWeaponButton;
    public GameObject useWeaponButton;
    public GameObject upgradeWeaponButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        gameObject.SetActive(false);
        upgradeUI.SetActive(false);
        buildUI.SetActive(false);
    }

    private void Update()
    {
        if (selectedBuild != null)
        {
            gameObject.transform.LookAt(Camera.main.transform);

            if (Input.GetButtonDown("Cancel"))
            {
                CloseUIButton();
            }
        }
    }

    public void OpenUI(CastleBuilder selected)
    {
        selectedBuild = selected;

        if (selectedBuild.myBuildedObject == null)
        {
            buildWeaponButton.SetActive(true);
            useWeaponButton.SetActive(false);
            upgradeWeaponButton.SetActive(false);
        }
        else
        {
            buildWeaponButton.SetActive(false);
            useWeaponButton.SetActive(true);
            upgradeWeaponButton.SetActive(true);
        }

        gameObject.transform.position = new Vector2(selected.transform.position.x, selected.transform.position.y + 2.5f);
        gameObject.SetActive(true);

        selected.buildButton.SetActive(false);
    }

    public void BuildWeaponButton()
    {
        buildUI.SetActive(true);
    }

    public void UseWeaponButton()
    {
        CloseUIButton();
    }

    public void UpgradeWeaponButton()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUIButton()
    {
        GetComponent<Animator>().SetTrigger("CloseUI");
    }

    public void AnimationEventCloseUI()
    {
        buildUI.SetActive(false);
        upgradeUI.SetActive(false);
        gameObject.SetActive(false);

        if (selectedBuild.myBuildedObject == null)
        {
            selectedBuild.buildButton.SetActive(true);
            selectedBuild = null;
        }
    }

    public void BuildWeapon(int type)
    {
        switch (type)
        {
            case 0:

                selectedBuild.BuildWeapon(CastleWeapon.WeaponType.Ballista);
                break;
            case 1:

                selectedBuild.BuildWeapon(CastleWeapon.WeaponType.Catapult);
                break;
            case 2:

                selectedBuild.BuildWeapon(CastleWeapon.WeaponType.Canon);
                break;
        }

        GetComponent<Animator>().SetTrigger("CloseUI");
    }
}
