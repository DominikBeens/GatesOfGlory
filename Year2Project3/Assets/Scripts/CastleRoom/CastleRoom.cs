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
        Damage,
        Ambush
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
    [HideInInspector]
    public int goldSpentOnThisObject;

    [Header("Upgrade Panel")]
    public GameObject upgradePanel;
    public Button buyUpgradeButton;
    public TextMeshProUGUI upgradeStatsText;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI roomNameText;

    [Space(10)]
    public GameObject useUI;
    private bool usingRoom;

    [Space(10)]
    public GameObject demolishPanel;

    public virtual void Awake()
    {
        useUI.SetActive(false);
        upgradePanel.SetActive(false);
        demolishPanel.SetActive(false);

        goldSpentOnThisObject += buildCost;
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

        ResourceManager.instance.RemoveGold((int)upgradeCost.currentValue);

        goldSpentOnThisObject += (int)upgradeCost.currentValue;
        upgradeCost.currentValue += upgradeCost.increaseValue;

        if (roomLevel >= maxRoomLevel)
        {
            buyUpgradeButton.interactable = false;
        }
    }

    public virtual void SetupUI()
    {
        roomNameText.text = "Level <color=green>" + roomLevel.ToString() + "</color> " + roomName;
        upgradeCostText.text = upgradeCost.currentValue.ToString();
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
        demolishPanel.SetActive(false);
    }

    public virtual void ToggleUpgradePanelButton()
    {
        if (demolishPanel.activeInHierarchy)
        {
            StartCoroutine(CloseDemolishUI());
        }

        if (!upgradePanel.activeInHierarchy)
        {
            upgradePanel.SetActive(true);
        }
        else
        {
            StartCoroutine(CloseUpgradeUI());
        }
    }

    public virtual void ToggleDemolishPanelButton()
    {
        if (upgradePanel.activeInHierarchy)
        {
            StartCoroutine(CloseUpgradeUI());
        }

        if (!demolishPanel.activeInHierarchy)
        {
            demolishPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(CloseDemolishUI());
        }
    }

    public void DemolishButton()
    {
        myBuilder.Demolish();
    }

    private IEnumerator CloseUpgradeUI()
    {
        upgradePanel.GetComponent<Animator>().SetTrigger("CloseUI");

        yield return new WaitForSeconds(0.5f);

        upgradePanel.SetActive(false);
    }

    private IEnumerator CloseDemolishUI()
    {
        demolishPanel.GetComponent<Animator>().SetTrigger("CloseUI");

        yield return new WaitForSeconds(0.5f);

        demolishPanel.SetActive(false);
    }

    private void OnDisable()
    {
        goldSpentOnThisObject = 0;
    }
}
