using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CastleUpgrader : MonoBehaviour
{

    public enum Type
    {
        Weapon,
        Room
    }
    [SerializeField]
    private Type type;

    [Space(10)]
    public GameObject buildUI;
    public TextMeshProUGUI selectBuildText;
    public GameObject buildButton;

    private void Awake()
    {
        buildUI.SetActive(false);
    }

    public void OpenUI()
    {
        if (CastleUpgradeManager.selectedBuild.myBuildedObject == null)
        {
            buildButton.SetActive(true);
            gameObject.SetActive(true);
        }
        else
        {
            buildButton.SetActive(false);
        }

        CastleUpgradeManager.selectedBuild.buildButton.SetActive(false);

        if (CastleUpgradeManager.selectedBuild.myBuildedObject != null)
        {
            CastleUpgradeManager.selectedBuild.myBuildedObject.StartUsing();
        }
    }

    public void OpenBuildUIButton()
    {
        buildUI.SetActive(true);
    }

    public void BuildButton(int build)
    {
        if (type == Type.Weapon)
        {
            CastleUpgradeManager.selectedBuild.BuildWeapon((CastleWeapon.WeaponType)build);
        }
        else
        {
            CastleUpgradeManager.selectedBuild.BuildRoom((CastleRoom.RoomType)build);
        }
    }

    private void OnDisable()
    {
        buildUI.SetActive(false);
    }
}