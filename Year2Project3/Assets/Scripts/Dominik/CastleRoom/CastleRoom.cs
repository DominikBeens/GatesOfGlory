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
    [Space(10)]
    public ModelToSprite_LOD lodGroup;

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

    public override void StartUsing()
    {
        base.StartUsing();

        if (lodGroup != null)
        {
            lodGroup.TogglePlayerMouseHover(false);
        }
    }

    public override void SetupUI()
    {
        myNameText.text = "Level <color=green>" + info.myLevel.ToString() + "</color> " + info.myName;
        upgradeCostText.text = info.myUpgradeCost.currentValue.ToString();
    }
}
