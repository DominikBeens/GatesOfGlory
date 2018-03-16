using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CastleWeaponUpgrader : MonoBehaviour
{

    public GameObject buildUI;
    [Space(10)]
    public TextMeshProUGUI upgradeText;

    public TextMeshProUGUI selectWeaponText;

    [Header("Buttons")]
    public GameObject buildWeaponButton;
    public GameObject useWeaponButton;
    public GameObject upgradeWeaponButton;
    public GameObject demolishWeaponButton;
    [Space(10)]
    public GameObject buyUpgradeButton;

    [Header("Upgrade Panel")]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponDamageText;
    public TextMeshProUGUI weaponForceText;
    public TextMeshProUGUI weaponFireRateText;
    public GameObject autoFireUpdateText;

    private void Awake()
    {
        buildUI.SetActive(false);
    }

    public void OpenUI()
    {
        if (CastleUpgradeManager.selectedBuild.myBuildedObject == null)
        {
            buildWeaponButton.SetActive(true);
            gameObject.SetActive(true);
        }
        else
        {
            buildWeaponButton.SetActive(false);
        }

        CastleUpgradeManager.selectedBuild.buildButton.SetActive(false);

        if (CastleUpgradeManager.selectedBuild.myBuildedObject != null)
        {
            CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleWeapon>().StartUsing();
        }
    }

    public void OpenBuildUIButton()
    {
        buildUI.SetActive(true);
    }

    public void UseWeaponButton()
    {
        CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleWeapon>().StartUsing();
    }

    public void OpenUpgradeUIButton()
    {
        CastleWeapon weaponComponent = CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleWeapon>();

        weaponNameText.text = "Level <color=green>" + weaponComponent.myLevel.ToString() + "</color> " + weaponComponent.myName;
        weaponDamageText.text = "Damage: <color=green>" + weaponComponent.damage.currentValue + "</color>";
        weaponForceText.text = "Force: <color=green>" + weaponComponent.force.currentValue + "</color>";
        weaponFireRateText.text = "Fire Rate: <color=green>" + weaponComponent.cooldown.currentValue.ToString("f2") + "</color>";

        upgradeText.text = "<color=#FFF800FF>" + weaponComponent.myUpgradeCost.currentValue + "</color>";

        if (weaponComponent.myLevel < weaponComponent.myMaxLevel)
        {
            buyUpgradeButton.SetActive(true);
        }
        else
        {
            buyUpgradeButton.SetActive(false);
        }

        if (weaponComponent.myLevel >= weaponComponent.autoFireLevelReq)
        {
            weaponComponent.autoFireToggle.SetActive(true);
        }
        else
        {
            weaponComponent.autoFireToggle.SetActive(false);
        }

        if (weaponComponent.myLevel == weaponComponent.autoFireLevelReq - 1)
        {
            autoFireUpdateText.SetActive(true);
        }
        else
        {
            autoFireUpdateText.SetActive(false);
        }

        //if (demolishUI.activeInHierarchy)
        //{
        //    demolishUI.GetComponent<Animator>().SetTrigger("CloseUI");
        //    Invoke("DisableDemolishUI", demolishUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        //}

        //upgradeUI.SetActive(true);
    }

    public void ShowUpgradeBenefits()
    {
        CastleWeapon weaponComponent = CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleWeapon>();

        if (weaponComponent.myLevel >= weaponComponent.myMaxLevel)
        {
            return;
        }

        weaponDamageText.text = "Damage: " + weaponComponent.damage.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(weaponComponent.damage.increaseValue) + "</color>)";
        weaponForceText.text = "Force: " + weaponComponent.force.currentValue + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(weaponComponent.force.increaseValue) + "</color>)";
        weaponFireRateText.text = "Fire Rate: " + weaponComponent.cooldown.currentValue.ToString("f2") + " (<color=green>" + CastleUpgradeManager.instance.CheckPositiveOrNegative(weaponComponent.cooldown.increaseValue) + "</color>)";
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
            case 2:

                selectWeaponText.text = "Catapult";
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

        if (!ResourceManager.instance.HasEnoughGold((int)weaponComponent.myUpgradeCost.currentValue) || weaponComponent.myLevel >= weaponComponent.myMaxLevel)
        {
            return;
        }

        weaponComponent.damage.currentValue += weaponComponent.damage.increaseValue;
        weaponComponent.force.currentValue += weaponComponent.force.increaseValue;
        weaponComponent.cooldown.currentValue += weaponComponent.cooldown.increaseValue;
        weaponComponent.anim.speed = 1 / weaponComponent.cooldown.currentValue;

        ResourceManager.instance.RemoveGold((int)weaponComponent.myUpgradeCost.currentValue);

        weaponComponent.goldSpentOnThisObject += (int)weaponComponent.myUpgradeCost.currentValue;
        weaponComponent.myUpgradeCost.currentValue += weaponComponent.myUpgradeCost.increaseValue;
        weaponComponent.myLevel++;

        OpenUpgradeUIButton();
        ShowUpgradeBenefits();

        if (weaponComponent.myLevel >= weaponComponent.myMaxLevel)
        {
            buyUpgradeButton.SetActive(false);
        }

        if (weaponComponent.myLevel >= weaponComponent.autoFireLevelReq)
        {
            weaponComponent.autoFireToggle.SetActive(true);
        }
    }

    //public void OpenDemolishUIButton()
    //{
    //    if (upgradeUI.activeInHierarchy)
    //    {
    //        upgradeUI.GetComponent<Animator>().SetTrigger("CloseUI");
    //        Invoke("DisableUpgradeUI", upgradeUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    //    }

    //    demolishUI.SetActive(true);
    //}

    public void DemolishButton()
    {
        CastleUpgradeManager.selectedBuild.Demolish();
    }

    private void OnDisable()
    {
        buildUI.SetActive(false);
    }

    //private void DisableUpgradeUI()
    //{
    //    upgradeUI.SetActive(false);
    //}

    //private void DisableDemolishUI()
    //{
    //    demolishUI.SetActive(false);
    //}
}