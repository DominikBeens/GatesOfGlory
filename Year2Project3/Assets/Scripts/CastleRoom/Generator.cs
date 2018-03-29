using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Generator : MonoBehaviour 
{

    private bool repaired;

    public Transform uiParent;
    public GameObject uiPanel;
    public GameObject uiOpenButton;

    public Animator anim;
    public Animator generatorAnim;

    public GameObject brokenUI;
    public GameObject repairedUI;

    public int repairCost;
    public TextMeshProUGUI repairCostText;
    public TextMeshProUGUI healDescriptionText;

    public OutlineOnMouseOver outline;

    [Header("Stats")]
    public Stat healAmount;
    public Stat healCooldown;

    private float nextHealTime;

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
            healDescriptionText.text = "Current rate: <color=green>" + healAmount.currentValue + "</color> HP per <color=green>" + healCooldown.currentValue + "</color> seconds.";
        }

        if (repaired)
        {
            if (Time.time >= nextHealTime)
            {
                nextHealTime = Time.time + healCooldown.currentValue;

                for (int i = 0; i < WaveManager.instance.allGates.Count; i++)
                {
                    if (!WaveManager.instance.allCastleGates[i].locked)
                    {
                        WaveManager.instance.allGates[i].Heal(healAmount.currentValue);
                    }
                }
            }
        }
    }

    public void OpenUIButton()
    {
        uiOpenButton.SetActive(false);
        uiPanel.SetActive(true);

        if (outline != null)
        {
            outline.canShowOutline = false;
            outline.OnMouseExit();
        }
    }

    public void CloseUIButton()
    {
        StartCoroutine(CloseUI());

        if (outline != null)
        {
            outline.canShowOutline = true;
        }
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

        repaired = true;

        uiPanel.SetActive(false);

        brokenUI.SetActive(false);
        repairedUI.SetActive(true);

        uiPanel.SetActive(true);
    }
}
