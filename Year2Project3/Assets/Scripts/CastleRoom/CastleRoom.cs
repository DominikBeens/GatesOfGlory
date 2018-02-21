using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleRoom : MonoBehaviour
{


    public enum RoomType
    {
        Knight,
        Ranger,
        Spearman,
        Gold,
        Heal,
        Damage
    }
    public RoomType roomType;

    public enum Side
    {
        Left,
        Right
    }
    public Side side;

    [Header("Basic Properties")]
    public string roomName;
    public int roomLevel;
    public int maxRoomLevel;
    public int buildCost;
    public Stat upgradeCost;

    [HideInInspector]
    public CastleBuilder myBuilder;

    [Header("Upgrade Panel")]
    public GameObject upgradePanel;
    public Button buyUpgradeButton;
    public TextMeshProUGUI upgradeStatsText;
    public TextMeshProUGUI upgradeDescriptionText;
    public TextMeshProUGUI roomNameText;

    [Space(10)]
    public GameObject useUI;
    private bool usingRoom;

    private void Awake()
    {
        useUI.SetActive(false);
        upgradePanel.SetActive(false);
    }

    public virtual void Update()
    {
        if (usingRoom)
        {
            useUI.transform.parent.parent.LookAt(Camera.main.transform);
        }
    }

    public virtual void UseRoom()
    {

    }

    public virtual void Upgrade()
    {
        roomLevel++;
        upgradeCost.currentValue += upgradeCost.increaseValue;

        if (roomLevel >= maxRoomLevel)
        {
            buyUpgradeButton.interactable = false;
        }
    }

    public virtual void SetupUI()
    {
        roomNameText.text = "Level <color=green>" + roomLevel.ToString() + "</color> " + roomName;
        upgradeDescriptionText.text = "Upgrade cost: <color=yellow>" + upgradeCost.currentValue + "</color> gold.";
    }

    public virtual void StartUsing()
    {
        usingRoom = true;
        SetupUI();
        useUI.SetActive(true);

        CastleUpgradeManager.instance.CloseAllUI(this);
    }

    public virtual void StopUsing()
    {
        StartCoroutine(EventStopUsing());
    }

    public IEnumerator EventStopUsing()
    {
        if (useUI.activeInHierarchy)
        {
            useUI.GetComponent<Animator>().SetTrigger("CloseUI");
        }

        yield return new WaitForSeconds(0.5f);

        usingRoom = false;
        myBuilder.useButton.SetActive(true);

        upgradePanel.SetActive(false);
        useUI.SetActive(false);
    }

    public virtual void ToggleUpgradePanelButton()
    {
        if (!upgradePanel.activeInHierarchy)
        {
            upgradePanel.SetActive(true);
        }
        else
        {
            StartCoroutine(CloseUpgradeUI());
        }
    }

    private IEnumerator CloseUpgradeUI()
    {
        upgradePanel.GetComponent<Animator>().SetTrigger("CloseUI");

        yield return new WaitForSeconds(0.5f);

        upgradePanel.SetActive(false);
    }
}
