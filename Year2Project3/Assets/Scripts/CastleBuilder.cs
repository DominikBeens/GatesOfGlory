using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBuilder : MonoBehaviour 
{

    public enum Type
    {
        Weapon,
        Room
    }
    public Type type;

    public enum Side
    {
        Left,
        Right
    }
    public Side side;

    public GameObject myBuildedObject;

    public List<GameObject> availableBuilds = new List<GameObject>();

    [Header("Buttons")]
    public GameObject buildButton;
    public GameObject useButton;

    public void InteractButton()
    {
        CastleUpgradeManager.instance.OpenUI(this);
    }

    public void BuildWeapon(CastleWeapon.WeaponType type)
    {
        for (int i = 0; i < availableBuilds.Count; i++)
        {
            if (availableBuilds[i].GetComponent<CastleWeapon>().weaponType == type)
            {
                if (ResourceManager.gold < availableBuilds[i].GetComponent<CastleWeapon>().buildCost)
                {
                    return;
                }

                GameObject newBuild = Instantiate(availableBuilds[i], transform.position, Quaternion.identity);
                newBuild.transform.SetParent(transform);
                myBuildedObject = newBuild;

                switch (side)
                {
                    case Side.Left:

                        myBuildedObject.GetComponent<CastleWeapon>().SetLeftSide();
                        break;
                    case Side.Right:

                        myBuildedObject.GetComponent<CastleWeapon>().SetRightSide();
                        break;
                }

                buildButton.SetActive(false);
                useButton.SetActive(true);

                CastleUpgradeManager.instance.CloseUIButton();

                return;
            }
        }
    }

    public void BuildRoom(CastleRoom.RoomType type)
    {
        for (int i = 0; i < availableBuilds.Count; i++)
        {
            if (availableBuilds[i].GetComponent<CastleRoom>().roomType == type)
            {
                if (ResourceManager.gold < availableBuilds[i].GetComponent<CastleRoom>().buildCost)
                {
                    return;
                }

                GameObject newBuild = Instantiate(availableBuilds[i], transform.position, Quaternion.identity);
                newBuild.transform.SetParent(transform);
                myBuildedObject = newBuild;

                switch (side)
                {
                    case Side.Left:

                        myBuildedObject.GetComponent<CastleRoom>().side = CastleRoom.Side.Left;
                        break;
                    case Side.Right:

                        myBuildedObject.GetComponent<CastleRoom>().side = CastleRoom.Side.Right;
                        break;
                }

                buildButton.SetActive(false);
                useButton.SetActive(true);

                CastleUpgradeManager.instance.CloseUIButton();

                return;
            }
        }
    }
}
