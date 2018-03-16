﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CastleRoomUpgrader : MonoBehaviour
{

    public GameObject buildUI;

    public TextMeshProUGUI selectRoomText;

    [Header("Buttons")]
    public GameObject buildRoomButton;

    private void Awake()
    {
        buildUI.SetActive(false);
    }

    public void OpenUI()
    {
        if (CastleUpgradeManager.selectedBuild.myBuildedObject == null)
        {
            buildRoomButton.SetActive(true);
            gameObject.SetActive(true);
        }
        else
        {
            buildRoomButton.SetActive(false);
        }

        CastleUpgradeManager.selectedBuild.buildButton.SetActive(false);

        if (CastleUpgradeManager.selectedBuild.myBuildedObject != null)
        {
            CastleUpgradeManager.selectedBuild.myBuildedObject.GetComponent<CastleRoom>().StartUsing();
        }
    }

    public void OpenBuildUIButton()
    {
        buildUI.SetActive(true);
    }

    public void SetSelectRoomText(int type)
    {
        switch (type)
        {
            case 0:

                selectRoomText.text = "Knight Room";
                break;
            case 1:

                selectRoomText.text = "Healing Room";
                break;
            case 2:

                selectRoomText.text = "Damage Room";
                break;
            case 3:

                selectRoomText.text = "Ambush Room";
                break;
        }
    }

    public void ResetSelectRoomText()
    {
        selectRoomText.text = "Select A Room";
    }

    public void BuildRoom(int type)
    {
        switch (type)
        {
            case 0:

                CastleUpgradeManager.selectedBuild.BuildRoom(CastleRoom.RoomType.Knight);
                break;
            case 1:

                CastleUpgradeManager.selectedBuild.BuildRoom(CastleRoom.RoomType.Heal);
                break;
            case 2:

                CastleUpgradeManager.selectedBuild.BuildRoom(CastleRoom.RoomType.Damage);
                break;
            case 3:

                CastleUpgradeManager.selectedBuild.BuildRoom(CastleRoom.RoomType.Ambush);
                break;
        }
    }

    public void UseRoomButton()
    {

    }

    public void UpgradeButton()
    {

    }

    private void OnDisable()
    {
        buildUI.SetActive(false);
    }
}