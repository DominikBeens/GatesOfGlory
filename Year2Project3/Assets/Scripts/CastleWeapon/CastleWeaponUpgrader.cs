﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CastleWeaponUpgrader : MonoBehaviour
{

    public GameObject buildUI;
    public TextMeshProUGUI selectWeaponText;
    public GameObject buildWeaponButton;

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

    private void OnDisable()
    {
        buildUI.SetActive(false);
    }
}