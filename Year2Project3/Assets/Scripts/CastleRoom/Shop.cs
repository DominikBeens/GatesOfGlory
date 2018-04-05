using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : PreBuiltCastleRoom
{

    [Space(10)]
    public float camZoomSpeed;
    private Transform cameraTarget;
    private CameraManager mainCamManager;

    public TextMeshProUGUI selectedShopItemText;

    [Header("Cost")]
    public int spikesCost;
    public int oilCost;
    public int archersCost;

    public override void Awake()
    {
        base.Awake();

        cameraTarget = GameObject.FindWithTag("CameraTarget").transform;
        mainCamManager = mainCam.GetComponent<CameraManager>();
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
        //StartCoroutine(PlaceObject("shopitem spears"));
        PlaceObject("shopitem spears");
    }

    public void BuyOilButton(Animator a)
    {
        if (!ResourceManager.instance.HasEnoughGold(oilCost))
        {
            return;
        }

        a.SetTrigger("Buy");
        ResourceManager.instance.RemoveGold(oilCost, true);
        PlaceObject("shopitem oil");
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

    //private IEnumerator PlaceObject(string obj)
    //{
    //    CloseUIButton();

    //    Vector3 zoomTo = new Vector3(0, 5, cameraTarget.position.z);
    //    mainCamManager.canMove = false;

    //    while (Vector3.Distance(cameraTarget.position, zoomTo) > 0.1f)
    //    {
    //        cameraTarget.position = Vector3.Lerp(cameraTarget.position, zoomTo, Time.deltaTime * camZoomSpeed);
    //        yield return null;
    //    }

    //    UIManager.instance.placeObjectUI.SetActive(true);
    //    mainCamManager.canMove = true;
    //    ObjectPooler.instance.GrabFromPool(obj, transform.position, Quaternion.identity);
    //}

    private void PlaceObject(string obj)
    {
        CloseUIButton();

        UIManager.instance.placeObjectUI.SetActive(true);
        mainCamManager.canMove = true;
        ObjectPooler.instance.GrabFromPool(obj, transform.position, Quaternion.identity);
    }
}
