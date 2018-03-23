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

    public TextMeshProUGUI selectedShopItemText;

    private void Awake()
    {
        uiPanel.SetActive(false);
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
}
