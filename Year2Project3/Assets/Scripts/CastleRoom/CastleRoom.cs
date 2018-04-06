using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleRoom : CastleBuild
{

    public enum RoomType
    {
        Minion,
        Ranger,
        Spearman,
        Gold,
        Heal,
        Damage,
        Ambush
    }
    public RoomType roomType;

    public override void Awake()
    {
        base.Awake();
    }

    public virtual void Update()
    {
        if (usingBuilding)
        {
            useUI.transform.parent.parent.LookAt(mainCam.transform);
        }
    }

    public virtual void UseRoom()
    {

    }

    public override void Upgrade()
    {
        base.Upgrade();
    }

    public override void SetupUI()
    {
        myNameText.text = "Level <color=green>" + myLevel.ToString() + "</color> " + myName;
        upgradeCostText.text = myUpgradeCost.currentValue.ToString();
    }
}
