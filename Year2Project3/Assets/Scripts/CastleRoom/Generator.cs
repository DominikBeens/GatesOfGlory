using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Generator : MonoBehaviour 
{

    public Transform uiParent;
    public GameObject uiPanel;
    public GameObject uiOpenButton;

    public Animator anim;
    public Animator generatorAnim;

    public GameObject brokenUI;
    public GameObject repairedUI;

    public int repairCost;
    public TextMeshProUGUI repairCostText;

    private void Awake()
    {
        uiPanel.SetActive(false);
        repairCostText.text = repairCost.ToString();
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

    public void RepairGeneratorButton()
    {
        if (!ResourceManager.instance.HasEnoughGold(repairCost))
        {
            return;
        }

        ResourceManager.instance.RemoveGold(repairCost, true);
        generatorAnim.SetBool("Repaired", true);

        uiPanel.SetActive(false);

        brokenUI.SetActive(false);
        repairedUI.SetActive(true);

        uiPanel.SetActive(true);
    }
}
