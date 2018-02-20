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

    public List<CastleWeapon> allBuiltWeapons = new List<CastleWeapon>();
    public List<CastleRoom> allBuiltRooms = new List<CastleRoom>();

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

            if (Input.GetButtonDown("Cancel"))
            {
                CloseAllUI();
            }
        }
    }

    public void OpenUI(CastleBuilder selected)
    {
        if (selectedBuild != null)
        {
            AnimationEventCloseUI();
        }

        selectedBuild = selected;

        if (selectedBuild.myBuildedObject == null)
        {
            CloseAllOtherUI();
        }

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

        selectedBuild.useButton.SetActive(false);
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

    public void CloseAllUI()
    {
        GetComponent<Animator>().SetTrigger("CloseUI");

        for (int i = 0; i < allBuiltWeapons.Count; i++)
        {
            allBuiltWeapons[i].StopUsing();
        }

        for (int i = 0; i < allBuiltRooms.Count; i++)
        {
            allBuiltRooms[i].StopUsing();
        }
    }

    public void CloseAllUI(CastleWeapon exception)
    {
        GetComponent<Animator>().SetTrigger("CloseUI");

        for (int i = 0; i < allBuiltWeapons.Count; i++)
        {
            if (allBuiltWeapons[i] != exception)
            {
                allBuiltWeapons[i].StopUsing();
            }
        }

        for (int i = 0; i < allBuiltRooms.Count; i++)
        {
            allBuiltRooms[i].StopUsing();
        }
    }

    public void CloseAllUI(CastleRoom exception)
    {
        GetComponent<Animator>().SetTrigger("CloseUI");

        for (int i = 0; i < allBuiltWeapons.Count; i++)
        {
            allBuiltWeapons[i].StopUsing();
        }

        for (int i = 0; i < allBuiltRooms.Count; i++)
        {
            if (allBuiltRooms[i] != exception)
            {
                allBuiltRooms[i].StopUsing();
            }
        }
    }

    public void CloseAllOtherUI()
    {
        for (int i = 0; i < allBuiltWeapons.Count; i++)
        {
            allBuiltWeapons[i].StopUsing();
        }

        for (int i = 0; i < allBuiltRooms.Count; i++)
        {
            allBuiltRooms[i].StopUsing();
        }
    }

    public string CheckPositiveOrNegative(float stat)
    {
        string newStat = null;

        if (stat > 0)
        {
            newStat = "+" + stat;
        }
        else
        {
            newStat = stat.ToString();
        }

        return newStat;
    }
}
