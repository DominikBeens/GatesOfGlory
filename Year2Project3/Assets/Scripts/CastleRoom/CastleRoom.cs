using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleRoom : CastleBuild
{

    public enum RoomType
    {
        Knight,
        Ranger,
        Spearman,
        Gold,
        Heal,
        Damage,
        Ambush
    }
    public RoomType roomType;

    [Header("Upgrade Panel")]
    public TextMeshProUGUI roomNameText;

    public override void Awake()
    {
        base.Awake();
    }

    public virtual void Update()
    {
        if (usingBuilding)
        {
            useUI.transform.parent.parent.LookAt(Camera.main.transform);
        }
    }

    public virtual void UseRoom()
    {

    }

    public override void Upgrade()
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

    public override void SetupUI()
    {
        roomNameText.text = "Level <color=green>" + myLevel.ToString() + "</color> " + myName;
        upgradeCostText.text = myUpgradeCost.currentValue.ToString();
    }
}
