using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBuildUpgradeManager : MonoBehaviour 
{

    public static CastleBuildUpgradeManager instance;

    private static CastleBuilder selectedBuild;

    public GameObject upgradeUI;

    [Header("Buttons")]
    public GameObject buildWeaponButton;
    public GameObject useWeaponButton;
    public GameObject upgradeWeaponButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        gameObject.SetActive(false);
        upgradeUI.SetActive(false);
    }

    private void Update()
    {
        if (selectedBuild != null)
        {
            gameObject.transform.LookAt(Camera.main.transform);

            if (Input.GetButtonDown("Cancel"))
            {
                CloseUIButton();
            }
        }
    }

    public void OpenUI(CastleBuilder selected)
    {
        if (true)
        {
            // If selected hasnt been built yet, activate build button, else activate use button.
        }

        selectedBuild = selected;

        gameObject.transform.position = new Vector2(selected.transform.position.x, selected.transform.position.y + 2f);
        gameObject.SetActive(true);
    }

    public void UseWeaponButton()
    {
        CloseUIButton();
    }

    public void UpgradeWeaponButton()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUIButton()
    {
        GetComponent<Animator>().SetTrigger("CloseUI");
    }

    public void AnimationEventCloseUI()
    {
        upgradeUI.SetActive(false);
        gameObject.SetActive(false);
    }
}
