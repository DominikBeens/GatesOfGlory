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

    public Button archerButton;
    public GameObject archerButtonMaxOverlay;
    private ArcherSpot[] archerSpots;

    public List<RectTransform> shopButtonCostOverlays = new List<RectTransform>();

    public override void Awake()
    {
        base.Awake();

        cameraTarget = GameObject.FindWithTag("CameraTarget").transform;
        mainCamManager = mainCam.GetComponent<CameraManager>();

        archerSpots = FindObjectsOfType<ArcherSpot>();
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

    public void BuyItemButton(ShopItemButton shopItemButton)
    {
        if (!ResourceManager.instance.HasEnoughGold(shopItemButton.myItem.itemCost))
        {
            return;
        }

        shopItemButton.myAnim.SetTrigger("Buy");
        ResourceManager.instance.RemoveGold(shopItemButton.myItem.itemCost, true);
        PlaceObject(shopItemButton.myItem.itemPoolName);
    }

    public void BuyArcherButton(ShopItemButton shopItemButton)
    {
        if (!ResourceManager.instance.HasEnoughGold(shopItemButton.myItem.itemCost))
        {
            return;
        }
        
        shopItemButton.myAnim.SetTrigger("Buy");
        ResourceManager.instance.RemoveGold(shopItemButton.myItem.itemCost, true);
        CloseUIButton();

        bool lookingForArcherLeft = true;
        bool lookingForArcherRight = true;
        for (int i = 0; i < archerSpots.Length; i++)
        {
            if (lookingForArcherLeft)
            {
                if (archerSpots[i].side == ArcherSpot.Side.Left && archerSpots[i].Isfree())
                {
                    archerSpots[i].SetArcher();
                    lookingForArcherLeft = false;
                }
            }

            if (lookingForArcherRight)
            {
                if (archerSpots[i].side == ArcherSpot.Side.Right && archerSpots[i].Isfree())
                {
                    archerSpots[i].SetArcher();
                    lookingForArcherRight = false;
                }
            }

            if (i == archerSpots.Length - 1)
            {
                if (lookingForArcherLeft && lookingForArcherRight)
                {
                    archerButtonMaxOverlay.SetActive(true);
                    archerButton.interactable = false;
                }
            }
        }
    }

    private void PlaceObject(string obj)
    {
        CloseUIButton();

        UIManager.instance.placeObjectUI.SetActive(true);
        mainCamManager.canMove = true;
        ObjectPooler.instance.GrabFromPool(obj, transform.position, Quaternion.identity);
    }
}
