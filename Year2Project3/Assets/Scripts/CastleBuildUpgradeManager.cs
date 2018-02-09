using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Header("Upgrade Panel")]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponDamageText;
    public TextMeshProUGUI weaponForceText;
    public TextMeshProUGUI weaponFireRateText;


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
        if (GameManager.instance.playerState == GameManager.PlayerState.UsingWeapon)
        {
            return;
        }

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

        gameObject.transform.position = new Vector2(selected.transform.position.x, selected.transform.position.y + 4f);
        gameObject.SetActive(true);

        selected.buildButton.SetActive(false);
    }

    public void OpenBuildUIButton()
    {
        buildUI.SetActive(true);
    }

    public void UseWeaponButton()
    {
        selectedBuild.myBuildedObject.GetComponent<CastleWeapon>().StartUsing();
        CloseUIButton();
    }

    public void OpenUpgradeUIButton()
    {
        CastleWeapon weaponComponent = selectedBuild.myBuildedObject.GetComponent<CastleWeapon>();

        weaponNameText.text = "Level <color=green>" + weaponComponent.weaponLevel.ToString() + "</color> " + weaponComponent.weaponName;
        weaponDamageText.text = "Damage: <color=green>" + weaponComponent.damage.currentValue.ToString() + "</color>";
        weaponForceText.text = "Force: <color=green>" + weaponComponent.force.currentValue.ToString() + "</color>";
        weaponFireRateText.text = "Fire Rate: <color=green>" + weaponComponent.coolDown.currentValue.ToString() + "</color>";

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

        CloseUIButton();
    }
}
