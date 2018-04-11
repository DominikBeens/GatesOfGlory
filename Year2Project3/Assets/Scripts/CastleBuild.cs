using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleBuild : MonoBehaviour 
{

    public enum Side
    {
        Left,
        Right
    }
    public Side side;

    [HideInInspector]
    public CastleBuilder myBuilder;
    [HideInInspector]
    public int goldSpentOnThisObject;

    [HideInInspector]
    public Animator anim;
    private Animator useUIAnim;
    private Animator demolishAnim;
    private Animator upgradeAnim;

    protected Camera mainCam;

    protected bool usingBuilding;

    private Canvas mainCanvas;

    [Header("Basic Properties")]
    public GeneralStats info;

    [Header("UI Panels")]
    public GameObject useUI;
    public GameObject upgradePanel;
    public GameObject demolishPanel;

    [Header("Upgrade Panel")]
    public Button buyUpgradeButton;
    public GameObject upgradesMaxedOverlay;
    public TextMeshProUGUI upgradeStatsText;
    public TextMeshProUGUI upgradeCostText;
    [Space(10)]
    public TextMeshProUGUI myNameText;

    // Awake assures that all of the panels are inactive and also means that this build has been built by the player. 
    // Increase the gold spent on this build by the cost of this build.
    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        useUIAnim = useUI.GetComponent<Animator>();
        demolishAnim = demolishPanel.GetComponent<Animator>();
        upgradeAnim = upgradePanel.GetComponent<Animator>();

        mainCam = Camera.main;

        mainCanvas = useUI.GetComponentInParent<Canvas>();

        useUI.SetActive(false);
        upgradePanel.SetActive(false);
        demolishPanel.SetActive(false);

        goldSpentOnThisObject += info.myBuildCost;

        info = Instantiate(info);
    }

    // Updates all UI elements with the right variables. Updates every time the player clicks on a build.
    public virtual void SetupUI()
    {

    }

    // Upgrades the build if the player has enough gold.
    public virtual void Upgrade()
    {
        info.myLevel++;

        ResourceManager.instance.RemoveGold((int)info.myUpgradeCost.currentValue, true);

        goldSpentOnThisObject += (int)info.myUpgradeCost.currentValue;
        info.myUpgradeCost.currentValue += info.myUpgradeCost.increaseValue;

        if (info.myLevel >= info.myMaxLevel)
        {
            buyUpgradeButton.interactable = false;
            upgradesMaxedOverlay.SetActive(true);
        }
    }

    // Opens/closes the upgrade panel.
    public void ToggleUpgradePanelButton()
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

    // Opens/closes the demolish panel.
    public void ToggleDemolishPanelButton()
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

    // Tells my builder to demolish this build.
    public void DemolishButton()
    {
        myBuilder.Demolish();
    }

    // Closes the upgrade UI with an animation and sets the panel inactive after a certain time.
    private IEnumerator CloseUpgradeUI()
    {
        upgradeAnim.SetTrigger("CloseUI");

        yield return new WaitForSeconds(0.5f);

        upgradePanel.SetActive(false);
    }

    // Closes the demolish UI with an animation and sets the panel inactive after a certain time.
    private IEnumerator CloseDemolishUI()
    {
        demolishAnim.SetTrigger("CloseUI");

        yield return new WaitForSeconds(0.5f);

        demolishPanel.SetActive(false);
    }

    // Starts using this build.
    public virtual void StartUsing()
    {
        usingBuilding = true;
        SetupUI();

        mainCanvas.sortingOrder = 1;
        useUI.SetActive(true);

        CastleUpgradeManager.instance.CloseAllUI(this);
    }

    // Stop using this build.
    public virtual void StopUsingButton()
    {
        StartCoroutine(EventStopUsing());
    }

    // Closes all UI and resets a few variables.
    private IEnumerator EventStopUsing()
    {
        if (useUI.activeInHierarchy)
        {
            useUIAnim.SetTrigger("CloseUI");
        }

        yield return new WaitForSeconds(0.5f);

        usingBuilding = false;
        myBuilder.useButton.SetActive(true);

        mainCanvas.sortingOrder = 0;

        upgradePanel.SetActive(false);
        useUI.SetActive(false);
        demolishPanel.SetActive(false);
    }

    // If this build gets disabled it means that it has been demolished. If that has happened, reset the gold the player has spent on this build.
    private void OnDisable()
    {
        goldSpentOnThisObject = 0;
    }
}
