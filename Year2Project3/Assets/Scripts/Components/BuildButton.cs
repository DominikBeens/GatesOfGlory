using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildButton : MonoBehaviour 
{

    public CastleBuild myBuild;

    public enum Type
    {
        Weapon,
        Room
    }
    [Space(10)]
    public Type type;
    [Space(10)]
    public TextMeshProUGUI buildCostText;
    public TextMeshProUGUI selectWeaponText;

    private void Awake()
    {
        buildCostText.text = myBuild.info.myBuildCost.ToString();
    }

    public void SetSelectBuildText()
    {
        selectWeaponText.text = myBuild.info.myName;
    }

    public void ResetSelectBuildText()
    {
        switch (type)
        {
            case Type.Weapon:

                selectWeaponText.text = "Select A Weapon";
                break;
            case Type.Room:

                selectWeaponText.text = "Select A Room";
                break;
        }
    }
}
