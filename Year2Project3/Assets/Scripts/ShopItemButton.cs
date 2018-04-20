using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItemButton : MonoBehaviour 
{

    public ShopItem myItem;
    public Animator myAnim;
    public TextMeshProUGUI myCostText;

    [System.Serializable]
    public struct ShopItem
    {
        public string itemPoolName;
        public int itemCost;
    }

    private void Awake()
    {
        myCostText.text = myItem.itemCost.ToString();
    }
}
