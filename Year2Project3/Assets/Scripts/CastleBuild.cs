﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleBuild : MonoBehaviour 
{

    public enum Side
    {
        Left,
        Right
    }
    public Side side;

    [HideInInspector]
    public CastleBuilder myBuilder;
    [HideInInspector]
    public int goldSpentOnThisObject;

    protected bool usingBuilding;

    [Header("Basic Properties")]
    public string myName;
    public int myLevel;
    public int myMaxLevel;
    public int myBuildCost;
    public Stat myUpgradeCost;

    [Header("UI Panels")]
    public GameObject useUI;
    public GameObject upgradePanel;
    public GameObject demolishPanel;

    [Header("Upgrade Panel")]
    public Button buyUpgradeButton;
    public TextMeshProUGUI upgradeStatsText;
    public TextMeshProUGUI upgradeCostText;

    // Awake assures that all of the panels are inactive and also means that this build has been built by the player. 
    // Increase the gold spent on this build by the cost of this build.
    public virtual void Awake()
    {
        useUI.SetActive(false);
        upgradePanel.SetActive(false);
        demolishPanel.SetActive(false);

        goldSpentOnThisObject += myBuildCost;
    }

    // Updates all UI elements with the right variables. Updates every time the player clicks on a build.
    public virtual void SetupUI()
    {

    }

    // Upgrades the build if the player has enough gold.
    public virtual void Upgrade()
    {
        myLevel++;

        ResourceManager.instance.RemoveGold((int)myUpgradeCost.currentValue);

        goldSpentOnThisObject += (int)myUpgradeCost.currentValue;
        myUpgradeCost.currentValue += myUpgradeCost.increaseValue;

        if (myLevel >= myMaxLevel)
        {
            buyUpgradeButton.interactable = false;
        }
    }

    // Opens/closes the upgrade panel.
    public void ToggleUpgradePanelButton()
    {
        if (demolishPanel.activeInHierarchy)
        {
            StartCoroutine(CloseDemolishUI());
        }

        if (!upgradePanel.activeInHierarchy)
        {
            upgradePanel.SetActive(true);
        }
        else
        {
            StartCoroutine(CloseUpgradeUI());
        }
    }

    // Opens/closes the demolish panel.
    public void ToggleDemolishPanelButton()
    {
        if (upgradePanel.activeInHierarchy)
        {
            StartCoroutine(CloseUpgradeUI());
        }

        if (!demolishPanel.activeInHierarchy)
        {
            demolishPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(CloseDemolishUI());
        }
    }

    // Tells my builder to demolish this build.
    public void DemolishButton()
    {
        myBuilder.Demolish();
    }

    // Closes the upgrade UI with an animation and sets the panel inactive after a certain time.
    private IEnumerator CloseUpgradeUI()
    {
        upgradePanel.GetComponent<Animator>().SetTrigger("CloseUI");

        yield return new WaitForSeconds(0.5f);

        upgradePanel.SetActive(false);
    }

    // Closes the demolish UI with an animation and sets the panel inactive after a certain time.
    private IEnumerator CloseDemolishUI()
    {
        demolishPanel.GetComponent<Animator>().SetTrigger("CloseUI");

        yield return new WaitForSeconds(0.5f);

        demolishPanel.SetActive(false);
    }

    // Starts using this build.
    public virtual void StartUsing()
    {
        usingBuilding = true;
        SetupUI();
        useUI.SetActive(true);

        CastleUpgradeManager.instance.CloseAllUI(this);
    }

    // Stop using this build.
    public virtual void StopUsingButton()
    {
        StartCoroutine(EventStopUsing());
    }

    // Closes all UI and resets a few variables.
    private IEnumerator EventStopUsing()
    {
        if (useUI.activeInHierarchy)
        {
            useUI.GetComponent<Animator>().SetTrigger("CloseUI");
        }

        yield return new WaitForSeconds(0.5f);

        usingBuilding = false;
        myBuilder.useButton.SetActive(true);

        upgradePanel.SetActive(false);
        useUI.SetActive(false);
        demolishPanel.SetActive(false);
    }

    // If this build gets disabled it means that it has been demolished. If that has happened, reset the gold the player has spent on this build.
    private void OnDisable()
    {
        goldSpentOnThisObject = 0;
    }
}