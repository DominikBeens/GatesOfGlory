using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CastleRoomUpgrader : MonoBehaviour
{

    public GameObject buildUI;
    public GameObject upgradeUI;

    [Header("Buttons")]
    public GameObject buildRoomButton;

    [Header("Upgrade Panel")]
    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI roomTypeText;
    public TextMeshProUGUI roomMinionCostText;

    private void Awake()
    {
        upgradeUI.SetActive(false);
        buildUI.SetActive(false);
    }

    public void OpenUI()
    {
        if (CastleUpgradeManager.selectedBuild.myBuildedObject == null)
        {
            buildRoomButton.SetActive(true);
        }
        else
        {
            buildRoomButton.SetActive(false);
        }

        gameObject.SetActive(true);

        CastleUpgradeManager.selectedBuild.buildButton.SetActive(false);

        if (CastleUpgradeManager.selectedBuild.myBuildedObject != null)
        {
            CastleRoom roomComponent = CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleRoom>();

            roomNameText.text = "Level <color=green>" + roomComponent.roomLevel.ToString() + "</color> " + roomComponent.roomName;
            roomTypeText.text = "Type: <color=green>" + roomComponent.roomType + "</color>";

            upgradeUI.SetActive(true);
        }
    }

    public void OpenBuildUIButton()
    {
        buildUI.SetActive(true);
    }

    public void BuildRoom(int type)
    {
        switch (type)
        {
            case 0:

                CastleUpgradeManager.selectedBuild.BuildRoom(CastleRoom.RoomType.Knight);
                break;
            case 1:

                CastleUpgradeManager.selectedBuild.BuildRoom(CastleRoom.RoomType.Gold);
                break;
        }
    }

    public void UseRoomButton()
    {
        if (CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleRoom_Minions>() != null)
        {
            if (ResourceManager.gold >= CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleRoom_Minions>().spawnCost)
            {
                CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleRoom_Minions>().UseRoom();
            }
        }
    }

    public void UpgradeButton()
    {

    }

    private void OnDisable()
    {
        upgradeUI.SetActive(false);
        buildUI.SetActive(false);
    }
}