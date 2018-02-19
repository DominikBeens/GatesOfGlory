using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CastleWeaponUpgrader : MonoBehaviour
{

    public GameObject buildUI;
    public GameObject upgradeUI;
    [Space(10)]
    public TextMeshProUGUI upgradeText;

    public TextMeshProUGUI selectWeaponText;

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
        upgradeUI.SetActive(false);
        buildUI.SetActive(false);
    }

    public void OpenUI()
    {
        if (CastleUpgradeManager.selectedBuild.myBuildedObject == null)
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

        gameObject.SetActive(true);

        CastleUpgradeManager.selectedBuild.buildButton.SetActive(false);
    }

    public void OpenBuildUIButton()
    {
        buildUI.SetActive(true);
    }

    public void UseWeaponButton()
    {
        CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleWeapon>().StartUsing();
        //CastleUpgradeManager.instance.CloseAllUI();
    }

    public void OpenUpgradeUIButton()
    {
        CastleWeapon weaponComponent = CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleWeapon>();

        weaponNameText.text = "Level <color=green>" + weaponComponent.weaponLevel.ToString() + "</color> " + weaponComponent.weaponName;
        weaponDamageText.text = "Damage: <color=green>" + weaponComponent.damage.currentValue + "</color>";
        weaponForceText.text = "Force: <color=green>" + weaponComponent.force.currentValue + "</color>";
        weaponFireRateText.text = "Fire Rate: <color=green>" + weaponComponent.coolDown.currentValue + "</color>";

        //upgradeText.text = "Upgrade\n<color=#FFF800FF>" + weaponComponent.upgradeCost.currentValue + "</color> Gold";
        upgradeText.text = "Upgrade\n" + weaponComponent.upgradeCost.currentValue + " Gold";

        upgradeUI.SetActive(true);
    }

    public void ShowUpgradeBenefits()
    {
        CastleWeapon weaponComponent = CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleWeapon>();

        weaponDamageText.text = "Damage: " + weaponComponent.damage.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(weaponComponent.damage.increaseValue) + "</color>)";
        weaponForceText.text = "Force: " + weaponComponent.force.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(weaponComponent.force.increaseValue) + "</color>)";
        weaponFireRateText.text = "Fire Rate: " + weaponComponent.coolDown.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(weaponComponent.coolDown.increaseValue) + "</color>)";
    }

    public void HideUpgradeBenefits()
    {
        OpenUpgradeUIButton();
    }

    public void SetSelectWeaponText(int type)
    {
        switch (type)
        {
            case 0:

                selectWeaponText.text = "Ballista";
                break;
            case 1:

                selectWeaponText.text = "Canon";
                break;
        }
    }

    public void ResetSelectWeaponText()
    {
        selectWeaponText.text = "Select A Weapon";
    }

    public void BuildWeapon(int type)
    {
        switch (type)
        {
            case 0:

                CastleUpgradeManager.selectedBuild.BuildWeapon(CastleWeapon.WeaponType.Ballista);
                break;
            case 1:

                CastleUpgradeManager.selectedBuild.BuildWeapon(CastleWeapon.WeaponType.Canon);
                break;
            case 2:

                CastleUpgradeManager.selectedBuild.BuildWeapon(CastleWeapon.WeaponType.Catapult);
                break;
        }
    }

    public void UpgradeButton()
    {
        CastleWeapon weaponComponent = CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleWeapon>();

        if (ResourceManager.gold < weaponComponent.upgradeCost.currentValue)
        {
            return;
        }

        weaponComponent.damage.currentValue += weaponComponent.damage.increaseValue;
        weaponComponent.force.currentValue += weaponComponent.force.increaseValue;
        weaponComponent.coolDown.currentValue += weaponComponent.coolDown.increaseValue;

        ResourceManager.gold -= (int)weaponComponent.upgradeCost.currentValue;

        weaponComponent.upgradeCost.currentValue += weaponComponent.upgradeCost.increaseValue;
        weaponComponent.weaponLevel++;

        OpenUpgradeUIButton();
        ShowUpgradeBenefits();
    }

    private void OnDisable()
    {
        upgradeUI.SetActive(false);
        buildUI.SetActive(false);
    }
}