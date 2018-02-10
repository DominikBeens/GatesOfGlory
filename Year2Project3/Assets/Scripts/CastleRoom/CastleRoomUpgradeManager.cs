using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CastleRoomUpgradeManager : MonoBehaviour 
{

    public static CastleRoomUpgradeManager instance;

    private static CastleBuilder selectedBuild;

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
        if (instance == null)
        {
            instance = this;
        }

        gameObject.SetActive(false);
        upgradeUI.SetActive(false);
        buildUI.SetActive(false);
    }

    private void Update()
    {
        if (selectedBuild != null)
        {
            gameObject.transform.LookAt(Camera.main.transform);

            if (Input.GetButtonDown("Cancel"))
            {
                CloseUIButton();
            }
        }
    }

    public void OpenUI(CastleBuilder selected)
    {
        if (GameManager.instance.playerState == GameManager.PlayerState.UsingWeapon)
        {
            return;
        }

        UIManager.instance.OpenCastleUI(gameObject);
        selectedBuild = selected;

        if (selectedBuild.myBuildedObject == null)
        {
            buildRoomButton.SetActive(true);
        }
        else
        {
            buildRoomButton.SetActive(false);
        }

        gameObject.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y + 4f, selected.transform.position.z - 6f);
        gameObject.SetActive(true);

        selected.buildButton.SetActive(false);

        if (selectedBuild.myBuildedObject != null)
        {
            CastleRoom roomComponent = selectedBuild.myBuildedObject.GetComponent<CastleRoom>();

            roomNameText.text = "Level <color=green>" + roomComponent.roomLevel.ToString() + "</color> " + roomComponent.roomName;
            roomTypeText.text = "Type: <color=green>" + roomComponent.roomType + "</color>";

            upgradeUI.SetActive(true);
        }
    }

    public void OpenBuildUIButton()
    {
        buildUI.SetActive(true);
    }

    public void AnimationEventCloseUI()
    {
        buildUI.SetActive(false);
        upgradeUI.SetActive(false);
        gameObject.SetActive(false);

        if (selectedBuild.myBuildedObject == null)
        {
            selectedBuild.buildButton.SetActive(true);
            selectedBuild = null;
        }

        UIManager.instance.CloseCastleUI(gameObject);
    }

    public void CloseUIButton()
    {
        GetComponent<Animator>().SetTrigger("CloseUI");
    }

    public void BuildRoom(int type)
    {
        switch (type)
        {
            case 0:

                selectedBuild.BuildRoom(CastleRoom.RoomType.Knight);
                break;
            case 1:

                selectedBuild.BuildRoom(CastleRoom.RoomType.Gold);
                break;
        }

        CloseUIButton();
    }
}
