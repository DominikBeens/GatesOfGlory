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
    public OutlineOnMouseOver outline;
    public ModelToSprite_LOD lodGroup;

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

    public override void StartUsing()
    {
        base.StartUsing();

        if (outline != null)
        {
            outline.canShowOutline = false;
            outline.OnMouseExit();
        }

        if (lodGroup != null)
        {
            lodGroup.TogglePlayerMouseHover(false);
        }
    }

    public override void StopUsingButton()
    {
        base.StopUsingButton();

        if (outline != null)
        {
            outline.canShowOutline = true;
        }
    }

    public override void SetupUI()
    {
        myNameText.text = "Level <color=green>" + info.myLevel.ToString() + "</color> " + info.myName;
        upgradeCostText.text = info.myUpgradeCost.currentValue.ToString();
    }
}
