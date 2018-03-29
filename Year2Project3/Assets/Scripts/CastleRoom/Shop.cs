using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour 
{

    public Transform uiParent;
    public GameObject uiPanel;
    public GameObject uiOpenButton;

    public Animator anim;

    public float camZoomSpeed;
    private Transform cameraTarget;
    private CameraManager mainCamManager;
    private Camera mainCam;

    public TextMeshProUGUI selectedShopItemText;

    [Header("Cost")]
    public int spikesCost;
    public int oilCost;
    public int archersCost;

    private void Awake()
    {
        uiPanel.SetActive(false);

        cameraTarget = GameObject.FindWithTag("CameraTarget").transform;
        mainCam = Camera.main;
        mainCamManager = mainCam.GetComponent<CameraManager>();
    }

    private void Update()
    {
        if (uiPanel.activeInHierarchy)
        {
            uiParent.LookAt(Camera.main.transform);
        }
    }

    public void OpenUIButton()
    {
        uiOpenButton.SetActive(false);
        uiPanel.SetActive(true);
    }

    public void CloseUIButton()
    {
        StartCoroutine(CloseUI());
    }

    private IEnumerator CloseUI()
    {
        anim.SetTrigger("CloseUI");

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        uiPanel.SetActive(false);
        uiOpenButton.SetActive(true);
    }

    public void SetSelectedShopItemButton(string s)
    {
        selectedShopItemText.text = s;
    }

    public void ResetSelectedShopItemButton()
    {
        selectedShopItemText.text = "What would you like to buy?";
    }

    public void ShowShopItemCost(Animator a)
    {
        a.ResetTrigger("Close");
        a.SetTrigger("Open");
    }

    public void HideShopItemCost(Animator a)
    {
        a.ResetTrigger("Open");
        a.SetTrigger("Close");
    }

    public void BuySpikesButton(Animator a)
    {
        if (!ResourceManager.instance.HasEnoughGold(spikesCost))
        {
            return;
        }

        a.SetTrigger("Buy");
        ResourceManager.instance.RemoveGold(spikesCost, true);
        StartCoroutine(PlaceObject("shopitem spears"));
    }

    public void BuyOilButton(Animator a)
    {
        if (!ResourceManager.instance.HasEnoughGold(oilCost))
        {
            return;
        }

        a.SetTrigger("Buy");
        ResourceManager.instance.RemoveGold(oilCost, true);
    }

    public void BuyArcherButton(Animator a)
    {
        if (!ResourceManager.instance.HasEnoughGold(archersCost))
        {
            return;
        }

        a.SetTrigger("Buy");
        ResourceManager.instance.RemoveGold(archersCost, true);
    }

    private IEnumerator PlaceObject(string obj)
    {
        CloseUIButton();

        Vector3 zoomTo = new Vector3(0, 5, cameraTarget.position.z);
        mainCamManager.canMove = false;

        while (Vector3.Distance(cameraTarget.position, zoomTo) > 0.1f)
        {
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, zoomTo, Time.deltaTime * camZoomSpeed);
            yield return null;
        }

        UIManager.instance.placeObjectUI.SetActive(true);
        mainCamManager.canMove = true;
        ObjectPooler.instance.GrabFromPool("shopitem spears", transform.position, Quaternion.identity);
    }
}
