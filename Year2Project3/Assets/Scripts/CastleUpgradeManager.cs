using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleUpgradeManager : MonoBehaviour 
{
    public static CastleUpgradeManager instance;

    public static CastleBuilder selectedBuild;

    public float uiScaleDivider;

    public GameObject castleWeaponUI;
    public GameObject castleRoomUI;

    public bool test;

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        gameObject.SetActive(false);
        castleWeaponUI.SetActive(false);
        castleRoomUI.SetActive(false);
    }

    public virtual void Update()
    {
        if (selectedBuild != null)
        {
            transform.parent.gameObject.transform.LookAt(Camera.main.transform);

            if (test)
            {
                transform.parent.localScale = new Vector3(Camera.main.fieldOfView / uiScaleDivider * 1.1f, Camera.main.fieldOfView / uiScaleDivider * 1.1f);
            }

            if (Input.GetButtonDown("Cancel"))
            {
                CloseUIButton();
            }
        }
    }

    public void OpenUI(CastleBuilder selected)
    {
        if (GameManager.instance.playerState == GameManager.PlayerState.UsingWeapon)
        {
            return;
        }

        if (selectedBuild != null)
        {
            AnimationEventCloseUI();
        }

        selectedBuild = selected;

        gameObject.SetActive(true);
        transform.parent.gameObject.transform.position = new Vector2(selectedBuild.transform.position.x, selectedBuild.transform.position.y + 4f);

        if (selectedBuild.type == CastleBuilder.Type.Weapon)
        {
            castleWeaponUI.GetComponent<CastleWeaponUpgrader>().OpenUI();
        }
        else
        {
            castleRoomUI.GetComponent<CastleRoomUpgrader>().OpenUI();
        }
    }

    public void CloseUIButton()
    {
        GetComponent<Animator>().SetTrigger("CloseUI");
    }

    public virtual void AnimationEventCloseUI()
    {
        castleWeaponUI.SetActive(false);
        castleRoomUI.SetActive(false);
        gameObject.SetActive(false);
        transform.localScale = Vector3.one;

        if (selectedBuild.myBuildedObject == null)
        {
            selectedBuild.buildButton.SetActive(true);
        }

        selectedBuild = null;
    }
}
