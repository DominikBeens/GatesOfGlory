using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Generator : PreBuiltCastleRoom
{

    private bool repaired;

    [Space(10)]
    public Animator generatorAnim;

    public GameObject brokenUI;
    public GameObject repairedUI;

    public int repairCost;
    public TextMeshProUGUI repairCostText;
    public TextMeshProUGUI healDescriptionText;

    [Header("Stats")]
    public Stat healAmount;
    public Stat healCooldown;

    private float nextHealTime;

    public override void Awake()
    {
        base.Awake();

        repairCostText.text = repairCost.ToString();
    }

    public override void Update()
    {
        base.Update();

        if (uiPanel.activeInHierarchy)
        {
            healDescriptionText.text = "Current rate: <color=green>" + healAmount.currentValue + "</color> HP per <color=green>" + healCooldown.currentValue + "</color> seconds.";
        }

        if (repaired)
        {
            if (Time.time >= nextHealTime)
            {
                nextHealTime = Time.time + healCooldown.currentValue;

                for (int i = 0; i < WaveManager.instance.allCastleGates.Count; i++)
                {
                    if (!WaveManager.instance.allCastleGates[i].locked)
                    {
                        WaveManager.instance.allCastleGates[i].Heal(healAmount.currentValue);
                    }
                }
            }
        }
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
