using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notary : MonoBehaviour 
{

    public Transform uiParent;
    public GameObject uiPanel;
    public GameObject uiOpenButton;

    public Animator anim;

    public static int goldAccumulated;
    public static int goldSpent;

    public TextMeshProUGUI goldAccumulatedText;
    public TextMeshProUGUI goldSpentText;

    private void Awake()
    {
        uiPanel.SetActive(false);    
    }

    private void Update()
    {
        if (uiPanel.activeInHierarchy)
        {
            goldAccumulatedText.text = goldAccumulated.ToString();
            goldSpentText.text = goldSpent.ToString();

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
}
