using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBuilder : MonoBehaviour 
{

    public GameObject myBuildedObject;

    public List<GameObject> availableBuilds = new List<GameObject>();

    [Header("Buttons")]
    public GameObject buildButton;
    public GameObject useButton;

    public void InteractButton()
    {
        CastleBuildUpgradeManager.instance.OpenUI(this);
    }

    public void BuildWeapon(CastleWeapon.WeaponType type)
    {
        for (int i = 0; i < availableBuilds.Count; i++)
        {
            if (availableBuilds[i].GetComponent<CastleWeapon>().weaponType == type)
            {
                GameObject newBuild = Instantiate(availableBuilds[i], transform.position, Quaternion.identity);
                newBuild.transform.SetParent(transform);
                myBuildedObject = newBuild;

                buildButton.SetActive(false);
                useButton.SetActive(true);

                return;
            }
        }
    }
}
